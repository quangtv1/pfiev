namespace OrientationPFIEV.Forms;

partial class FrmSelectLangue
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
        btnFrancais = new System.Windows.Forms.Button();
        btnVietnamese = new System.Windows.Forms.Button();
        labelTitle = new System.Windows.Forms.Label();
        SuspendLayout();

        // labelTitle
        labelTitle.AutoSize = true;
        labelTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        labelTitle.Location = new System.Drawing.Point(30, 16);
        labelTitle.Size = new System.Drawing.Size(200, 19);
        labelTitle.Text = "Choisir la langue / Chọn ngôn ngữ";
        labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // btnFrancais
        btnFrancais.Location = new System.Drawing.Point(30, 50);
        btnFrancais.Size = new System.Drawing.Size(96, 64);
        btnFrancais.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
        btnFrancais.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        btnFrancais.Text = "Français";
        btnFrancais.TabIndex = 0;
        btnFrancais.Click += new System.EventHandler(btnFrancais_Click);

        // btnVietnamese
        btnVietnamese.Location = new System.Drawing.Point(140, 50);
        btnVietnamese.Size = new System.Drawing.Size(96, 64);
        btnVietnamese.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
        btnVietnamese.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        btnVietnamese.Text = "Tiếng Việt";
        btnVietnamese.TabIndex = 1;
        btnVietnamese.Click += new System.EventHandler(btnVietnamese_Click);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(266, 134);
        Controls.Add(labelTitle);
        Controls.Add(btnFrancais);
        Controls.Add(btnVietnamese);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name = "FrmSelectLangue";
        Text = "Langue / Ngôn ngữ";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button btnFrancais;
    private System.Windows.Forms.Button btnVietnamese;
    private System.Windows.Forms.Label labelTitle;
}
