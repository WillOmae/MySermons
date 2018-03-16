using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    /// <summary>
    /// Represents a custom control consisting of a RichTextBoxEx control with a corresponding ToolStrip control.
    /// </summary>
    public class RichTextBoxPro : UserControl
    {
        private RichTextBox rtbTemp = new RichTextBox();

        public RichTextBoxEx rtb;

        private ToolStrip ts;
        private ToolStripButton tsiCopy, tsiCut, tsiPaste, tsiBold, tsiItalic, tsiUnderline, tsiAlignLeft, tsiAlignCentre, tsiAlignRight, tsiAlignJustify, tsiFontColor, tsiHighlightColor;
        private ToolStripSeparator tsiSeparator1, tsiSeparator2, tsiSeparator3, tsiSeparator4;
        private ToolStripComboBox tscmbxFontFamily, tscmbxFontSize;

        private ColorDialog cdlgFont = new ColorDialog();

        private FontFamily[] ffInstalledFonts = null;

        #region Access underlying rtbEx properties
        public string[] Lines
        {
            get
            {
                return rtb.Lines;
            }
        }
        public string Rtf
        {
            get
            {
                return rtb.Rtf;
            }
            set
            {
                rtb.Rtf = value;
            }
        }
        override public string Text
        {
            get
            {
                return rtb.Text;
            }
            set
            {
                rtb.Text = value;
            }
        }
        #endregion

        /// <summary>
        ///     Initializes a new instance of the RichTextBoxPro class.
        /// </summary>
        public RichTextBoxPro()
        {
            InitializeComponents();
            Dock = DockStyle.Top;
            Visible = true;
            SizeChanged += new EventHandler(ControlSizeChanged);
            //rtb.Font = new Font(tscmbxFontFamily.Text, float.Parse(tscmbxFontSize.Text));
            //UpdateToolbar();
        }

        private void InitializeComponents()
        {
            #region ****************** ToolStrip ******************
            ts = new ToolStrip()
            {
                ImageList = new ImageList(),
                Location = new Point(Left, Top),
                Name = "ts",
                ShowItemToolTips = true,
                Size = new Size(ClientSize.Width, 26),
                TabIndex = 0
            };
            ts.ItemClicked += new ToolStripItemClickedEventHandler(ToolStripItem_Click);
            #endregion

            #region ****************** RichTextBoxEx ******************
            rtb = new RichTextBoxEx()
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(Left, ts.Bottom + 2),
                Name = "rtb",
                Size = new Size(ClientSize.Width, ClientSize.Height - ts.Bottom)
            };
            rtb.SelectionChanged += RtbEx_SelectionChanged;
            rtb.KeyDown += Rtb_KeyDown;
            rtb.KeyPress += Rtb_KeyPress;
            if (RichTextBoxEx.ffInstalledFonts != null)
            {
                ffInstalledFonts = RichTextBoxEx.ffInstalledFonts;
            }
            else
            {
                ffInstalledFonts = new System.Drawing.Text.InstalledFontCollection().Families;
            }
            #endregion

            #region ****************** ToolStripButton Copy ******************
            tsiCopy = new ToolStripButton()
            {
                Name = "tsiCopy",
                Tag = "copy",
                ToolTipText = "Copy",
                ImageIndex = 0,
                CheckOnClick = false
            };
            #endregion

            #region ****************** ToolStripButton Cut ******************
            tsiCut = new ToolStripButton()
            {
                Name = "tsiCut",
                Tag = "cut",
                ToolTipText = "Cut",
                ImageIndex = 1,
                CheckOnClick = false
            };
            #endregion

            #region ****************** ToolStripButton Paste ******************
            tsiPaste = new ToolStripButton()
            {
                Name = "tsiPaste",
                Tag = "paste",
                ToolTipText = "Paste",
                ImageIndex = 2,
                CheckOnClick = false
            };
            #endregion

            #region ****************** ToolStripSeparator ******************
            tsiSeparator1 = new ToolStripSeparator()
            {
                Name = "tsiSeparator1",
                Tag = "separator",
                AutoSize = true
            };
            #endregion

            #region ****************** ToolStripButton Bold ******************
            tsiBold = new ToolStripButton()
            {
                Name = "tsiBold",
                Tag = "bold",
                ToolTipText = "Bold",
                ImageIndex = 3,
                CheckOnClick = true
            };
            #endregion

            #region ****************** ToolStripButton Italic ******************
            tsiItalic = new ToolStripButton()
            {
                Name = "tsiItalic",
                Tag = "italic",
                ToolTipText = "Italize",
                ImageIndex = 4,
                CheckOnClick = true
            };
            #endregion

            #region ****************** ToolStripButton Underline ******************
            tsiUnderline = new ToolStripButton()
            {
                Name = "tsiUnderline",
                Tag = "underline",
                ToolTipText = "Underline",
                ImageIndex = 5,
                CheckOnClick = true
            };
            #endregion

            #region ****************** ToolStripSeparator ******************
            tsiSeparator2 = new ToolStripSeparator()
            {
                Name = "tsiSeparator2",
                Tag = "separator"
            };
            #endregion

            #region ****************** ToolStripButton Align Left ******************
            tsiAlignLeft = new ToolStripButton()
            {
                Name = "tsiAlignLeft",
                Tag = "alignLeft",
                ToolTipText = "Align left",
                CheckOnClick = true,
                ImageIndex = 8
            };
            #endregion

            #region ****************** ToolStripButton Align Centre ******************
            tsiAlignCentre = new ToolStripButton()
            {
                Name = "tsiAlignCentre",
                Tag = "alignCentre",
                ToolTipText = "Align centre",
                CheckOnClick = true,
                ImageIndex = 9
            };
            #endregion

            #region ****************** ToolStripButton Align Right ******************
            tsiAlignRight = new ToolStripButton()
            {
                Name = "tsiAlignRight",
                Tag = "alignRight",
                ToolTipText = "Align right",
                CheckOnClick = true,
                ImageIndex = 10
            };
            #endregion

            #region ****************** ToolStripButton Align Justify ******************
            tsiAlignJustify = new ToolStripButton()
            {
                Name = "tsiAlignJustify",
                Tag = "alignJustify",
                ToolTipText = "Justify",
                CheckOnClick = true,
                ImageIndex = 11
            };
            #endregion

            #region ****************** ToolStripSeparator ******************
            tsiSeparator3 = new ToolStripSeparator()
            {
                Name = "tsiSeparator3",
                Tag = "separator"
            };
            #endregion

            #region ****************** ToolStripComboBox FontFamilies ******************
            tscmbxFontFamily = new ToolStripComboBox("Times New Roman")
            {
                Name = "tscmbxFontFamily",
                Text = "Times New Roman",
                ToolTipText = "Set font"
            };
            tscmbxFontFamily.SelectedIndexChanged += new EventHandler(Font_Click);
            for (int i = 0; i < ffInstalledFonts.Length; i++)
            {
                tscmbxFontFamily.Items.Add(ffInstalledFonts[i].Name);
            }
            #endregion

            #region ****************** ToolStripCombobox FontSize ******************
            tscmbxFontSize = new ToolStripComboBox("12")
            {
                Name = "tscmbxFontSize",
                Text = "12"
            };
            tscmbxFontSize.TextChanged += new EventHandler(Control_TextChanged);
            tscmbxFontSize.ToolTipText = "Set font-size";
            tscmbxFontSize.Tag = tscmbxFontSize.Text;
            tscmbxFontSize.Size = new Size((int)(2 * tscmbxFontSize.Font.Size), tscmbxFontSize.Height);
            tscmbxFontSize.SelectedIndexChanged += new EventHandler(FontSize_Click);
            string[] textSizes = new string[] { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };
            foreach (string s in textSizes)
            {
                tscmbxFontSize.Items.Add(new MenuItem().Text = s);
            }
            #endregion

            #region ****************** ToolStripButton Font Color ******************
            tsiFontColor = new ToolStripButton()
            {
                Name = "tsiFontColor",
                Tag = "fontColour",
                ToolTipText = "Font colour",
                CheckOnClick = false,
                ImageIndex = 6
            };
            #endregion

            #region ****************** ToolStripSeparator ******************
            tsiSeparator4 = new ToolStripSeparator()
            {
                Name = "tsiSeparator4",
                Tag = "separator"
            };
            #endregion

            #region ****************** ToolStripButton Highlight Color ******************
            tsiHighlightColor = new ToolStripButton()
            {
                Name = "tsiHighlightColor",
                Tag = "highlightColor",
                ToolTipText = "Highlight colour",
                CheckOnClick = false,
                ImageIndex = 7
            };
            #endregion
            //Add ToolStripItems to the ToolStrip
            ts.Items.AddRange(new ToolStripItem[]
            {
                tsiCopy, tsiCut, tsiPaste, tsiSeparator1,
                tsiBold, tsiItalic, tsiUnderline, tsiSeparator2,
                tsiAlignLeft,tsiAlignCentre,tsiAlignRight,tsiAlignJustify, tsiSeparator3,
                tscmbxFontFamily, tscmbxFontSize, tsiFontColor, tsiSeparator4,
                tsiHighlightColor
            });

            //Add items to the ToolStrip.ImageList
            ts.ImageList.ImageSize = new Size((int)(ts.ClientSize.Height * 0.90), (int)(ts.ClientSize.Height * 0.90));
            ts.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            ts.ImageList.TransparentColor = Color.Transparent;
            ts.ImageList.Images.AddRange(new Image[]
            {
                Properties.Resources.Copy,
                Properties.Resources.Cut,
                Properties.Resources.Paste,
                Properties.Resources.Bold,
                Properties.Resources.Italic,
                Properties.Resources.Underline,
                Properties.Resources.ColorFont,
                Properties.Resources.Highlight,
                Properties.Resources.Align_Left,
                Properties.Resources.Align_Center,
                Properties.Resources.Align_Right,
                Properties.Resources.Align_Justified
            });

            //Add controls to the UserControl
            try
            {
                Controls.AddRange(new Control[] { ts, rtb });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        #region ****************** EventHandlers ******************
        /// <summary>
        ///     Handler for the size changed event.
        /// </summary>
        /// <param name="sender">The control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ControlSizeChanged(object sender, EventArgs e)
        {
            ts.Width = ClientSize.Width;
            rtb.Size = new Size(ClientSize.Width, ClientSize.Height - ts.Bottom);
        }
        /// <summary>
        ///     Handler for the toolbar button click event
        /// </summary>
        private void ToolStripItem_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            //Switch based on the tag of the button pressed
            switch (e.ClickedItem.Tag.ToString().ToUpper())
            {
                case "BOLD":
                    tsiBold.Checked = ChangeFontStyle(FontStyle.Bold);
                    break;
                case "ITALIC":
                    tsiItalic.Checked = ChangeFontStyle(FontStyle.Italic);
                    break;
                case "UNDERLINE":
                    tsiUnderline.Checked = ChangeFontStyle(FontStyle.Underline);
                    break;

                case "COLOR":
                    rtb.SelectionColor = Color.Black;
                    break;

                case "UNDO":
                    rtb.Undo();
                    break;
                case "REDO":
                    rtb.Redo();
                    break;

                case "CUT":
                    if (rtb.SelectedText.Length <= 0) break;
                    rtb.Cut();
                    break;
                case "COPY":
                    if (rtb.SelectedText.Length <= 0) break;
                    rtb.Copy();
                    break;
                case "PASTE":
                    rtb.Paste();
                    break;

                case "ALIGNLEFT":
                    ChangeTextAlignment(0);
                    tsiAlignLeft.Checked = true;
                    tsiAlignCentre.Checked = tsiAlignRight.Checked = tsiAlignJustify.Checked = false;
                    break;
                case "ALIGNCENTRE":
                    ChangeTextAlignment(1);
                    tsiAlignCentre.Checked = true;
                    tsiAlignLeft.Checked = tsiAlignRight.Checked = tsiAlignJustify.Checked = false;
                    break;
                case "ALIGNRIGHT":
                    ChangeTextAlignment(2);
                    tsiAlignRight.Checked = true;
                    tsiAlignLeft.Checked = tsiAlignCentre.Checked = tsiAlignJustify.Checked = false;
                    break;
                case "ALIGNJUSTIFY":
                    ChangeTextAlignment(3);
                    break;

                case "FONTCOLOUR":
                    if (cdlgFont.ShowDialog() == DialogResult.OK)
                    {
                        ChangeFontColor(cdlgFont.Color);
                    }
                    break;

                case "HIGHLIGHTCOLOR":
                    if (cdlgFont.ShowDialog() == DialogResult.OK)
                    {
                        ChangeTextHighlight(cdlgFont.Color);
                    }
                    break;
            }
        }
        /// <summary>
        ///     Change the richtextbox font.
        /// </summary>
        private void Font_Click(object sender, EventArgs e)
        {
            var fontFamilyBox = sender as ToolStripComboBox;
            fontFamilyBox.Text = (string)fontFamilyBox.SelectedItem;
            ChangeFont(fontFamilyBox.Text);
        }
        /// <summary>
        ///     Change the richtextbox font size.
        /// </summary>
        private void FontSize_Click(object sender, EventArgs e)
        {
            var fontSizeBox = sender as ToolStripComboBox;
            fontSizeBox.Text = (string)fontSizeBox.SelectedItem;
            ChangeFontSize(float.Parse(fontSizeBox.Text));
        }
        /// <summary>
        ///		Change the toolbar buttons when new text is selected
        ///		and raise event SelChanged
        /// </summary>
        private void RtbEx_SelectionChanged(object sender, EventArgs e)
        {
            //Update the toolbar buttons
            UpdateToolbar();
            //Send the SelChangedEvent
            SelChanged?.Invoke(this, e);
        }
        private void Rtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                ToolStripButton tsi = null;

                switch (e.KeyCode)
                {
                    case Keys.B:
                        tsi = tsiBold;
                        break;
                    case Keys.I:
                        tsi = tsiItalic;
                        break;
                    case Keys.U:
                        tsi = tsiUnderline;
                        break;
                }

                if (tsi != null)
                {
                    ToolStripItem_Click(null, new ToolStripItemClickedEventArgs(tsi));
                }
            }

            //Insert a tab if the tab key was pressed.
            /* NOTE: This was needed because in rtb1_KeyPress I tell the richtextbox not
			 * to handle tab events.  I do that because CTRL+I inserts a tab for some
			 * strange reason.  What was MicroSoft thinking?
			 * Richard Parsons 02/08/2007
			 */
            if (e.KeyCode == Keys.Tab)
                rtb.SelectedText = "\t";

        }
        private void Rtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 9)
                e.Handled = true; // Stops Ctrl+I from inserting a tab (char HT) into the richtextbox
        }
        private void Control_TextChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripComboBox item)
            {
                foreach (char testchar in item.Text)
                {
                    if (char.IsLetter(testchar) || char.IsWhiteSpace(testchar))
                    {
                        item.Text = item.Text.Remove(item.Text.IndexOf(testchar), 1);
                    }
                    if (string.IsNullOrEmpty(item.Text))
                    {
                        item.Text = (string)tscmbxFontSize.Tag;
                    }
                    tscmbxFontSize.Tag = item.Text;
                }
            }
        }
        #endregion

        #region ****************** OTHER FUNCTIONS ******************
        /// <summary>
        ///     Change the richtextbox font for the current selection.
        /// </summary>
        /// <param name="fontFamily">The new font name.</param>
        public void ChangeFont(string fontFamily)
        {
            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;

            // Step through the selected text one char at a time
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(i, 1);
                rtbTemp.SelectionFont = new Font(fontFamily, rtbTemp.SelectionFont.Size, rtbTemp.SelectionFont.Style);
            }

            // Replace & reselect
            rtbTemp.SelectAll();
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);
            return;
        }
        /// <summary>
        ///     Change the richtextbox font size for the current selection.
        /// </summary>
        /// <param name="fontSize">The new font size.</param>
        public void ChangeFontSize(float fontSize)
        {
            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;

            // Step through the selected text one char at a time
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(i, 1);
                rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont.FontFamily, fontSize, rtbTemp.SelectionFont.Style);
            }

            // Replace & reselect
            rtbTemp.SelectAll();
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);
            return;
        }
        /// <summary>
        ///     Change the richtextbox font color for the current selection.
        /// </summary>
        /// <param name="newColor">The new font color.</param>
        public void ChangeFontColor(Color newColor)
        {
            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;

            // Step through the selected text one char at a time	
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(i, 1);
                rtbTemp.SelectionColor = newColor;
            }

            // Replace & reselect
            rtbTemp.SelectAll();
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);
            return;
        }
        /// <summary>
		///     Change the richtextbox style for the current selection
		/// </summary>
        /// <param name="style">The new font style e.g. FontStyle.Bold</param>
        /// <param name="add">If true then add; else remove.</param>
        public bool ChangeFontStyle(FontStyle style)
        {
            // Here's the logic:
            // The characters of the selected text are looked at one at a time
            // If the first contains the style, it is assumed that the entire range of characters contains the style
            // The style is then removed
            // If the first does not contain the style, then the style will be applied
            // Return a boolean showing whether the style button should be checked or not depending on whether the style was applied or not

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            bool containsStyle = false;

            rtbTemp.Rtf = rtb.SelectedRtf;

            rtbTemp.Select(0, 1);
            switch (style)
            {
                case FontStyle.Bold:
                    if (rtbTemp.SelectionFont.Bold)
                    {
                        containsStyle = true;
                    }
                    break;
                case FontStyle.Italic:
                    if (rtbTemp.SelectionFont.Italic)
                    {
                        containsStyle = true;
                    }
                    break;
                case FontStyle.Underline:
                    if (rtbTemp.SelectionFont.Underline)
                    {
                        containsStyle = true;
                    }
                    break;
            }
            rtbTemp.SelectAll();
            if (containsStyle)
            {
                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(i, 1);
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style & ~style);
                }
            }
            else
            {
                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(i, 1);
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style | style);
                }
            }
            rtbTemp.SelectAll();
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);

            return !containsStyle;
        }
        /// <summary>
        /// Change the richtextbox text alignment for the current selection
        /// </summary>
        /// <param name="type">Value representing the alignment type</param>
        public void ChangeTextAlignment(int type)
        {
            switch (type)
            {
                case 0://left
                    rtb.SelectionAlignment = HorizontalAlignment.Left;
                    break;
                case 1://centre
                    rtb.SelectionAlignment = HorizontalAlignment.Center;
                    break;
                case 2://right
                    rtb.SelectionAlignment = HorizontalAlignment.Right;
                    break;
                case 3://justify
                    break;
            }
        }
        /// <summary>
        /// Change the richtextbox highlight colour for the current selection
        /// </summary>
        /// <param name="highlight">Colour of the highlighter</param>
        public void ChangeTextHighlight(Color highlight)
        {
            rtb.SelectionBackColor = highlight;
        }
        /// <summary>
        ///     Update the toolbar button statuses
        /// </summary>
        public void UpdateToolbar()
        {
            if (rtb.SelectedText.Length > 0)
            {
                // Get the font, fontsize and style to apply to the toolstrip buttons
                Font fnt = GetFontDetails();

                //Set all the style buttons using the gathered style
                tsiBold.Checked = fnt.Bold;
                tsiItalic.Checked = fnt.Italic;
                tsiUnderline.Checked = fnt.Underline;

                //Check the correct font
                if (tscmbxFontFamily.Items.Contains(fnt.FontFamily.Name))
                {
                    tscmbxFontFamily.Text = (fnt.FontFamily.Name);
                }
                //Check the correct font size
                if (tscmbxFontSize.Items.Contains(fnt.SizeInPoints))
                {
                    tscmbxFontSize.Text = (Convert.ToInt32(fnt.SizeInPoints).ToString());
                }
            }
        }
        /// <summary>
        ///     Returns a Font with:
        ///     1) The font applying to the entire selection, if none is the default font.
        ///     2) The font size applying to the entire selection, if none is the size of the default font.
        ///     3) A style containing the attributes that are common to the entire selection, default regular.
        /// </summary>
        /// 
        public Font GetFontDetails()
        {
            rtbTemp.Rtf = rtb.SelectedRtf;
            rtbTemp.Select(0, 1);

            if (rtbTemp.SelectionFont != null)
                return rtbTemp.SelectionFont;
            else
                return rtbTemp.Font;
        }
        #endregion

        #region Selection Change event	
        [Description("Occurs when the selection is changed"),
        Category("Behavior")]
        // Raised in tb1 SelectionChanged event so that user can do useful things
        public event EventHandler SelChanged;
        #endregion
    }
}
