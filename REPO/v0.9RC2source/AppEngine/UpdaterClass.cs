using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AppEngine
{
    // The reason a file is used instead of checking the assembly's metadata itself
    // is because checking the metadata requires loading the assembly to check the
    // version. Once the assembly is loaded and it turns out an upgrade is available,
    // you cannot unload the assembly to overwrite the binaries. This is unfortunately
    // a general .NET restriction. 
    public class UpdaterClass
    {
        private static bool check = true;
        //The URL of the file containing the version number of the latest release
        static private string UriVersion => FileNames.uriVersion;
        //The URL of the actual file for the latest version.
        static private string UriAppUI => FileNames.uriAppUI;
        static private string UriAppEngine => FileNames.uriAppEngine;
        static private string UriAppLauncher => FileNames.uriAppLauncher;
        static private string UriBible => FileNames.uriBible;
        //binaries
        private static string DllAppUI => FileNames.dllAppUI;
        private static string DllAppEngine => FileNames.dllAppEngine;
        //executables
        private static string ExeAppLauncher => FileNames.exeAppLauncher;
        private static string ExeDownloadUpdates => FileNames.exeDownloadUpdates;
        //temp files - updates
        private static string TempAppUI => FileNames.tempAppUI;
        private static string TempAppEngine => FileNames.tempAppEngine;
        private static string TempAppLauncher => FileNames.tempAppLauncher;
        private static string TempBible => FileNames.tempBible;
        //files
        private static string LatestVersionInfoFile => FileNames.PermVersionFile;
        private static string Bible => FileNames.Bible;
        //A regular expression for the version number
        private static readonly Regex versionNumberRegex = new Regex(@"([0-9]+\.)*[0-9]+");
        private static readonly bool shouldPromptForUpgrade = true;
        private static string LatestVersion
        {
            get
            {
                string version = string.Empty;
                try
                {
                    using (StreamReader reader = File.OpenText(FileNames.TempVersionFile))
                    {
                        version = FormatVersionString(reader.ReadLine());
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
                    File.Delete(FileNames.TempVersionFile);
                    using(StreamWriter writer = File.CreateText(FileNames.TempVersionFile))
                    {
                        writer.Write(FormatVersionString(value));
                    }
                }
                catch
                {

                }
            }
        }

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
        /// <returns>Latest version number</returns>
        private static string DetermineLatestVersion()
        {
            string latestVersion = string.Empty;
            WebClient webClient = new WebClient();

            try
            {
                latestVersion = webClient.DownloadString(UriVersion);
                latestVersion = FormatVersionString(latestVersion);
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
        private static string FormatVersionString(string toFormat)
        {
            string formatted = string.Empty;
            for (int i = 0; i < toFormat.Length; i++)
            {
                if (char.IsDigit(toFormat[i]) || (char.IsPunctuation(toFormat[i]) && toFormat[i] == '.'))
                {
                    formatted += toFormat[i];
                }
            }
            return formatted;
        }
        /// <summary>
        /// Determine whether the latest binaries are to be downloaded.
        /// </summary>
        /// <param name="localVersion">Version number of installed binaries.</param>
        /// <param name="latestVersion">Version number of server binaries.</param>
        /// <param name="shouldAskFirst">Indicates whether the user should be prompted.</param>
        /// <returns>Indicates whether the binaries should be donwloaded.</returns>
        private static bool ShallIDownloadTheLatestBinaries(string localVersion, string latestVersion, bool shouldAskFirst)
        {
            if (localVersion == latestVersion)
            {
                MessageBox.Show("You are running the latest version of MySermons.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!shouldAskFirst)
            {
                return true;
            }

            return DialogResult.Yes == MessageBox.Show(
                "MySermons version " + latestVersion.TrimEnd(char.Parse("\n")) + " is available. Would you like to download it?",
                "Update available v" + latestVersion,
                MessageBoxButtons.YesNo);
        }
        private static bool CheckForUpdates()
        {
            if (File.Exists(TempAppUI) || File.Exists(TempAppEngine) || File.Exists(TempAppLauncher))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Downloads the new files from the server into temporary files. Existing files are not affected.
        /// </summary>
        /// <returns>Indicates success</returns>
        private static void DownloadLatestAssembly(string latestVersion)
        {
            /* Launch another exe
             * Since file IO in the program files folder requires admin privileges, a new process needs to be begun with these privileges (Process.StartInfo.Verb = "runas").
             * This process is started and passed string arguments (Process.StartInfo.Arguments = "string[]") which will be interpreted by the process as an array of strings.
             * When the process exits, check for the existence of the updates and notify the user accordingly.
             */
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                Verb = "runas",
                Arguments = "\"" + UriAppUI + "\" \"" + TempAppUI + "\" \"" + UriAppEngine + "\" \"" + TempAppEngine + "\" \"" + UriAppLauncher + "\" \"" + TempAppLauncher + "\" \"" + UriBible + "\" \"" + TempBible,
                FileName = ExeDownloadUpdates
            };
            Process processUpdater = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo = processStartInfo
            };
            processUpdater.Exited += delegate
            {
                if (CheckForUpdates())
                {
                    MessageBox.Show("Downloaded updates. Changes will be made on app restart.\nYou may choose to restart the application to apply them now.", "Downloaded updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetLocalVersionNumber(latestVersion);
                }
                else
                {
                    MessageBox.Show("No updates were downloaded.\nPlease try again later.", "Updates not downloaded.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            try
            {
                processUpdater.Start();
                MessageBox.Show("The download will run in the background. You will be notified when all is ready.", "Downloading updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Win32Exception exception)
            {
                MessageBox.Show("The error: " + exception.Message + " was encountered.\nPlease try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void Update()
        {
            try
            {
                string latestVersion = DetermineLatestVersion();
                if (latestVersion != null)
                {
                    LatestVersion = latestVersion;
                    string localVersion = GetLocalVersionNumber();
                    if (ShallIDownloadTheLatestBinaries(localVersion, latestVersion, shouldPromptForUpgrade))
                    {
                        try
                        {
                            DownloadLatestAssembly(latestVersion);
                        }
                        catch
                        {
                            MessageBox.Show("Update cancelled.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error updating", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                check = true;
            }
        }
        /// <summary>
        /// Class constructor
        /// </summary>
        public UpdaterClass()
        {
            if (check)
            {
                MessageBox.Show("Updates will be checked in the background. You will be notified when the task is done.", "Searching for updates");
                Thread updateThread = new Thread(new ThreadStart(Update))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.AboveNormal
                };
                updateThread.Start();
                check = false;
            }
            else
            {
                MessageBox.Show("Please wait.", "Update check in progress");
            }
        }
        public UpdaterClass(string download)
        {
            if (download.ToUpper() == "BIBLE")
            {
                WebClient downloader = new WebClient();
                try
                {
                    byte[] latestAppUI = downloader.DownloadData(UriBible);
                    File.Create(Bible).Close();
                    File.WriteAllBytes(TempBible, latestAppUI);
                    try
                    {
                        File.Move(TempBible, Bible);
                        MessageBox.Show("Successfully downloaded Bible.", "MySermons Updater Service Success");
                    }
                    catch
                    {
                        MessageBox.Show("Successfully downloaded Bible but failed to load it. It will be loaded when the application is next started.", "MySermons Updater Service Success");
                    }
                }
                catch (WebException)
                {
                    MessageBox.Show("Please check your internet connection.", "MySermons Updater Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "MySermons Updater Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
