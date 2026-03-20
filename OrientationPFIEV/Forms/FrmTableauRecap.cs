using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Recap tables form: two DataGridViews (Internes top, Externes bottom).
/// Navigation buttons open each individual statistics form.
/// </summary>
public partial class FrmTableauRecap : Form
{
    private readonly ResultatsRepository _repo;

    public FrmTableauRecap()
    {
        InitializeComponent();
        _repo = new ResultatsRepository(SessionDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadData();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleTableauRecap");
        lblInternes.Text = "Internes";
        lblExternes.Text = "Externes";
        btnStat.Text        = "Statistiques";
        btnStat1.Text       = "Stat. Langue";
        btnStat6.Text       = "Top 6";
        btnStatEtab.Text    = "Stat. Etab.";
        btnStatMoyenne.Text = "Distribution";
        btnMoyenneSpe.Text  = "Moy. Spé";
        btnGraphe.Text      = "Graphiques";
        btnClose.Text       = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvInternes.DataSource = _repo.GetTableauRecapInternes();
            dgvExternes.DataSource = _repo.GetTableauRecapExternes();
            ConfigureGrid(dgvInternes);
            ConfigureGrid(dgvExternes);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void ConfigureGrid(System.Windows.Forms.DataGridView dgv)
    {
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows    = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        dgv.RowHeadersVisible = false;
        if (dgv.Columns.Contains("CandidatID"))
            dgv.Columns["CandidatID"].Visible = false;
    }

    // ── Navigation buttons ───────────────────────────────────────────────────

    private void btnStat_Click(object s, EventArgs e)        => new FrmStat().Show();
    private void btnStat1_Click(object s, EventArgs e)       => new FrmStat1().Show();
    private void btnStat6_Click(object s, EventArgs e)       => new FrmStat6().Show();
    private void btnStatEtab_Click(object s, EventArgs e)    => new FrmStatEtab().Show();
    private void btnStatMoyenne_Click(object s, EventArgs e) => new FrmStatMoyenne().Show();
    private void btnMoyenneSpe_Click(object s, EventArgs e)  => new FrmMoyenneSpe().Show();
    private void btnGraphe_Click(object s, EventArgs e)      => new FrmResultatsGraphe().Show();
    private void btnClose_Click(object s, EventArgs e)       => Close();
}
