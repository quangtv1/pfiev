using System.Windows.Forms;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Dialog to set the maximum number of filière choices a candidate may submit.
/// Stores the value in AppState.MaxChoices.
/// </summary>
public partial class FrmFixerNbFiliere : Form
{
    public FrmFixerNbFiliere()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        txtMaxChoices.Text = AppState.MaxChoices.ToString();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleFixerNbFiliere");
        lblMaxChoices.Text = L.Get("LblMaxChoices");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnAnnuler");
    }

    private void txtMaxChoices_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!ValidationHelper.IsIntegerKey(e.KeyChar))
            e.Handled = true;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaxChoices.Text))
        {
            MessageBox.Show(L.Get("MsgFieldRequired"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!int.TryParse(txtMaxChoices.Text, out int maxChoices) || maxChoices < 1)
        {
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AppState.MaxChoices = maxChoices;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
