using System;
using System.Windows.Forms;

namespace AppUI
{
    public class TextCardHolder : Form
    {
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
            LostFocus += EhLostFocus;

            int height = 0;
            int scrollBarWidth = new VScrollBar().Width;
            int textCardWidth = 250;
            foreach (string entry in arrayDisplayText)
            {
                TextCard cardDisplay = new TextCard()
                {
                    DisplayText = entry,
                    Left = ((Width - scrollBarWidth - textCardWidth) / 2),
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
