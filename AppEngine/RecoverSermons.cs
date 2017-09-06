using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AppEngine.MyDateTime;

namespace AppEngine
{
    public class RecoverSermons
    {
        #region ****************** GLOBAL VARIABLES ******************
        #endregion

        #region ****************** CONSTRUCTOR ******************
        public RecoverSermons()
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog()
            {
                AddExtension = true,
                Multiselect = false,
                Title = "Sermon Text File...",
                Filter = "Text Files (*.txt)|*.txt"
            };
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                string szFileName = dlgOpenFile.FileName;
                GetPreRecordedSermons(szFileName);
                dlgOpenFile.Dispose();
            }
        }
        #endregion

        #region ****************** OTHER FUNCTIONS ******************
        /* GetPreRecordedSermons();
         * This function is used to recover sermons written in other simple formats e.g. xyz.txt
         * Using predefined tags, it scans through the existing file, extracting data for each tag as required
         */
        private void GetPreRecordedSermons(string szSourceFilename)
        {
            try
            {
                string szLine, szDate = "", szVenue = "", szActivity = "", szSpeaker = "", szKeyText = "",
                    szHymn = "", szTitle = "", szContent = "";

                StreamReader myFile = File.OpenText(szSourceFilename);
                int iPrevCount = 0, iFinalCount = 0, iError = 0;
                iPrevCount = Sermon.GetSermonCount();

                while ((szLine = myFile.ReadLine()) != null)
                {
                    if (szLine.StartsWith("Venue:") == true)
                    {
                        if (szVenue.Length != 0)
                        {
                            szContent = GenerateRtf(szContent);
                            try
                            {
                                string[] arraySubItems = {"0","0", szDate, szVenue, szVenue, szActivity, szSpeaker, szTitle, szKeyText, szHymn, szContent };
                                if (Sermon.WriteSermon(arraySubItems) == -1)
                                {
                                    iError++;
                                }
                            }
                            catch
                            {
                                string[] arraySubItems = { "0", "0", "01/01/2015", szVenue, szVenue, szActivity, szSpeaker, szTitle, szKeyText, szHymn, szContent };
                                if (Sermon.WriteSermon(arraySubItems) == -1)
                                {
                                    iError++;
                                }
                            }
                        }
                        szVenue = szLine.Replace("Venue: ", "");
                        szActivity = "_default_value_"; szDate = "_default_value_";
                        szSpeaker = "_default_value_"; szTitle = "_default_value_";
                        szKeyText = "_default_value_"; szHymn = "_default_value_";
                        szContent = "_default_value_"; szLine = "_default_value_";
                    }
                    else if (szLine.ToLower().StartsWith("activity:") == true)
                    {
                        szActivity = szLine.Replace("Activity: ", "");
                    }
                    else if (szLine.ToLower().StartsWith("date:") == true)
                    {
                        szDate = szLine.Replace("Date: ", "");
                    }
                    else if (szLine.ToLower().StartsWith("speaker:") == true)
                    {
                        szSpeaker = szLine.Replace("Speaker: ", "");
                    }
                    else if (szLine.ToLower().StartsWith("theme: ") == true)
                    {
                        szTitle = szLine.Replace("Theme: ", "");
                    }
                    else if (szLine.ToLower().StartsWith("key text:") == true)
                    {
                        szKeyText = szLine.Replace("Key Text: ", "");
                    }
                    else if (szLine.ToLower().StartsWith("hymn:") == true)
                    {
                        szHymn = szLine.Replace("Hymn: ", "");
                    }
                    else
                    {
                        if (szContent.Contains("_default_value_")) { szContent = ""; }
                        szContent += szLine + Environment.NewLine;
                    }
                }
                iFinalCount = Sermon.GetSermonCount();
                MessageBox.Show("Recovered " + (iFinalCount - iPrevCount) + " sermons."
                    + "\n\n" + iError.ToString() + " sermon(s) already exist and were not added.",
                    "SUCCESSFUL RECOVERY!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string GenerateRtf(string plainText)
        {
            string RTF = "";
            RichTextBox rtb = new RichTextBox()
            {
                Text = plainText
            };
            RTF = rtb.Rtf;
            return RTF;
        }
        #endregion
    }
}
