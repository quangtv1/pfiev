namespace OrientationPFIEV.Forms;

partial class FrmAjouterCandidat
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
        lblNom           = new System.Windows.Forms.Label();
        txtNom           = new System.Windows.Forms.TextBox();
        lblNomInter      = new System.Windows.Forms.Label();
        txtNomInter      = new System.Windows.Forms.TextBox();
        lblPrenom        = new System.Windows.Forms.Label();
        txtPrenom        = new System.Windows.Forms.TextBox();
        lblAnonymat      = new System.Windows.Forms.Label();
        txtAnonymat      = new System.Windows.Forms.TextBox();
        lblDOB           = new System.Windows.Forms.Label();
        dtpDateNaissance = new System.Windows.Forms.DateTimePicker();
        lblSexe          = new System.Windows.Forms.Label();
        comboSexe        = new System.Windows.Forms.ComboBox();
        lblStatut        = new System.Windows.Forms.Label();
        comboStatut      = new System.Windows.Forms.ComboBox();
        lblLangue        = new System.Windows.Forms.Label();
        comboLangue      = new System.Windows.Forms.ComboBox();
        lblEtab          = new System.Windows.Forms.Label();
        comboEtab        = new System.Windows.Forms.ComboBox();
        btnOk            = new System.Windows.Forms.Button();
        btnSuivant       = new System.Windows.Forms.Button();
        btnAnnuler       = new System.Windows.Forms.Button();

        SuspendLayout();

        int lx = 12, tx = 170, tw = 220, lw = 152, h = 23, gap = 34;
        int y = 12;

        // Row helpers
        void Row(System.Windows.Forms.Label lbl, System.Windows.Forms.Control ctrl, string lt)
        {
            lbl.Location = new System.Drawing.Point(lx, y + 2);
            lbl.Size     = new System.Drawing.Size(lw, 18);
            lbl.Text     = lt;
            ctrl.Location = new System.Drawing.Point(tx, y);
            ctrl.Size     = new System.Drawing.Size(tw, h);
            y += gap;
        }

        Row(lblNom,      txtNom,      "Nom :");
        Row(lblNomInter, txtNomInter, "Nom intermédiaire :");
        Row(lblPrenom,   txtPrenom,   "Prénom :");
        Row(lblAnonymat, txtAnonymat, "N° anonymat :");
        Row(lblDOB,      dtpDateNaissance, "Date de naissance :");
        Row(lblSexe,     comboSexe,   "Sexe :");
        Row(lblStatut,   comboStatut, "Statut (I/E) :");
        Row(lblLangue,   comboLangue, "Langue :");
        Row(lblEtab,     comboEtab,   "Etablissement :");

        // Combo styles
        comboSexe.DropDownStyle   = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboStatut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboLangue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboEtab.DropDownStyle   = System.Windows.Forms.ComboBoxStyle.DropDownList;

        // Buttons
        int btnY = y + 8;
        btnOk.Location      = new System.Drawing.Point(tx, btnY);
        btnOk.Size          = new System.Drawing.Size(68, 28);
        btnOk.Text          = "OK";
        btnOk.Click        += new System.EventHandler(btnOk_Click);

        btnSuivant.Location = new System.Drawing.Point(tx + 74, btnY);
        btnSuivant.Size     = new System.Drawing.Size(72, 28);
        btnSuivant.Text     = "Suivant";
        btnSuivant.Click   += new System.EventHandler(btnSuivant_Click);

        btnAnnuler.Location = new System.Drawing.Point(tx + 152, btnY);
        btnAnnuler.Size     = new System.Drawing.Size(72, 28);
        btnAnnuler.Text     = "Annuler";
        btnAnnuler.Click   += new System.EventHandler(btnAnnuler_Click);

        // Form
        AcceptButton        = btnOk;
        CancelButton        = btnAnnuler;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(408, btnY + 50);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        MinimizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                = "FrmAjouterCandidat";
        Text                = "Ajouter / Modifier candidat";

        Controls.Add(lblNom);      Controls.Add(txtNom);
        Controls.Add(lblNomInter); Controls.Add(txtNomInter);
        Controls.Add(lblPrenom);   Controls.Add(txtPrenom);
        Controls.Add(lblAnonymat); Controls.Add(txtAnonymat);
        Controls.Add(lblDOB);      Controls.Add(dtpDateNaissance);
        Controls.Add(lblSexe);     Controls.Add(comboSexe);
        Controls.Add(lblStatut);   Controls.Add(comboStatut);
        Controls.Add(lblLangue);   Controls.Add(comboLangue);
        Controls.Add(lblEtab);     Controls.Add(comboEtab);
        Controls.Add(btnOk);
        Controls.Add(btnSuivant);
        Controls.Add(btnAnnuler);

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label           lblNom;
    private System.Windows.Forms.TextBox         txtNom;
    private System.Windows.Forms.Label           lblNomInter;
    private System.Windows.Forms.TextBox         txtNomInter;
    private System.Windows.Forms.Label           lblPrenom;
    private System.Windows.Forms.TextBox         txtPrenom;
    private System.Windows.Forms.Label           lblAnonymat;
    private System.Windows.Forms.TextBox         txtAnonymat;
    private System.Windows.Forms.Label           lblDOB;
    private System.Windows.Forms.DateTimePicker  dtpDateNaissance;
    private System.Windows.Forms.Label           lblSexe;
    private System.Windows.Forms.ComboBox        comboSexe;
    private System.Windows.Forms.Label           lblStatut;
    private System.Windows.Forms.ComboBox        comboStatut;
    private System.Windows.Forms.Label           lblLangue;
    private System.Windows.Forms.ComboBox        comboLangue;
    private System.Windows.Forms.Label           lblEtab;
    private System.Windows.Forms.ComboBox        comboEtab;
    private System.Windows.Forms.Button          btnOk;
    private System.Windows.Forms.Button          btnSuivant;
    private System.Windows.Forms.Button          btnAnnuler;
}
