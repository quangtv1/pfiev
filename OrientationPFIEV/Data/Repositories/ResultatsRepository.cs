using System.Data;
using System.Data.OleDb;
using OrientationPFIEV.Data;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>
/// Read-only queries for result and statistics screens (Phase 06).
/// All methods return DataTable for direct DataGridView binding.
/// Access SQL rules: nested parens for LEFT JOINs, IIF() not CASE WHEN, ChoixAdmis = True.
/// </summary>
public class ResultatsRepository
{
    private readonly DatabaseContext _db;
    public ResultatsRepository(DatabaseContext db) => _db = db;

    /// <summary>
    /// QueryResultats: all candidates with their assigned filiere and school.
    /// 4-table LEFT JOIN using Access nested-paren syntax.
    /// </summary>
    public DataTable GetResultats()
    {
        const string sql = @"
            SELECT c.CandidatID, c.Nom, c.NomIntermediaire, c.Prenom,
                   c.CandidatStatut, c.CandidatMoyenne, c.CandidatClassement,
                   f.FiliereCode, f.FiliereNom, e.EtabNom AS EtabFiliere
            FROM (((Candidat c
            LEFT JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = True)
            LEFT JOIN Filiere f ON ch.FiliereID = f.FiliereID)
            LEFT JOIN Etablissement e ON f.EtabID = e.EtabID)
            ORDER BY c.CandidatClassement";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// Diep_ClassementMoyenneSpe: average score and admitted count per specialty.
    /// </summary>
    public DataTable GetClassementMoyenneSpe()
    {
        const string sql = @"
            SELECT f.FiliereNom, AVG(c.CandidatMoyenne) AS MoyenneSpe,
                   COUNT(c.CandidatID) AS NbAdmis
            FROM (Filiere f
            INNER JOIN Choix ch ON f.FiliereID = ch.FiliereID AND ch.ChoixAdmis = True)
            INNER JOIN Candidat c ON ch.CandidatID = c.CandidatID
            GROUP BY f.FiliereNom";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// StatEtabTemp: per-school breakdown with internal/external split using IIF().
    /// </summary>
    public DataTable GetStatEtab()
    {
        const string sql = @"
            SELECT e.EtabNom, COUNT(c.CandidatID) AS NbCandidats,
                   SUM(IIF(c.CandidatStatut='I',1,0)) AS NbInternes,
                   SUM(IIF(c.CandidatStatut='E',1,0)) AS NbExternes
            FROM Etablissement e INNER JOIN Candidat c ON e.EtabID = c.EtabID
            GROUP BY e.EtabNom";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// Diep_TableauRecapInternes: recap table for internal candidates (CandidatStatut='I').
    /// Shows filiere attribution summary for internal students.
    /// </summary>
    public DataTable GetTableauRecapInternes()
    {
        const string sql = @"
            SELECT c.CandidatID, c.Nom, c.NomIntermediaire, c.Prenom,
                   c.CandidatMoyenne, c.CandidatClassement,
                   f.FiliereCode, f.FiliereNom, e.EtabNom AS EtabFiliere
            FROM (((Candidat c
            LEFT JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = True)
            LEFT JOIN Filiere f ON ch.FiliereID = f.FiliereID)
            LEFT JOIN Etablissement e ON f.EtabID = e.EtabID)
            WHERE c.CandidatStatut = 'I'
            ORDER BY c.CandidatClassement";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// Diep_TableauRecapExternes: recap table for external candidates (CandidatStatut='E').
    /// Shows filiere attribution summary for external students.
    /// </summary>
    public DataTable GetTableauRecapExternes()
    {
        const string sql = @"
            SELECT c.CandidatID, c.Nom, c.NomIntermediaire, c.Prenom,
                   c.CandidatMoyenne, c.CandidatClassement,
                   f.FiliereCode, f.FiliereNom, e.EtabNom AS EtabFiliere
            FROM (((Candidat c
            LEFT JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = True)
            LEFT JOIN Filiere f ON ch.FiliereID = f.FiliereID)
            LEFT JOIN Etablissement e ON f.EtabID = e.EtabID)
            WHERE c.CandidatStatut = 'E'
            ORDER BY c.CandidatClassement";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// Stats by candidate language: count per Langue value.
    /// </summary>
    public DataTable GetStatByLanguage()
    {
        const string sql = @"
            SELECT c.Langue, COUNT(c.CandidatID) AS NbCandidats
            FROM Candidat c
            GROUP BY c.Langue
            ORDER BY c.Langue";
        return ExecuteQuery(sql);
    }

    /// <summary>
    /// Top N admitted candidates per filiere, ordered by score descending.
    /// Uses a subquery rank pattern compatible with Access SQL.
    /// </summary>
    public DataTable GetStatTopPerFiliere(int topN)
    {
        // Access doesn't support ROW_NUMBER; retrieve all admitted ordered by filiere+score,
        // then filter in code — or fetch with a correlated count subquery.
        // For simplicity, return all admitted rows ordered; caller can limit display.
        var sql = $@"
            SELECT TOP {topN * 100} c.Nom, c.NomIntermediaire, c.Prenom,
                   c.CandidatMoyenne, c.CandidatClassement,
                   f.FiliereNom
            FROM ((Candidat c
            INNER JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = True)
            INNER JOIN Filiere f ON ch.FiliereID = f.FiliereID)
            ORDER BY f.FiliereNom, c.CandidatClassement";
        var dt = ExecuteQuery(sql);
        return FilterTopNPerFiliere(dt, topN);
    }

    /// <summary>
    /// Score distribution: count of admitted candidates per average score range.
    /// Ranges: [0-5), [5-6), [6-7), [7-8), [8-9), [9-10].
    /// </summary>
    public DataTable GetScoreDistribution()
    {
        const string sql = @"
            SELECT
                SUM(IIF(c.CandidatMoyenne >= 0 AND c.CandidatMoyenne < 5, 1, 0))  AS Tranche_0_5,
                SUM(IIF(c.CandidatMoyenne >= 5 AND c.CandidatMoyenne < 6, 1, 0))  AS Tranche_5_6,
                SUM(IIF(c.CandidatMoyenne >= 6 AND c.CandidatMoyenne < 7, 1, 0))  AS Tranche_6_7,
                SUM(IIF(c.CandidatMoyenne >= 7 AND c.CandidatMoyenne < 8, 1, 0))  AS Tranche_7_8,
                SUM(IIF(c.CandidatMoyenne >= 8 AND c.CandidatMoyenne < 9, 1, 0))  AS Tranche_8_9,
                SUM(IIF(c.CandidatMoyenne >= 9 AND c.CandidatMoyenne <= 10, 1, 0)) AS Tranche_9_10
            FROM Candidat c";
        var raw = ExecuteQuery(sql);
        return PivotScoreDistribution(raw);
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    // Runs sql via OleDbDataAdapter; on error returns single-row table with "Error" column.
    private DataTable ExecuteQuery(string sql)
    {
        var dt = new DataTable();
        try
        {
            using var adapter = new OleDbDataAdapter(sql, _db.GetConnection());
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            dt.Columns.Add("Error");
            dt.Rows.Add(ex.Message);
        }
        return dt;
    }

    // Keeps top N rows per FiliereNom (Access SQL has no window functions).
    private static DataTable FilterTopNPerFiliere(DataTable source, int topN)
    {
        var result = source.Clone();
        var counts = new Dictionary<string, int>();
        foreach (DataRow row in source.Rows)
        {
            var filiere = row["FiliereNom"]?.ToString() ?? "";
            counts.TryGetValue(filiere, out int n);
            if (n < topN) { result.ImportRow(row); counts[filiere] = n + 1; }
        }
        return result;
    }

    // Pivots single-row score aggregate into (Tranche, NbCandidats) rows.
    private static DataTable PivotScoreDistribution(DataTable raw)
    {
        var result = new DataTable();
        result.Columns.Add("Tranche", typeof(string));
        result.Columns.Add("NbCandidats", typeof(int));
        if (raw.Rows.Count == 0 || raw.Columns.Contains("Error")) return result;

        var row = raw.Rows[0];
        foreach (var (label, col) in new[]
        {
            ("0 – 5",  "Tranche_0_5"),  ("5 – 6",  "Tranche_5_6"),
            ("6 – 7",  "Tranche_6_7"),  ("7 – 8",  "Tranche_7_8"),
            ("8 – 9",  "Tranche_8_9"),  ("9 – 10", "Tranche_9_10"),
        })
        {
            result.Rows.Add(label, raw.Columns.Contains(col) ? Convert.ToInt32(row[col]) : 0);
        }
        return result;
    }
}
