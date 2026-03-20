using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>Details for a single Choix row joined with Filiere capacity.</summary>
public class ChoixDetail
{
    public int ChoixID { get; set; }
    public int FiliereID { get; set; }
    public int FiliereNbPlace { get; set; }
    public int NumeroDeChoix { get; set; }
}

/// <summary>
/// Operations for the Choix table in the session .mdb.
/// Handles preference ordering, admission flags, and attribution logic.
/// </summary>
public class ChoixRepository
{
    private readonly DatabaseContext _ctx;
    public ChoixRepository(DatabaseContext ctx) => _ctx = ctx;

    /// <summary>
    /// Returns the ordered choice list for a candidate, joined with Filiere capacity.
    /// SQL mirrors QueryCandidatChoix from the original VB6 Access queries.
    /// </summary>
    public List<ChoixDetail> GetOrderedChoices(int candidatId)
    {
        const string sql =
            "SELECT ch.ChoixID, ch.FiliereID, f.FiliereNbPlace, ch.NumeroDeChoix " +
            "FROM Choix ch INNER JOIN Filiere f ON ch.FiliereID = f.FiliereID " +
            "WHERE ch.CandidatID = ? " +
            "ORDER BY ch.NumeroDeChoix ASC";

        var result = new List<ChoixDetail>();
        using var cmd = new OleDbCommand(sql, _ctx.GetConnection());
        cmd.Parameters.AddWithValue("cid", candidatId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new ChoixDetail
            {
                ChoixID = reader.GetInt32(reader.GetOrdinal("ChoixID")),
                FiliereID = reader.GetInt32(reader.GetOrdinal("FiliereID")),
                FiliereNbPlace = reader.IsDBNull(reader.GetOrdinal("FiliereNbPlace"))
                    ? 0 : reader.GetInt32(reader.GetOrdinal("FiliereNbPlace")),
                NumeroDeChoix = reader.GetInt32(reader.GetOrdinal("NumeroDeChoix")),
            });
        }
        return result;
    }

    /// <summary>Returns how many candidates have been admitted to a given filiere.</summary>
    public int CountAdmis(int filiereId)
    {
        using var cmd = new OleDbCommand(
            "SELECT COUNT(*) FROM Choix WHERE FiliereID=? AND ChoixAdmis=True",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("fid", filiereId);
        return (int)cmd.ExecuteScalar()!;
    }

    /// <summary>Marks a specific Choix row as admitted.</summary>
    public void SetAdmis(int choixId, bool admis)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Choix SET ChoixAdmis=? WHERE ChoixID=?",
            _ctx.GetConnection());
        // Access stores Yes/No as -1 (true) / 0 (false) via OleDb boolean
        cmd.Parameters.AddWithValue("admis", admis);
        cmd.Parameters.AddWithValue("id", choixId);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Resets all ChoixAdmis flags to false — call before re-running attribution.</summary>
    public void ResetAdmis()
    {
        using var cmd = new OleDbCommand("UPDATE Choix SET ChoixAdmis=False", _ctx.GetConnection());
        cmd.ExecuteNonQuery();
    }

    /// <summary>Deletes all choices for a given candidate.</summary>
    public void DeleteByCandidatId(int candidatId)
    {
        using var cmd = new OleDbCommand("DELETE FROM Choix WHERE CandidatID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("cid", candidatId);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Insert or update a Choix row.
    /// Matches on (CandidatID, NumeroDeChoix); updates FiliereID if row exists.
    /// </summary>
    public void Upsert(Choix c)
    {
        int existingId = 0;
        using (var chk = new OleDbCommand(
            "SELECT ChoixID FROM Choix WHERE CandidatID=? AND NumeroDeChoix=?",
            _ctx.GetConnection()))
        {
            chk.Parameters.AddWithValue("cid", c.CandidatID);
            chk.Parameters.AddWithValue("num", c.NumeroDeChoix);
            var val = chk.ExecuteScalar();
            if (val != null && val != DBNull.Value)
                existingId = Convert.ToInt32(val);
        }

        if (existingId > 0)
        {
            using var upd = new OleDbCommand(
                "UPDATE Choix SET FiliereID=?, ChoixAdmis=? WHERE ChoixID=?",
                _ctx.GetConnection());
            upd.Parameters.AddWithValue("fid", c.FiliereID);
            upd.Parameters.AddWithValue("admis", c.ChoixAdmis);
            upd.Parameters.AddWithValue("id", existingId);
            upd.ExecuteNonQuery();
        }
        else
        {
            using var ins = new OleDbCommand(
                "INSERT INTO Choix (CandidatID, FiliereID, NumeroDeChoix, ChoixAdmis) VALUES (?, ?, ?, ?)",
                _ctx.GetConnection());
            ins.Parameters.AddWithValue("cid", c.CandidatID);
            ins.Parameters.AddWithValue("fid", c.FiliereID);
            ins.Parameters.AddWithValue("num", c.NumeroDeChoix);
            ins.Parameters.AddWithValue("admis", c.ChoixAdmis);
            ins.ExecuteNonQuery();
        }
    }
}
