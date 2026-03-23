namespace OrientationPFIEV.Forms;

partial class FrmChangerSpe
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
        lblCandidat   = new System.Windows.Forms.Label();
        comboCandidat = new System.Windows.Forms.ComboBox();
        lblFiliere    = new System.Windows.Forms.Label();
        comboFiliere  = new System.Windows.Forms.ComboBox();
        btnApply      = new System.Windows.Forms.Button();
        btnClose      = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblCandidat
        lblCandidat.Location = new System.Drawing.Point(12, 15);
        lblCandidat.Size     = new System.Drawing.Size(100, 20);
        lblCandidat.Text     = "Candidat :";

        // comboCandidat
        comboCandidat.Location      = new System.Drawing.Point(118, 12);
        comboCandidat.Size          = new System.Drawing.Size(240, 23);
        comboCandidat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        // lblFiliere
        lblFiliere.Location = new System.Drawing.Point(12, 50);
        lblFiliere.Size     = new System.Drawing.Size(100, 20);
        lblFiliere.Text     = "Nouvelle filière :";

        // comboFiliere
        comboFiliere.Location      = new System.Drawing.Point(118, 47);
        comboFiliere.Size          = new System.Drawing.Size(240, 23);
        comboFiliere.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        // btnApply
        btnApply.Location = new System.Drawing.Point(118, 86);
        btnApply.Size     = new System.Drawing.Size(100, 28);
        btnApply.Text     = "Appliquer";
        btnApply.Click   += new System.EventHandler(btnApply_Click);

        // btnClose
        btnClose.Location = new System.Drawing.Point(258, 86);
        btnClose.Size     = new System.Drawing.Size(100, 28);
        btnClose.Text     = "Fermer";
        btnClose.Click   += new System.EventHandler(btnClose_Click);

        // Form
        CancelButton        = btnClose;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(372, 130);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        MinimizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                = "FrmChangerSpe";
        Text                = "Changer la spécialité";
        Controls.Add(lblCandidat);
        Controls.Add(comboCandidat);
        Controls.Add(lblFiliere);
        Controls.Add(comboFiliere);
        Controls.Add(btnApply);
        Controls.Add(btnClose);

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label    lblCandidat;
    private System.Windows.Forms.ComboBox comboCandidat;
    private System.Windows.Forms.Label    lblFiliere;
    private System.Windows.Forms.ComboBox comboFiliere;
    private System.Windows.Forms.Button   btnApply;
    private System.Windows.Forms.Button   btnClose;
}
