using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>CRUD operations for the Filiere table in CONFIG.MDB.</summary>
public class FiliereRepository
{
    private readonly DatabaseContext _ctx;
    public FiliereRepository(DatabaseContext ctx) => _ctx = ctx;

    public List<Filiere> GetAll()
    {
        var result = new List<Filiere>();
        using var cmd = new OleDbCommand("SELECT * FROM Filiere ORDER BY FiliereID", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));
        return result;
    }

    public Filiere? GetById(int id)
    {
        using var cmd = new OleDbCommand("SELECT * FROM Filiere WHERE FiliereID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public void Add(Filiere f)
    {
        using var cmd = new OleDbCommand(
            "INSERT INTO Filiere (FiliereNom, FiliereCode, FiliereNbPlace, EtabID) VALUES (?, ?, ?, ?)",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", f.FiliereNom);
        cmd.Parameters.AddWithValue("code", f.FiliereCode);
        cmd.Parameters.AddWithValue("nbplace", f.FiliereNbPlace);
        cmd.Parameters.AddWithValue("etab", f.EtabID);
        cmd.ExecuteNonQuery();
    }

    public void Update(Filiere f)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Filiere SET FiliereNom=?, FiliereCode=?, FiliereNbPlace=?, EtabID=? WHERE FiliereID=?",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", f.FiliereNom);
        cmd.Parameters.AddWithValue("code", f.FiliereCode);
        cmd.Parameters.AddWithValue("nbplace", f.FiliereNbPlace);
        cmd.Parameters.AddWithValue("etab", f.EtabID);
        cmd.Parameters.AddWithValue("id", f.FiliereID);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var cmd = new OleDbCommand("DELETE FROM Filiere WHERE FiliereID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Sets FiliereNbPlace to <paramref name="nbPlace"/> for all filières.</summary>
    public void UpdateAllSlots(int nbPlace)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Filiere SET FiliereNbPlace = ?",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("n", nbPlace);
        cmd.ExecuteNonQuery();
    }

    private static Filiere Map(OleDbDataReader r) => new()
    {
        FiliereID = r.GetInt32(r.GetOrdinal("FiliereID")),
        FiliereNom = r.IsDBNull(r.GetOrdinal("FiliereNom")) ? "" : r.GetString(r.GetOrdinal("FiliereNom")),
        FiliereCode = r.IsDBNull(r.GetOrdinal("FiliereCode")) ? "" : r.GetString(r.GetOrdinal("FiliereCode")),
        FiliereNbPlace = r.IsDBNull(r.GetOrdinal("FiliereNbPlace")) ? 0 : r.GetInt32(r.GetOrdinal("FiliereNbPlace")),
        EtabID = r.IsDBNull(r.GetOrdinal("EtabID")) ? 0 : r.GetInt32(r.GetOrdinal("EtabID")),
    };
}
