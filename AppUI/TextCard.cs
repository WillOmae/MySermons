using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class TextCard : TextBox
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
        private string textToDisplay = "";
        public int MaximumWidth = 300;

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoSize = false;
            Font = new Font("Times New Roman", 15F);
            Width = MaximumWidth - new VScrollBar().Width - 5;
            SizeF size = CreateGraphics().MeasureString(textToDisplay, Font, Width);
            Height = (int)size.Height;
            Multiline = true;
            ReadOnly = true;
            BackColor = Color.White;
            Text = textToDisplay;
            TextAlign = HorizontalAlignment.Left;
            BorderStyle = BorderStyle.Fixed3D;

            ResumeLayout(true);
        }
    }
}
