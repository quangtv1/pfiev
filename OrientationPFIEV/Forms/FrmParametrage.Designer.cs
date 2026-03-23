namespace OrientationPFIEV.Forms;

partial class FrmParametrage
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
        tabControl = new System.Windows.Forms.TabControl();
        tabMatieres = new System.Windows.Forms.TabPage();
        listBoxMatieres = new System.Windows.Forms.ListBox();
        panelMatieresBtns = new System.Windows.Forms.Panel();
        btnAjouterMatiere = new System.Windows.Forms.Button();
        btnModifierMatiere = new System.Windows.Forms.Button();
        btnEffacerMatiere = new System.Windows.Forms.Button();
        tabEtablissements = new System.Windows.Forms.TabPage();
        listBoxEtabs = new System.Windows.Forms.ListBox();
        panelEtabsBtns = new System.Windows.Forms.Panel();
        btnAjouterEtab = new System.Windows.Forms.Button();
        btnModifierEtab = new System.Windows.Forms.Button();
        btnEffacerEtab = new System.Windows.Forms.Button();
        tabFilieres = new System.Windows.Forms.TabPage();
        listBoxFilieres = new System.Windows.Forms.ListBox();
        panelFilieresBtns = new System.Windows.Forms.Panel();
        btnAjouterFiliere = new System.Windows.Forms.Button();
        btnModifierFiliere = new System.Windows.Forms.Button();
        btnEffacerFiliere = new System.Windows.Forms.Button();
        btnFixerNbrePlaces = new System.Windows.Forms.Button();
        btnQuitter = new System.Windows.Forms.Button();

        tabControl.SuspendLayout();
        tabMatieres.SuspendLayout();
        panelMatieresBtns.SuspendLayout();
        tabEtablissements.SuspendLayout();
        panelEtabsBtns.SuspendLayout();
        tabFilieres.SuspendLayout();
        panelFilieresBtns.SuspendLayout();
        SuspendLayout();

        // ── tabControl ────────────────────────────────────────────────────────
        tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
        tabControl.Controls.Add(tabMatieres);
        tabControl.Controls.Add(tabEtablissements);
        tabControl.Controls.Add(tabFilieres);
        tabControl.Location = new System.Drawing.Point(12, 12);
        tabControl.Size = new System.Drawing.Size(660, 380);
        tabControl.Name = "tabControl";

        // ── Tab: Matières ─────────────────────────────────────────────────────
        tabMatieres.Controls.Add(listBoxMatieres);
        tabMatieres.Controls.Add(panelMatieresBtns);
        tabMatieres.Location = new System.Drawing.Point(4, 24);
        tabMatieres.Size = new System.Drawing.Size(652, 352);
        tabMatieres.Name = "tabMatieres";
        tabMatieres.Text = "Matières";
        tabMatieres.Padding = new System.Windows.Forms.Padding(6);

        listBoxMatieres.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left;
        listBoxMatieres.Location = new System.Drawing.Point(6, 6);
        listBoxMatieres.Size = new System.Drawing.Size(454, 340);
        listBoxMatieres.Name = "listBoxMatieres";

        panelMatieresBtns.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        panelMatieresBtns.Controls.Add(btnAjouterMatiere);
        panelMatieresBtns.Controls.Add(btnModifierMatiere);
        panelMatieresBtns.Controls.Add(btnEffacerMatiere);
        panelMatieresBtns.Location = new System.Drawing.Point(466, 6);
        panelMatieresBtns.Size = new System.Drawing.Size(180, 340);
        panelMatieresBtns.Name = "panelMatieresBtns";

        btnAjouterMatiere.Location = new System.Drawing.Point(0, 8);
        btnAjouterMatiere.Size = new System.Drawing.Size(170, 30);
        btnAjouterMatiere.Text = "Ajouter";
        btnAjouterMatiere.Click += new System.EventHandler(btnAjouterMatiere_Click);

        btnModifierMatiere.Location = new System.Drawing.Point(0, 46);
        btnModifierMatiere.Size = new System.Drawing.Size(170, 30);
        btnModifierMatiere.Text = "Modifier";
        btnModifierMatiere.Click += new System.EventHandler(btnModifierMatiere_Click);

        btnEffacerMatiere.Location = new System.Drawing.Point(0, 84);
        btnEffacerMatiere.Size = new System.Drawing.Size(170, 30);
        btnEffacerMatiere.Text = "Effacer";
        btnEffacerMatiere.Click += new System.EventHandler(btnEffacerMatiere_Click);

        // ── Tab: Etablissements ───────────────────────────────────────────────
        tabEtablissements.Controls.Add(listBoxEtabs);
        tabEtablissements.Controls.Add(panelEtabsBtns);
        tabEtablissements.Location = new System.Drawing.Point(4, 24);
        tabEtablissements.Size = new System.Drawing.Size(652, 352);
        tabEtablissements.Name = "tabEtablissements";
        tabEtablissements.Text = "Établissements";
        tabEtablissements.Padding = new System.Windows.Forms.Padding(6);

        listBoxEtabs.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left;
        listBoxEtabs.Location = new System.Drawing.Point(6, 6);
        listBoxEtabs.Size = new System.Drawing.Size(454, 340);
        listBoxEtabs.Name = "listBoxEtabs";

        panelEtabsBtns.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        panelEtabsBtns.Controls.Add(btnAjouterEtab);
        panelEtabsBtns.Controls.Add(btnModifierEtab);
        panelEtabsBtns.Controls.Add(btnEffacerEtab);
        panelEtabsBtns.Location = new System.Drawing.Point(466, 6);
        panelEtabsBtns.Size = new System.Drawing.Size(180, 340);
        panelEtabsBtns.Name = "panelEtabsBtns";

        btnAjouterEtab.Location = new System.Drawing.Point(0, 8);
        btnAjouterEtab.Size = new System.Drawing.Size(170, 30);
        btnAjouterEtab.Text = "Ajouter";
        btnAjouterEtab.Click += new System.EventHandler(btnAjouterEtab_Click);

        btnModifierEtab.Location = new System.Drawing.Point(0, 46);
        btnModifierEtab.Size = new System.Drawing.Size(170, 30);
        btnModifierEtab.Text = "Modifier";
        btnModifierEtab.Click += new System.EventHandler(btnModifierEtab_Click);

        btnEffacerEtab.Location = new System.Drawing.Point(0, 84);
        btnEffacerEtab.Size = new System.Drawing.Size(170, 30);
        btnEffacerEtab.Text = "Effacer";
        btnEffacerEtab.Click += new System.EventHandler(btnEffacerEtab_Click);

        // ── Tab: Filières ─────────────────────────────────────────────────────
        tabFilieres.Controls.Add(listBoxFilieres);
        tabFilieres.Controls.Add(panelFilieresBtns);
        tabFilieres.Location = new System.Drawing.Point(4, 24);
        tabFilieres.Size = new System.Drawing.Size(652, 352);
        tabFilieres.Name = "tabFilieres";
        tabFilieres.Text = "Filières";
        tabFilieres.Padding = new System.Windows.Forms.Padding(6);

        listBoxFilieres.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left;
        listBoxFilieres.Location = new System.Drawing.Point(6, 6);
        listBoxFilieres.Size = new System.Drawing.Size(454, 340);
        listBoxFilieres.Name = "listBoxFilieres";

        panelFilieresBtns.Anchor = System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        panelFilieresBtns.Controls.Add(btnAjouterFiliere);
        panelFilieresBtns.Controls.Add(btnModifierFiliere);
        panelFilieresBtns.Controls.Add(btnEffacerFiliere);
        panelFilieresBtns.Controls.Add(btnFixerNbrePlaces);
        panelFilieresBtns.Location = new System.Drawing.Point(466, 6);
        panelFilieresBtns.Size = new System.Drawing.Size(180, 340);
        panelFilieresBtns.Name = "panelFilieresBtns";

        btnAjouterFiliere.Location = new System.Drawing.Point(0, 8);
        btnAjouterFiliere.Size = new System.Drawing.Size(170, 30);
        btnAjouterFiliere.Text = "Ajouter";
        btnAjouterFiliere.Click += new System.EventHandler(btnAjouterFiliere_Click);

        btnModifierFiliere.Location = new System.Drawing.Point(0, 46);
        btnModifierFiliere.Size = new System.Drawing.Size(170, 30);
        btnModifierFiliere.Text = "Modifier";
        btnModifierFiliere.Click += new System.EventHandler(btnModifierFiliere_Click);

        btnEffacerFiliere.Location = new System.Drawing.Point(0, 84);
        btnEffacerFiliere.Size = new System.Drawing.Size(170, 30);
        btnEffacerFiliere.Text = "Effacer";
        btnEffacerFiliere.Click += new System.EventHandler(btnEffacerFiliere_Click);

        btnFixerNbrePlaces.Location = new System.Drawing.Point(0, 130);
        btnFixerNbrePlaces.Size = new System.Drawing.Size(170, 40);
        btnFixerNbrePlaces.Text = "Fixer nombre de places général";
        btnFixerNbrePlaces.Click += new System.EventHandler(btnFixerNbrePlaces_Click);

        // ── btnQuitter ────────────────────────────────────────────────────────
        btnQuitter.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Right;
        btnQuitter.Location = new System.Drawing.Point(556, 402);
        btnQuitter.Size = new System.Drawing.Size(116, 30);
        btnQuitter.Text = "Quitter le paramétrage";
        btnQuitter.Click += new System.EventHandler(btnQuitter_Click);

        // ── Form ──────────────────────────────────────────────────────────────
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(684, 444);
        Controls.Add(tabControl);
        Controls.Add(btnQuitter);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name = "FrmParametrage";
        Text = "Paramétrage";

        tabControl.ResumeLayout(false);
        tabMatieres.ResumeLayout(false);
        panelMatieresBtns.ResumeLayout(false);
        tabEtablissements.ResumeLayout(false);
        panelEtabsBtns.ResumeLayout(false);
        tabFilieres.ResumeLayout(false);
        panelFilieresBtns.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabMatieres;
    private System.Windows.Forms.ListBox listBoxMatieres;
    private System.Windows.Forms.Panel panelMatieresBtns;
    private System.Windows.Forms.Button btnAjouterMatiere;
    private System.Windows.Forms.Button btnModifierMatiere;
    private System.Windows.Forms.Button btnEffacerMatiere;
    private System.Windows.Forms.TabPage tabEtablissements;
    private System.Windows.Forms.ListBox listBoxEtabs;
    private System.Windows.Forms.Panel panelEtabsBtns;
    private System.Windows.Forms.Button btnAjouterEtab;
    private System.Windows.Forms.Button btnModifierEtab;
    private System.Windows.Forms.Button btnEffacerEtab;
    private System.Windows.Forms.TabPage tabFilieres;
    private System.Windows.Forms.ListBox listBoxFilieres;
    private System.Windows.Forms.Panel panelFilieresBtns;
    private System.Windows.Forms.Button btnAjouterFiliere;
    private System.Windows.Forms.Button btnModifierFiliere;
    private System.Windows.Forms.Button btnEffacerFiliere;
    private System.Windows.Forms.Button btnFixerNbrePlaces;
    private System.Windows.Forms.Button btnQuitter;
}
