using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Per-school statistics: candidate counts split by Internes/Externes.
/// Bound to ResultatsRepository.GetStatEtab().
/// </summary>
public partial class FrmStatEtab : Form
{
    private readonly ResultatsRepository _repo;

    public FrmStatEtab()
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
        Text = L.Get("TitleStatEtab");
        btnClose.Text = L.Get("BtnClose");
    }

    private void LoadData()
    {
        try
        {
            dgvStat.DataSource = _repo.GetStatEtab();
            dgvStat.ReadOnly = true;
            dgvStat.AllowUserToAddRows = false;
            dgvStat.AllowUserToDeleteRows = false;
            dgvStat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvStat.RowHeadersVisible = false;
            if (dgvStat.Columns.Contains("EtabNom"))
                dgvStat.Columns["EtabNom"].HeaderText = L.Get("LblSchool");
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
