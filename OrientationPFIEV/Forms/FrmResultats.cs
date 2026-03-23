using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Services;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Main results form: displays all candidates with their assigned filiere.
/// Entry point after attribution algorithm completes (Phase 05).
/// readOnly=true hides Export button when opened from history view.
/// </summary>
public partial class FrmResultats : Form
{
    private readonly bool _readOnly;
    private readonly ResultatsRepository _repo;

    public FrmResultats(bool readOnly = false)
    {
        InitializeComponent();
        _readOnly = readOnly;
        _repo = new ResultatsRepository(SessionDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadData();
        btnExport.Enabled = !_readOnly;
    }

    // ── Localization ─────────────────────────────────────────────────────────

    private void ApplyLocalization()
    {
        Text = L.Get("TitleResultats");
        btnExport.Text = L.Get("BtnExport");
        btnTableaux.Text = L.Get("BtnTableaux");
        btnRetour.Text = L.Get("BtnRetour");
    }

    // ── Data ─────────────────────────────────────────────────────────────────

    private void LoadData()
    {
        try
        {
            dgvResultats.DataSource = _repo.GetResultats();
            ConfigureGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ConfigureGrid()
    {
        dgvResultats.ReadOnly = true;
        dgvResultats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvResultats.AllowUserToAddRows = false;
        dgvResultats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        if (dgvResultats.Columns.Contains("CandidatID"))
            dgvResultats.Columns["CandidatID"].Visible = false;

        // Set localised column headers
        SetColumnHeader("Nom",               L.Get("ColNom"));
        SetColumnHeader("NomIntermediaire",  L.Get("ColNom"));
        SetColumnHeader("Prenom",            L.Get("ColPrenom"));
        SetColumnHeader("CandidatStatut",    L.Get("ColStatut"));
        SetColumnHeader("CandidatMoyenne",   L.Get("ColMoyenne"));
        SetColumnHeader("CandidatClassement", L.Get("ColClassement"));
        SetColumnHeader("FiliereCode",       L.Get("ColFiliere"));
        SetColumnHeader("FiliereNom",        L.Get("ColFiliereAdmis"));
        SetColumnHeader("EtabFiliere",       L.Get("ColEtabFiliere"));
    }

    private void SetColumnHeader(string colName, string header)
    {
        if (dgvResultats.Columns.Contains(colName))
            dgvResultats.Columns[colName].HeaderText = header;
    }

    // ── Event handlers ───────────────────────────────────────────────────────

    private void btnExport_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel|*.xlsx",
            FileName = "ResultatsPFIEV.xlsx"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            ExcelService.ExportResultats(_repo.GetResultats(), dlg.FileName);
            MessageBox.Show(
                L.Get("MsgExportDone"),
                L.Get("AppTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnTableaux_Click(object sender, EventArgs e)
        => new FrmTableauRecap().Show();

    private void btnRetour_Click(object sender, EventArgs e) => Close();
}
