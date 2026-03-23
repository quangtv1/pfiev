using System.Windows.Forms;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Dialog to set a global number of places for all filières at once.
/// Calls FiliereRepository.UpdateAllSlots() on confirmation.
/// </summary>
public partial class FrmFixerNbrePlace : Form
{
    private readonly FiliereRepository _repo;

    public FrmFixerNbrePlace(FiliereRepository repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleFixerNbrePlace");
        lblNbPlace.Text = L.Get("LblNbPlaceGlobal");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnAnnuler");
    }

    private void txtNbPlace_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!ValidationHelper.IsIntegerKey(e.KeyChar))
            e.Handled = true;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNbPlace.Text))
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

        _repo.UpdateAllSlots(nbPlace);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
