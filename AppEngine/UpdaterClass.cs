using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

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
        static private string UriVersionXML => FileNames.uriVersionXML;
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
        private static string TempVersionInfoFile => FileNames.TempVersionFile;
        private static string PermVersionInfoFile => FileNames.PermVersionFile;
        private static string Bible => FileNames.Bible;

        private static readonly bool shouldPromptForUpgrade = true;
        
        private static bool DownloadLatestVersionFile()
        {
            bool isSuccessful = false;
            WebClient webClient = new WebClient();

            try
            {
                webClient.DownloadFile(UriVersionXML, TempVersionInfoFile);
                isSuccessful = true;
            }
            catch (WebException)//server or connection is having issues
            {
                MessageBox.Show("Please check your internet connection.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error checking for updates");
            }
            
            return isSuccessful;
        }
        private static bool DownloadLatestAssembly()
        {
            /* Launch another exe
             * Since file IO in the program files folder requires admin privileges, a new process needs to be begun with these privileges (Process.StartInfo.Verb = "runas").
             * This process is started and passed string arguments (Process.StartInfo.Arguments = "string[]") which will be interpreted by the process as an array of strings.
             * When the process exits, check for the existence of the updates and notify the user accordingly.
             */
            bool isSuccessful = false;
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
                    isSuccessful = true;
                }
                else
                {
                    MessageBox.Show("No updates were downloaded.\nPlease try again later.", "Updates not downloaded.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isSuccessful = false;
                }
            };
            try
            {
                processUpdater.Start();
                MessageBox.Show("The download will run in the background. You will be notified when all is ready.", "Downloading updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Win32Exception exception)
            {
                MessageBox.Show("The error: " + exception.Message + " was encountered.\nPlease try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccessful;
        }
        public static bool DownloadBible()
        {
            bool isSuccessful = false;
            WebClient downloader = new WebClient();
            try
            {
                byte[] NewBible = downloader.DownloadData(UriBible);
                File.Create(Bible).Close();
                File.WriteAllBytes(TempBible, NewBible);
                isSuccessful = true;
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
            return isSuccessful;
        }
        public static bool DownloadDocs()
        {
            bool isSuccessful = false;

            return isSuccessful;
        }
        private static bool ShouldDownload(string localVersion, string latestVersion, bool shouldAskFirst)
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
        private static void Update()
        {
            try
            {
                if (DownloadLatestVersionFile())
                {
                    ReadXMLFile oldVersion = new ReadXMLFile(PermVersionInfoFile);
                    ReadXMLFile latestVersion = new ReadXMLFile(TempVersionInfoFile);
                    WriteXMLFile writeVersion = new WriteXMLFile(PermVersionInfoFile);
                    
                    if (ShouldDownload(oldVersion.AppVersion, latestVersion.AppVersion, shouldPromptForUpgrade))
                    {
                        try
                        {
                            if (DownloadLatestAssembly())
                            {
                                writeVersion.AppGuid = latestVersion.AppGuid;
                                writeVersion.AppVersion = latestVersion.AppVersion;
                            }
                        }
                        catch
                        {
                            writeVersion.AppGuid = oldVersion.AppGuid;
                            writeVersion.AppVersion = oldVersion.AppVersion;
                            MessageBox.Show("Update cancelled.");
                        }
                    }
                    if (oldVersion.BibleGuid != latestVersion.BibleGuid)
                    {
                        if (DownloadBible())
                        {
                            writeVersion.BibleGuid = latestVersion.BibleGuid;
                        }
                        else
                        {
                            writeVersion.BibleGuid = oldVersion.BibleGuid;
                        }
                    }
                    if (oldVersion.DocsGuid != latestVersion.DocsGuid)
                    {
                        if (DownloadDocs())
                        {
                            writeVersion.DocsGuid = latestVersion.DocsGuid;
                        }
                        else
                        {
                            writeVersion.DocsGuid = oldVersion.DocsGuid;
                        }
                    }
                    writeVersion.Save(PermVersionInfoFile);
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
    }
    public class ReadXMLFile
    {
        public string AppVersion, AppGuid, BibleGuid, DocsGuid;

        public ReadXMLFile(string filename)
        {
            AppVersion = AppGuid = BibleGuid = DocsGuid = string.Empty;
            XmlDocument File = new XmlDocument();
            File.Load(filename);
            GetDetails(File);
        }
        private void GetDetails(XmlDocument doc)
        {
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "APP":
                        AppGuid = node.Attributes["GUID"].Value;
                        AppVersion = node.Attributes["VERSION"].Value;
                        break;
                    case "BIBLE":
                        BibleGuid = node.Attributes["GUID"].Value;
                        break;
                    case "DOCS":
                        DocsGuid = node.Attributes["GUID"].Value;
                        break;
                }
            }
        }
    }
    public class WriteXMLFile
    {
        XmlDocument File;
        public string AppVersion
        {
            set
            {
                WriteAppVersion(value);
            }
        }
        public string AppGuid
        {
            set
            {
                WriteAppGuid(value);
            }
        }
        public string BibleGuid
        {
            set
            {
                WriteBibleGuid(value);
            }
        }
        public string DocsGuid
        {
            set
            {
                WriteDocsGuid(value);
            }
        }

        public WriteXMLFile(string filename)
        {
            File = new XmlDocument();
            File.Load(filename);
        }
        private void WriteAppVersion(string version)
        {
            File.DocumentElement.ChildNodes[0].Attributes["VERSION"].Value = version;
        }
        private void WriteAppGuid(string guid)
        {
            File.DocumentElement.ChildNodes[0].Attributes["GUID"].Value = guid;
        }
        private void WriteBibleGuid(string guid)
        {
            File.DocumentElement.ChildNodes[1].Attributes["GUID"].Value = guid;
        }
        private void WriteDocsGuid(string guid)
        {
            File.DocumentElement.ChildNodes[2].Attributes["GUID"].Value = guid;
        }
        public void Save(string filename)
        {
            File.Save(filename);
        }

        public static bool CreateFile(string filename)
        {
            bool isSuccessful = false;
            using (var writer = System.IO.File.CreateText(filename))
            {
                writer.WriteLine("<?xml version=\"1.0\"?>");
                writer.WriteLine("<FILES>");
                writer.WriteLine("<APP GUID=\"cac9a888-a8d6-418a-a892-5092e6481308\" VERSION=\"1.0.0\"/>");
                writer.WriteLine("<BIBLE GUID=\"06c7a53f-4ab7-4b75-8ef0-2f6cbfaa6ded\"/>");
                writer.WriteLine("<DOCS GUID=\"db5dca46-8b8c-48dc-a779-e2b1dd9eb4c4\"/>");
                writer.WriteLine("</FILES>");
            }
            return isSuccessful;
        }
    }
}
