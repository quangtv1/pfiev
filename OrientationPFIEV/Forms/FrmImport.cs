using System.Globalization;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;
using OrientationPFIEV.Services;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Imports candidates + notes from an Excel file (.xlsx only).
/// On success sets DialogResult = OK so the caller can refresh its grid.
/// </summary>
public partial class FrmImport : Form
{
    private string? _filePath;

    public FrmImport()
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
        Text           = L.Get("TitleImport");
        btnBrowse.Text = L.Get("BtnBrowseFile");
        btnImport.Text = L.Get("BtnImport");
        btnFermer.Text = L.Get("BtnClose");
    }

    // ── Event handlers ────────────────────────────────────────────────────────

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx",
            Title  = L.Get("TitleSelectExcelFile")
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        _filePath    = dlg.FileName;
        txtPath.Text = _filePath;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
        {
            MessageBox.Show(L.Get("MsgNoFileSelected"), L.Get("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!_filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(L.Get("MsgXlsxOnly"), L.Get("AppTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var rows        = ExcelService.ImportCandidats(_filePath);
            var candidatRepo = new CandidatRepository(SessionDatabase.Context);
            var etabRepo    = new EtablissementRepository(ConfigDatabase.Context);
            var matiereRepo = new MatiereRepository(ConfigDatabase.Context);
            var noteRepo    = new NoteRepository(SessionDatabase.Context);

            var etabs    = etabRepo.GetAll();
            var matieres = matiereRepo.GetAll();

            int imported = 0, skipped = 0;

            foreach (var row in rows)
            {
                // Skip rows with no last name
                if (string.IsNullOrWhiteSpace(row.Nom)) { skipped++; continue; }

                // Resolve EtabID from EtabCode; 0 if not found
                var etab  = etabs.FirstOrDefault(et => et.EtabCode == row.EtabCode);
                int etabId = etab?.EtabID ?? 0;

                var candidat = new Candidat
                {
                    Nom              = row.Nom,
                    NomIntermediaire = row.NomIntermediaire,
                    Prenom           = row.Prenom,
                    Sexe             = row.Sexe,
                    CandidatStatut   = row.Statut,
                    Langue           = row.Langue,
                    EtabID           = etabId
                };

                // Parse date dd/MM/yyyy
                if (DateTime.TryParseExact(row.DateDeNaissance, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob))
                    candidat.DateDeNaissance = dob;

                candidatRepo.Add(candidat);
                int newId = candidatRepo.GetLastInsertedId();

                // Import notes mapped by matiere order (col 10, 11, … → matiere[0], [1], …)
                for (int i = 0; i < row.Notes.Count && i < matieres.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(row.Notes[i]))
                        noteRepo.Upsert(new Note
                        {
                            CandidatID = newId,
                            MatiereID  = matieres[i].MatiereID,
                            NoteValue  = row.Notes[i]
                        });
                }
                imported++;
            }

            MessageBox.Show(
                string.Format(L.Get("MsgImportSuccess"), imported, skipped),
                L.Get("AppTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
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
