using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Language selection dialog shown after the splash screen.
/// User picks French or Vietnamese; sets AppState.Language and L culture,
/// then opens FrmLancement.
/// </summary>
public partial class FrmSelectLangue : Form
{
    public FrmSelectLangue()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        LoadFlagImages();
    }

    /// <summary>Loads flag GIFs from output directory at runtime with silent fallback.</summary>
    private void LoadFlagImages()
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var frPath = Path.Combine(baseDir, "Resources", "french.gif");
            var vnPath = Path.Combine(baseDir, "Resources", "vietnam.gif");

            if (File.Exists(frPath))
                btnFrancais.Image = Image.FromFile(frPath);

            if (File.Exists(vnPath))
                btnVietnamese.Image = Image.FromFile(vnPath);
        }
        catch
        {
            // Images are decorative — continue without them
        }
    }

    private void btnFrancais_Click(object sender, EventArgs e) => SelectLanguage("fr");
    private void btnVietnamese_Click(object sender, EventArgs e) => SelectLanguage("vi");

    private void SelectLanguage(string lang)
    {
        AppState.Language = lang;
        L.SetLanguage(lang);
        new FrmLancement().Show();
        Close();
    }
}
