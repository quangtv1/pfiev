namespace OrientationPFIEV.Forms;

partial class FrmExportExcel
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
        grpChoix              = new System.Windows.Forms.GroupBox();
        radioExportCandidats  = new System.Windows.Forms.RadioButton();
        radioExportResultats  = new System.Windows.Forms.RadioButton();
        btnExport             = new System.Windows.Forms.Button();
        btnFermer             = new System.Windows.Forms.Button();

        grpChoix.SuspendLayout();
        SuspendLayout();

        // radioExportCandidats
        radioExportCandidats.Location = new System.Drawing.Point(12, 22);
        radioExportCandidats.Size     = new System.Drawing.Size(260, 20);
        radioExportCandidats.Text     = "Exporter la liste des candidats";
        radioExportCandidats.Checked  = true;
        radioExportCandidats.TabIndex = 0;

        // radioExportResultats
        radioExportResultats.Location = new System.Drawing.Point(12, 48);
        radioExportResultats.Size     = new System.Drawing.Size(260, 20);
        radioExportResultats.Text     = "Exporter les résultats";
        radioExportResultats.TabIndex = 1;

        // grpChoix
        grpChoix.Location = new System.Drawing.Point(12, 12);
        grpChoix.Size     = new System.Drawing.Size(290, 80);
        grpChoix.Text     = "Export";
        grpChoix.Controls.Add(radioExportCandidats);
        grpChoix.Controls.Add(radioExportResultats);

        // btnExport
        btnExport.Location = new System.Drawing.Point(100, 104);
        btnExport.Size     = new System.Drawing.Size(90, 28);
        btnExport.Text     = "Exporter";
        btnExport.Click   += new System.EventHandler(btnExport_Click);

        // btnFermer
        btnFermer.Location   = new System.Drawing.Point(200, 104);
        btnFermer.Size       = new System.Drawing.Size(90, 28);
        btnFermer.Text       = "Fermer";
        btnFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        btnFermer.Click      += new System.EventHandler(btnFermer_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(316, 144);
        Controls.Add(grpChoix);
        Controls.Add(btnExport);
        Controls.Add(btnFermer);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
        Name            = "FrmExportExcel";
        Text            = "Export Excel";
        AcceptButton    = btnExport;
        CancelButton    = btnFermer;

        grpChoix.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox    grpChoix;
    private System.Windows.Forms.RadioButton radioExportCandidats;
    private System.Windows.Forms.RadioButton radioExportResultats;
    private System.Windows.Forms.Button      btnExport;
    private System.Windows.Forms.Button      btnFermer;
}
