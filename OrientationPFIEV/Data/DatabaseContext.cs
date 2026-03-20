using System.Data;
using System.Data.OleDb;

namespace OrientationPFIEV.Data;

/// <summary>
/// OleDb connection factory wrapping a single open connection to an Access .mdb file.
/// Uses Jet 4.0 driver (Access 97-2003). For .accdb or newer .mdb, switch to ACE 12.0.
/// </summary>
public class DatabaseContext : IDisposable
{
    private readonly string _connectionString;
    private OleDbConnection? _connection;

    public DatabaseContext(string mdbPath)
    {
        // Jet 4.0 for .mdb (Access 97-2003). ACE fallback:
        // $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath};"
        _connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};";
    }

    /// <summary>Returns the open connection, opening it if necessary.</summary>
    public OleDbConnection GetConnection()
    {
        _connection ??= new OleDbConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _connection = null;
    }
}
