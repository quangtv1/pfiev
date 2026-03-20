using System.Data.OleDb;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>
/// Manages the 'dataPath' table in CONFIG.MDB — a history of session .mdb paths.
/// Assumed columns: PathID (AutoNumber PK), PathValue (Text).
/// </summary>
public class DataPathRepository
{
    private readonly DatabaseContext _ctx;
    public DataPathRepository(DatabaseContext ctx) => _ctx = ctx;

    /// <summary>Returns all stored session paths.</summary>
    public List<string> GetAll()
    {
        var result = new List<string>();
        using var cmd = new OleDbCommand("SELECT PathValue FROM dataPath ORDER BY PathID", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (!reader.IsDBNull(0))
                result.Add(reader.GetString(0));
        }
        return result;
    }

    /// <summary>Adds a path if it does not already exist.</summary>
    public void Add(string path)
    {
        // Guard: skip if already stored
        using var chk = new OleDbCommand(
            "SELECT COUNT(*) FROM dataPath WHERE PathValue=?", _ctx.GetConnection());
        chk.Parameters.AddWithValue("p", path);
        int count = (int)chk.ExecuteScalar()!;
        if (count > 0) return;

        using var ins = new OleDbCommand(
            "INSERT INTO dataPath (PathValue) VALUES (?)", _ctx.GetConnection());
        ins.Parameters.AddWithValue("p", path);
        ins.ExecuteNonQuery();
    }

    /// <summary>Removes all stored session paths.</summary>
    public void ClearAll()
    {
        using var cmd = new OleDbCommand("DELETE FROM dataPath", _ctx.GetConnection());
        cmd.ExecuteNonQuery();
    }
}
