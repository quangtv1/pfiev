using System.Data;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using OrientationPFIEV.Models;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Grade entry dialog for a single candidate.
/// Shows all Matieres with their coefficients; Note column is editable.
/// Saves via NoteRepository.Upsert on OK.
/// </summary>
public partial class FrmNoteMatiere : Form
{
    private readonly int _candidatId;
    private readonly NoteRepository _noteRepo;
    private readonly MatiereRepository _matiereRepo;
    private DataTable _dt = new();

    // Column name constants
    private const string ColMatiereID = "MatiereID";
    private const string ColMatiere   = "Matiere";
    private const string ColCoeff     = "Coefficient";
    private const string ColNote      = "Note";

    public FrmNoteMatiere(int candidatId, DatabaseContext sessionDb)
    {
        InitializeComponent();
        _candidatId  = candidatId;
        _noteRepo    = new NoteRepository(sessionDb);
        _matiereRepo = new MatiereRepository(ConfigDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        BuildGrid();
    }

    private void ApplyLocalization()
    {
        Text          = L.Get("TitleNoteMatiere");
        btnSave.Text  = L.Get("BtnSave");
        btnClose.Text = L.Get("BtnClose");
    }

    // ── Grid ───────────────────────────────────────────────────────────────────

    private void BuildGrid()
    {
        _dt = new DataTable();
        _dt.Columns.Add(ColMatiereID, typeof(int));
        _dt.Columns.Add(ColMatiere,   typeof(string));
        _dt.Columns.Add(ColCoeff,     typeof(double));
        _dt.Columns.Add(ColNote,      typeof(string));

        var matieres = _matiereRepo.GetAll();
        var notes    = _noteRepo.GetByCandidatId(_candidatId)
                                .ToDictionary(n => n.MatiereID);

        foreach (var m in matieres)
        {
            string noteVal = notes.TryGetValue(m.MatiereID, out var n) ? n.NoteValue : "";
            _dt.Rows.Add(m.MatiereID, m.MatiereNom, m.MatiereCoefficient, noteVal);
        }

        dgvNotes.DataSource = _dt;

        // Configure columns
        dgvNotes.Columns[ColMatiereID].Visible   = false;
        dgvNotes.Columns[ColMatiere].ReadOnly    = true;
        dgvNotes.Columns[ColMatiere].HeaderText  = "Matière";
        dgvNotes.Columns[ColCoeff].ReadOnly      = true;
        dgvNotes.Columns[ColCoeff].HeaderText    = L.Get("LblCoefficient");
        dgvNotes.Columns[ColNote].ReadOnly       = false;
        dgvNotes.Columns[ColNote].HeaderText     = "Note";

        // Column widths
        dgvNotes.Columns[ColMatiere].Width = 180;
        dgvNotes.Columns[ColCoeff].Width   = 90;
        dgvNotes.Columns[ColNote].Width    = 90;
    }

    // ── Validation: decimal only in Note column ────────────────────────────────

    private void dgvNotes_EditingControlShowing(object sender,
        System.Windows.Forms.DataGridViewEditingControlShowingEventArgs e)
    {
        if (dgvNotes.CurrentCell?.OwningColumn.Name == ColNote &&
            e.Control is TextBox tb)
        {
            tb.KeyPress -= OnNoteKeyPress;
            tb.KeyPress += OnNoteKeyPress;
        }
    }

    private void OnNoteKeyPress(object? sender, KeyPressEventArgs e)
        => ValidationHelper.EnforceDecimal(sender, e);

    // ── Button handlers ────────────────────────────────────────────────────────

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Commit any in-progress edit
        dgvNotes.EndEdit();
        try
        {
            foreach (DataRow row in _dt.Rows)
            {
                _noteRepo.Upsert(new Note
                {
                    CandidatID = _candidatId,
                    MatiereID  = (int)row[ColMatiereID],
                    NoteValue  = row[ColNote]?.ToString() ?? "",
                });
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
