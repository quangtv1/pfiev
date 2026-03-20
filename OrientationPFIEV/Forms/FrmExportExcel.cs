using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Services;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Lets the user choose between exporting the candidate list or the
/// attribution results, then saves to an .xlsx file via ExcelService.
/// </summary>
public partial class FrmExportExcel : Form
{
    public FrmExportExcel()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
    }

    // ── Localization ──────────────────────────────────────────────────────────

    private void ApplyLocalization()
    {
        Text                       = L.Get("TitleExportExcel");
        grpChoix.Text              = L.Get("TitleExportExcel");
        radioExportCandidats.Text  = L.Get("OptExportCandidats");
        radioExportResultats.Text  = L.Get("OptExportResultats");
        btnExport.Text             = L.Get("BtnExport");
        btnFermer.Text             = L.Get("BtnClose");
    }

    // ── Event handlers ────────────────────────────────────────────────────────

    private void btnExport_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Filter      = "Excel|*.xlsx",
            DefaultExt  = "xlsx",
            FileName    = radioExportCandidats.Checked
                ? "CandidatsPFIEV.xlsx"
                : "ResultatsPFIEV.xlsx"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            if (radioExportCandidats.Checked)
            {
                var dt = new CandidatRepository(SessionDatabase.Context).GetSessionView();
                ExcelService.ExportCandidats(dt, dlg.FileName);
            }
            else
            {
                var dt = new ResultatsRepository(SessionDatabase.Context).GetResultats();
                ExcelService.ExportResultats(dt, dlg.FileName);
            }

            MessageBox.Show(
                L.Get("MsgExportSuccess"),
                L.Get("AppTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"{L.Get("MsgImportError")}: {ex.Message}",
                L.Get("AppTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnFermer_Click(object sender, EventArgs e) => Close();
}
