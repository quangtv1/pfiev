using System.Windows.Forms;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Add / Edit dialog for an Etablissement record.
/// Pass existing Etablissement to constructor for edit mode; omit for add mode.
/// </summary>
public partial class FrmAjouterEtab : Form
{
    private readonly EtablissementRepository _repo;
    private readonly Etablissement? _existing;

    public FrmAjouterEtab(EtablissementRepository repo, Etablissement? existing = null)
    {
        InitializeComponent();
        _repo = repo;
        _existing = existing;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        if (_existing != null)
        {
            txtNom.Text = _existing.EtabNom;
            txtCode.Text = _existing.EtabCode;
            txtVille.Text = _existing.EtabVille;
        }
    }

    private void ApplyLocalization()
    {
        Text = _existing == null ? L.Get("TitleAjouterEtab") : L.Get("TitleModifierEtab");
        lblNom.Text = L.Get("LblNom");
        lblCode.Text = L.Get("LblCode");
        lblVille.Text = L.Get("LblVille");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnAnnuler");
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNom.Text) ||
            string.IsNullOrWhiteSpace(txtCode.Text) ||
            string.IsNullOrWhiteSpace(txtVille.Text))
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var etab = new Etablissement
        {
            EtabNom = txtNom.Text.Trim(),
            EtabCode = txtCode.Text.Trim(),
            EtabVille = txtVille.Text.Trim(),
        };

        if (_existing != null)
        {
            etab.EtabID = _existing.EtabID;
            _repo.Update(etab);
        }
        else
        {
            _repo.Add(etab);
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
