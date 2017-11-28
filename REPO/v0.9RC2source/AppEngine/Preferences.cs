using System;
using System.IO;
using System.Windows.Forms;

namespace AppEngine
{
    public class Preferences
    {
        /*Constants/Defaults*/
        public const string DefPrinterScheme = "White/Black";
        public const string DefTheme = "Default";
        public const int DefROD_MaxNumber = 10;
        public const string DefSortingFilter = "SPEAKER";
        public const string DefFont = "Times New Roman";
        public const string DefBackColor = "222233";
        public const string DefForeColor = "AACCFF";
        /*Public properties*/
        static public string PrinterName
        {
            get
            {
                return printerName;
            }
            set
            {
                printerName = value;
            }
        }
        static public string PrinterScheme
        {
            get
            {
                return printerScheme;
            }
            set
            {
                printerScheme = value;
            }
        }
        static public string SortingFilter
        {
            get
            {
                return sortingFilter;
            }
            set
            {
                sortingFilter = value;
            }
        }
        static public int ROD_MaxNumber
        {
            get
            {
                return rod_maxnumber;
            }
            set
            {
                rod_maxnumber = value;
            }
        }
        static public string ColourFont
        {
            get
            {
                return colourFont;
            }
            set
            {
                colourFont = value;
            }
        }
        static public string ColourControls
        {
            get
            {
                return colourControls;
            }
            set
            {
                colourControls = value;
            }
        }
        static public string FontSystem
        {
            get
            {
                return fontSystem;
            }
            set
            {
                fontSystem = value;
            }
        }
        static public string FontReader
        {
            get
            {
                return fontReader;
            }
            set
            {
                fontReader = value;
            }
        }
        static public string FontWriter
        {
            get
            {
                return fontWriter;
            }
            set
            {
                fontWriter = value;
            }
        }
        static public bool ShowWelcomeScreen
        {
            get
            {
                return showwelcomescreen;
            }
            set
            {
                showwelcomescreen = value;
            }
        }
        /*Private variables used by the public properties*/
        static private string printerName;
        static private string printerScheme;
        static private string sortingFilter;
        static private int rod_maxnumber;
        static private string colourFont;
        static private string colourControls;
        static private string fontSystem;
        static private string fontReader;
        static private string fontWriter;
        static private bool showwelcomescreen;


        /// <summary>
        /// The class constructor.
        /// </summary>
        public static void SetPreferences()
        {
            try
            {
                Database.Preferences prefs = Database.Preferences.SelectAllStatic()[0];
                printerName = prefs.PrinterName;
                printerScheme = prefs.PrinterScheme;
                sortingFilter = prefs.SortingFilter;
                rod_maxnumber = prefs.ROD_MaxNumber;
                colourFont = prefs.ColourFont;
                colourControls = prefs.ColourControls;
                fontSystem = prefs.FontSystem;
                fontReader = prefs.FontReader;
                fontWriter = prefs.FontWriter;
                showwelcomescreen = prefs.ShowWelcomeScreen;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void SaveData()
        {
            Database.Preferences preferences = new Database.Preferences()
            {
                ColourControls = Preferences.ColourControls,
                ColourFont = Preferences.ColourFont,
                FontReader = Preferences.FontReader,
                FontSystem = Preferences.FontSystem,
                FontWriter = Preferences.FontWriter,
                PrinterName = Preferences.PrinterName,
                PrinterScheme = Preferences.PrinterScheme,
                ROD_MaxNumber = Preferences.ROD_MaxNumber,
                ShowWelcomeScreen = Preferences.ShowWelcomeScreen,
                SortingFilter = Preferences.SortingFilter
            };
            preferences.Update(preferences);
        }
        /// <summary>
        /// Creates a new Xml file to hold preferences
        /// </summary>
        /// <param name="path">The path to the file to be created.</param>
        static public void CreateNewPrefsFile(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine("<PREFS PRINTER_NAME=\"\" PRINTER_SCHEME=\"\" COLOUR_FONT=\"AACCFF\" COLOUR_CONTROLS=\"222233\" FONT_SYSTEM=\"Times New Roman\" FONT_READER=\"Times New Roman\" FONT_WRITER=\"Times New Roman\" ROD_MAXNUMBER=\"11\" SORTING_FILTER=\"Year\" SHOW_WELCOMESCREEN=\"True\" />");
                }
            }
            catch
            {
                try
                {
                    //using (StreamWriter sw = new StreamWriter(File.Create(FileNames.Sermons)))
                    //{
                    //    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    //    sw.WriteLine("<PREFS PRINTER_NAME=\"\" PRINTER_SCHEME=\"\" COLOUR_FONT=\"AACCFF\" COLOUR_CONTROLS=\"222233\" FONT_SYSTEM=\"Times New Roman\" FONT_READER=\"Times New Roman\" FONT_WRITER=\"Times New Roman\" ROD_MAXNUMBER=\"11\" SORTING_FILTER=\"Year\" SHOW_WELCOMESCREEN=\"True\" />");
                    //}
                }
                catch
                {
                    MessageBox.Show("An error was encountered while accessing core files.\nYou may need to reinstall the application.");
                }
            }
        }
    }
}
