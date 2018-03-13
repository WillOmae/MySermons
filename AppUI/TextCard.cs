using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class TextCard : Label
    {
        public string DisplayText
        {
            get
            {
                return textToDisplay;
            }
            set
            {
                textToDisplay = value;
                InitializeComponent();
            }
        }
        private string textToDisplay = string.Empty;
        public int MaximumWidth = 250;

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoSize = false;
            Font = new Font("Times New Roman", 15F);
            Width = MaximumWidth;
            SizeF size = CreateGraphics().MeasureString(textToDisplay, Font, Width);
            Height = (int)size.Height;
            BackColor = Color.White;
            Text = textToDisplay;
            BorderStyle = BorderStyle.Fixed3D;

            ResumeLayout(true);

            MouseClick += TextCard_MouseClick;
        }

        private void TextCard_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.SetText(DisplayText);
            }
        }
    }
}
