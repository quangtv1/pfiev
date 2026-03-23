using System.Globalization;
using System.Windows.Forms;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Add / Edit dialog for a Matiere record.
/// Pass existing Matiere to constructor for edit mode; omit for add mode.
/// </summary>
public partial class FrmAjouterMatiere : Form
{
    private readonly MatiereRepository _repo;
    private readonly Matiere? _existing;

    public FrmAjouterMatiere(MatiereRepository repo, Matiere? existing = null)
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
            txtNom.Text = _existing.MatiereNom;
            txtCoefficient.Text = _existing.MatiereCoefficient.ToString(CultureInfo.InvariantCulture);
        }
    }

    private void ApplyLocalization()
    {
        Text = _existing == null ? L.Get("TitleAjouterMatiere") : L.Get("TitleModifierMatiere");
        lblNom.Text = L.Get("LblNom");
        lblCoefficient.Text = L.Get("LblCoefficient");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnAnnuler");
    }

    private void txtCoefficient_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!ValidationHelper.IsDecimalKey(e.KeyChar))
        {
            e.Handled = true;
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNom.Text) ||
            string.IsNullOrWhiteSpace(txtCoefficient.Text))
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var normalized = txtCoefficient.Text.Replace(',', '.');
        if (!double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double coeff))
        {
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var m = new Matiere
        {
            MatiereNom = txtNom.Text.Trim(),
            MatiereCoefficient = coeff,
        };

        if (_existing != null)
        {
            m.MatiereID = _existing.MatiereID;
            _repo.Update(m);
        }
        else
        {
            _repo.Add(m);
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
