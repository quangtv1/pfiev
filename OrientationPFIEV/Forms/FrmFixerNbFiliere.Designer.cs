namespace OrientationPFIEV.Forms;

partial class FrmFixerNbFiliere
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
        lblMaxChoices = new System.Windows.Forms.Label();
        txtMaxChoices = new System.Windows.Forms.TextBox();
        btnOk = new System.Windows.Forms.Button();
        btnAnnuler = new System.Windows.Forms.Button();

        SuspendLayout();

        // lblMaxChoices
        lblMaxChoices.Location = new System.Drawing.Point(12, 15);
        lblMaxChoices.Size = new System.Drawing.Size(220, 20);
        lblMaxChoices.Text = "Nombre max de filières par candidat :";

        // txtMaxChoices
        txtMaxChoices.Location = new System.Drawing.Point(12, 40);
        txtMaxChoices.Size = new System.Drawing.Size(80, 23);
        txtMaxChoices.MaxLength = 4;
        txtMaxChoices.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMaxChoices_KeyPress);

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
        ClientSize = new System.Drawing.Size(244, 124);
        Controls.Add(lblMaxChoices);
        Controls.Add(txtMaxChoices);
        Controls.Add(btnOk);
        Controls.Add(btnAnnuler);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Name = "FrmFixerNbFiliere";
        Text = "Fixer nombre de filières";

        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label lblMaxChoices;
    private System.Windows.Forms.TextBox txtMaxChoices;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnAnnuler;
}
