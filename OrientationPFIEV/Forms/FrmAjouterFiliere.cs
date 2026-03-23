using System.Windows.Forms;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Add / Edit dialog for a Filiere record.
/// Includes integer validation on NbPlace and a ComboBox for Etablissement selection.
/// Pass existing Filiere to constructor for edit mode; omit for add mode.
/// </summary>
public partial class FrmAjouterFiliere : Form
{
    private readonly FiliereRepository _repo;
    private readonly EtablissementRepository _etabRepo;
    private readonly Filiere? _existing;

    public FrmAjouterFiliere(FiliereRepository repo, EtablissementRepository etabRepo,
        Filiere? existing = null)
    {
        InitializeComponent();
        _repo = repo;
        _etabRepo = etabRepo;
        _existing = existing;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadEtablissements();

        if (_existing != null)
        {
            txtNom.Text = _existing.FiliereNom;
            txtCode.Text = _existing.FiliereCode;
            txtNbPlace.Text = _existing.FiliereNbPlace.ToString();
            // Select matching etab in combobox
            foreach (var item in cboEtab.Items)
            {
                if (item is Etablissement etab && etab.EtabID == _existing.EtabID)
                {
                    cboEtab.SelectedItem = item;
                    break;
                }
            }
        }
        else
        {
            txtNbPlace.Text = "20";
        }
    }

    private void ApplyLocalization()
    {
        Text = _existing == null ? L.Get("TitleAjouterFiliere") : L.Get("TitleModifierFiliere");
        lblNom.Text = L.Get("LblNom");
        lblCode.Text = L.Get("LblCode");
        lblNbPlace.Text = L.Get("LblNbPlace");
        lblEtab.Text = L.Get("LblEtablissement");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnAnnuler");
    }

    private void LoadEtablissements()
    {
        var etabs = _etabRepo.GetAll();
        cboEtab.DataSource = etabs;
        cboEtab.DisplayMember = "EtabCode";
        cboEtab.ValueMember = "EtabID";
    }

    private void txtNbPlace_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!ValidationHelper.IsIntegerKey(e.KeyChar))
            e.Handled = true;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNom.Text) ||
            string.IsNullOrWhiteSpace(txtCode.Text) ||
            string.IsNullOrWhiteSpace(txtNbPlace.Text) ||
            cboEtab.SelectedItem == null)
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!int.TryParse(txtNbPlace.Text, out int nbPlace) || nbPlace < 0)
        {
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var selectedEtab = (Etablissement)cboEtab.SelectedItem;
        var f = new Filiere
        {
            FiliereNom = txtNom.Text.Trim(),
            FiliereCode = txtCode.Text.Trim(),
            FiliereNbPlace = nbPlace,
            EtabID = selectedEtab.EtabID,
        };

        if (_existing != null)
        {
            f.FiliereID = _existing.FiliereID;
            _repo.Update(f);
        }
        else
        {
            _repo.Add(f);
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
