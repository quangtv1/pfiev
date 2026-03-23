using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Data;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Add / Edit dialog for a Candidat record.
/// Pass candidatId for edit mode; omit for add mode.
/// Suivant button saves and clears for the next candidate entry.
/// </summary>
public partial class FrmAjouterCandidat : Form
{
    private readonly CandidatRepository _repo;
    private readonly int? _existingId;
    private readonly EtablissementRepository _etabRepo;

    public FrmAjouterCandidat(CandidatRepository repo, int? candidatId = null)
    {
        InitializeComponent();
        _repo       = repo;
        _existingId = candidatId;
        _etabRepo   = new EtablissementRepository(ConfigDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadComboBoxes();
        if (_existingId.HasValue) PopulateFields(_existingId.Value);
    }

    // ── Localization ───────────────────────────────────────────────────────────

    private void ApplyLocalization()
    {
        Text             = L.Get("TitleAjouterCandidat");
        lblNom.Text      = L.Get("LblNom") + " :";
        lblNomInter.Text = "Nom intermédiaire :";
        lblPrenom.Text   = L.Get("ColPrenom") + " :";
        lblAnonymat.Text = L.Get("ColAnonymat") + " :";
        lblDOB.Text      = L.Get("LblDOB") + " :";
        lblSexe.Text     = L.Get("LblSex") + " :";
        lblStatut.Text   = L.Get("LblStatus") + " :";
        lblLangue.Text   = L.Get("LblLanguage") + " :";
        lblEtab.Text     = L.Get("LblEtablissement") + " :";
        btnOk.Text       = L.Get("BtnOk");
        btnAnnuler.Text  = L.Get("BtnCancel");
        btnSuivant.Text  = "Suivant";
        // Hide Suivant in edit mode — only useful for batch add
        btnSuivant.Visible = !_existingId.HasValue;
    }

    // ── Data ───────────────────────────────────────────────────────────────────

    private void LoadComboBoxes()
    {
        comboSexe.Items.Clear();
        comboSexe.Items.Add("M");
        comboSexe.Items.Add("F");
        comboSexe.SelectedIndex = 0;

        comboStatut.Items.Clear();
        comboStatut.Items.Add("I");
        comboStatut.Items.Add("E");
        comboStatut.SelectedIndex = 0;

        comboLangue.Items.Clear();
        comboLangue.Items.Add("fr");
        comboLangue.Items.Add("vi");
        comboLangue.SelectedIndex = 0;

        var etabs = _etabRepo.GetAll();
        comboEtab.DataSource    = etabs;
        comboEtab.DisplayMember = "EtabNom";
        comboEtab.ValueMember   = "EtabID";
        if (etabs.Count > 0) comboEtab.SelectedIndex = 0;
    }

    private void PopulateFields(int id)
    {
        var c = _repo.GetById(id);
        if (c == null) return;
        txtNom.Text              = c.Nom;
        txtNomInter.Text         = c.NomIntermediaire;
        txtPrenom.Text           = c.Prenom;
        txtAnonymat.Text         = c.Anonymat;
        if (c.DateDeNaissance.HasValue)
            dtpDateNaissance.Value = c.DateDeNaissance.Value;
        comboSexe.SelectedItem   = c.Sexe;
        comboStatut.SelectedItem = c.CandidatStatut;
        comboLangue.SelectedItem = c.Langue;
        // Match etab by ID
        foreach (Etablissement etab in comboEtab.Items)
        {
            if (etab.EtabID == c.EtabID)
            {
                comboEtab.SelectedItem = etab;
                break;
            }
        }
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(txtNom.Text) ||
            string.IsNullOrWhiteSpace(txtPrenom.Text))
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private Candidat BuildCandidat() => new()
    {
        Nom              = txtNom.Text.Trim(),
        NomIntermediaire = txtNomInter.Text.Trim(),
        Prenom           = txtPrenom.Text.Trim(),
        Anonymat         = txtAnonymat.Text.Trim(),
        DateDeNaissance  = dtpDateNaissance.Value.Date,
        Sexe             = comboSexe.SelectedItem?.ToString() ?? "M",
        CandidatStatut   = comboStatut.SelectedItem?.ToString() ?? "I",
        Langue           = comboLangue.SelectedItem?.ToString() ?? "fr",
        EtabID           = comboEtab.SelectedValue is int eid ? eid : 0,
    };

    private void ClearForm()
    {
        txtNom.Text      = "";
        txtNomInter.Text = "";
        txtPrenom.Text   = "";
        txtAnonymat.Text = "";
        dtpDateNaissance.Value   = DateTime.Today;
        comboSexe.SelectedIndex  = 0;
        comboStatut.SelectedIndex = 0;
        comboLangue.SelectedIndex = 0;
        if (comboEtab.Items.Count > 0) comboEtab.SelectedIndex = 0;
        txtNom.Focus();
    }

    // ── Event handlers ─────────────────────────────────────────────────────────

    private void btnOk_Click(object sender, EventArgs e) => SaveAndClose(close: true);

    private void btnSuivant_Click(object sender, EventArgs e) => SaveAndClose(close: false);

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void SaveAndClose(bool close)
    {
        if (!ValidateInputs()) return;
        var c = BuildCandidat();
        try
        {
            if (_existingId.HasValue)
            {
                c.CandidatID = _existingId.Value;
                _repo.Update(c);
            }
            else
            {
                _repo.Add(c);
            }

            if (close)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                ClearForm();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
