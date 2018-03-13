/* Author: Wilbur Omae
 * Date: 
 * RichTextBoxEx
 * This is an extended RichTextBox with the following added capabilities:
 *      Watermark support
 *      Printing (necessary classes)
 *      Custom links
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Printing;
using AppEngine;

namespace AppUI
{
    /// <summary>
    /// Represents a custom extended Windows rich text box control derived from the RichTextBox control.
    /// </summary>
    public class RichTextBoxEx : RichTextBox
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

                Enter += new EventHandler(EhEnter);
                Leave += new EventHandler(EhLeave);

            }
        }
        private string szWatermark = string.Empty;
        static public FontFamily[] ffInstalledFonts = null;
        private const char DELIMITER = "~";

        /// <summary>
        /// Initializes a new instance of the RichTextBoxEx class.
        /// </summary>
        public RichTextBoxEx()
        {
            if (ffInstalledFonts == null)
            {
                ffInstalledFonts = new System.Drawing.Text.InstalledFontCollection().Families;
            }
            AutoWordSelection = true;
            AcceptsTab = true;
            DetectUrls = true;
            LinkClicked += new LinkClickedEventHandler(EhLinkClicked);
            Multiline = true;
            KeyUp += EhKeyUp;
            WordWrap = true;
        }
        private void RestoreText(string textToRestore, int position)
        {
            SelectionStart = position;
            SelectedText = textToRestore;
            Select(position + textToRestore.Length, 0);
        }
        private string DeleteRTBText(int start, int end)
        {
            Select(start, (end + 1) - start);
            string originalRTF = SelectedRtf;
            SelectedRtf = string.Empty;
            SelectionStart = start;

            return originalRTF;
        }
        private void GetVerse(int boundStart, int boundEnd)
        {
            /* Here's the logic:
             * When the KeyUp event handler calls this method,
             * get the text between the bounds of the verse delimiter (~ in this case).
             * This text is then parsed to the XMLBible class parsing functions.
             * The result of the above step is then inserted as a custom link.
             */

            if ((boundEnd - boundStart) <= 3)
            {
                return;
            }

            string parseString = GetTextBetweenBounds(boundStart, boundEnd);
            List<XMLBible.BIBLETEXTINFO> list = XMLBible.ParseStringToVerse(parseString);

            if (list != null && list.Count != 0)
            {
                int cursorPosition = -1;
                DeleteRTBText(boundStart, boundEnd);
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == (list.Count - 1))//last item in the list
                    {
                        InsertVerse(list[i].FriendlyText, list[i].bcv, boundStart);
                        cursorPosition = boundStart + list[i].FriendlyText.Length;
                    }
                    else
                    {
                        InsertVerse(list[i].FriendlyText + "; ", list[i].bcv, boundStart);
                        cursorPosition = boundStart + list[i].FriendlyText.Length + 1;
                    }
                    boundStart = SelectionStart;
                }
                SelectionStart = cursorPosition;
            }
            else
            {
                Text = Text.Insert(boundStart, "[");
                Text = Text.Remove(boundStart + 1, 1);
                Text = Text.Insert(boundEnd, "]");
                Text = Text.Remove(boundEnd + 1, 1);
                SelectionStart = boundEnd + 1;
            }
        }
        public void InsertVerse(string text, string hyperlink, int position)
        {
            if (position < 0 || position > Text.Length)
                return;

            var sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi ");
            sb.Append(@"\b ");
            sb.Append(text);
            sb.Append(@"\b0 ");
            sb.Append(@"\v ");
            sb.Append(hyperlink);
            sb.Append(@"\v0 ");

            SelectionStart = position;
            SelectedRtf = sb.ToString();
            SelectionStart = position + sb.Length;
        }
        /// <summary>
        /// Extracts text between given bounds.
        /// </summary>
        /// <param name="bound1"></param>
        /// <param name="bound2"></param>
        /// <returns></returns>
        private string GetTextBetweenBounds(int bound1, int bound2)
        {
            try
            {
                return Text.Substring(bound1 + 1, bound2 - 1 - bound1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the hidden text corresponding to the link text.
        /// </summary>
        /// <param name="linkText"></param>
        /// <returns></returns>
        private string GetLinkHiddenText(string linkText)
        {
            /* Here's the logic:
             * When the document is created with verses,
             * the friendly verse text is placed between \b and \b0 -> bold;
             * the bcv is placed between \v and \v0 -> hidden.
             * The purpose of this method is to find the text between \v and \v0
             * just after the linkText.
             */

            int currentPosition = SelectionStart;
            string rtf = Rtf;

            int indexOfLinkText = Text.IndexOf(linkText);
            if (indexOfLinkText != -1)
            {
                Select(indexOfLinkText, linkText.Length);
                int indexOfSelectedText = rtf.IndexOf(SelectedText);
                rtf = rtf.Substring(indexOfSelectedText);
                int iStart = rtf.IndexOf(@"\v ");
                if (iStart != -1)
                {
                    rtf = rtf.Substring(iStart + 3);
                    Select(currentPosition, 0);
                    int iEnd = rtf.IndexOf(@"\");
                    if (iEnd != -1)
                    {
                        return rtf.Substring(0, iEnd);
                    }
                }
            }
            return linkText;
        }

        #region EventHandlers
        /* private void Enter(object sender, EventArgs e) && private void Leave(object sender, EventArgs e)
         * These event handlers govern the display of the watermark in the textbox
         * When the textbox is empty<
         *      -hide watermark on mouse enter
         *      -show watermark on mouse leave
         */
        /// <summary>
        /// Occurs when the mouse enters the RichTextBoxEx control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EhEnter(object sender, EventArgs e)
        {
            if (Text == szWatermark)
            {
                Clear();
                ForeColor = Color.Black;
            }
        }
        /// <summary>
        /// Occurs when the mouse leaves the RichTextBoxEx control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EhLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text) == true)
            {
                Text = szWatermark;
                ForeColor = Color.Gray;
            }
        }
        private void EhLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch
            {
                string szHeader = e.LinkText;
                List<string> listofTextToDisplay = new List<string>();

                string bcv = GetLinkHiddenText(e.LinkText);
                XMLBible.BCVSTRUCT start = new XMLBible.BCVSTRUCT();
                XMLBible.BCVSTRUCT end = new XMLBible.BCVSTRUCT();
                if (XMLBible.ParseForBCVStructs(bcv, ref start, ref end) != "NOT_A_VERSE")
                {
                    listofTextToDisplay = XMLBible.GetVerseText(ref start, ref end);
                    TextCardHolder PopUp = new TextCardHolder(listofTextToDisplay.ToArray(), szHeader);
                }
            }
        }
        private void EhKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde && e.Shift == true)
            {
                var endPos = SelectionStart - 1;
                var prevPos = Text.Substring(0, endPos).LastIndexOf(DELIMITER);
                if (prevPos != -1)
                {
                    String verse = GetTextBetweenBounds(prevPos, endPos);

                    if (verse != null)
                    {
                        Font prevFont = SelectionFont;//get initial font
                        GetVerse(prevPos, endPos);
                        SelectionFont = prevFont;//revert to previous font
                    }
                    else
                    {
                        MessageBox.Show("ERROR");
                    }
                }
            }
        }

        #endregion


        //Extended capabilities from https://www.codeproject.com/articles/9196/links-with-arbitrary-text-in-a-richtextbox

        #region Interop-Defines
        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public uint cbSize;
            public uint dwMask;
            public uint dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public UInt16 wWeight;
            public UInt16 sSpacing;
            public int crBackColor; // Color.ToArgb() -> int
            public int lcid;
            public int dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_USER = 0x0400;
        private const int WM_SETREDRAW = 0x000B;

        private const int EM_FORMATRANGE = WM_USER + 57;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int EM_GETEVENTMASK = WM_USER + 59;
        private const int EM_SETEVENTMASK = WM_USER + 69;

        private const int SCF_SELECTION = 0x0001;
        private const int SCF_WORD = 0x0002;
        private const int SCF_ALL = 0x0004;

        #region CHARFORMAT2 Flags
        private const uint CFE_BOLD = 0x0001;
        private const uint CFE_ITALIC = 0x0002;
        private const uint CFE_UNDERLINE = 0x0004;
        private const uint CFE_STRIKEOUT = 0x0008;
        private const uint CFE_PROTECTED = 0x0010;
        private const uint CFE_LINK = 0x0020;
        private const uint CFE_AUTOCOLOR = 0x40000000;
        private const uint CFE_SUBSCRIPT = 0x00010000;        /* Superscript and subscript are */
        private const uint CFE_SUPERSCRIPT = 0x00020000;      /*  mutually exclusive			 */

        private const int CFM_SMALLCAPS = 0x0040;           /* (*)	*/
        private const int CFM_ALLCAPS = 0x0080;         /* Displayed by 3.0	*/
        private const int CFM_HIDDEN = 0x0100;          /* Hidden by 3.0 */
        private const int CFM_OUTLINE = 0x0200;         /* (*)	*/
        private const int CFM_SHADOW = 0x0400;          /* (*)	*/
        private const int CFM_EMBOSS = 0x0800;          /* (*)	*/
        private const int CFM_IMPRINT = 0x1000;         /* (*)	*/
        private const int CFM_DISABLED = 0x2000;
        private const int CFM_REVISED = 0x4000;

        private const int CFM_BACKCOLOR = 0x04000000;
        private const int CFM_LCID = 0x02000000;
        private const int CFM_UNDERLINETYPE = 0x00800000;       /* Many displayed by 3.0 */
        private const int CFM_WEIGHT = 0x00400000;
        private const int CFM_SPACING = 0x00200000;     /* Displayed by 3.0	*/
        private const int CFM_KERNING = 0x00100000;     /* (*)	*/
        private const int CFM_STYLE = 0x00080000;       /* (*)	*/
        private const int CFM_ANIMATION = 0x00040000;       /* (*)	*/
        private const int CFM_REVAUTHOR = 0x00008000;


        private const uint CFM_BOLD = 0x00000001;
        private const uint CFM_ITALIC = 0x00000002;
        private const uint CFM_UNDERLINE = 0x00000004;
        private const uint CFM_STRIKEOUT = 0x00000008;
        private const uint CFM_PROTECTED = 0x00000010;
        private const uint CFM_LINK = 0x00000020;
        private const uint CFM_SIZE = 0x80000000;
        private const uint CFM_COLOR = 0x40000000;
        private const uint CFM_FACE = 0x20000000;
        private const uint CFM_OFFSET = 0x10000000;
        private const uint CFM_CHARSET = 0x08000000;
        private const uint CFM_SUBSCRIPT = CFE_SUBSCRIPT | CFE_SUPERSCRIPT;
        private const uint CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

        private const byte CFU_UNDERLINENONE = 0x00000000;
        private const byte CFU_UNDERLINE = 0x00000001;
        private const byte CFU_UNDERLINEWORD = 0x00000002; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOUBLE = 0x00000003; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOTTED = 0x00000004;
        private const byte CFU_UNDERLINEDASH = 0x00000005;
        private const byte CFU_UNDERLINEDASHDOT = 0x00000006;
        private const byte CFU_UNDERLINEDASHDOTDOT = 0x00000007;
        private const byte CFU_UNDERLINEWAVE = 0x00000008;
        private const byte CFU_UNDERLINETHICK = 0x00000009;
        private const byte CFU_UNDERLINEHAIRLINE = 0x0000000A; /* (*) displayed as ordinary underline	*/

        #endregion

        #endregion

        [DefaultValue(true)]
        new public bool DetectUrls
        {
            get { return base.DetectUrls; }
            set { base.DetectUrls = value; }
        }

        /// <summary>
        /// Insert a given text as a link into the RichTextBox at the current insert position.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        public void InsertLink(string text)
        {
            InsertLink(text, SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. 
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, int position)
        {
            if (position < 0 || position > Text.Length)
                throw new ArgumentOutOfRangeException("position");

            SelectionStart = position;
            SelectedText = text;
            Select(position, text.Length);
            SetSelectionStyle(CFM_LINK, CFE_LINK);////////////////////
            Select(position + text.Length, 0);
        }

        /// <summary>
        /// Insert a given text at at the current input position as a link.
        /// The link text is followed by a hash (#) and the given hyperlink text, both of
        /// them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        public void InsertLink(string text, string hyperlink)
        {
            InsertLink(text, hyperlink, SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. The link text is followed by
        /// a hash (#) and the given hyperlink text, both of them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, string hyperlink, int position)
        {
            if (position < 0 || position > Text.Length)
                return;

            var sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi");
            sb.Append(@"\b ");
            sb.Append(text);
            sb.Append(@"\b0\b0");

            SelectionStart = position;
            SelectedRtf = sb.ToString();
            //Select(position, sb.Length);
            //SetSelectionLink(true);
            SelectionStart = position + sb.Length;
        }

        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        /// <param name="link">true: set link style, false: clear link style</param>
        public void SetSelectionLink(bool link)
        {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }
        /// <summary>
        /// Get the link style for the current selection
        /// </summary>
        /// <returns>0: link style not set, 1: link style set, -1: mixed</returns>
        public int GetSelectionLink()
        {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }

        private void SetSelectionStyle(uint mask, uint effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (uint)Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

            Marshal.FreeCoTaskMem(lpar);
        }

        private int GetSelectionStyle(uint mask, uint effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (uint)Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

            cf = (CHARFORMAT2_STRUCT)Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT));

            int state;
            // dwMask holds the information which properties are consistent throughout the selection:
            if ((cf.dwMask & mask) == mask)
            {
                if ((cf.dwEffects & effect) == effect)
                    state = 1;
                else
                    state = 0;
            }
            else
            {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }



        #region BORROWED CODE FROM MSDN

        //Convert the unit used by the .NET framework (1/100 inch) 
        //and the unit used by Win32 API calls (twips 1/1440 inch)
        private const double anInch = 14.4;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;         //First character of range (0 for start of doc)
            public int cpMax;           //Last character of range (-1 for end of doc)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;             //Actual DC to draw on
            public IntPtr hdcTarget;       //Target DC for determining text formatting
            public RECT rc;                //Region of the DC to draw to (in twips)
            public RECT rcPage;            //Region of the whole DC (page size) (in twips)
            public CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
        }



        // Render the contents of the RichTextBox for printing
        // Return the last character printed + 1 (printing start from this point for next page)
        public int Print(int charFrom, int charTo, PrintPageEventArgs e)
        {
            //Calculate the area to render and print
            RECT rectToPrint;
            rectToPrint.Top = (int)(e.MarginBounds.Top * anInch);
            rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * anInch);
            rectToPrint.Left = (int)(e.MarginBounds.Left * anInch);
            rectToPrint.Right = (int)(e.MarginBounds.Right * anInch);

            //Calculate the size of the page
            RECT rectPage;
            rectPage.Top = (int)(e.PageBounds.Top * anInch);
            rectPage.Bottom = (int)(e.PageBounds.Bottom * anInch);
            rectPage.Left = (int)(e.PageBounds.Left * anInch);
            rectPage.Right = (int)(e.PageBounds.Right * anInch);

            IntPtr hdc = e.Graphics.GetHdc();

            FORMATRANGE fmtRange;
            fmtRange.chrg.cpMax = charTo;//Indicate character from to character to 
            fmtRange.chrg.cpMin = charFrom;
            fmtRange.hdc = hdc;                    //Use the same DC for measuring and rendering
            fmtRange.hdcTarget = hdc;              //Point at printer hDC
            fmtRange.rc = rectToPrint;             //Indicate the area on page to print
            fmtRange.rcPage = rectPage;            //Indicate size of page

            IntPtr res = IntPtr.Zero;

            IntPtr wparam = IntPtr.Zero;
            wparam = new IntPtr(1);

            //Get the pointer to the FORMATRANGE structure in memory
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lparam, false);

            //Send the rendered data for printing 
            res = SendMessage(Handle, EM_FORMATRANGE, wparam, lparam);

            //Free the block of memory allocated
            Marshal.FreeCoTaskMem(lparam);

            //Release the device context handle obtained by a previous call
            e.Graphics.ReleaseHdc(hdc);

            //Return last + 1 character printer
            return res.ToInt32();
        }
        #endregion

        private IntPtr eventMask;
        public void StopRepaint()
        {
            SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(0), IntPtr.Zero);
            eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, new IntPtr(0), IntPtr.Zero);
        }
        public void StartRepaint()
        {
            SendMessage(this.Handle, EM_SETEVENTMASK, new IntPtr(0), eventMask);
            SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);

            this.Invalidate();
        }
    }
}

/*
private string FxnGetLastTypedWord(int iSelectionStart, char delimiter)
{
    Select(iSelectionStart, 1);
    SelectedText = "";
    int wordEndPosition = iSelectionStart;
    int currentPosition = wordEndPosition;

    while (currentPosition > 0 && Text[currentPosition - 1] != delimiter)
    {
        currentPosition--;
    }
    if ((wordEndPosition - currentPosition) < 2)
    {
        return null;
    }
    string foundText = Text.Substring(currentPosition, wordEndPosition - currentPosition);
    //Just to check an error
    try
    {
        Select(currentPosition - 1, (wordEndPosition - currentPosition) + 1);//currentPosition - 1 to include the preceding tilde
        SelectedRtf = "";
        SelectionStart = currentPosition - 1;
    }
    catch
    {
        SelectionStart = wordEndPosition;
    }
    return foundText;
}*/
