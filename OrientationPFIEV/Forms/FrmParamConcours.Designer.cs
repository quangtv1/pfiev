namespace OrientationPFIEV.Forms;

partial class FrmParamConcours
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
        lblAnnee   = new System.Windows.Forms.Label();
        txtAnnee   = new System.Windows.Forms.TextBox();
        lblMoyMin  = new System.Windows.Forms.Label();
        txtMoyMin  = new System.Windows.Forms.TextBox();
        btnOk      = new System.Windows.Forms.Button();
        btnAnnuler = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblAnnee
        lblAnnee.Location = new System.Drawing.Point(12, 15);
        lblAnnee.Size     = new System.Drawing.Size(130, 20);
        lblAnnee.Text     = "Année :";

        // txtAnnee
        txtAnnee.Location  = new System.Drawing.Point(148, 12);
        txtAnnee.Size      = new System.Drawing.Size(80, 23);
        txtAnnee.MaxLength = 4;
        txtAnnee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAnnee_KeyPress);

        // lblMoyMin
        lblMoyMin.Location = new System.Drawing.Point(12, 50);
        lblMoyMin.Size     = new System.Drawing.Size(130, 20);
        lblMoyMin.Text     = "Moyenne minimale :";

        // txtMoyMin
        txtMoyMin.Location  = new System.Drawing.Point(148, 47);
        txtMoyMin.Size      = new System.Drawing.Size(80, 23);
        txtMoyMin.MaxLength = 6;
        txtMoyMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMoyMin_KeyPress);

        // btnOk
        btnOk.Location = new System.Drawing.Point(148, 86);
        btnOk.Size     = new System.Drawing.Size(80, 28);
        btnOk.Text     = "OK";
        btnOk.Click   += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(236, 86);
        btnAnnuler.Size     = new System.Drawing.Size(80, 28);
        btnAnnuler.Text     = "Annuler";
        btnAnnuler.Click   += new System.EventHandler(btnAnnuler_Click);

        // Form
        AcceptButton        = btnOk;
        CancelButton        = btnAnnuler;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(330, 130);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        MinimizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                = "FrmParamConcours";
        Text                = "Paramètres du concours";
        Controls.Add(lblAnnee);
        Controls.Add(txtAnnee);
        Controls.Add(lblMoyMin);
        Controls.Add(txtMoyMin);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label   lblAnnee;
    private System.Windows.Forms.TextBox txtAnnee;
    private System.Windows.Forms.Label   lblMoyMin;
    private System.Windows.Forms.TextBox txtMoyMin;
    private System.Windows.Forms.Button  btnOk;
    private System.Windows.Forms.Button  btnAnnuler;
}
