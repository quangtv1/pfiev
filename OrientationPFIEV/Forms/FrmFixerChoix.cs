using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Dual-listbox dialog for setting a candidate's filiere preferences.
/// Left: available filieres. Right: ordered choices (priority 1..N).
/// Up/Down buttons reorder; Save deletes existing Choix and re-inserts ordered list.
/// </summary>
public partial class FrmFixerChoix : Form
{
    private readonly int _candidatId;
    private readonly ChoixRepository _choixRepo;
    private readonly FiliereRepository _filiereRepo;

    // Internal display wrapper so ListBox shows filiere name
    private sealed class FiliereItem
    {
        public int    FiliereID  { get; init; }
        public string FiliereNom { get; init; } = "";
        public override string ToString() => FiliereNom;
    }

    public FrmFixerChoix(int candidatId, DatabaseContext sessionDb)
    {
        InitializeComponent();
        _candidatId  = candidatId;
        _choixRepo   = new ChoixRepository(sessionDb);
        _filiereRepo = new FiliereRepository(ConfigDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadLists();
    }

    private void ApplyLocalization()
    {
        Text              = L.Get("TitleFixerChoix");
        lblAvailable.Text = L.Get("LblAvailableFilieres");
        lblChoices.Text   = L.Get("LblCandidateChoices");
        btnSave.Text      = L.Get("BtnSave");
        btnClose.Text     = L.Get("BtnClose");
    }

    // ── Data ───────────────────────────────────────────────────────────────────

    private void LoadLists()
    {
        var allFilieres = _filiereRepo.GetAll();
        var existingChoix = _choixRepo.GetOrderedChoices(_candidatId);
        var choixFiliereIds = new HashSet<int>(existingChoix.Select(c => c.FiliereID));

        lbAvailable.Items.Clear();
        lbChoices.Items.Clear();

        // Available = all filieres not already in choices
        foreach (var f in allFilieres)
        {
            if (!choixFiliereIds.Contains(f.FiliereID))
                lbAvailable.Items.Add(new FiliereItem { FiliereID = f.FiliereID, FiliereNom = f.FiliereNom });
        }

        // Choices = ordered by NumeroDeChoix, show as FiliereItem
        var filiereMap = allFilieres.ToDictionary(f => f.FiliereID);
        foreach (var ch in existingChoix)
        {
            if (filiereMap.TryGetValue(ch.FiliereID, out var f))
                lbChoices.Items.Add(new FiliereItem { FiliereID = f.FiliereID, FiliereNom = f.FiliereNom });
        }
    }

    // ── Move buttons ───────────────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (lbAvailable.SelectedItem is not FiliereItem item) return;
        if (lbChoices.Items.Count >= AppState.MaxChoices)
        {
            MessageBox.Show($"Maximum {AppState.MaxChoices} choix autorisés.", "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        lbChoices.Items.Add(item);
        lbAvailable.Items.Remove(item);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        if (lbChoices.SelectedItem is not FiliereItem item) return;
        lbAvailable.Items.Add(item);
        lbChoices.Items.Remove(item);
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
        int idx = lbChoices.SelectedIndex;
        if (idx <= 0) return;
        var item = lbChoices.Items[idx];
        lbChoices.Items.RemoveAt(idx);
        lbChoices.Items.Insert(idx - 1, item);
        lbChoices.SelectedIndex = idx - 1;
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
        int idx = lbChoices.SelectedIndex;
        if (idx < 0 || idx >= lbChoices.Items.Count - 1) return;
        var item = lbChoices.Items[idx];
        lbChoices.Items.RemoveAt(idx);
        lbChoices.Items.Insert(idx + 1, item);
        lbChoices.SelectedIndex = idx + 1;
    }

    // ── Save / Close ───────────────────────────────────────────────────────────

    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            _choixRepo.DeleteByCandidatId(_candidatId);
            for (int i = 0; i < lbChoices.Items.Count; i++)
            {
                if (lbChoices.Items[i] is FiliereItem fi)
                {
                    _choixRepo.Upsert(new Choix
                    {
                        CandidatID    = _candidatId,
                        FiliereID     = fi.FiliereID,
                        NumeroDeChoix = i + 1,
                        ChoixAdmis    = false,
                    });
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();
}
