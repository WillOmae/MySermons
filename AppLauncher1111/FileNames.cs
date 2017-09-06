using System;

namespace AppLauncher
{
    public class FileNamesLauncher
    {
        static public readonly string Sermons = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\SermonsRecorded.xml";
        static public readonly string Series = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\Series.xml";
        static public readonly string RODs = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\RecentlyOpenedDocs.xml";
        static public readonly string Preferences = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\Preferences.xml";

        static public readonly string Bible = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\BibleXml.xml";
        static public readonly string WalkthroughsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\Walkthroughs\";

        static public readonly string TempVersionFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\tempVersionFile";
        static public readonly string PermVersionFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\VersionFile";


        //The physical location of the binaries
        //garbage @"C:\Users\Wilbur Omae\Documents\Visual Studio 2017\Projects\MySermons\AppUI\bin\Release\";
        //garbage //System.Windows.Forms.Application.StartupPath + @"\bin\";
        private static readonly string startupPath = @"C:\Users\Wilbur Omae\Documents\Visual Studio 2017\Projects\MySermons\AppUI\bin\Release\";
        public static readonly string dllAppUI = startupPath + "AppUI.dll";
        public static readonly string dllTempAppUI = dllAppUI.Replace(".dll", "_t.dll");
        public static readonly string dllAppEngine = startupPath + "AppEngine.dll";
        public static readonly string dllTempAppEngine = dllAppEngine.Replace(".dll", "_t.dll");
        public static readonly string dllAppLauncher = startupPath + "AppLauncher.dll";
        public static readonly string dllTempAppLauncher = dllAppLauncher.Replace(".dll", "_t.dll");

        //The URLs of the files to be downloaded
        private static readonly string downloadLocation= "https://sourceforge.net/projects/mysermons/files/";
        public static readonly string uriAppUI = downloadLocation + "MySermons.dll/download";
        public static readonly string uriAppEngine = downloadLocation + "MySermons.dll/download";
        public static readonly string uriAppLauncher = downloadLocation + "MySermons.dll/download";
        public static readonly string uriVersion = "";
        public static readonly string uriEntireApp = downloadLocation + "MySermonsVersion1.0.0/download";
    }
}
