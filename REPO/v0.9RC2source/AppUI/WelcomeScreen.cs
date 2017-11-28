using AppEngine;
using System.Windows.Forms;

namespace AppUI
{
    public class WelcomeScreen : Form
    {
        private CheckBox chkbxHide;
        private TextBox textBox1;

        public WelcomeScreen()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreen));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkbxHide = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Square721 BT", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(412, 186);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkbxHide
            // 
            this.chkbxHide.AutoSize = true;
            this.chkbxHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkbxHide.Location = new System.Drawing.Point(260, 192);
            this.chkbxHide.Name = "chkbxHide";
            this.chkbxHide.Size = new System.Drawing.Size(140, 19);
            this.chkbxHide.TabIndex = 1;
            this.chkbxHide.Text = "Don\'t show this again";
            this.chkbxHide.UseVisualStyleBackColor = true;
            this.chkbxHide.CheckedChanged += new System.EventHandler(this.ChkbxHide_CheckedChanged);
            // 
            // WelcomeScreen
            // 
            this.ClientSize = new System.Drawing.Size(412, 220);
            this.Controls.Add(this.chkbxHide);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ahoy there!";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ChkbxHide_CheckedChanged(object sender, System.EventArgs e)
        {
            Preferences.ShowWelcomeScreen = !chkbxHide.Checked;
        }
    }
}
