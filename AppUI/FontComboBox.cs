using System.Windows.Forms;
using System.Drawing;

namespace AppUI
{
    public class FontComboBox : ComboBox
    {
        private string SelectedFont;
        public FontComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            //DropDownStyle = ComboBoxStyle.DropDown;

            SelectedIndexChanged += delegate
            {
                SelectedFont = (string)SelectedItem;
                Font = new Font(SelectedFont, Font.SizeInPoints);
            };
        }
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            if (e.Index > -1 && e.Index < Items.Count)
            {
                e.ItemHeight = TextRenderer.MeasureText("yY", Font).Height + 2;
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Index > -1 && e.Index < Items.Count)
            {
                e.DrawBackground();

                if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    e.DrawFocusRectangle();
                }
                string itemName = Items[e.Index].ToString();
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), new Font(itemName, Font.SizeInPoints), brush, e.Bounds, StringFormat.GenericDefault);
                }
            }
        }
    }
}
