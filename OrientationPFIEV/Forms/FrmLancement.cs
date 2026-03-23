using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Launcher form: lets the user choose between configuring the app,
/// starting a new session, or resuming an existing session from history.
/// Opened after language selection in FrmSelectLangue.
/// </summary>
public partial class FrmLancement : Form
{
    private List<string> _recentPaths = new();
    private bool _navigating = false; // true when transitioning to FrmSession (suppress Application.Exit)

    public FrmLancement()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadRecentPaths();
        // Default: session existante (requires path selection)
        radioSessionExistante.Checked = true;
    }

    // ── Localization ───────────────────────────────────────────────────────────

    private void ApplyLocalization()
    {
        Text = L.Get("AppTitle");
        groupBoxLaunch.Text = L.Get("FrameLaunch");
        radioParametrage.Text = L.Get("OptParametrer");
        radioNouvelleSession.Text = L.Get("OptNewSession");
        radioSessionExistante.Text = L.Get("OptExistingSession");
        btnOk.Text = L.Get("BtnOk");
        btnEffacer.Text = L.Get("BtnClearHistory");
        btnQuitter.Text = L.Get("BtnQuit");
    }

    // ── Data ───────────────────────────────────────────────────────────────────

    private void LoadRecentPaths()
    {
        try
        {
            _recentPaths = new DataPathRepository(ConfigDatabase.Context).GetAll();
        }
        catch
        {
            // CONFIG.MDB not yet initialized — show empty list
            _recentPaths = new List<string>();
        }

        comboBoxChemin.DataSource = null;
        comboBoxChemin.DataSource = _recentPaths;
        comboBoxChemin.Enabled = radioSessionExistante.Checked;
    }

    // ── Event handlers ─────────────────────────────────────────────────────────

    private void radio_CheckedChanged(object sender, EventArgs e)
    {
        comboBoxChemin.Enabled = radioSessionExistante.Checked;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (radioParametrage.Checked)
        {
            AppState.IsFirstOpen = true;
            new FrmParametrage().ShowDialog(this);
            // Do not close — user returns to this launcher after configuring
        }
        else if (radioNouvelleSession.Checked)
        {
            var dlg = new FrmOuvertureSE();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _navigating = true;
                Close();
            }
        }
        else // Session existante
        {
            if (comboBoxChemin.SelectedItem is not string path || !File.Exists(path))
            {
                MessageBox.Show(L.Get("MsgInvalidPath"), L.Get("AppTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SessionDatabase.Open(path);
                _navigating = true;
                new FrmSession().Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, L.Get("AppTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnEffacer_Click(object sender, EventArgs e)
    {
        var confirm = MessageBox.Show(
            L.Get("MsgConfirmClearHistory"),
            L.Get("AppTitle"),
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            new DataPathRepository(ConfigDatabase.Context).ClearAll();
            _recentPaths.Clear();
            comboBoxChemin.DataSource = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnQuitter_Click(object sender, EventArgs e) => Application.Exit();

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        if (!_navigating) Application.Exit(); // user closed window without navigating → quit
    }
}
