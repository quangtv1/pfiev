using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>CRUD operations for the Etablissement table in CONFIG.MDB.</summary>
public class EtablissementRepository
{
    private readonly DatabaseContext _ctx;
    public EtablissementRepository(DatabaseContext ctx) => _ctx = ctx;

    public List<Etablissement> GetAll()
    {
        var result = new List<Etablissement>();
        using var cmd = new OleDbCommand("SELECT * FROM Etablissement ORDER BY EtabID", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));
        return result;
    }

    public void Add(Etablissement e)
    {
        using var cmd = new OleDbCommand(
            "INSERT INTO Etablissement (EtabNom, EtabCode, EtabVille) VALUES (?, ?, ?)",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", e.EtabNom);
        cmd.Parameters.AddWithValue("code", e.EtabCode);
        cmd.Parameters.AddWithValue("ville", e.EtabVille);
        cmd.ExecuteNonQuery();
    }

    public void Update(Etablissement e)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Etablissement SET EtabNom=?, EtabCode=?, EtabVille=? WHERE EtabID=?",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", e.EtabNom);
        cmd.Parameters.AddWithValue("code", e.EtabCode);
        cmd.Parameters.AddWithValue("ville", e.EtabVille);
        cmd.Parameters.AddWithValue("id", e.EtabID);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var cmd = new OleDbCommand("DELETE FROM Etablissement WHERE EtabID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    private static Etablissement Map(OleDbDataReader r) => new()
    {
        EtabID = r.GetInt32(r.GetOrdinal("EtabID")),
        EtabNom = r.IsDBNull(r.GetOrdinal("EtabNom")) ? "" : r.GetString(r.GetOrdinal("EtabNom")),
        EtabCode = r.IsDBNull(r.GetOrdinal("EtabCode")) ? "" : r.GetString(r.GetOrdinal("EtabCode")),
        EtabVille = r.IsDBNull(r.GetOrdinal("EtabVille")) ? "" : r.GetString(r.GetOrdinal("EtabVille")),
    };
}
