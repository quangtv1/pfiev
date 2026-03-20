using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Statistics by candidate language: count per Langue value.
/// Bound to ResultatsRepository.GetStatByLanguage().
/// </summary>
public partial class FrmStat1 : Form
{
    private readonly ResultatsRepository _repo;

    public FrmStat1()
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
        Text = L.Get("TitleStat1");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetStatByLanguage();
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("Langue"))
                dgvStat.Columns["Langue"].HeaderText = L.Get("LblLanguage");
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
