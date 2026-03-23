namespace OrientationPFIEV.Forms;

partial class FrmAjouterFiliere
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
        lblNom = new System.Windows.Forms.Label();
        txtNom = new System.Windows.Forms.TextBox();
        lblCode = new System.Windows.Forms.Label();
        txtCode = new System.Windows.Forms.TextBox();
        lblNbPlace = new System.Windows.Forms.Label();
        txtNbPlace = new System.Windows.Forms.TextBox();
        lblEtab = new System.Windows.Forms.Label();
        cboEtab = new System.Windows.Forms.ComboBox();
        btnOk = new System.Windows.Forms.Button();
        btnAnnuler = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblNom
        lblNom.Location = new System.Drawing.Point(12, 15);
        lblNom.Size = new System.Drawing.Size(100, 20);
        lblNom.Text = "Nom :";

        // txtNom
        txtNom.Location = new System.Drawing.Point(118, 12);
        txtNom.Size = new System.Drawing.Size(200, 23);
        txtNom.MaxLength = 100;

        // lblCode
        lblCode.Location = new System.Drawing.Point(12, 48);
        lblCode.Size = new System.Drawing.Size(100, 20);
        lblCode.Text = "Code :";

        // txtCode
        txtCode.Location = new System.Drawing.Point(118, 45);
        txtCode.Size = new System.Drawing.Size(100, 23);
        txtCode.MaxLength = 20;

        // lblNbPlace
        lblNbPlace.Location = new System.Drawing.Point(12, 81);
        lblNbPlace.Size = new System.Drawing.Size(100, 20);
        lblNbPlace.Text = "Nb. places :";

        // txtNbPlace
        txtNbPlace.Location = new System.Drawing.Point(118, 78);
        txtNbPlace.Size = new System.Drawing.Size(80, 23);
        txtNbPlace.MaxLength = 6;
        txtNbPlace.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNbPlace_KeyPress);

        // lblEtab
        lblEtab.Location = new System.Drawing.Point(12, 114);
        lblEtab.Size = new System.Drawing.Size(100, 20);
        lblEtab.Text = "Établissement :";

        // cboEtab
        cboEtab.Location = new System.Drawing.Point(118, 111);
        cboEtab.Size = new System.Drawing.Size(200, 23);
        cboEtab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

        // btnOk
        btnOk.Location = new System.Drawing.Point(118, 151);
        btnOk.Size = new System.Drawing.Size(90, 28);
        btnOk.Text = "OK";
        btnOk.Click += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(218, 151);
        btnAnnuler.Size = new System.Drawing.Size(90, 28);
        btnAnnuler.Text = "Annuler";
        btnAnnuler.Click += new System.EventHandler(btnAnnuler_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(334, 196);
        Controls.Add(lblNom);
        Controls.Add(txtNom);
        Controls.Add(lblCode);
        Controls.Add(txtCode);
        Controls.Add(lblNbPlace);
        Controls.Add(txtNbPlace);
        Controls.Add(lblEtab);
        Controls.Add(cboEtab);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Name = "FrmAjouterFiliere";
        Text = "Ajouter Filière";

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblNom;
    private System.Windows.Forms.TextBox txtNom;
    private System.Windows.Forms.Label lblCode;
    private System.Windows.Forms.TextBox txtCode;
    private System.Windows.Forms.Label lblNbPlace;
    private System.Windows.Forms.TextBox txtNbPlace;
    private System.Windows.Forms.Label lblEtab;
    private System.Windows.Forms.ComboBox cboEtab;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnAnnuler;
}
