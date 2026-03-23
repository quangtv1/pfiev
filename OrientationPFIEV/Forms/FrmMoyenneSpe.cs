using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Average score per specialty with admitted count.
/// Bound to ResultatsRepository.GetClassementMoyenneSpe().
/// </summary>
public partial class FrmMoyenneSpe : Form
{
    private readonly ResultatsRepository _repo;

    public FrmMoyenneSpe()
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
        Text = L.Get("TitleMoyenneSpe");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetClassementMoyenneSpe();
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("FiliereNom"))
                dgvStat.Columns["FiliereNom"].HeaderText = L.Get("ColFiliere");
            if (dgvStat.Columns.Contains("MoyenneSpe"))
                dgvStat.Columns["MoyenneSpe"].HeaderText = L.Get("ColMoyenneSpe");
            if (dgvStat.Columns.Contains("NbAdmis"))
                dgvStat.Columns["NbAdmis"].HeaderText = L.Get("ColNbAdmis");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object s, EventArgs e) => Close();
}
