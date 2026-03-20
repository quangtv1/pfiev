namespace OrientationPFIEV.Forms;

partial class FrmOuvertureSE
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
        lblPath    = new System.Windows.Forms.Label();
        txtPath    = new System.Windows.Forms.TextBox();
        btnBrowse  = new System.Windows.Forms.Button();
        lblAnnee   = new System.Windows.Forms.Label();
        txtAnnee   = new System.Windows.Forms.TextBox();
        lblMoyMin  = new System.Windows.Forms.Label();
        txtMoyMin  = new System.Windows.Forms.TextBox();
        btnOk      = new System.Windows.Forms.Button();
        btnAnnuler = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblPath
        lblPath.Location = new System.Drawing.Point(12, 15);
        lblPath.Size     = new System.Drawing.Size(130, 20);
        lblPath.Text     = "Fichier de session :";

        // txtPath
        txtPath.Location = new System.Drawing.Point(148, 12);
        txtPath.Size     = new System.Drawing.Size(240, 23);
        txtPath.ReadOnly = true;

        // btnBrowse
        btnBrowse.Location = new System.Drawing.Point(396, 11);
        btnBrowse.Size     = new System.Drawing.Size(36, 25);
        btnBrowse.Text     = "...";
        btnBrowse.Click   += new System.EventHandler(btnBrowse_Click);

        // lblAnnee
        lblAnnee.Location = new System.Drawing.Point(12, 50);
        lblAnnee.Size     = new System.Drawing.Size(130, 20);
        lblAnnee.Text     = "Année :";

        // txtAnnee
        txtAnnee.Location  = new System.Drawing.Point(148, 47);
        txtAnnee.Size      = new System.Drawing.Size(80, 23);
        txtAnnee.MaxLength = 4;
        txtAnnee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAnnee_KeyPress);

        // lblMoyMin
        lblMoyMin.Location = new System.Drawing.Point(12, 85);
        lblMoyMin.Size     = new System.Drawing.Size(130, 20);
        lblMoyMin.Text     = "Moyenne minimale :";

        // txtMoyMin
        txtMoyMin.Location  = new System.Drawing.Point(148, 82);
        txtMoyMin.Size      = new System.Drawing.Size(80, 23);
        txtMoyMin.MaxLength = 6;
        txtMoyMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMoyMin_KeyPress);

        // btnOk
        btnOk.Location = new System.Drawing.Point(248, 120);
        btnOk.Size     = new System.Drawing.Size(88, 28);
        btnOk.Text     = "OK";
        btnOk.Click   += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(344, 120);
        btnAnnuler.Size     = new System.Drawing.Size(88, 28);
        btnAnnuler.Text     = "Annuler";
        btnAnnuler.Click   += new System.EventHandler(btnAnnuler_Click);

        // Form
        AcceptButton          = btnOk;
        CancelButton          = btnAnnuler;
        AutoScaleDimensions   = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode         = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize            = new System.Drawing.Size(446, 162);
        FormBorderStyle       = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox           = false;
        MinimizeBox           = false;
        StartPosition         = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                  = "FrmOuvertureSE";
        Text                  = "Nouvelle session";
        Controls.Add(lblPath);
        Controls.Add(txtPath);
        Controls.Add(btnBrowse);
        Controls.Add(lblAnnee);
        Controls.Add(txtAnnee);
        Controls.Add(lblMoyMin);
        Controls.Add(txtMoyMin);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label   lblPath;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button  btnBrowse;
    private System.Windows.Forms.Label   lblAnnee;
    private System.Windows.Forms.TextBox txtAnnee;
    private System.Windows.Forms.Label   lblMoyMin;
    private System.Windows.Forms.TextBox txtMoyMin;
    private System.Windows.Forms.Button  btnOk;
    private System.Windows.Forms.Button  btnAnnuler;
}
