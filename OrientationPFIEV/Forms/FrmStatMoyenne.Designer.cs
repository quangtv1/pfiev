namespace OrientationPFIEV.Forms;

partial class FrmStatMoyenne
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
        dgvStat  = new System.Windows.Forms.DataGridView();
        btnClose = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvStat).BeginInit();
        SuspendLayout();

        // dgvStat
        dgvStat.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvStat.Location = new System.Drawing.Point(12, 12);
        dgvStat.Size     = new System.Drawing.Size(360, 240);
        dgvStat.TabIndex = 0;

        // btnClose
        btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        btnClose.Location = new System.Drawing.Point(284, 266);
        btnClose.Size     = new System.Drawing.Size(88, 30);
        btnClose.TabIndex = 1;
        btnClose.Text     = "Fermer";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += new System.EventHandler(btnClose_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(384, 308);
        Controls.Add(dgvStat);
        Controls.Add(btnClose);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        MinimumSize     = new System.Drawing.Size(360, 300);
        StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name            = "FrmStatMoyenne";
        Text            = "Distribution des moyennes";

        ((System.ComponentModel.ISupportInitialize)dgvStat).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.DataGridView dgvStat;
    private System.Windows.Forms.Button       btnClose;
}
