using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class TextBoxEx : TextBox
    {
        public string Watermark
        {
            get
            {
                return szWatermark;
            }
            set
            {
                szWatermark = value;

                ForeColor = Color.Gray;
                Text = value;
                DeselectAll();
                
                Enter += new EventHandler(ControlEntered);
                Leave += new EventHandler(ControlLeft);

            }
        }
        private string szWatermark = "";

        private void ControlEntered(object sender, EventArgs e)
        {
            if (Text == szWatermark)
            {
                Clear();
                ForeColor = Color.Black;
            }
        }

        private void ControlLeft(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text) == true)
            {
                Text = szWatermark;
                ForeColor = Color.Gray;
            }
        }
    }
}
