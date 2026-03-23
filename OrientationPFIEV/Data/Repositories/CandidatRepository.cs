using System.Data;
using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>
/// CRUD + query operations for the Candidat table in the session .mdb.
/// Also handles weighted average computation and ranking.
/// </summary>
public class CandidatRepository
{
    private readonly DatabaseContext _ctx;
    public CandidatRepository(DatabaseContext ctx) => _ctx = ctx;

    public List<Candidat> GetAll()
    {
        var result = new List<Candidat>();
        using var cmd = new OleDbCommand("SELECT * FROM Candidat ORDER BY CandidatID", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));
        return result;
    }

    public Candidat? GetById(int id)
    {
        using var cmd = new OleDbCommand("SELECT * FROM Candidat WHERE CandidatID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public void Add(Candidat c)
    {
        using var cmd = new OleDbCommand(
            "INSERT INTO Candidat (Nom, NomIntermediaire, Prenom, DateDeNaissance, Sexe, " +
            "CandidatStatut, Langue, EtabID, CandidatMoyenne, CandidatClassement, anonymat) " +
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            _ctx.GetConnection());
        BindParams(cmd, c);
        cmd.ExecuteNonQuery();
    }

    public void Update(Candidat c)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Candidat SET Nom=?, NomIntermediaire=?, Prenom=?, DateDeNaissance=?, " +
            "Sexe=?, CandidatStatut=?, Langue=?, EtabID=?, CandidatMoyenne=?, " +
            "CandidatClassement=?, anonymat=? WHERE CandidatID=?",
            _ctx.GetConnection());
        BindParams(cmd, c);
        cmd.Parameters.AddWithValue("id", c.CandidatID);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var cmd = new OleDbCommand("DELETE FROM Candidat WHERE CandidatID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Returns DataTable from QueryCandidatSession:
    /// Candidat fields + EtabNom via LEFT JOIN Etablissement.
    /// </summary>
    public DataTable GetSessionView()
    {
        const string sql =
            "SELECT c.*, e.EtabNom " +
            "FROM Candidat c LEFT JOIN Etablissement e ON c.EtabID = e.EtabID " +
            "ORDER BY c.CandidatID";
        var dt = new DataTable();
        using var adapter = new OleDbDataAdapter(sql, _ctx.GetConnection());
        adapter.Fill(dt);
        return dt;
    }

    /// <summary>
    /// Returns candidates eligible for attribution ordered by ranking.
    /// Includes internal (I) and external (E) candidates whose average exceeds MoyenneMin.
    /// </summary>
    public List<int> GetRankedForAttribution()
    {
        const string sql =
            "SELECT CandidatID FROM Candidat " +
            "WHERE CandidatStatut = 'I' " +
            "   OR (CandidatStatut = 'E' AND CandidatMoyenne > (SELECT MoyenneMin FROM Concours)) " +
            "ORDER BY CandidatClassement";
        var ids = new List<int>();
        using var cmd = new OleDbCommand(sql, _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            ids.Add(reader.GetInt32(0));
        return ids;
    }

    /// <summary>
    /// Computes weighted average (SUM(Note*Coeff)/SUM(Coeff)) for all candidates,
    /// then assigns integer ranking (1 = highest average).
    /// NoteValue is stored as string; unparseable values are treated as 0.
    /// Requires access to CONFIG.MDB Matiere coefficients via configCtx.
    /// </summary>
    public void ComputeAveragesAndRanking(DatabaseContext configCtx)
    {
        // Step 1: load all matieres (coefficients) from config DB
        var coefficients = new Dictionary<int, double>();
        using (var mCmd = new OleDbCommand("SELECT MatiereID, MatiereCoefficient FROM Matiere", configCtx.GetConnection()))
        using (var mReader = mCmd.ExecuteReader())
        {
            while (mReader.Read())
                coefficients[mReader.GetInt32(0)] = mReader.IsDBNull(1) ? 1.0 : mReader.GetDouble(1);
        }

        // Step 2: load all notes from session DB
        var notesByCandidat = new Dictionary<int, List<(int MatiereID, double Value)>>();
        using (var nCmd = new OleDbCommand("SELECT CandidatID, MatiereID, Note FROM Note", _ctx.GetConnection()))
        using (var nReader = nCmd.ExecuteReader())
        {
            while (nReader.Read())
            {
                int cid = nReader.GetInt32(0);
                int mid = nReader.GetInt32(1);
                string raw = nReader.IsDBNull(2) ? "0" : nReader.GetValue(2).ToString()!;
                // Normalise locale separator
                double val = double.TryParse(raw.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double parsed) ? parsed : 0.0;

                if (!notesByCandidat.ContainsKey(cid))
                    notesByCandidat[cid] = new();
                notesByCandidat[cid].Add((mid, val));
            }
        }

        // Step 3: compute average for each candidate and UPDATE
        foreach (var (cid, notes) in notesByCandidat)
        {
            double sumWeighted = 0, sumCoeff = 0;
            foreach (var (mid, val) in notes)
            {
                double coeff = coefficients.TryGetValue(mid, out double c) ? c : 1.0;
                sumWeighted += val * coeff;
                sumCoeff += coeff;
            }
            double moyenne = sumCoeff > 0 ? sumWeighted / sumCoeff : 0.0;

            using var upd = new OleDbCommand(
                "UPDATE Candidat SET CandidatMoyenne=? WHERE CandidatID=?",
                _ctx.GetConnection());
            upd.Parameters.AddWithValue("moy", moyenne);
            upd.Parameters.AddWithValue("id", cid);
            upd.ExecuteNonQuery();
        }

        // Step 4: rank by moyenne DESC — fetch all then update in order
        var ranked = new List<(int CandidatID, double Moyenne)>();
        using (var rCmd = new OleDbCommand(
            "SELECT CandidatID, CandidatMoyenne FROM Candidat ORDER BY CandidatMoyenne DESC",
            _ctx.GetConnection()))
        using (var rReader = rCmd.ExecuteReader())
        {
            while (rReader.Read())
            {
                int cid = rReader.GetInt32(0);
                double moy = rReader.IsDBNull(1) ? 0.0 : rReader.GetDouble(1);
                ranked.Add((cid, moy));
            }
        }

        for (int i = 0; i < ranked.Count; i++)
        {
            using var upd = new OleDbCommand(
                "UPDATE Candidat SET CandidatClassement=? WHERE CandidatID=?",
                _ctx.GetConnection());
            upd.Parameters.AddWithValue("rank", i + 1);
            upd.Parameters.AddWithValue("id", ranked[i].CandidatID);
            upd.ExecuteNonQuery();
        }
    }

    /// <summary>Returns the CandidatID of the last auto-inserted row via @@IDENTITY.</summary>
    public int GetLastInsertedId()
    {
        using var cmd = new OleDbCommand("SELECT @@IDENTITY", _ctx.GetConnection());
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // --- helpers ---

    private static void BindParams(OleDbCommand cmd, Candidat c)
    {
        cmd.Parameters.AddWithValue("nom", c.Nom);
        cmd.Parameters.AddWithValue("nomInter", c.NomIntermediaire);
        cmd.Parameters.AddWithValue("prenom", c.Prenom);
        cmd.Parameters.AddWithValue("ddn", c.DateDeNaissance.HasValue ? (object)c.DateDeNaissance.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("sexe", c.Sexe);
        cmd.Parameters.AddWithValue("statut", c.CandidatStatut);
        cmd.Parameters.AddWithValue("langue", c.Langue);
        cmd.Parameters.AddWithValue("etab", c.EtabID);
        cmd.Parameters.AddWithValue("moy", c.CandidatMoyenne.HasValue ? (object)c.CandidatMoyenne.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("classement", c.CandidatClassement.HasValue ? (object)c.CandidatClassement.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("anonymat", c.Anonymat);
    }

    private static Candidat Map(OleDbDataReader r)
    {
        int ordDdn = r.GetOrdinal("DateDeNaissance");
        int ordMoy = r.GetOrdinal("CandidatMoyenne");
        int ordCls = r.GetOrdinal("CandidatClassement");
        return new Candidat
        {
            CandidatID = r.GetInt32(r.GetOrdinal("CandidatID")),
            Nom = r.IsDBNull(r.GetOrdinal("Nom")) ? "" : r.GetString(r.GetOrdinal("Nom")),
            NomIntermediaire = r.IsDBNull(r.GetOrdinal("NomIntermediaire")) ? "" : r.GetString(r.GetOrdinal("NomIntermediaire")),
            Prenom = r.IsDBNull(r.GetOrdinal("Prenom")) ? "" : r.GetString(r.GetOrdinal("Prenom")),
            DateDeNaissance = r.IsDBNull(ordDdn) ? null : r.GetDateTime(ordDdn),
            Sexe = r.IsDBNull(r.GetOrdinal("Sexe")) ? "" : r.GetString(r.GetOrdinal("Sexe")),
            CandidatStatut = r.IsDBNull(r.GetOrdinal("CandidatStatut")) ? "" : r.GetString(r.GetOrdinal("CandidatStatut")),
            Langue = r.IsDBNull(r.GetOrdinal("Langue")) ? "" : r.GetString(r.GetOrdinal("Langue")),
            EtabID = r.IsDBNull(r.GetOrdinal("EtabID")) ? 0 : r.GetInt32(r.GetOrdinal("EtabID")),
            CandidatMoyenne = r.IsDBNull(ordMoy) ? null : r.GetDouble(ordMoy),
            CandidatClassement = r.IsDBNull(ordCls) ? null : r.GetInt32(ordCls),
            Anonymat = r.IsDBNull(r.GetOrdinal("anonymat")) ? "" : r.GetValue(r.GetOrdinal("anonymat")).ToString()!,
        };
    }
}
