namespace OrientationPFIEV.Forms;

partial class FrmSession
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
        grpConcours           = new System.Windows.Forms.GroupBox();
        lblAnneeCaption       = new System.Windows.Forms.Label();
        lblAnneeValue         = new System.Windows.Forms.Label();
        lblMoyMinCaption      = new System.Windows.Forms.Label();
        lblMoyMinValue        = new System.Windows.Forms.Label();
        dgvCandidats          = new System.Windows.Forms.DataGridView();
        panelRight            = new System.Windows.Forms.Panel();
        btnAjouter            = new System.Windows.Forms.Button();
        btnModifier           = new System.Windows.Forms.Button();
        btnEffacer            = new System.Windows.Forms.Button();
        btnFixerNotes         = new System.Windows.Forms.Button();
        btnFixerChoix         = new System.Windows.Forms.Button();
        btnParamConcours      = new System.Windows.Forms.Button();
        panelBottom           = new System.Windows.Forms.Panel();
        btnImporterExcel      = new System.Windows.Forms.Button();
        btnExporterExcel      = new System.Windows.Forms.Button();
        btnLancerTraitement   = new System.Windows.Forms.Button();
        btnVisualiserResultats = new System.Windows.Forms.Button();
        btnQuitter            = new System.Windows.Forms.Button();

        grpConcours.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCandidats).BeginInit();
        panelRight.SuspendLayout();
        panelBottom.SuspendLayout();
        SuspendLayout();

        // grpConcours
        grpConcours.Location = new System.Drawing.Point(12, 8);
        grpConcours.Size     = new System.Drawing.Size(760, 54);
        grpConcours.Text     = "Paramètres du concours";
        grpConcours.Controls.Add(lblAnneeCaption);
        grpConcours.Controls.Add(lblAnneeValue);
        grpConcours.Controls.Add(lblMoyMinCaption);
        grpConcours.Controls.Add(lblMoyMinValue);

        // lblAnneeCaption
        lblAnneeCaption.Location = new System.Drawing.Point(12, 22);
        lblAnneeCaption.Size     = new System.Drawing.Size(50, 20);
        lblAnneeCaption.Text     = "Année :";

        // lblAnneeValue
        lblAnneeValue.Location  = new System.Drawing.Point(66, 22);
        lblAnneeValue.Size      = new System.Drawing.Size(80, 20);
        lblAnneeValue.Font      = new System.Drawing.Font(System.Drawing.SystemFonts.DefaultFont.FontFamily, 9f, System.Drawing.FontStyle.Bold);

        // lblMoyMinCaption
        lblMoyMinCaption.Location = new System.Drawing.Point(200, 22);
        lblMoyMinCaption.Size     = new System.Drawing.Size(120, 20);
        lblMoyMinCaption.Text     = "Moyenne minimale :";

        // lblMoyMinValue
        lblMoyMinValue.Location = new System.Drawing.Point(324, 22);
        lblMoyMinValue.Size     = new System.Drawing.Size(80, 20);
        lblMoyMinValue.Font     = new System.Drawing.Font(System.Drawing.SystemFonts.DefaultFont.FontFamily, 9f, System.Drawing.FontStyle.Bold);

        // dgvCandidats
        dgvCandidats.Location              = new System.Drawing.Point(12, 70);
        dgvCandidats.Size                  = new System.Drawing.Size(640, 460);
        dgvCandidats.ReadOnly              = true;
        dgvCandidats.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvCandidats.MultiSelect           = false;
        dgvCandidats.AllowUserToAddRows    = false;
        dgvCandidats.AllowUserToDeleteRows = false;
        dgvCandidats.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        dgvCandidats.RowHeadersVisible     = false;
        dgvCandidats.BackgroundColor       = System.Drawing.SystemColors.Window;

        // panelRight
        panelRight.Location = new System.Drawing.Point(660, 70);
        panelRight.Size     = new System.Drawing.Size(148, 460);
        panelRight.Controls.Add(btnAjouter);
        panelRight.Controls.Add(btnModifier);
        panelRight.Controls.Add(btnEffacer);
        panelRight.Controls.Add(btnFixerNotes);
        panelRight.Controls.Add(btnFixerChoix);
        panelRight.Controls.Add(btnParamConcours);

        // right-panel buttons — stacked vertically
        int bx = 4, by = 0, bw = 140, bh = 32, gap = 10;
        btnAjouter.Location       = new System.Drawing.Point(bx, by);        by += bh + gap;
        btnModifier.Location      = new System.Drawing.Point(bx, by);        by += bh + gap;
        btnEffacer.Location       = new System.Drawing.Point(bx, by);        by += bh + gap + 10;
        btnFixerNotes.Location    = new System.Drawing.Point(bx, by);        by += bh + gap;
        btnFixerChoix.Location    = new System.Drawing.Point(bx, by);        by += bh + gap + 10;
        btnParamConcours.Location = new System.Drawing.Point(bx, by);

        foreach (System.Windows.Forms.Button b in new[] { btnAjouter, btnModifier, btnEffacer,
                                                           btnFixerNotes, btnFixerChoix, btnParamConcours })
            b.Size = new System.Drawing.Size(bw, bh);

        btnAjouter.Text        = "Ajouter";
        btnModifier.Text       = "Modifier";
        btnEffacer.Text        = "Effacer";
        btnFixerNotes.Text     = "Fixer notes";
        btnFixerChoix.Text     = "Fixer choix";
        btnParamConcours.Text  = "Param. concours";

        btnAjouter.Click        += new System.EventHandler(btnAjouter_Click);
        btnModifier.Click       += new System.EventHandler(btnModifier_Click);
        btnEffacer.Click        += new System.EventHandler(btnEffacer_Click);
        btnFixerNotes.Click     += new System.EventHandler(btnFixerNotes_Click);
        btnFixerChoix.Click     += new System.EventHandler(btnFixerChoix_Click);
        btnParamConcours.Click  += new System.EventHandler(btnParamConcours_Click);

        // panelBottom
        panelBottom.Location = new System.Drawing.Point(12, 538);
        panelBottom.Size     = new System.Drawing.Size(796, 40);
        panelBottom.Controls.Add(btnImporterExcel);
        panelBottom.Controls.Add(btnExporterExcel);
        panelBottom.Controls.Add(btnLancerTraitement);
        panelBottom.Controls.Add(btnVisualiserResultats);
        panelBottom.Controls.Add(btnQuitter);

        int bpx = 0;
        btnImporterExcel.Location       = new System.Drawing.Point(bpx, 4); bpx += 130 + 6;
        btnExporterExcel.Location       = new System.Drawing.Point(bpx, 4); bpx += 130 + 6;
        btnLancerTraitement.Location    = new System.Drawing.Point(bpx, 4); bpx += 160 + 6;
        btnVisualiserResultats.Location = new System.Drawing.Point(bpx, 4); bpx += 160 + 6;
        btnQuitter.Location             = new System.Drawing.Point(bpx, 4);

        btnImporterExcel.Size       = new System.Drawing.Size(130, 30);
        btnExporterExcel.Size       = new System.Drawing.Size(130, 30);
        btnLancerTraitement.Size    = new System.Drawing.Size(160, 30);
        btnVisualiserResultats.Size = new System.Drawing.Size(160, 30);
        btnQuitter.Size             = new System.Drawing.Size(110, 30);

        btnImporterExcel.Text       = "Importer Excel";
        btnExporterExcel.Text       = "Exporter Excel";
        btnLancerTraitement.Text    = "Lancer traitement";
        btnVisualiserResultats.Text = "Anciens résultats";
        btnQuitter.Text             = "Quitter";

        btnImporterExcel.Click       += new System.EventHandler(btnImporterExcel_Click);
        btnExporterExcel.Click       += new System.EventHandler(btnExporterExcel_Click);
        btnLancerTraitement.Click    += new System.EventHandler(btnLancerTraitement_Click);
        btnVisualiserResultats.Click += new System.EventHandler(btnVisualiserResultats_Click);
        btnQuitter.Click             += new System.EventHandler(btnQuitter_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(820, 590);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name                = "FrmSession";
        Text                = "Session";
        Controls.Add(grpConcours);
        Controls.Add(dgvCandidats);
        Controls.Add(panelRight);
        Controls.Add(panelBottom);

        grpConcours.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCandidats).EndInit();
        panelRight.ResumeLayout(false);
        panelBottom.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox        grpConcours;
    private System.Windows.Forms.Label           lblAnneeCaption;
    private System.Windows.Forms.Label           lblAnneeValue;
    private System.Windows.Forms.Label           lblMoyMinCaption;
    private System.Windows.Forms.Label           lblMoyMinValue;
    private System.Windows.Forms.DataGridView    dgvCandidats;
    private System.Windows.Forms.Panel           panelRight;
    private System.Windows.Forms.Button          btnAjouter;
    private System.Windows.Forms.Button          btnModifier;
    private System.Windows.Forms.Button          btnEffacer;
    private System.Windows.Forms.Button          btnFixerNotes;
    private System.Windows.Forms.Button          btnFixerChoix;
    private System.Windows.Forms.Button          btnParamConcours;
    private System.Windows.Forms.Panel           panelBottom;
    private System.Windows.Forms.Button          btnImporterExcel;
    private System.Windows.Forms.Button          btnExporterExcel;
    private System.Windows.Forms.Button          btnLancerTraitement;
    private System.Windows.Forms.Button          btnVisualiserResultats;
    private System.Windows.Forms.Button          btnQuitter;
}
