namespace OrientationPFIEV.Forms;

partial class FrmAjouterEtab
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
        lblVille = new System.Windows.Forms.Label();
        txtVille = new System.Windows.Forms.TextBox();
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

        // lblVille
        lblVille.Location = new System.Drawing.Point(12, 81);
        lblVille.Size = new System.Drawing.Size(100, 20);
        lblVille.Text = "Ville :";

        // txtVille
        txtVille.Location = new System.Drawing.Point(118, 78);
        txtVille.Size = new System.Drawing.Size(200, 23);
        txtVille.MaxLength = 100;

        // btnOk
        btnOk.Location = new System.Drawing.Point(118, 118);
        btnOk.Size = new System.Drawing.Size(90, 28);
        btnOk.Text = "OK";
        btnOk.Click += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(218, 118);
        btnAnnuler.Size = new System.Drawing.Size(90, 28);
        btnAnnuler.Text = "Annuler";
        btnAnnuler.Click += new System.EventHandler(btnAnnuler_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(334, 162);
        Controls.Add(lblNom);
        Controls.Add(txtNom);
        Controls.Add(lblCode);
        Controls.Add(txtCode);
        Controls.Add(lblVille);
        Controls.Add(txtVille);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Name = "FrmAjouterEtab";
        Text = "Ajouter Établissement";

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblNom;
    private System.Windows.Forms.TextBox txtNom;
    private System.Windows.Forms.Label lblCode;
    private System.Windows.Forms.TextBox txtCode;
    private System.Windows.Forms.Label lblVille;
    private System.Windows.Forms.TextBox txtVille;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnAnnuler;
}
