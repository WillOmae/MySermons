using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class ColorComboBox : ComboBox
    {
        public Color SelectedColor;

        public ColorComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            Text = "";
            SelectedText = "";
            SelectedIndexChanged += delegate
            {
                UpdateColorComboBox((Color)SelectedItem);
            };
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (Items != null && Items.Count > 0)
            {
                Graphics g = e.Graphics;
                Rectangle rect = e.Bounds;
                if (e.Index >= 0)
                {
                    Color itemColor = (Color)Items[e.Index];
                    using (Brush brush = new SolidBrush(itemColor))
                    {
                        g.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                    }
                }
            }
        }
        public void UpdateColorComboBox(Color Selected)
        {
            try
            {
                if ((Color)SelectedItem != Color.Transparent)
                {
                    Text = "";
                    SelectedText = "";
                    BackColor = Selected;
                    SelectedColor = Selected;
                }
            }
            catch { }
        }
    }
}
