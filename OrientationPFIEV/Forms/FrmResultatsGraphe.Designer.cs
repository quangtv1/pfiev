namespace OrientationPFIEV.Forms;

/// <summary>
/// Designer for FrmResultatsGraphe.
/// Note: PlotView controls are NOT declared here — they are created
/// programmatically in OnLoad because PlotView is not a standard
/// WinForms designer component. Only the Close button lives here.
/// </summary>
partial class FrmResultatsGraphe
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
        btnClose = new System.Windows.Forms.Button();
        SuspendLayout();

        // btnClose
        btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        btnClose.Location = new System.Drawing.Point(908, 446);
        btnClose.Size     = new System.Drawing.Size(88, 30);
        btnClose.TabIndex = 2;
        btnClose.Text     = "Fermer";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += new System.EventHandler(btnClose_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(1008, 488);
        Controls.Add(btnClose);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox     = false;
        MinimizeBox     = false;
        StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name            = "FrmResultatsGraphe";
        Text            = "Graphiques des résultats";

        ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnClose;
}
