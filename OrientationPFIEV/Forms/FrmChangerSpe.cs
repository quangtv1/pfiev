using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Allows manually reassigning a candidate's admitted filiere after attribution.
/// Sets the previously admitted Choix to ChoixAdmis=false and marks the new one true
/// (or inserts a new Choix row if the candidate had no entry for that filiere).
/// </summary>
public partial class FrmChangerSpe : Form
{
    private readonly CandidatRepository _candidatRepo;
    private readonly ChoixRepository    _choixRepo;
    private readonly FiliereRepository  _filiereRepo;

    public FrmChangerSpe(DatabaseContext sessionDb)
    {
        InitializeComponent();
        _candidatRepo = new CandidatRepository(sessionDb);
        _choixRepo    = new ChoixRepository(sessionDb);
        _filiereRepo  = new FiliereRepository(ConfigDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadCandidats();
        LoadFilieres();
    }

    private void ApplyLocalization()
    {
        Text             = L.Get("TitleChangerSpe");
        lblCandidat.Text = L.Get("ColNom") + " :";
        lblFiliere.Text  = L.Get("ColFiliere") + " :";
        btnApply.Text    = "Appliquer";
        btnClose.Text    = L.Get("BtnClose");
    }

    private void LoadCandidats()
    {
        var candidats = _candidatRepo.GetAll();
        comboCandidat.DataSource    = candidats;
        comboCandidat.DisplayMember = "Nom";
        comboCandidat.ValueMember   = "CandidatID";
        if (candidats.Count > 0) comboCandidat.SelectedIndex = 0;
    }

    private void LoadFilieres()
    {
        var filieres = _filiereRepo.GetAll();
        comboFiliere.DataSource    = filieres;
        comboFiliere.DisplayMember = "FiliereNom";
        comboFiliere.ValueMember   = "FiliereID";
        if (filieres.Count > 0) comboFiliere.SelectedIndex = 0;
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
        if (comboCandidat.SelectedValue is not int candidatId) return;
        if (comboFiliere.SelectedValue is not int newFiliereId) return;

        try
        {
            // Reset all existing admis flags for this candidate
            var existingChoix = _choixRepo.GetOrderedChoices(candidatId);
            foreach (var ch in existingChoix)
            {
                if (ch.FiliereID != newFiliereId)
                    _choixRepo.SetAdmis(ch.ChoixID, false);
            }

            // Find or create the Choix row for the new filiere
            var targetChoix = existingChoix.FirstOrDefault(c => c.FiliereID == newFiliereId);
            if (targetChoix != null)
            {
                _choixRepo.SetAdmis(targetChoix.ChoixID, true);
            }
            else
            {
                // Insert new Choix at next available NumeroDeChoix
                int nextNum = existingChoix.Count > 0
                    ? existingChoix.Max(c => c.NumeroDeChoix) + 1
                    : 1;
                // Upsert inserts with ChoixAdmis=true directly
                _choixRepo.Upsert(new Choix
                {
                    CandidatID    = candidatId,
                    FiliereID     = newFiliereId,
                    NumeroDeChoix = nextNum,
                    ChoixAdmis    = true,
                });
            }

            MessageBox.Show("Spécialité modifiée avec succès.", "",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();
}
