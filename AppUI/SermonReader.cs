using AppEngine;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AppUI
{
    public class SermonReader : Form
    {
        public RichTextBoxEx textBox = new RichTextBoxEx();
        public string RTFpublic = "";
        private string RTF = "";
        static public ParentForm parentForm;
        private string[] SermonComponents;

        public SermonReader(string rtf, string[] sermonComponents)
        {
            SermonComponents = sermonComponents;
            RTF = rtf;

            Controls.Add(textBox);
            Shown += new EventHandler(FormShown);
            FormBorderStyle = FormBorderStyle.None;
            TopLevel = false;
            Dock = DockStyle.Fill;

            textBox.Dock = DockStyle.Fill;
            textBox.MouseUp += TextBox_MouseUp;
            textBox.ReadOnly = true;
            textBox.DetectUrls = true;
        }

        /// <summary>
        /// Specifies how a recorded sermon is to appear on a form.
        /// </summary>
        /// <param name="arraySermonComponents">An array of strings consisting of sermon components.</param>
        /// <returns>A form containing the sermon to be displayed.</returns>
        static public SermonReader DisplayStoredSermon(string[] arraySermonComponents)
        {
            AlterArraySermonComponents(ref arraySermonComponents);

            if (arraySermonComponents != null && arraySermonComponents.Length > 0)
            {
                RichTextBoxEx textBox = new RichTextBoxEx()
                {
                    Font = new Font("Times New Roman", 14)
                };
                textBox.Text += arraySermonComponents[Sermon.iTitle] + " by " + arraySermonComponents[Sermon.iSpeaker] + Environment.NewLine;
                textBox.Text += arraySermonComponents[Sermon.iDateCreated];

                string RTF = textBox.Rtf;
                string RTFNew1 = arraySermonComponents[Sermon.iContent].Remove(0, arraySermonComponents[Sermon.iContent].IndexOf(";}") + 2);
                RTFNew1 = RTFNew1.Remove(RTFNew1.IndexOf(";}}") + 3);

                string RTFNew2 = arraySermonComponents[Sermon.iContent].Remove(0, arraySermonComponents[Sermon.iContent].IndexOf(";}}") + 3);

                string RTFOld1 = textBox.Rtf.Remove(textBox.Rtf.IndexOf("}}") + 1);

                string RTFOld2 = textBox.Rtf.Replace(RTFOld1, "");
                RTFOld2 = RTFOld2.Remove(0, 1);
                RTFOld2 = RTFOld2.Remove(RTFOld2.LastIndexOf("}"));

                RTF = RTFOld1 + RTFNew1 + RTFOld2 + RTFNew2;

                SermonReader sermonReader = new SermonReader(RTF, arraySermonComponents)
                {
                    Text = arraySermonComponents[Sermon.iTitle],
                    Name = arraySermonComponents[Sermon.iID],
                    RTFpublic = RTF
                };
                return sermonReader;
            }
            else
            {
                return null;
            }
        }
        static private void AlterArraySermonComponents(ref string[] arraySermonComponents)
        {
            try
            {
                if (arraySermonComponents[Sermon.iTitle] == "_default_value_")
                    arraySermonComponents[Sermon.iTitle] = "A sermon";
                if (arraySermonComponents[Sermon.iSpeaker] == "_default_value_")
                    arraySermonComponents[Sermon.iSpeaker] = "an Anonymous Speaker";
                if (arraySermonComponents[Sermon.iVenue] == "_default_value_")
                    arraySermonComponents[Sermon.iVenue] = "[Location undisclosed]";
                if (arraySermonComponents[Sermon.iVenueTown] == "_default_value_")
                    arraySermonComponents[Sermon.iVenueTown] = "[Town undisclosed]";
                if (arraySermonComponents[Sermon.iVenueActivity] == "_default_value_")
                    arraySermonComponents[Sermon.iVenueActivity] = "Study";

                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iVenue]))
                    arraySermonComponents[Sermon.iVenue] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iVenueTown]))
                    arraySermonComponents[Sermon.iVenueTown] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iVenueActivity]))
                    arraySermonComponents[Sermon.iVenueActivity] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iSpeaker]))
                    arraySermonComponents[Sermon.iSpeaker] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iKeyText]))
                    arraySermonComponents[Sermon.iKeyText] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iHymn]))
                    arraySermonComponents[Sermon.iHymn] = "NOT SET";
                if (string.IsNullOrEmpty(arraySermonComponents[Sermon.iTheme]))
                    arraySermonComponents[Sermon.iTheme] = "NOT SET";

                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iVenue]))
                    arraySermonComponents[Sermon.iVenue] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iVenueTown]))
                    arraySermonComponents[Sermon.iVenueTown] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iVenueActivity]))
                    arraySermonComponents[Sermon.iVenueActivity] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iSpeaker]))
                    arraySermonComponents[Sermon.iSpeaker] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iKeyText]))
                    arraySermonComponents[Sermon.iKeyText] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iHymn]))
                    arraySermonComponents[Sermon.iHymn] = "NOT SET";
                if (string.IsNullOrWhiteSpace(arraySermonComponents[Sermon.iTheme]))
                    arraySermonComponents[Sermon.iTheme] = "NOT SET";
            }
            catch {; }
        }
        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                textBox.Cursor = Cursors.Arrow;

                textBox.ContextMenu = new ContextMenu();

                textBox.ContextMenu.MenuItems.AddRange
                    (new MenuItem[] {
                        new MenuItem("Edit", TextBox_Edit),
                        new MenuItem("Print", TextBox_Print)
                    });

                textBox.ContextMenu.Show(textBox, new Point(e.X, e.Y), LeftRightAlignment.Left);
                textBox.ContextMenu.Popup += TextBoxContextMenu_Popup;
                textBox.ContextMenu.Collapse += TextBoxContextMenu_Collapse;
            }
        }

        private void TextBox_Edit(object sender, EventArgs e)
        {
            SermonViewNew sermonViewEdit = new SermonViewNew(parentForm);
            sermonViewEdit.SetEditingValues(SermonComponents);

            foreach (TabPage tabpage in parentForm.tabControl.TabPages)
            {
                if (tabpage.Name == sermonViewEdit.Name)
                {
                    parentForm.tabControl.TabPages.Remove(tabpage);
                    break;
                }
            }
            parentForm.AddNewTabPage(sermonViewEdit);
        }
        private void TextBox_Print(object sender, EventArgs e)
        {
            parentForm.menuItemFile_Print.PerformClick();
        }
        private void TextBoxContextMenu_Popup(object sender, EventArgs e)
        {
            textBox.Cursor = Cursors.Arrow;
        }
        private void TextBoxContextMenu_Collapse(object sender, EventArgs e)
        {
            textBox.Cursor = Cursors.IBeam;
        }

        
        /// <summary>
        /// When the form is shown, a background thread is started which detects and updates verse links in the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormShown(object sender, EventArgs e)
        {
            textBox.StopRepaint();
            textBox.Rtf = RTF;
            var rangeMatches = Regex.Matches(textBox.Text, @"\b\w{3}\.\d{1,3}\.\d{1,3}\b\-\b\w{3}\.\d{1,3}\.\d{1,3}\b|\b\w{3}\.\d{1,3}\.\d{1,3}\b");
            foreach (Match match in rangeMatches)
            {
                InsertLink(match.Value);
            }
            BackToStart();
            textBox.StartRepaint();
        }
        /// <summary>
        /// Determines the start and end of a bcv, then extracts the substring from the text.
        /// It also checks whether a range exists.
        /// </summary>
        /// <param name="text">The bulky text to search</param>
        /// <param name="iSelectionStart">The start position</param>
        /// <param name="endPos">The end of the bcv (it is an out parameter)</param>
        /// <param name="isRange">A range exists or not (it is an out parameter)</param>
        /// <returns></returns>
        private string GetWord(string text, int iSelectionStart, out int endPos, out bool isRange)
        {
            bool recursion = false;

            int wordEndPosition = iSelectionStart;
            int currentPosition = wordEndPosition;

            while (currentPosition >= 0 && text[currentPosition] != ' ' && text[currentPosition] != '\n' && text[currentPosition] != '\t' && text[currentPosition] != '\r')
            {
                if (char.IsPunctuation(text[currentPosition]) && text[currentPosition] != '.')
                {//break at any punctuation other than '.'
                    break;
                }
                else
                {
                    currentPosition--;
                }
            }
            while (wordEndPosition < text.Length && text[wordEndPosition] != ' ' && text[wordEndPosition] != '\n' && text[wordEndPosition] != '\t' && text[wordEndPosition] != '\r' && text[wordEndPosition] != ';')
            {
                if (char.IsPunctuation(text[wordEndPosition]))
                {
                    try
                    {
                        if (text[wordEndPosition] == '.' && char.IsDigit(text[wordEndPosition + 1]))
                        {
                            //if punctuation is '.', check next character
                            wordEndPosition++;
                        }
                        else if (text[wordEndPosition] == '-')
                        {
                            //if punctuation is '-', a few checks
                            //if next char is letter or digit, notify of the existence of a range then break
                            if (char.IsLetterOrDigit(text[wordEndPosition + 1]))
                            {
                                recursion = true;
                            }
                            else
                            {
                                recursion = false;
                            }
                            break;
                        }
                        else
                        {
                            //break at any other punctuation
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
                else
                {
                    wordEndPosition++;
                }
            }
            endPos = wordEndPosition + 1;//get the position just after the word end
            if ((uint)((wordEndPosition - 1) - (currentPosition + 1)) < 2)
            {//too short to be of value
                endPos = wordEndPosition;
                isRange = recursion;
                return null;
            }
            string foundText = "";
            //Just to check an error
            try
            {
                foundText = text.Substring(currentPosition + 1, (wordEndPosition - 1 - currentPosition));
            }
            catch {; }

            isRange = recursion;
            return foundText;
        }

        private void InsertLink(string linkText)
        {
            try
            {
                int iPos;

                string stringToSplit = textBox.Text;
                do
                {
                    iPos = stringToSplit.IndexOf(linkText);
                    if (iPos >= stringToSplit.Length || iPos < 0)
                    {
                        break;
                    }
                    else
                    {
                        //textBox.Select(iPos, linkText.Length);
                        textBox.Select(textBox.Text.IndexOf(stringToSplit) + iPos, linkText.Length);
                        textBox.SetSelectionLink(true);
                        //textBox.Select(iPos + linkText.Length, 0);textBox.Text.IndexOf(stringToSplit) + iPos
                        textBox.Select(textBox.Text.IndexOf(stringToSplit) + iPos + linkText.Length, 0);
                        stringToSplit = textBox.Text.Remove(0, textBox.SelectionStart);
                    }
                } while (textBox.SelectionStart < (textBox.TextLength - 2));
            }
            catch { };
        }
        private void BackToStart()
        {
            textBox.SelectionStart = 0;
        }
        private void SuspendLayout(string empty)
        {
            textBox.SuspendLayout();
        }
        private void ResumeLayout(string empty)
        {
            textBox.ResumeLayout(true);
        }
    }
}
