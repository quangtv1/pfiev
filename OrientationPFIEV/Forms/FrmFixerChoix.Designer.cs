namespace OrientationPFIEV.Forms;

partial class FrmFixerChoix
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
        lblAvailable = new System.Windows.Forms.Label();
        lbAvailable  = new System.Windows.Forms.ListBox();
        lblChoices   = new System.Windows.Forms.Label();
        lbChoices    = new System.Windows.Forms.ListBox();
        panelMid     = new System.Windows.Forms.Panel();
        btnAdd       = new System.Windows.Forms.Button();
        btnRemove    = new System.Windows.Forms.Button();
        btnUp        = new System.Windows.Forms.Button();
        btnDown      = new System.Windows.Forms.Button();
        btnSave      = new System.Windows.Forms.Button();
        btnClose     = new System.Windows.Forms.Button();

        panelMid.SuspendLayout();
        SuspendLayout();

        int lbW = 200, lbH = 280, midW = 80;
        int lbTop = 30, lx1 = 12, lx2 = lx1 + lbW + midW + 8;

        // lblAvailable
        lblAvailable.Location = new System.Drawing.Point(lx1, 8);
        lblAvailable.Size     = new System.Drawing.Size(lbW, 18);
        lblAvailable.Text     = "Filières disponibles";

        // lbAvailable
        lbAvailable.Location = new System.Drawing.Point(lx1, lbTop);
        lbAvailable.Size     = new System.Drawing.Size(lbW, lbH);

        // panelMid (arrow buttons)
        panelMid.Location = new System.Drawing.Point(lx1 + lbW + 4, lbTop + 50);
        panelMid.Size     = new System.Drawing.Size(midW, 140);
        panelMid.Controls.Add(btnAdd);
        panelMid.Controls.Add(btnRemove);
        panelMid.Controls.Add(btnUp);
        panelMid.Controls.Add(btnDown);

        int bx = 8, bw = 64, bh = 26, by2 = 0;
        btnAdd.Location    = new System.Drawing.Point(bx, by2);    by2 += bh + 4;
        btnRemove.Location = new System.Drawing.Point(bx, by2);    by2 += bh + 10;
        btnUp.Location     = new System.Drawing.Point(bx, by2);    by2 += bh + 4;
        btnDown.Location   = new System.Drawing.Point(bx, by2);
        foreach (System.Windows.Forms.Button b in new[] { btnAdd, btnRemove, btnUp, btnDown })
            b.Size = new System.Drawing.Size(bw, bh);
        btnAdd.Text    = "→";
        btnRemove.Text = "←";
        btnUp.Text     = "↑";
        btnDown.Text   = "↓";
        btnAdd.Click    += new System.EventHandler(btnAdd_Click);
        btnRemove.Click += new System.EventHandler(btnRemove_Click);
        btnUp.Click     += new System.EventHandler(btnUp_Click);
        btnDown.Click   += new System.EventHandler(btnDown_Click);

        // lblChoices
        lblChoices.Location = new System.Drawing.Point(lx2, 8);
        lblChoices.Size     = new System.Drawing.Size(lbW, 18);
        lblChoices.Text     = "Choix du candidat";

        // lbChoices
        lbChoices.Location = new System.Drawing.Point(lx2, lbTop);
        lbChoices.Size     = new System.Drawing.Size(lbW, lbH);

        // btnSave / btnClose
        int btnY = lbTop + lbH + 12;
        btnSave.Location  = new System.Drawing.Point(lx2 + lbW - 160, btnY);
        btnSave.Size      = new System.Drawing.Size(76, 28);
        btnSave.Text      = "Enregistrer";
        btnSave.Click    += new System.EventHandler(btnSave_Click);

        btnClose.Location = new System.Drawing.Point(lx2 + lbW - 80, btnY);
        btnClose.Size     = new System.Drawing.Size(76, 28);
        btnClose.Text     = "Fermer";
        btnClose.Click   += new System.EventHandler(btnClose_Click);

        // Form
        CancelButton        = btnClose;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize          = new System.Drawing.Size(lx2 + lbW + 12, btnY + 46);
        FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox         = false;
        MinimizeBox         = false;
        StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
        Name                = "FrmFixerChoix";
        Text                = "Fixer les choix";
        Controls.Add(lblAvailable);
        Controls.Add(lbAvailable);
        Controls.Add(panelMid);
        Controls.Add(lblChoices);
        Controls.Add(lbChoices);
        Controls.Add(btnSave);
        Controls.Add(btnClose);

        panelMid.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label   lblAvailable;
    private System.Windows.Forms.ListBox lbAvailable;
    private System.Windows.Forms.Label   lblChoices;
    private System.Windows.Forms.ListBox lbChoices;
    private System.Windows.Forms.Panel   panelMid;
    private System.Windows.Forms.Button  btnAdd;
    private System.Windows.Forms.Button  btnRemove;
    private System.Windows.Forms.Button  btnUp;
    private System.Windows.Forms.Button  btnDown;
    private System.Windows.Forms.Button  btnSave;
    private System.Windows.Forms.Button  btnClose;
}
