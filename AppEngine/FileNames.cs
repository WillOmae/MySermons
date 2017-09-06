using System;
using System.IO;

namespace AppEngine
{
    public class FileNames
    {
        static public readonly string Bible = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySermons\Required Files\BibleXml.xml");
        static public readonly string WalkthroughsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySermons\Required Files\Walkthroughs\");

        static public readonly string TempVersionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySermons\Required Files\tempVersion.xml");
        static public readonly string PermVersionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySermons\Required Files\Version.xml");

        
        //folders
        public static string StartupPath
        {
            get
            {
                return AppLauncher.Program.StartupFolder;
            }
        }
        private static readonly string startupPathBin = StartupPath + @"\bin\";
        private static readonly string startupPathExe = StartupPath + @"\exe\";
        public static readonly string dbFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MySermons\Required Files\Storage");
        //binaries
        public static readonly string dllAppUI = startupPathBin + "AppUI.dll";
        public static readonly string dllAppEngine = startupPathBin + "AppEngine.dll";
        //executables
        public static readonly string exeDownloadUpdates = startupPathExe + "Updater.exe";
        public static readonly string exeAppLauncher = startupPathExe + "AppLauncher.exe";
        //databases
        public static readonly string dbLocation = dbFolder + @"\MySermons.chest";
        //temp files
        public static readonly string tempAppUI = dllAppUI.Replace(".dll", "_t.dll");
        public static readonly string tempAppEngine = dllAppEngine.Replace(".dll", "_t.dll");
        public static readonly string tempAppLauncher = exeAppLauncher.Replace(".exe", "_t.exe");
        public static readonly string tempBible = Bible.Replace(".xml", "_t.xml");
        //The URLs of the files to be downloaded
        private static readonly string downloadLocation= "https://github.com/WillOmae/MySermons/raw/master/Downloads/";
        public static readonly string uriAppUI = downloadLocation + "bin/AppUI.dll";
        public static readonly string uriAppEngine = downloadLocation + "bin/AppEngine.dll";
        public static readonly string uriAppLauncher = downloadLocation + "exe/AppLauncher.exe";
        public static readonly string uriVersionXML = downloadLocation + "Version.xml";
        public static readonly string uriBible = downloadLocation + "misc/BibleXml.xml";
        //Sqlite connection string
        public static readonly string ConnectionString = "Data Source = " + dbLocation + ";Version=3;Journal Mode=off;Pooling=true;Cache Size=10000;Synchronous=off";
    }
}