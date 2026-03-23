namespace OrientationPFIEV.Forms;

partial class FrmAccueil
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
        components = new System.ComponentModel.Container();
        pictureBoxLogo = new System.Windows.Forms.PictureBox();
        progressBar1 = new System.Windows.Forms.ProgressBar();
        timer1 = new System.Windows.Forms.Timer(components);

        ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
        SuspendLayout();

        // pictureBoxLogo — fills entire form
        pictureBoxLogo.Dock = System.Windows.Forms.DockStyle.Fill;
        pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        pictureBoxLogo.TabStop = false;

        // progressBar1 — anchored to bottom
        progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
        progressBar1.Height = 8;
        progressBar1.Maximum = 15;
        progressBar1.Minimum = 0;
        progressBar1.Value = 0;
        progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

        // timer1
        timer1.Interval = 100;
        timer1.Tick += new System.EventHandler(timer1_Tick);

        // Form
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(480, 320);
        Controls.Add(pictureBoxLogo);
        Controls.Add(progressBar1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Name = "FrmAccueil";
        Text = "OrientationPFIEV";

        ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.PictureBox pictureBoxLogo;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Timer timer1;
}
