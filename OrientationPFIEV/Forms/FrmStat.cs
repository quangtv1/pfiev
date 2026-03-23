using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// General statistics: shows full results table (all candidates with filiere).
/// Useful as a quick overview of the attribution outcome.
/// </summary>
public partial class FrmStat : Form
{
    private readonly ResultatsRepository _repo;

    public FrmStat()
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
        Text = L.Get("TitleStat");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetResultats();
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("CandidatID"))
                dgvStat.Columns["CandidatID"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, L.Get("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object s, EventArgs e) => Close();
}
