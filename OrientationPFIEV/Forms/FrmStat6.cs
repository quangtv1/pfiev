using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Top 6 candidates per specialty, ordered by ranking.
/// Bound to ResultatsRepository.GetStatTopPerFiliere(6).
/// </summary>
public partial class FrmStat6 : Form
{
    private readonly ResultatsRepository _repo;

    public FrmStat6()
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
        Text = L.Get("TitleStat6");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetStatTopPerFiliere(6);
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("FiliereNom"))
                dgvStat.Columns["FiliereNom"].HeaderText = L.Get("ColFiliere");
            if (dgvStat.Columns.Contains("CandidatMoyenne"))
                dgvStat.Columns["CandidatMoyenne"].HeaderText = L.Get("ColMoyenne");
            if (dgvStat.Columns.Contains("CandidatClassement"))
                dgvStat.Columns["CandidatClassement"].HeaderText = L.Get("ColClassement");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object s, EventArgs e) => Close();
}
