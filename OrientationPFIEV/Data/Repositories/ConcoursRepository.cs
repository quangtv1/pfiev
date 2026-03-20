using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>
/// Read/write for the single-row Concours table in the session .mdb.
/// </summary>
public class ConcoursRepository
{
    private readonly DatabaseContext _ctx;
    public ConcoursRepository(DatabaseContext ctx) => _ctx = ctx;

    /// <summary>Returns the concours parameters, or null if no row exists.</summary>
    public Concours? Get()
    {
        using var cmd = new OleDbCommand("SELECT * FROM Concours", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new Concours
        {
            Annee = r_Int(reader, "Annee"),
            MoyenneMin = r_Dbl(reader, "MoyenneMin"),
            NumLigne = r_Int(reader, "NumLigne"),
        };
    }

    /// <summary>
    /// Updates Concours row. Inserts a new row if none exists,
    /// otherwise updates the first (and only) row.
    /// </summary>
    public void SetParams(Concours c)
    {
        // Check whether a row exists
        int count;
        using (var cntCmd = new OleDbCommand("SELECT COUNT(*) FROM Concours", _ctx.GetConnection()))
            count = (int)cntCmd.ExecuteScalar()!;

        if (count == 0)
        {
            using var ins = new OleDbCommand(
                "INSERT INTO Concours (Annee, MoyenneMin, NumLigne) VALUES (?, ?, ?)",
                _ctx.GetConnection());
            ins.Parameters.AddWithValue("annee", c.Annee);
            ins.Parameters.AddWithValue("min", c.MoyenneMin);
            ins.Parameters.AddWithValue("num", c.NumLigne);
            ins.ExecuteNonQuery();
        }
        else
        {
            using var upd = new OleDbCommand(
                "UPDATE Concours SET Annee=?, MoyenneMin=?, NumLigne=?",
                _ctx.GetConnection());
            upd.Parameters.AddWithValue("annee", c.Annee);
            upd.Parameters.AddWithValue("min", c.MoyenneMin);
            upd.Parameters.AddWithValue("num", c.NumLigne);
            upd.ExecuteNonQuery();
        }
    }

    private static int r_Int(OleDbDataReader r, string col)
        => r.IsDBNull(r.GetOrdinal(col)) ? 0 : r.GetInt32(r.GetOrdinal(col));

    private static double r_Dbl(OleDbDataReader r, string col)
        => r.IsDBNull(r.GetOrdinal(col)) ? 0.0 : r.GetDouble(r.GetOrdinal(col));
}
