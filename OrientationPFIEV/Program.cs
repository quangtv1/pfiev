using OrientationPFIEV.Helpers;

namespace OrientationPFIEV;

/// <summary>
/// Application entry point.
/// Initialises visual styles, sets default language, then opens the splash/launcher form.
/// FrmAccueil is created in Phase 03 — placeholder reference kept here.
/// </summary>
internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Default language is French; user can switch to Vietnamese on the launcher.
        L.SetLanguage(AppState.Language);

        // Initialize CONFIG.MDB from the application directory (next to the .exe).
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CONFIG.MDB");
        if (File.Exists(configPath))
            Data.ConfigDatabase.Initialize(configPath);

        Application.Run(new Forms.FrmAccueil());
    }
}
