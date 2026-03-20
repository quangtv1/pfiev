namespace OrientationPFIEV.Forms;

partial class FrmFixerNbrePlace
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
        lblNbPlace = new System.Windows.Forms.Label();
        txtNbPlace = new System.Windows.Forms.TextBox();
        btnOk = new System.Windows.Forms.Button();
        btnAnnuler = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblNbPlace
        lblNbPlace.Location = new System.Drawing.Point(12, 15);
        lblNbPlace.Size = new System.Drawing.Size(200, 20);
        lblNbPlace.Text = "Nombre de places global :";

        // txtNbPlace
        txtNbPlace.Location = new System.Drawing.Point(12, 40);
        txtNbPlace.Size = new System.Drawing.Size(80, 23);
        txtNbPlace.MaxLength = 6;
        txtNbPlace.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNbPlace_KeyPress);

        // btnOk
        btnOk.Location = new System.Drawing.Point(12, 80);
        btnOk.Size = new System.Drawing.Size(90, 28);
        btnOk.Text = "OK";
        btnOk.Click += new System.EventHandler(btnOk_Click);

        // btnAnnuler
        btnAnnuler.Location = new System.Drawing.Point(112, 80);
        btnAnnuler.Size = new System.Drawing.Size(90, 28);
        btnAnnuler.Text = "Annuler";
        btnAnnuler.Click += new System.EventHandler(btnAnnuler_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(224, 124);
        Controls.Add(lblNbPlace);
        Controls.Add(txtNbPlace);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Name = "FrmFixerNbrePlace";
        Text = "Fixer nombre de places";

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblNbPlace;
    private System.Windows.Forms.TextBox txtNbPlace;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnAnnuler;
}
