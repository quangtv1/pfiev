namespace OrientationPFIEV.Forms;

partial class FrmResultats
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
        dgvResultats = new System.Windows.Forms.DataGridView();
        pnlButtons   = new System.Windows.Forms.Panel();
        btnExport    = new System.Windows.Forms.Button();
        btnTableaux  = new System.Windows.Forms.Button();
        btnRetour    = new System.Windows.Forms.Button();

        ((System.ComponentModel.ISupportInitialize)dgvResultats).BeginInit();
        pnlButtons.SuspendLayout();
        SuspendLayout();

        // dgvResultats
        dgvResultats.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        dgvResultats.Location = new System.Drawing.Point(12, 12);
        dgvResultats.Size     = new System.Drawing.Size(860, 500);
        dgvResultats.TabIndex = 0;
        dgvResultats.ReadOnly = true;
        dgvResultats.AllowUserToAddRows    = false;
        dgvResultats.AllowUserToDeleteRows = false;
        dgvResultats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvResultats.MultiSelect   = false;
        dgvResultats.RowHeadersVisible = false;
        dgvResultats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

        // pnlButtons — right side panel
        pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        pnlButtons.Location = new System.Drawing.Point(880, 12);
        pnlButtons.Size     = new System.Drawing.Size(170, 500);
        pnlButtons.Controls.Add(btnExport);
        pnlButtons.Controls.Add(btnTableaux);
        pnlButtons.Controls.Add(btnRetour);

        // btnExport
        btnExport.Location = new System.Drawing.Point(0, 0);
        btnExport.Size     = new System.Drawing.Size(170, 34);
        btnExport.TabIndex = 0;
        btnExport.Text     = "Exporter vers Excel";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += new System.EventHandler(btnExport_Click);

        // btnTableaux
        btnTableaux.Location = new System.Drawing.Point(0, 44);
        btnTableaux.Size     = new System.Drawing.Size(170, 34);
        btnTableaux.TabIndex = 1;
        btnTableaux.Text     = "Tableaux et statistiques";
        btnTableaux.UseVisualStyleBackColor = true;
        btnTableaux.Click += new System.EventHandler(btnTableaux_Click);

        // btnRetour
        btnRetour.Location = new System.Drawing.Point(0, 88);
        btnRetour.Size     = new System.Drawing.Size(170, 34);
        btnRetour.TabIndex = 2;
        btnRetour.Text     = "Retour";
        btnRetour.UseVisualStyleBackColor = true;
        btnRetour.Click += new System.EventHandler(btnRetour_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(1064, 524);
        Controls.Add(dgvResultats);
        Controls.Add(pnlButtons);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        MinimumSize     = new System.Drawing.Size(800, 400);
        StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name            = "FrmResultats";
        Text            = "Résultats du traitement des données";

        ((System.ComponentModel.ISupportInitialize)dgvResultats).EndInit();
        pnlButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.DataGridView dgvResultats;
    private System.Windows.Forms.Panel        pnlButtons;
    private System.Windows.Forms.Button       btnExport;
    private System.Windows.Forms.Button       btnTableaux;
    private System.Windows.Forms.Button       btnRetour;
}
