namespace OrientationPFIEV.Data;

/// <summary>
/// Static singleton wrapper for CONFIG.MDB (global config — fixed path, always open).
/// Call Initialize() once at application startup.
/// </summary>
public static class ConfigDatabase
{
    private static DatabaseContext? _ctx;

    public static void Initialize(string configMdbPath)
    {
        _ctx?.Dispose();
        _ctx = new DatabaseContext(configMdbPath);
    }

    /// <summary>Returns the active DatabaseContext. Throws if not initialized.</summary>
    public static DatabaseContext Context
        => _ctx ?? throw new InvalidOperationException("ConfigDatabase not initialized. Call Initialize() first.");
}
