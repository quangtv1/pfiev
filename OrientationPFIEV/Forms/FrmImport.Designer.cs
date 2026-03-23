namespace OrientationPFIEV.Forms;

partial class FrmImport
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
        lblFile        = new System.Windows.Forms.Label();
        txtPath        = new System.Windows.Forms.TextBox();
        btnBrowse      = new System.Windows.Forms.Button();
        btnImport      = new System.Windows.Forms.Button();
        btnFermer      = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblFile
        lblFile.Location = new System.Drawing.Point(12, 15);
        lblFile.Size     = new System.Drawing.Size(80, 20);
        lblFile.Text     = "Fichier :";

        // txtPath
        txtPath.Location  = new System.Drawing.Point(98, 12);
        txtPath.Size      = new System.Drawing.Size(320, 23);
        txtPath.ReadOnly  = true;
        txtPath.TabStop   = false;

        // btnBrowse
        btnBrowse.Location = new System.Drawing.Point(426, 11);
        btnBrowse.Size     = new System.Drawing.Size(80, 25);
        btnBrowse.Text     = "...";
        btnBrowse.Click   += new System.EventHandler(btnBrowse_Click);

        // btnImport
        btnImport.Location = new System.Drawing.Point(258, 52);
        btnImport.Size     = new System.Drawing.Size(110, 30);
        btnImport.Text     = "Importer";
        btnImport.Click   += new System.EventHandler(btnImport_Click);

        // btnFermer
        btnFermer.Location   = new System.Drawing.Point(378, 52);
        btnFermer.Size       = new System.Drawing.Size(110, 30);
        btnFermer.Text       = "Fermer";
        btnFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        btnFermer.Click      += new System.EventHandler(btnFermer_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(520, 96);
        Controls.Add(lblFile);
        Controls.Add(txtPath);
        Controls.Add(btnBrowse);
        Controls.Add(btnImport);
        Controls.Add(btnFermer);
        FormBorderStyle  = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox      = false;
        MinimizeBox      = false;
        StartPosition    = System.Windows.Forms.FormStartPosition.CenterParent;
        Name             = "FrmImport";
        Text             = "Import Excel";
        AcceptButton     = btnImport;
        CancelButton     = btnFermer;

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label   lblFile;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button  btnBrowse;
    private System.Windows.Forms.Button  btnImport;
    private System.Windows.Forms.Button  btnFermer;
}
