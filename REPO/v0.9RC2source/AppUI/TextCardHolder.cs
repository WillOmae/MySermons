using System;
using System.Windows.Forms;

namespace AppUI
{
    public class TextCardHolder : Form
    {
        private TextBox txbContent = new TextBox();

        public TextCardHolder(string[] arrayDisplayText, string szHeader)
        {
            MaximumSize = new System.Drawing.Size(300, 500);
            SuspendLayout();
            AutoSize = true;
            AutoScroll = true;
            ControlBox = MinimizeBox = MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ShowInTaskbar = false;
            ShowIcon = false;
            Size = new System.Drawing.Size(300, 0);
            TopLevel = TopMost = true;
            Text = szHeader;
            LostFocus += new EventHandler(EhLostFocus);
            
            int height = 0;
            foreach (string entry in arrayDisplayText)
            {
                TextCard cardDisplay = new TextCard()
                {
                    DisplayText = entry,
                    Top = height
                };
                cardDisplay.LostFocus += EhLostFocus;
                Controls.Add(cardDisplay);
                height += cardDisplay.Height + 3;
                Height = height;
            }
            HScroll = false;
            ResumeLayout(true);
            StartPosition = FormStartPosition.CenterScreen;
            Show();
        }
        private void EhLostFocus(object sender, EventArgs e)
        {
            if (!ContainsFocus)
                Close();
        }
    }
}
