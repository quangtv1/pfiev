namespace OrientationPFIEV.Data;

/// <summary>
/// Static singleton wrapper for the per-session .mdb file chosen by the user.
/// Call Open() when a session is loaded or created; Close() when it ends.
/// </summary>
public static class SessionDatabase
{
    private static DatabaseContext? _ctx;

    /// <summary>Opens a new session database, closing any previous one.</summary>
    public static void Open(string sessionMdbPath)
    {
        _ctx?.Dispose();
        _ctx = new DatabaseContext(sessionMdbPath);
        AppState.DatabasePath = sessionMdbPath;
        AppState.SessionInProgress = true;
    }

    /// <summary>Closes and disposes the current session database.</summary>
    public static void Close()
    {
        _ctx?.Dispose();
        _ctx = null;
        AppState.SessionInProgress = false;
    }

    /// <summary>Returns the active DatabaseContext. Throws if no session is open.</summary>
    public static DatabaseContext Context
        => _ctx ?? throw new InvalidOperationException("No session open. Call SessionDatabase.Open() first.");

    /// <summary>True when a session database is currently open.</summary>
    public static bool IsOpen => _ctx != null;
}
