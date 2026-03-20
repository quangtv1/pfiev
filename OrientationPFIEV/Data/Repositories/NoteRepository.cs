using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>CRUD operations for the Note table in the session .mdb.</summary>
public class NoteRepository
{
    private readonly DatabaseContext _ctx;
    public NoteRepository(DatabaseContext ctx) => _ctx = ctx;

    /// <summary>Returns all notes for a given candidate.</summary>
    public List<Note> GetByCandidatId(int candidatId)
    {
        var result = new List<Note>();
        using var cmd = new OleDbCommand(
            "SELECT * FROM Note WHERE CandidatID=? ORDER BY MatiereID",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("cid", candidatId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));
        return result;
    }

    /// <summary>
    /// Insert or update a note. Updates existing row if NoteID is found
    /// for (CandidatID, MatiereID), otherwise inserts.
    /// </summary>
    public void Upsert(Note n)
    {
        // Check for existing note
        int existingId = 0;
        using (var chk = new OleDbCommand(
            "SELECT NoteID FROM Note WHERE CandidatID=? AND MatiereID=?",
            _ctx.GetConnection()))
        {
            chk.Parameters.AddWithValue("cid", n.CandidatID);
            chk.Parameters.AddWithValue("mid", n.MatiereID);
            var val = chk.ExecuteScalar();
            if (val != null && val != DBNull.Value)
                existingId = Convert.ToInt32(val);
        }

        if (existingId > 0)
        {
            using var upd = new OleDbCommand(
                "UPDATE Note SET Note=? WHERE NoteID=?",
                _ctx.GetConnection());
            upd.Parameters.AddWithValue("val", n.NoteValue);
            upd.Parameters.AddWithValue("nid", existingId);
            upd.ExecuteNonQuery();
        }
        else
        {
            using var ins = new OleDbCommand(
                "INSERT INTO Note (CandidatID, MatiereID, Note) VALUES (?, ?, ?)",
                _ctx.GetConnection());
            ins.Parameters.AddWithValue("cid", n.CandidatID);
            ins.Parameters.AddWithValue("mid", n.MatiereID);
            ins.Parameters.AddWithValue("val", n.NoteValue);
            ins.ExecuteNonQuery();
        }
    }

    /// <summary>Deletes all notes belonging to a candidate.</summary>
    public void DeleteByCandidatId(int candidatId)
    {
        using var cmd = new OleDbCommand("DELETE FROM Note WHERE CandidatID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("cid", candidatId);
        cmd.ExecuteNonQuery();
    }

    private static Note Map(OleDbDataReader r) => new()
    {
        NoteID = r.GetInt32(r.GetOrdinal("NoteID")),
        CandidatID = r.GetInt32(r.GetOrdinal("CandidatID")),
        MatiereID = r.GetInt32(r.GetOrdinal("MatiereID")),
        // Note column stores the value; keep as string to preserve locale-specific decimals
        NoteValue = r.IsDBNull(r.GetOrdinal("Note")) ? "" : r.GetValue(r.GetOrdinal("Note")).ToString()!,
    };
}
