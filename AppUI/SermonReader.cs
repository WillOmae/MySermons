using AppEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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

        private delegate void d_InsertLink(string linkText);

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
        /// When the form is shown, verse links are detected and updated in the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormShown(object sender, EventArgs e)
        {
            textBox.Rtf = RTF;

            string text = textBox.Text;
            string rtf = textBox.Rtf;

            d_InsertLink myDelegate;
            LoadingForm loadingForm = new LoadingForm();
            BackgroundWorker bw = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bw.DoWork += delegate
            {
                /* Here's the logic:
                 * The first regex operation ensures backwards compartibility
                 * The second is the modern implementation due to the inclusion of friendlyText
                 */
                var textMatches = Regex.Matches(rtf, @"\w{3}\.\d{1,3}\.\d{1,3}\b\-\b\w{3}\.\d{1,3}\.\d{1,3}\;|\w{3}\.\d{1,3}\.\d{1,3}\;|\w{3}\.\d{1,3}\.\d{1,3}\b\-\b\w{3}\.\d{1,3}\.\d{1,3}\\b0|\w{3}\.\d{1,3}\.\d{1,3}\\b0");
                if (textMatches.Count > 0)
                {
                    foreach(Match match in textMatches)
                    {
                        textMatches = Regex.Matches(match.Value, @"\w{3}\.\d{1,3}\.\d{1,3}\b\-\b\w{3}\.\d{1,3}\.\d{1,3}|\w{3}\.\d{1,3}\.\d{1,3}");
                        foreach (Match match1 in textMatches)
                        {
                            myDelegate = new d_InsertLink(InsertLink_TextMethod);
                            textBox.Invoke(myDelegate, match1.Value);
                        }
                    }
                    textMatches.Count.ToString();
                }
                else
                {
                    var rtfMatches = Regex.Matches(rtf, @"\w{3}\.\d{1,3}\.\d{1,3}\b\-\b\w{3}\.\d{1,3}\.\d{1,3}|\w{3}\.\d{1,3}\.\d{1,3}");
                    foreach (Match match in rtfMatches)
                    {
                        myDelegate = new d_InsertLink(InsertLink_RtfMethod);
                        textBox.Invoke(myDelegate, match.Value);
                    }
                }
            };
            bw.RunWorkerCompleted += delegate
            {
                BackToStart();
                loadingForm.Close();
            };
            loadingForm.FormClosing += delegate
            {
                if (bw != null && bw.IsBusy)
                {
                    bw.CancelAsync();
                }
            };

            bw.RunWorkerAsync();
            loadingForm.ShowDialog();
        }

        private List<string> listOfVerseToDisplay = new List<string>();
        private void InsertLink_RtfMethod(string linkText)
        {
            /* Here's the logic:
             * The linkText passed is the hidden bcv (bound by \v and \v0).
             * We know that the visible text is just before the hidden
             * and is bound by \b and \b0 (bold).
             * So we loop backwards, determining the end and beginning of the bold
             * just before the linkText position.
             * This text, called verseToDisplay is checked for any stray rtf control characters.
             * A nested do...while loop is then employed to find any other occurrences of this verseToDisplay.
             * The listOfVerseToDisplay is populated accordingly and used to check any future verseToDisplay.
             */

            int iStart = -1, iEnd = -1;
            bool isEndSet = false;
            for (int i = textBox.Rtf.IndexOf(linkText); i > 3; --i)
            {
                if (textBox.Rtf[i] == 48 && textBox.Rtf[i - 1] == 98 && textBox.Rtf[i - 2] == 92)
                {
                    iEnd = i - 3;
                    isEndSet = true;
                    i = iEnd;
                }
                else if (textBox.Rtf[i] == 98 && textBox.Rtf[i - 1] == 92)
                {
                    if (isEndSet)
                    {
                        iStart = i + 2;
                        string verseToDisplay = textBox.Rtf.Substring(iStart, iEnd - iStart + 1);
                        verseToDisplay = CheckLinkText(verseToDisplay);
                        int iVerse = textBox.Text.IndexOf(verseToDisplay);
                        if (iVerse == -1)
                        {
                            verseToDisplay = CheckLinkText2(verseToDisplay);
                            iVerse = textBox.Text.IndexOf(verseToDisplay);
                        }
                        string x = string.Empty;
                        if (listOfVerseToDisplay.Contains(verseToDisplay))
                        {
                            break;
                        }
                        else
                        {
                            listOfVerseToDisplay.Add(verseToDisplay);
                            do
                            {
                                textBox.Select(iVerse, verseToDisplay.Length);
                                textBox.SetSelectionLink(true);
                                textBox.Select(iVerse + verseToDisplay.Length, 0);

                                x = textBox.Text.Substring(iVerse + verseToDisplay.Length);
                                iVerse = iVerse + verseToDisplay.Length + x.IndexOf(verseToDisplay);
                            } while (x.IndexOf(verseToDisplay) != -1);
                            break;
                        }
                    }
                }
            }
        }
        private void InsertLink_TextMethod(string linkText)
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
        private void BackToStart()
        {
            textBox.SelectionStart = 0;
        }

        /// <summary>
        /// Returns a substring of the passed string after removing rtf control characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CheckLinkText(string text)
        {
            if (text.Contains((char)92))
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    if (char.IsWhiteSpace(text[i]))
                    {
                        text = text.Substring(i + 1);
                        break;
                    }
                }
                CheckLinkText(text);
            }
            return text;
        }
        /// <summary>
        /// Returns a substring of the passed string based on the position of the first whitespace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CheckLinkText2(string text)
        {
            int index = text.IndexOf(' ');
            if (index >= 0)
            {
                text = text.Substring(index + 1);
            }
            return text;
        }
    }
}
