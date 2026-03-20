namespace OrientationPFIEV;

/// <summary>
/// Global application state shared across all forms.
/// Holds language preference, database paths, and session flags.
/// </summary>
public static class AppState
{
    /// <summary>Current UI language: "fr" (French) or "vi" (Vietnamese).</summary>
    public static string Language { get; set; } = "fr";

    /// <summary>Path to the main session Access (.mdb) database.</summary>
    public static string? DatabasePath { get; set; }

    /// <summary>Path to the configuration Access database (CONFIG.MDB).</summary>
    public static string? ConfigDbPath { get; set; }

    /// <summary>True on first application open before any session is created.</summary>
    public static bool IsFirstOpen { get; set; } = true;

    /// <summary>True when a session is currently active (data processing in progress).</summary>
    public static bool SessionInProgress { get; set; } = false;

    /// <summary>Maximum number of filiere choices a candidate may submit.</summary>
    public static int MaxChoices { get; set; } = 3;
}
