using System.Data.OleDb;
using System.Windows.Forms;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Main configuration form with TabControl (Matières / Etablissements / Filières).
/// Context-aware: uses ConfigDatabase when IsFirstOpen=true, else SessionDatabase.
/// </summary>
public partial class FrmParametrage : Form
{
    private readonly DatabaseContext _db;
    private readonly MatiereRepository _matiereRepo;
    private readonly EtablissementRepository _etabRepo;
    private readonly FiliereRepository _filiereRepo;

    public FrmParametrage()
    {
        InitializeComponent();
        _db = AppState.IsFirstOpen ? ConfigDatabase.Context : SessionDatabase.Context;
        _matiereRepo = new MatiereRepository(_db);
        _etabRepo = new EtablissementRepository(_db);
        _filiereRepo = new FiliereRepository(_db);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadMatieres();
        LoadEtablissements();
        LoadFilieres();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleParametrage");
        tabMatieres.Text = L.Get("TabMatieres");
        tabEtablissements.Text = L.Get("TabEtablissements");
        tabFilieres.Text = L.Get("TabFilieres");
        btnAjouterMatiere.Text = L.Get("BtnAjouter");
        btnModifierMatiere.Text = L.Get("BtnModifier");
        btnEffacerMatiere.Text = L.Get("BtnEffacer");
        btnAjouterEtab.Text = L.Get("BtnAjouter");
        btnModifierEtab.Text = L.Get("BtnModifier");
        btnEffacerEtab.Text = L.Get("BtnEffacer");
        btnAjouterFiliere.Text = L.Get("BtnAjouter");
        btnModifierFiliere.Text = L.Get("BtnModifier");
        btnEffacerFiliere.Text = L.Get("BtnEffacer");
        btnFixerNbrePlaces.Text = L.Get("BtnFixerNbrePlaces");
        btnQuitter.Text = L.Get("BtnClose");
    }

    // ── Matières ──────────────────────────────────────────────────────────────

    private void LoadMatieres()
    {
        listBoxMatieres.DataSource = null;
        listBoxMatieres.DataSource = _matiereRepo.GetAll();
        listBoxMatieres.DisplayMember = "MatiereNom";
        listBoxMatieres.ValueMember = "MatiereID";
    }

    private void btnAjouterMatiere_Click(object sender, EventArgs e)
    {
        using var frm = new FrmAjouterMatiere(_matiereRepo);
        if (frm.ShowDialog() == DialogResult.OK) LoadMatieres();
    }

    private void btnModifierMatiere_Click(object sender, EventArgs e)
    {
        if (listBoxMatieres.SelectedItem is not Matiere selected) return;
        using var frm = new FrmAjouterMatiere(_matiereRepo, selected);
        if (frm.ShowDialog() == DialogResult.OK) LoadMatieres();
    }

    private void btnEffacerMatiere_Click(object sender, EventArgs e)
    {
        if (listBoxMatieres.SelectedItem is not Matiere selected) return;
        if (MessageBox.Show(L.Get("MsgConfirmDelete"), "",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            _matiereRepo.Delete(selected.MatiereID);
            LoadMatieres();
        }
        catch (OleDbException ex) when (ex.Message.Contains("constraint") || ex.Errors.Count > 0)
        {
            MessageBox.Show(L.Get("MsgDeleteFkError"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Etablissements ────────────────────────────────────────────────────────

    private void LoadEtablissements()
    {
        listBoxEtabs.DataSource = null;
        listBoxEtabs.DataSource = _etabRepo.GetAll();
        listBoxEtabs.DisplayMember = "EtabNom";
        listBoxEtabs.ValueMember = "EtabID";
    }

    private void btnAjouterEtab_Click(object sender, EventArgs e)
    {
        using var frm = new FrmAjouterEtab(_etabRepo);
        if (frm.ShowDialog() == DialogResult.OK) LoadEtablissements();
    }

    private void btnModifierEtab_Click(object sender, EventArgs e)
    {
        if (listBoxEtabs.SelectedItem is not Etablissement selected) return;
        using var frm = new FrmAjouterEtab(_etabRepo, selected);
        if (frm.ShowDialog() == DialogResult.OK) LoadEtablissements();
    }

    private void btnEffacerEtab_Click(object sender, EventArgs e)
    {
        if (listBoxEtabs.SelectedItem is not Etablissement selected) return;
        if (MessageBox.Show(L.Get("MsgConfirmDelete"), "",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            _etabRepo.Delete(selected.EtabID);
            LoadEtablissements();
        }
        catch (OleDbException)
        {
            MessageBox.Show(L.Get("MsgDeleteFkError"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Filières ──────────────────────────────────────────────────────────────

    private void LoadFilieres()
    {
        listBoxFilieres.DataSource = null;
        listBoxFilieres.DataSource = _filiereRepo.GetAll();
        listBoxFilieres.DisplayMember = "FiliereNom";
        listBoxFilieres.ValueMember = "FiliereID";
    }

    private void btnAjouterFiliere_Click(object sender, EventArgs e)
    {
        using var frm = new FrmAjouterFiliere(_filiereRepo, _etabRepo);
        if (frm.ShowDialog() == DialogResult.OK) LoadFilieres();
    }

    private void btnModifierFiliere_Click(object sender, EventArgs e)
    {
        if (listBoxFilieres.SelectedItem is not Filiere selected) return;
        using var frm = new FrmAjouterFiliere(_filiereRepo, _etabRepo, selected);
        if (frm.ShowDialog() == DialogResult.OK) LoadFilieres();
    }

    private void btnEffacerFiliere_Click(object sender, EventArgs e)
    {
        if (listBoxFilieres.SelectedItem is not Filiere selected) return;
        if (MessageBox.Show(L.Get("MsgConfirmDelete"), "",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            _filiereRepo.Delete(selected.FiliereID);
            LoadFilieres();
        }
        catch (OleDbException)
        {
            MessageBox.Show(L.Get("MsgDeleteFkError"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnFixerNbrePlaces_Click(object sender, EventArgs e)
    {
        using var frm = new FrmFixerNbrePlace(_filiereRepo);
        if (frm.ShowDialog() == DialogResult.OK) LoadFilieres();
    }

    private void btnQuitter_Click(object sender, EventArgs e) => Close();
}
