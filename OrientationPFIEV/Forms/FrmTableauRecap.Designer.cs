namespace OrientationPFIEV.Forms;

partial class FrmTableauRecap
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
        lblInternes     = new System.Windows.Forms.Label();
        dgvInternes     = new System.Windows.Forms.DataGridView();
        lblExternes     = new System.Windows.Forms.Label();
        dgvExternes     = new System.Windows.Forms.DataGridView();
        pnlNavButtons   = new System.Windows.Forms.Panel();
        btnStat         = new System.Windows.Forms.Button();
        btnStat1        = new System.Windows.Forms.Button();
        btnStat6        = new System.Windows.Forms.Button();
        btnStatEtab     = new System.Windows.Forms.Button();
        btnStatMoyenne  = new System.Windows.Forms.Button();
        btnMoyenneSpe   = new System.Windows.Forms.Button();
        btnGraphe       = new System.Windows.Forms.Button();
        btnClose        = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvInternes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvExternes).BeginInit();
        pnlNavButtons.SuspendLayout();
        SuspendLayout();

        // lblInternes
        lblInternes.Location  = new System.Drawing.Point(12, 8);
        lblInternes.Size      = new System.Drawing.Size(100, 20);
        lblInternes.Text      = "Internes";
        lblInternes.Font      = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

        // dgvInternes — top half
        dgvInternes.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvInternes.Location = new System.Drawing.Point(12, 30);
        dgvInternes.Size     = new System.Drawing.Size(960, 230);
        dgvInternes.TabIndex = 0;

        // lblExternes
        lblExternes.Location = new System.Drawing.Point(12, 272);
        lblExternes.Size     = new System.Drawing.Size(100, 20);
        lblExternes.Text     = "Externes";
        lblExternes.Font     = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold);

        // dgvExternes — bottom half
        dgvExternes.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvExternes.Location = new System.Drawing.Point(12, 294);
        dgvExternes.Size     = new System.Drawing.Size(960, 230);
        dgvExternes.TabIndex = 1;

        // pnlNavButtons — horizontal row at bottom
        pnlNavButtons.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        pnlNavButtons.Location = new System.Drawing.Point(12, 538);
        pnlNavButtons.Size     = new System.Drawing.Size(960, 40);
        pnlNavButtons.Controls.Add(btnStat);
        pnlNavButtons.Controls.Add(btnStat1);
        pnlNavButtons.Controls.Add(btnStat6);
        pnlNavButtons.Controls.Add(btnStatEtab);
        pnlNavButtons.Controls.Add(btnStatMoyenne);
        pnlNavButtons.Controls.Add(btnMoyenneSpe);
        pnlNavButtons.Controls.Add(btnGraphe);
        pnlNavButtons.Controls.Add(btnClose);

        // Nav buttons — evenly spaced
        int bx = 0, bw = 118, bh = 34;
        btnStat.Location        = new System.Drawing.Point(bx,         3); btnStat.Size        = new System.Drawing.Size(bw, bh); btnStat.Text        = "Statistiques";     btnStat.TabIndex = 0;        btnStat.UseVisualStyleBackColor = true; btnStat.Click        += new System.EventHandler(btnStat_Click);
        btnStat1.Location       = new System.Drawing.Point(bx + 122,   3); btnStat1.Size       = new System.Drawing.Size(bw, bh); btnStat1.Text       = "Stat. Langue";     btnStat1.TabIndex = 1;       btnStat1.UseVisualStyleBackColor = true; btnStat1.Click       += new System.EventHandler(btnStat1_Click);
        btnStat6.Location       = new System.Drawing.Point(bx + 244,   3); btnStat6.Size       = new System.Drawing.Size(bw, bh); btnStat6.Text       = "Top 6";            btnStat6.TabIndex = 2;       btnStat6.UseVisualStyleBackColor = true; btnStat6.Click       += new System.EventHandler(btnStat6_Click);
        btnStatEtab.Location    = new System.Drawing.Point(bx + 366,   3); btnStatEtab.Size    = new System.Drawing.Size(bw, bh); btnStatEtab.Text    = "Stat. Etab.";     btnStatEtab.TabIndex = 3;    btnStatEtab.UseVisualStyleBackColor = true; btnStatEtab.Click    += new System.EventHandler(btnStatEtab_Click);
        btnStatMoyenne.Location = new System.Drawing.Point(bx + 488,   3); btnStatMoyenne.Size = new System.Drawing.Size(bw, bh); btnStatMoyenne.Text = "Distribution";    btnStatMoyenne.TabIndex = 4; btnStatMoyenne.UseVisualStyleBackColor = true; btnStatMoyenne.Click += new System.EventHandler(btnStatMoyenne_Click);
        btnMoyenneSpe.Location  = new System.Drawing.Point(bx + 610,   3); btnMoyenneSpe.Size  = new System.Drawing.Size(bw, bh); btnMoyenneSpe.Text  = "Moy. Spé";        btnMoyenneSpe.TabIndex = 5;  btnMoyenneSpe.UseVisualStyleBackColor = true; btnMoyenneSpe.Click  += new System.EventHandler(btnMoyenneSpe_Click);
        btnGraphe.Location      = new System.Drawing.Point(bx + 732,   3); btnGraphe.Size      = new System.Drawing.Size(bw, bh); btnGraphe.Text      = "Graphiques";      btnGraphe.TabIndex = 6;      btnGraphe.UseVisualStyleBackColor = true; btnGraphe.Click      += new System.EventHandler(btnGraphe_Click);
        btnClose.Location       = new System.Drawing.Point(bx + 854,   3); btnClose.Size       = new System.Drawing.Size(bw, bh); btnClose.Text       = "Fermer";          btnClose.TabIndex = 7;       btnClose.UseVisualStyleBackColor = true; btnClose.Click       += new System.EventHandler(btnClose_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(984, 592);
        Controls.Add(lblInternes);
        Controls.Add(dgvInternes);
        Controls.Add(lblExternes);
        Controls.Add(dgvExternes);
        Controls.Add(pnlNavButtons);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        MinimumSize     = new System.Drawing.Size(800, 500);
        StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name            = "FrmTableauRecap";
        Text            = "Tableaux récapitulatifs";

        ((System.ComponentModel.ISupportInitialize)dgvInternes).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvExternes).EndInit();
        pnlNavButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label          lblInternes;
    private System.Windows.Forms.DataGridView   dgvInternes;
    private System.Windows.Forms.Label          lblExternes;
    private System.Windows.Forms.DataGridView   dgvExternes;
    private System.Windows.Forms.Panel          pnlNavButtons;
    private System.Windows.Forms.Button         btnStat;
    private System.Windows.Forms.Button         btnStat1;
    private System.Windows.Forms.Button         btnStat6;
    private System.Windows.Forms.Button         btnStatEtab;
    private System.Windows.Forms.Button         btnStatMoyenne;
    private System.Windows.Forms.Button         btnMoyenneSpe;
    private System.Windows.Forms.Button         btnGraphe;
    private System.Windows.Forms.Button         btnClose;
}
