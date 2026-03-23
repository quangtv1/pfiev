using OrientationPFIEV.Forms;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Splash screen shown at application startup.
/// Displays logo and a progress bar for ~1.5 seconds, then opens FrmSelectLangue.
/// BorderStyle=None, no taskbar entry, centered on screen.
/// </summary>
public partial class FrmAccueil : Form
{
    private int _tick = 0;
    private const int MaxTicks = 15;

    public FrmAccueil()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        LoadLogo();
        timer1.Start();
    }

    /// <summary>Loads logo.jpg at runtime with fallback to solid background.</summary>
    private void LoadLogo()
    {
        try
        {
            var logoPath = Path.Combine(AppContext.BaseDirectory, "Resources", "logo.jpg");
            if (File.Exists(logoPath))
                pictureBoxLogo.Image = Image.FromFile(logoPath);
            else
                BackColor = Color.FromArgb(0, 82, 147); // PFIEV blue fallback
        }
        catch
        {
            BackColor = Color.FromArgb(0, 82, 147);
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        progressBar1.Value = ++_tick;
        if (_tick >= MaxTicks)
        {
            timer1.Stop();
            new FrmSelectLangue().Show();
            Hide(); // Don't Close() — FrmAccueil is the Application.Run main form; closing it exits the app
        }
    }
}
