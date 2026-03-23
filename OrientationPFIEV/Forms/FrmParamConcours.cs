using System.Globalization;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Dialog to view and edit Annee and MoyenneMin for the current session's Concours row.
/// </summary>
public partial class FrmParamConcours : Form
{
    private readonly ConcoursRepository _repo;

    public FrmParamConcours(DatabaseContext sessionDb)
    {
        InitializeComponent();
        _repo = new ConcoursRepository(sessionDb);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        LoadParams();
    }

    private void ApplyLocalization()
    {
        Text            = L.Get("TitleParamConcours");
        lblAnnee.Text   = L.Get("LblAnnee") + " :";
        lblMoyMin.Text  = L.Get("LblMoyMin") + " :";
        btnOk.Text      = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnCancel");
    }

    private void LoadParams()
    {
        var c = _repo.Get();
        if (c == null) return;
        txtAnnee.Text  = c.Annee.ToString();
        txtMoyMin.Text = c.MoyenneMin.ToString(CultureInfo.InvariantCulture);
    }

    private void txtAnnee_KeyPress(object sender, KeyPressEventArgs e)
        => ValidationHelper.EnforceInteger(sender, e);

    private void txtMoyMin_KeyPress(object sender, KeyPressEventArgs e)
        => ValidationHelper.EnforceDecimal(sender, e);

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAnnee.Text) ||
            string.IsNullOrWhiteSpace(txtMoyMin.Text))
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(txtAnnee.Text, out int annee))
        {
            MessageBox.Show(L.Get("MsgOnlyDigits"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var normalized = txtMoyMin.Text.Replace(',', '.');
        if (!double.TryParse(normalized, NumberStyles.Any,
                CultureInfo.InvariantCulture, out double moyMin))
        {
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _repo.SetParams(new Concours { Annee = annee, MoyenneMin = moyMin });
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
