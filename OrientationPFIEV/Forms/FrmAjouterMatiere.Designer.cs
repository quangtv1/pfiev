namespace OrientationPFIEV.Forms;

partial class FrmAjouterMatiere
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
        lblCoefficient = new System.Windows.Forms.Label();
        txtCoefficient = new System.Windows.Forms.TextBox();
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

        // lblCoefficient
        lblCoefficient.Location = new System.Drawing.Point(12, 48);
        lblCoefficient.Size = new System.Drawing.Size(100, 20);
        lblCoefficient.Text = "Coefficient :";

        // txtCoefficient
        txtCoefficient.Location = new System.Drawing.Point(118, 45);
        txtCoefficient.Size = new System.Drawing.Size(80, 23);
        txtCoefficient.MaxLength = 10;
        txtCoefficient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCoefficient_KeyPress);

        // btnOk
        btnOk.Location = new System.Drawing.Point(118, 85);
        btnOk.Size = new System.Drawing.Size(90, 28);
        btnOk.Text = "OK";
        btnOk.DialogResult = System.Windows.Forms.DialogResult.None;
        btnOk.Click += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(218, 85);
        btnAnnuler.Size = new System.Drawing.Size(90, 28);
        btnAnnuler.Text = "Annuler";
        btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.None;
        btnAnnuler.Click += new System.EventHandler(btnAnnuler_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(334, 128);
        Controls.Add(lblNom);
        Controls.Add(txtNom);
        Controls.Add(lblCoefficient);
        Controls.Add(txtCoefficient);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Name = "FrmAjouterMatiere";
        Text = "Ajouter Matière";

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblNom;
    private System.Windows.Forms.TextBox txtNom;
    private System.Windows.Forms.Label lblCoefficient;
    private System.Windows.Forms.TextBox txtCoefficient;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnAnnuler;
}
