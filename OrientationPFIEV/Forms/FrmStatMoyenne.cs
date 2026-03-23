using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Score distribution: count of candidates per average score range (0-5, 5-6 … 9-10).
/// Bound to ResultatsRepository.GetScoreDistribution() (pivoted two-column table).
/// </summary>
public partial class FrmStatMoyenne : Form
{
    private readonly ResultatsRepository _repo;

    public FrmStatMoyenne()
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
        Text = L.Get("TitleStatMoyenne");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetScoreDistribution();
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("Tranche"))
                dgvStat.Columns["Tranche"].HeaderText = L.Get("ColMoyenne");
            if (dgvStat.Columns.Contains("NbCandidats"))
                dgvStat.Columns["NbCandidats"].HeaderText = L.Get("ColNbAdmis");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object s, EventArgs e) => Close();
}
