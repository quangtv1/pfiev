using System.Data.OleDb;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Data.Repositories;

/// <summary>CRUD operations for the Matiere table in CONFIG.MDB.</summary>
public class MatiereRepository
{
    private readonly DatabaseContext _ctx;
    public MatiereRepository(DatabaseContext ctx) => _ctx = ctx;

    public List<Matiere> GetAll()
    {
        var result = new List<Matiere>();
        using var cmd = new OleDbCommand("SELECT * FROM Matiere ORDER BY MatiereID", _ctx.GetConnection());
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(Map(reader));
        return result;
    }

    public void Add(Matiere m)
    {
        using var cmd = new OleDbCommand(
            "INSERT INTO Matiere (MatiereNom, MatiereCoefficient) VALUES (?, ?)",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", m.MatiereNom);
        cmd.Parameters.AddWithValue("coeff", m.MatiereCoefficient);
        cmd.ExecuteNonQuery();
    }

    public void Update(Matiere m)
    {
        using var cmd = new OleDbCommand(
            "UPDATE Matiere SET MatiereNom=?, MatiereCoefficient=? WHERE MatiereID=?",
            _ctx.GetConnection());
        cmd.Parameters.AddWithValue("nom", m.MatiereNom);
        cmd.Parameters.AddWithValue("coeff", m.MatiereCoefficient);
        cmd.Parameters.AddWithValue("id", m.MatiereID);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var cmd = new OleDbCommand("DELETE FROM Matiere WHERE MatiereID=?", _ctx.GetConnection());
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    private static Matiere Map(OleDbDataReader r) => new()
    {
        MatiereID = r.GetInt32(r.GetOrdinal("MatiereID")),
        MatiereNom = r.IsDBNull(r.GetOrdinal("MatiereNom")) ? "" : r.GetString(r.GetOrdinal("MatiereNom")),
        MatiereCoefficient = r.IsDBNull(r.GetOrdinal("MatiereCoefficient")) ? 1.0 : r.GetDouble(r.GetOrdinal("MatiereCoefficient")),
    };
}
