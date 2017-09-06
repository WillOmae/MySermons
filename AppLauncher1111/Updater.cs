using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AppLauncher
{
    public class UpdaterClass
    {
        static public Assembly AssemblyUI = Assembly.LoadFrom(FileNamesLauncher.dllAppUI);
        static public Assembly AssemblyEngine = Assembly.LoadFrom(FileNamesLauncher.dllAppEngine);
        //The URL of the file containing the version number of the latest release
        static private string UriVersion => FileNamesLauncher.uriVersion;
        //The URL of the actual file for the latest version.
        static private string UriAppUI => FileNamesLauncher.uriAppUI;
        static private string UriAppEngine => FileNamesLauncher.uriAppEngine;
        //The physical location where the binaries are stored
        private static string DllAppUI => Application.StartupPath + @"\bin\AppUI.dll";
        private static string DllTempAppUI => Application.StartupPath + @"\bin\TempAppUI.dll";
        private static string DllAppEngine => Application.StartupPath + @"\bin\AppEngine.dll";
        private static string DllTempAppEngine => Application.StartupPath + @"\bin\TempAppEngine.dll";
        //A regular expression for the version number
        private static readonly Regex versionNumberRegex = new Regex(@"([0-9]+\.)*[0-9]+");

        private static readonly bool shouldPromptForUpgrade = true;
        private static readonly bool disableAutomaticChecking = false;


        private static string LatestVersion
        {
            get
            {
                string version = string.Empty;
                try
                {
                    using (StreamReader reader = File.OpenText(FileNamesLauncher.TempVersionFile))
                    {
                        version = reader.ReadLine();
                    }
                }
                catch
                {
                    version = "1.0.0";
                }
                return version;
            }
            set
            {
                try
                {
                    File.Delete(FileNamesLauncher.TempVersionFile);
                    using(StreamWriter writer = File.CreateText(FileNamesLauncher.TempVersionFile))
                    {
                        writer.Write(value);
                    }
                }
                catch
                {

                }
            }
        }
        // The name of the file that will store the latest version. 
        private static string LatestVersionInfoFile => FileNamesLauncher.PermVersionFile;

        // The reason a file is used instead of checking the assembly's metadata itself
        // is because checking the metadata requires loading the assembly to check the
        // version. Once the assembly is loaded and it turns out an upgrade is available,
        // you cannot unload the assembly to overwrite the binaries. This is unfortunately
        // a general .NET restriction. 
        private static string GetLocalVersionNumber()
        {
            if (File.Exists(LatestVersionInfoFile) && File.Exists(DllAppUI) && File.Exists(DllAppEngine))
            {
                return File.ReadAllText(LatestVersionInfoFile);
            }
            return null;
        }

        private static void SetLocalVersionNumber(string version)
        {
            File.WriteAllText(LatestVersionInfoFile, version);
        }

        /// <summary>
        /// Gets the latest version number from the server.
        /// </summary>
        private static string DetermineLatestVersion()
        {
            string latestVersion = string.Empty;
            WebClient webClient = new WebClient();

            try
            {
                latestVersion = webClient.DownloadString(UriVersion);
            }
            catch (WebException)//server or connection is having issues
            {
                MessageBox.Show("Please check your internet connection.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error checking for updates");
            }

            // Just in case the server returned something other than a valid version number. 
            return versionNumberRegex.IsMatch(latestVersion)
                ? latestVersion
                : null;
        }
        /// <summary>
        /// Loads the local assembly from the specified file name
        /// </summary>
        /// <returns></returns>
        public static (Assembly, Assembly) LoadAssemblies()
        {
            if (File.Exists(DllAppUI))
            {
                try
                {
                    AssemblyUI = Assembly.LoadFrom(FileNamesLauncher.dllAppUI);

                }
                catch
                {
                    AssemblyUI = null;
                }
            }
            else
            {
                AssemblyUI = null;
            }
            if (File.Exists(DllAppEngine))
            {
                try
                {
                    AssemblyEngine = Assembly.LoadFrom(FileNamesLauncher.dllAppEngine);
                }
                catch
                {
                    AssemblyEngine = null;
                }
            }
            else
            {
                AssemblyEngine = null;
            }
            return (AssemblyUI, AssemblyEngine);
        }
        public static Assembly GetLocalAssembly()
        {
            if (File.Exists(DllAppUI))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(Application.StartupPath + @"\bin\AppUI.dll");

                    return assembly;
                }
                catch (Exception) { }
            }

            return null;
        }

        private static Assembly GetAssembly()
        {
            bool localAssemblyExists = File.Exists(DllAppUI);

            if (disableAutomaticChecking && localAssemblyExists)
            {
                return GetLocalAssembly();
            }

            LatestVersion = DetermineLatestVersion();
            if (LatestVersion == null)
            {
                // Something wrong with connection/server.
                // Just go with the local assembly. 
                return GetLocalAssembly();
            }

            string localVersion = GetLocalVersionNumber();

            if (ShallIDownloadTheLatestBinaries(localVersion, LatestVersion, shouldPromptForUpgrade))
            {
                if (DownloadLatestAssembly())
                {
                    MessageBox.Show("Update downloaded.", "Success");
                }
                else
                {
                    MessageBox.Show("Failed to download the update. Please try again later.", "Download failed.");
                }
            }

            return GetLocalAssembly();
        }

        private static bool ShallIDownloadTheLatestBinaries(string localVersion, string latestVersion, bool shouldAskFirst)
        {
            if (localVersion == latestVersion)
            {
                return false;
            }

            if (!shouldAskFirst)
            {
                return true;
            }

            return DialogResult.Yes == MessageBox.Show(
                "MySermons version " + latestVersion + " is available. Would you like to download it?",
                "Update available v" + latestVersion,
                MessageBoxButtons.YesNo);
        }
        /// <summary>
        /// Downloads the new files from the server and stores them in temporary files without affecting the existing files.
        /// </summary>
        /// <returns>Indicates success</returns>
        private static bool DownloadLatestAssembly()
        {
            WebClient downloader = new WebClient();
            try
            {
                byte[] latestAppUI = downloader.DownloadData(UriAppUI);
                byte[] latestAppEngine = downloader.DownloadData(UriAppEngine);

                File.Create(DllTempAppUI);
                File.WriteAllBytes(DllTempAppUI, latestAppUI);
                File.Create(DllTempAppEngine);
                File.WriteAllBytes(DllTempAppEngine, latestAppEngine);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetUpdatedFilesExist()
        {
            try
            {
                try
                {
                    if (File.Exists(DllTempAppUI))
                    {
                        if (File.Exists(DllAppUI))
                        {
                            File.Delete(DllAppUI);
                        }
                        File.Move(DllTempAppUI, DllAppUI);
                    }
                }
                catch { }
                try
                {
                    if (File.Exists(DllTempAppEngine))
                    {
                        if (File.Exists(DllAppEngine))
                        {
                            File.Delete(DllAppEngine);
                        }
                        File.Move(DllTempAppEngine, DllAppEngine);
                    }
                }
                catch { }
                SetLocalVersionNumber(LatestVersion);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not update because:\t" + e.Message);
            }
            return false;
        }

        public UpdaterClass()
        {
            Assembly binaries = null;
            try
            {
                binaries = GetAssembly();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error in constructor: " + exception.ToString());
            }
        }
    }
}
