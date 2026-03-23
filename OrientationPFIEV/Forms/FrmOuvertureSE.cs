using System.Globalization;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Dialog for creating a new session: choose path for .mdb file,
/// enter Annee and MoyenneMin, then copy InitBase.mdb to the target path.
/// </summary>
public partial class FrmOuvertureSE : Form
{
    private string? _selectedPath;

    public FrmOuvertureSE()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleOuvertureSE");
        lblPath.Text = L.Get("TitleNouvelleSession") + " :";
        lblAnnee.Text = L.Get("LblAnnee") + " :";
        lblMoyMin.Text = L.Get("LblMoyMin") + " :";
        btnBrowse.Text = L.Get("BtnBrowse");
        btnOk.Text = L.Get("BtnOk");
        btnAnnuler.Text = L.Get("BtnCancel");
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Access Database|*.mdb",
            DefaultExt = "mdb",
            Title = L.Get("TitleNouvelleSession"),
        };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _selectedPath = dlg.FileName;
            txtPath.Text = _selectedPath;
        }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedPath) ||
            string.IsNullOrWhiteSpace(txtAnnee.Text) ||
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

        var normalizedMoy = txtMoyMin.Text.Replace(',', '.');
        if (!double.TryParse(normalizedMoy, NumberStyles.Any, CultureInfo.InvariantCulture, out double moyMin))
        {
            MessageBox.Show(L.Get("MsgOnlyNumbers"), "",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var initBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InitBase.mdb");
            if (!File.Exists(initBasePath))
            {
                MessageBox.Show($"InitBase.mdb introuvable: {initBasePath}", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // NOTE: InitBase.mdb must contain empty Etablissement, Filiere, Matiere tables
            // matching the schema in CONFIG.MDB. Verify this against the original Src/InitBase.mdb.
            File.Copy(initBasePath, _selectedPath!, overwrite: false);
            SessionDatabase.Open(_selectedPath!);

            var concoursRepo = new ConcoursRepository(SessionDatabase.Context);
            concoursRepo.SetParams(new Concours { Annee = annee, MoyenneMin = moyMin });

            // Copy config tables into new session DB
            CopyConfigTablesIntoSession();

            new DataPathRepository(ConfigDatabase.Context).Add(_selectedPath!);

            DialogResult = DialogResult.OK;
            new FrmSession().Show();
        }
        catch (IOException ioEx)
        {
            MessageBox.Show(ioEx.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CopyConfigTablesIntoSession()
    {
        // Read from ConfigDatabase
        var matieres = new MatiereRepository(ConfigDatabase.Context).GetAll();
        var etabs = new EtablissementRepository(ConfigDatabase.Context).GetAll();
        var filieres = new FiliereRepository(ConfigDatabase.Context).GetAll();

        // Write into SessionDatabase (tables already exist in InitBase.mdb schema)
        var mRepo = new MatiereRepository(SessionDatabase.Context);
        foreach (var m in matieres) mRepo.Add(m);

        var eRepo = new EtablissementRepository(SessionDatabase.Context);
        foreach (var e in etabs) eRepo.Add(e);

        var fRepo = new FiliereRepository(SessionDatabase.Context);
        foreach (var f in filieres) fRepo.Add(f);
    }

    private void btnAnnuler_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtAnnee_KeyPress(object sender, KeyPressEventArgs e)
        => ValidationHelper.EnforceInteger(sender, e);

    private void txtMoyMin_KeyPress(object sender, KeyPressEventArgs e)
        => ValidationHelper.EnforceDecimal(sender, e);
}
