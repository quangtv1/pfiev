namespace OrientationPFIEV.Forms;

partial class FrmNoteMatiere
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        dgvNotes  = new System.Windows.Forms.DataGridView();
        btnSave   = new System.Windows.Forms.Button();
        btnClose  = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvNotes).BeginInit();
        SuspendLayout();

        // dgvNotes
        dgvNotes.Location              = new System.Drawing.Point(12, 12);
        dgvNotes.Size                  = new System.Drawing.Size(420, 360);
        dgvNotes.AllowUserToAddRows    = false;
        dgvNotes.AllowUserToDeleteRows = false;
        dgvNotes.RowHeadersVisible     = false;
        dgvNotes.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
        dgvNotes.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
        dgvNotes.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvNotes_EditingControlShowing);

        // btnSave
        btnSave.Location = new System.Drawing.Point(258, 384);
        btnSave.Size     = new System.Drawing.Size(80, 28);
        btnSave.Text     = "Enregistrer";
        btnSave.Click   += new System.EventHandler(btnSave_Click);

        // btnClose
        btnClose.Location = new System.Drawing.Point(352, 384);
        btnClose.Size     = new System.Drawing.Size(80, 28);
        btnClose.Text     = "Fermer";
        btnClose.Click   += new System.EventHandler(btnClose_Click);

        // Form
        CancelButton        = btnClose;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(444, 424);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        MinimizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                = "FrmNoteMatiere";
        Text                = "Notes du candidat";
        Controls.Add(dgvNotes);
        Controls.Add(btnSave);
        Controls.Add(btnClose);

        ((System.ComponentModel.ISupportInitialize)dgvNotes).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.DataGridView dgvNotes;
    private System.Windows.Forms.Button       btnSave;
    private System.Windows.Forms.Button       btnClose;
}
