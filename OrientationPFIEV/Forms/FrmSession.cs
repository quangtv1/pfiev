using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Main session form. Shows candidate list in a DataGridView with full
/// CRUD, note entry, choice priority, attribution launch and results.
/// </summary>
public partial class FrmSession : Form
{
    private readonly CandidatRepository _candidatRepo;
    private readonly ConcoursRepository _concoursRepo;

    public FrmSession()
    {
        InitializeComponent();
        _candidatRepo = new CandidatRepository(SessionDatabase.Context);
        _concoursRepo = new ConcoursRepository(SessionDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadConcoursInfo();
        LoadCandidats();
    }

    // ── Localization ───────────────────────────────────────────────────────────

    private void ApplyLocalization()
    {
        Text = L.Get("TitleSession");
        grpConcours.Text      = L.Get("TitleParamConcours");
        lblAnneeCaption.Text  = L.Get("LblAnnee") + " :";
        lblMoyMinCaption.Text = L.Get("LblMoyMin") + " :";
        btnAjouter.Text           = L.Get("BtnAjouter");
        btnModifier.Text          = L.Get("BtnModifier");
        btnEffacer.Text           = L.Get("BtnEffacer");
        btnFixerNotes.Text        = L.Get("BtnFixerNotes");
        btnFixerChoix.Text        = L.Get("BtnFixerChoix");
        btnParamConcours.Text     = L.Get("BtnParamConcours");
        btnImporterExcel.Text     = L.Get("BtnImport");
        btnExporterExcel.Text     = L.Get("BtnExport");
        btnLancerTraitement.Text  = L.Get("BtnLancerTraitement");
        btnVisualiserResultats.Text = L.Get("BtnVisualiserResultats");
        btnQuitter.Text           = L.Get("BtnQuitterSession");
    }

    // ── Data loading ───────────────────────────────────────────────────────────

    private void LoadConcoursInfo()
    {
        var c = _concoursRepo.Get();
        if (c == null) return;
        lblAnneeValue.Text  = c.Annee.ToString();
        lblMoyMinValue.Text = c.MoyenneMin.ToString("G");
    }

    private void LoadCandidats()
    {
        var dt = _candidatRepo.GetSessionView();
        dgvCandidats.DataSource = dt;
        ConfigureGrid();
    }

    private void ConfigureGrid()
    {
        if (dgvCandidats.Columns.Count == 0) return;

        // Hide internal key columns
        if (dgvCandidats.Columns.Contains("CandidatID"))
            dgvCandidats.Columns["CandidatID"].Visible = false;
        if (dgvCandidats.Columns.Contains("EtabID"))
            dgvCandidats.Columns["EtabID"].Visible = false;
        if (dgvCandidats.Columns.Contains("CandidatMoyenne"))
            dgvCandidats.Columns["CandidatMoyenne"].HeaderText = L.Get("ColMoyenne");
        if (dgvCandidats.Columns.Contains("CandidatClassement"))
            dgvCandidats.Columns["CandidatClassement"].HeaderText = L.Get("ColClassement");
        if (dgvCandidats.Columns.Contains("Nom"))
            dgvCandidats.Columns["Nom"].HeaderText = L.Get("ColNom");
        if (dgvCandidats.Columns.Contains("Prenom"))
            dgvCandidats.Columns["Prenom"].HeaderText = L.Get("ColPrenom");
        if (dgvCandidats.Columns.Contains("CandidatStatut"))
            dgvCandidats.Columns["CandidatStatut"].HeaderText = L.Get("ColStatut");
        if (dgvCandidats.Columns.Contains("anonymat"))
            dgvCandidats.Columns["anonymat"].HeaderText = L.Get("ColAnonymat");
    }

    private int? GetSelectedCandidatId()
    {
        if (dgvCandidats.SelectedRows.Count == 0) return null;
        var cell = dgvCandidats.SelectedRows[0].Cells["CandidatID"];
        if (cell.Value == null || cell.Value == DBNull.Value) return null;
        return Convert.ToInt32(cell.Value);
    }

    // ── Right-panel button handlers ────────────────────────────────────────────

    private void btnAjouter_Click(object sender, EventArgs e)
    {
        using var frm = new FrmAjouterCandidat(_candidatRepo);
        if (frm.ShowDialog() == DialogResult.OK) LoadCandidats();
    }

    private void btnModifier_Click(object sender, EventArgs e)
    {
        if (GetSelectedCandidatId() is not int id) return;
        using var frm = new FrmAjouterCandidat(_candidatRepo, id);
        if (frm.ShowDialog() == DialogResult.OK) LoadCandidats();
    }

    private void btnEffacer_Click(object sender, EventArgs e)
    {
        if (GetSelectedCandidatId() is not int id) return;
        if (MessageBox.Show(L.Get("MsgConfirmDelete"), "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            _candidatRepo.Delete(id);
            LoadCandidats();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnFixerNotes_Click(object sender, EventArgs e)
    {
        if (GetSelectedCandidatId() is not int id) return;
        using var frm = new FrmNoteMatiere(id, SessionDatabase.Context);
        frm.ShowDialog();
        LoadCandidats();
    }

    private void btnFixerChoix_Click(object sender, EventArgs e)
    {
        if (GetSelectedCandidatId() is not int id) return;
        using var frm = new FrmFixerChoix(id, SessionDatabase.Context);
        frm.ShowDialog();
        LoadCandidats();
    }

    private void btnParamConcours_Click(object sender, EventArgs e)
    {
        using var frm = new FrmParamConcours(SessionDatabase.Context);
        if (frm.ShowDialog() == DialogResult.OK) LoadConcoursInfo();
    }

    // ── Bottom-panel button handlers ───────────────────────────────────────────

    private void btnImporterExcel_Click(object sender, EventArgs e)
    {
        using var frm = new FrmImport();
        if (frm.ShowDialog() == DialogResult.OK) LoadCandidats();
    }

    private void btnExporterExcel_Click(object sender, EventArgs e)
    {
        using var frm = new FrmExportExcel();
        frm.ShowDialog();
    }

    private void btnLancerTraitement_Click(object sender, EventArgs e)
    {
        try
        {
            new ChoixRepository(SessionDatabase.Context).ResetAdmis();
            _candidatRepo.ComputeAveragesAndRanking(ConfigDatabase.Context);
            Services.AttributionService.Run(SessionDatabase.Context);
            new FrmResultats().Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnVisualiserResultats_Click(object sender, EventArgs e)
    {
        new FrmResultats(readOnly: true).Show();
    }

    private void btnQuitter_Click(object sender, EventArgs e)
    {
        SessionDatabase.Close();
        new FrmLancement().Show();
        Close();
    }
}
