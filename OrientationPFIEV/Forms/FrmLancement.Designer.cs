namespace OrientationPFIEV.Forms;

partial class FrmLancement
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
        groupBoxLaunch = new System.Windows.Forms.GroupBox();
        radioParametrage = new System.Windows.Forms.RadioButton();
        radioNouvelleSession = new System.Windows.Forms.RadioButton();
        radioSessionExistante = new System.Windows.Forms.RadioButton();
        comboBoxChemin = new System.Windows.Forms.ComboBox();
        btnOk = new System.Windows.Forms.Button();
        btnEffacer = new System.Windows.Forms.Button();
        btnQuitter = new System.Windows.Forms.Button();

        groupBoxLaunch.SuspendLayout();
        SuspendLayout();

        // groupBoxLaunch
        groupBoxLaunch.Controls.Add(radioParametrage);
        groupBoxLaunch.Controls.Add(radioNouvelleSession);
        groupBoxLaunch.Controls.Add(radioSessionExistante);
        groupBoxLaunch.Controls.Add(comboBoxChemin);
        groupBoxLaunch.Location = new System.Drawing.Point(12, 12);
        groupBoxLaunch.Size = new System.Drawing.Size(380, 160);
        groupBoxLaunch.TabStop = false;
        groupBoxLaunch.Text = "Lancement du logiciel";

        // radioParametrage
        radioParametrage.AutoSize = true;
        radioParametrage.Location = new System.Drawing.Point(16, 28);
        radioParametrage.Size = new System.Drawing.Size(200, 19);
        radioParametrage.TabIndex = 0;
        radioParametrage.Text = "Paramétrer le logiciel";
        radioParametrage.CheckedChanged += new System.EventHandler(radio_CheckedChanged);

        // radioNouvelleSession
        radioNouvelleSession.AutoSize = true;
        radioNouvelleSession.Location = new System.Drawing.Point(16, 60);
        radioNouvelleSession.Size = new System.Drawing.Size(200, 19);
        radioNouvelleSession.TabIndex = 1;
        radioNouvelleSession.Text = "Nouvelle session";
        radioNouvelleSession.CheckedChanged += new System.EventHandler(radio_CheckedChanged);

        // radioSessionExistante
        radioSessionExistante.AutoSize = true;
        radioSessionExistante.Location = new System.Drawing.Point(16, 92);
        radioSessionExistante.Size = new System.Drawing.Size(200, 19);
        radioSessionExistante.TabIndex = 2;
        radioSessionExistante.Text = "Session existante";
        radioSessionExistante.Checked = true;
        radioSessionExistante.CheckedChanged += new System.EventHandler(radio_CheckedChanged);

        // comboBoxChemin
        comboBoxChemin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxChemin.Location = new System.Drawing.Point(16, 120);
        comboBoxChemin.Size = new System.Drawing.Size(348, 23);
        comboBoxChemin.TabIndex = 3;

        // btnOk
        btnOk.Location = new System.Drawing.Point(12, 188);
        btnOk.Size = new System.Drawing.Size(88, 30);
        btnOk.TabIndex = 4;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += new System.EventHandler(btnOk_Click);

        // btnEffacer
        btnEffacer.Location = new System.Drawing.Point(112, 188);
        btnEffacer.Size = new System.Drawing.Size(148, 30);
        btnEffacer.TabIndex = 5;
        btnEffacer.Text = "Effacer historique";
        btnEffacer.UseVisualStyleBackColor = true;
        btnEffacer.Click += new System.EventHandler(btnEffacer_Click);

        // btnQuitter
        btnQuitter.Location = new System.Drawing.Point(272, 188);
        btnQuitter.Size = new System.Drawing.Size(88, 30);
        btnQuitter.TabIndex = 6;
        btnQuitter.Text = "Quitter";
        btnQuitter.UseVisualStyleBackColor = true;
        btnQuitter.Click += new System.EventHandler(btnQuitter_Click);

        // Form
        AcceptButton = btnOk;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(404, 232);
        Controls.Add(groupBoxLaunch);
        Controls.Add(btnOk);
        Controls.Add(btnEffacer);
        Controls.Add(btnQuitter);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name = "FrmLancement";
        Text = "OrientationPFIEV";

        groupBoxLaunch.ResumeLayout(false);
        groupBoxLaunch.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox groupBoxLaunch;
    private System.Windows.Forms.RadioButton radioParametrage;
    private System.Windows.Forms.RadioButton radioNouvelleSession;
    private System.Windows.Forms.RadioButton radioSessionExistante;
    private System.Windows.Forms.ComboBox comboBoxChemin;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnEffacer;
    private System.Windows.Forms.Button btnQuitter;
}
