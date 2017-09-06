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
        private RichTextBox rtbTemp = new RichTextBox();//Used for looping

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
            rtb.Font = new Font(tscmbxFontFamily.Text, Convert.ToInt32(tscmbxFontSize.Text));
            UpdateToolbar();
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
            rtb.KeyDown += rtb_KeyDown;
            rtb.KeyPress += rtb_KeyPress;
            if (RichTextBoxEx.ffInstalledFonts != null)
            { ffInstalledFonts = RichTextBoxEx.ffInstalledFonts; }
            else
            { ffInstalledFonts = new System.Drawing.Text.InstalledFontCollection().Families; }
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
            tscmbxFontSize = new ToolStripComboBox("8")
            {
                Name = "tscmbxFontSize",
                Text = "20"
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
            // true if style to be added; false to remove style
            bool add = false;
            if (e.ClickedItem is ToolStripButton tsbtn)
            {
                if (tsbtn.CheckState == CheckState.Checked)
                {
                    add = true;
                }
                else if (tsbtn.CheckState == CheckState.Unchecked)
                {
                    add = false;
                }
            }

            //Switch based on the tag of the button pressed
            switch (e.ClickedItem.Tag.ToString().ToUpper())
            {
                case "BOLD":
                    ChangeFontStyle(FontStyle.Bold, add);
                    break;
                case "ITALIC":
                    ChangeFontStyle(FontStyle.Italic, add);
                    break;
                case "UNDERLINE":
                    ChangeFontStyle(FontStyle.Underline, add);
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
                    {
                        if (rtb.SelectedText.Length <= 0) break;
                        rtb.Cut();
                        break;
                    }
                case "COPY":
                    {
                        if (rtb.SelectedText.Length <= 0) break;
                        rtb.Copy();
                        break;
                    }
                case "PASTE":
                    {
                        try
                        {
                            rtb.Paste();
                        }
                        catch
                        {
                            MessageBox.Show("Paste Failed");
                        }
                        break;
                    }

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
            } //end switch
        }
        /// <summary>
        ///     Change the richtextbox font.
        /// </summary>
        private void Font_Click(object sender, EventArgs e)
        {
            // Set the font for the entire selection

            //the sender is a ToolStripComboBox control
            tscmbxFontFamily.Text = (string)tscmbxFontFamily.SelectedItem;
            ChangeFont(tscmbxFontFamily.Text);
        }
        /// <summary>
        ///     Change the richtextbox font size.
        /// </summary>
        private void FontSize_Click(object sender, EventArgs e)
        {
            //set the richtextbox font size based on the name of the menu item

            //the sender is a ToolStripComboBox control
            tscmbxFontSize.Text = (string)tscmbxFontSize.SelectedItem;
            ChangeFontSize(float.Parse(tscmbxFontSize.Text));
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
        private void rtb_KeyDown(object sender, KeyEventArgs e)
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
                    if (e.KeyCode != Keys.S) tsi.Checked = !tsi.Checked;
                    {
                        ToolStripItem_Click(null, new ToolStripItemClickedEventArgs(tsi));
                    }
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
        private void rtb_KeyPress(object sender, KeyPressEventArgs e)
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
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            // fontFamily - the font to be applied, eg "Courier New"

            // Reason: The reason this method and the others exist is because
            // setting these items via the selection font doen't work because
            // a null selection font is returned for a selection with more 
            // than one font!

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int rtbTempStart = 0;

            // If len <= 1 and there is a selection font, amend and return
            if (len <= 1 && rtb.SelectionFont != null)
            {
                rtb.SelectionFont =
                    new Font(fontFamily, rtb.SelectionFont.Size, rtb.SelectionFont.Style);
                return;
            }

            // Step through the selected text one char at a time
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);
                rtbTemp.SelectionFont = new Font(fontFamily, rtbTemp.SelectionFont.Size, rtbTemp.SelectionFont.Style);
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
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
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            // fontSize - the fontsize to be applied, eg 33.5

            if (fontSize <= 0.0)
                throw new System.InvalidProgramException("Invalid font size parameter to ChangeFontSize");

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int rtbTempStart = 0;

            // If len <= 1 and there is a selection font, amend and return
            if (len <= 1 && rtb.SelectionFont != null)
            {
                rtb.SelectionFont =
                    new Font(rtb.SelectionFont.FontFamily, fontSize, rtb.SelectionFont.Style);
                return;
            }

            // Step through the selected text one char at a time
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);
                rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont.FontFamily, fontSize, rtbTemp.SelectionFont.Style);
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
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
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            //	newColor - eg Color.Red

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && rtb.SelectionFont != null)
            {
                rtb.SelectionColor = newColor;
                return;
            }

            // Step through the selected text one char at a time	
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                //change color
                rtbTemp.SelectionColor = newColor;
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);
            return;
        }
        /// <summary>
		///     Change the richtextbox style for the current selection
		/// </summary>
        /// <param name="style">The new font style e.g. FontStyle.Bold</param>
        /// <param name="add">If true then add; else remove.</param>
		public void ChangeFontStyle(FontStyle style, bool add)
        {
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            //	style - eg FontStyle.Bold
            //	add - IF true then add else remove

            // throw error if style isn't: bold, italic, strikeout or underline
            if (style != FontStyle.Bold
                && style != FontStyle.Italic
                && style != FontStyle.Strikeout
                && style != FontStyle.Underline)
                throw new InvalidProgramException("Invalid style parameter to ChangeFontStyle");

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && rtb.SelectionFont != null)
            {
                //add or remove style 
                if (add)
                    rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style | style);
                else
                    rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style & ~style);

                return;
            }

            // Step through the selected text one char at a time	
            rtbTemp.Rtf = rtb.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                //add or remove style 
                if (add)
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style | style);
                else
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style & ~style);
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
            rtb.SelectedRtf = rtbTemp.SelectedRtf;
            rtb.Select(rtbstart, len);
            return;
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
            // Get the font, fontsize and style to apply to the toolstrip buttons
            Font fnt = GetFontDetails();

            //Set all the style buttons using the gathered style
            tsiBold.Checked = fnt.Bold; //bold button
            tsiItalic.Checked = fnt.Italic; //italic button
            tsiUnderline.Checked = fnt.Underline; //underline button

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
        /// <summary>
        ///     Returns a Font with:
        ///     1) The font applying to the entire selection, if none is the default font. 
        ///     2) The font size applying to the entire selection, if none is the size of the default font.
        ///     3) A style containing the attributes that are common to the entire selection, default regular.
        /// </summary>		
        /// 
        public Font GetFontDetails()
        {
            //This method should handle cases that occur when multiple fonts/styles are selected

            int rtbstart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int rtbTempStart = 0;

            if (len <= 1)
            {
                // Return the selection or default font
                if (rtb.SelectionFont != null)
                    return rtb.SelectionFont;
                else
                    return rtb.Font;
            }

            // Step through the selected text one char at a time	
            // after setting defaults from first char
            rtbTemp.Rtf = rtb.SelectedRtf;

            //Turn everything on so we can turn it off one by one
            FontStyle replystyle =
                FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout | FontStyle.Underline;

            // Set reply font, size and style to that of first char in selection.
            rtbTemp.Select(rtbTempStart, 1);
            string replyfont = rtbTemp.SelectionFont.Name;
            float replyfontsize = rtbTemp.SelectionFont.Size;
            replystyle = replystyle & rtbTemp.SelectionFont.Style;

            // Search the rest of the selection
            for (int i = 1; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                // Check reply for different style
                replystyle = replystyle & rtbTemp.SelectionFont.Style;

                // Check font
                if (replyfont != rtbTemp.SelectionFont.FontFamily.Name)
                    replyfont = "";

                // Check font size
                if (replyfontsize != rtbTemp.SelectionFont.Size)
                    replyfontsize = (float)0.0;
            }

            // Now set font and size if more than one font or font size was selected
            if (replyfont == "")
                replyfont = rtbTemp.Font.FontFamily.Name;

            if (replyfontsize == 0.0)
                replyfontsize = rtbTemp.Font.Size;

            // generate reply font
            Font reply
                = new Font(replyfont, replyfontsize, replystyle);

            return reply;
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
