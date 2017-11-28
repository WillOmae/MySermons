using AppEngine;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class SplashScreen : Form
    {
        private Label lbl1 = new Label();
        private Label lbl2 = new Label();

        public SplashScreen()
        {
            ShowSplashScreen();
        }
        private void ShowSplashScreen()
        {
            var helper = new WindowInteropHelper(this);
            var currentScreen = Screen.FromHandle(helper.Handle);
            FormClosing += new FormClosingEventHandler(EhClosing);
            Opacity = 0;
            TopMost = false; //Enabling other apps to be accessed while it loads
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = ColorExtractor.ExtractColor(Preferences.ColourControls);
            ForeColor = ColorExtractor.ExtractColor(Preferences.ColourFont);
            StartPosition = FormStartPosition.CenterScreen;

            Size = new Size(currentScreen.WorkingArea.Height * 3 / 4, currentScreen.WorkingArea.Height * 1 / 2);
            AddControls(Size, this);

            Show();
            Update();
            FadeIn();
        }
        private void AddControls(Size size, SplashScreen parent)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF stringSize;
                Font = new Font("Magneto", 18, FontStyle.Underline, GraphicsUnit.Point);

                lbl1.Name = "lbl1";
                lbl1.Text = Properties.Resources.AppName;
                lbl1.Font = new Font("Magneto", Font.Size, FontStyle.Underline, GraphicsUnit.Millimeter);
                stringSize = g.MeasureString(lbl1.Text, lbl1.Font);
                lbl1.Width = this.Width;
                lbl1.TextAlign = ContentAlignment.MiddleCenter;
                lbl1.Height = (int)stringSize.Height;
                lbl1.ForeColor = ForeColor;
                lbl1.Visible = true;
                lbl1.Location = new Point((size.Width - lbl1.Width) / 2, (size.Height / 2) - lbl1.Height);
                
                lbl2.Name = "lbl2";
                lbl2.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                lbl2.Font = new Font("Magneto", Font.Size, GraphicsUnit.Point);
                stringSize = g.MeasureString(lbl2.Text, lbl2.Font);
                lbl2.Width = this.Width;
                lbl2.TextAlign = ContentAlignment.MiddleCenter;
                lbl2.Height = (int)stringSize.Height;
                lbl2.ForeColor = ForeColor;
                lbl2.Visible = true;
                lbl2.Location = new Point((size.Width - lbl2.Width) / 2, (size.Height / 2));

                parent.Controls.Add(lbl1);
                parent.Controls.Add(lbl2);
            }
        }
        private void EhClosing(object sender, FormClosingEventArgs e)
        {
            FadeOut();
        }
        private void FadeIn()
        {
            while (Opacity < 1)
            {
                Opacity += 0.00001;
            }
        }
        private void FadeOut()
        {
            while (Opacity > 0)
            {
                Opacity -= 0.00001;
            }
        }
    }
}
