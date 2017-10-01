using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MySermons
{
    public class Program
    {
        #region
        //folders
        private static readonly string startupPath = Application.StartupPath;
        private static readonly string folderBin = startupPath + @"\bin\";
        private static readonly string folderExe = startupPath + @"\exe\";
        //private static string requiredFiles = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySermons\Required Files\";
        //The physical location of binaries
        private static readonly string dllAppEngine = folderBin + "AppEngine.dll";
        public static readonly string dllAppUI = folderBin + "AppUI.dll";
        //The physical locations of executables
        public static readonly string exeAppLauncher = folderExe + "AppLauncher.exe";
        public static readonly string exeInstallUpdates = folderExe + "UpdateInstaller.exe";
        //temp files - updates
        private static readonly string tempAppUI = dllAppUI.Replace(".dll", "_t.dll");
        private static readonly string tempAppEngine = dllAppEngine.Replace(".dll", "_t.dll");
        private static readonly string tempAppLauncher = exeAppLauncher.Replace(".exe", "_t.exe");
        //private static readonly string tempBible = Bible.Replace(".xml", "_t.xml");
        //files
        //private static string Bible = requiredFiles + "BibleXml.xml";
        #endregion

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (VerifyProcess())
                {
                    if (CheckForUpdates())
                    {
                        if (DialogResult.Yes == MessageBox.Show("Downloaded updates have been detected. Would you like to install them?", "Found updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        {
                            LaunchUpdater();
                        }
                    }
                    LaunchApp();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private static bool VerifyProcess()
        {
            /* Only one instance of this process is allowed at a time */
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool CheckForUpdates()
        {
            try
            {
                if (File.Exists(tempAppUI) || File.Exists(tempAppEngine) || File.Exists(tempAppLauncher))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                return false;
            }
        }
        private static void LaunchUpdater()
        {
            /* Launch another exe
             * Since file IO in the program files folder requires admin privileges, a new process needs to be begun with these privileges (Process.StartInfo.Verb = "runas").
             * This process is started and passed string arguments (Process.StartInfo.Arguments = "string[]") which will be interpreted by the process as an array of strings.
             */
            bool before = false, after = false;
            before = CheckForUpdates();
            if (before)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    Arguments = "\"" + dllAppUI + "\" \"" + tempAppUI + "\" \"" + dllAppEngine + "\" \"" + tempAppEngine + "\" \"" + exeAppLauncher + "\" \"" + tempAppLauncher,
                    FileName = exeInstallUpdates,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process processUpdater = new Process()
                {
                    EnableRaisingEvents = true,
                    StartInfo = startInfo
                };
                processUpdater.Exited += delegate
                {
                    after = CheckForUpdates();
                    if (before != after)//updates have been installed
                    {
                        MessageBox.Show("Updates have been installed\nYou are running the latest version of MySermons.", "Update install success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else//updates were not installed
                    {
                        MessageBox.Show("Downloaded updates were not installed.\nPlease try again later.", "Error installing updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                try
                {
                    processUpdater.Start();
                    processUpdater.WaitForExit();
                }
                catch (Win32Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error installing updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private static void LaunchApp()
        {
            /* Launch another exe
             * This process is started and passed string arguments (Process.StartInfo.Arguments = "string[]") which will be interpreted by the process as an array of strings.
             */
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                Arguments = "\"" + Path.GetFullPath(Application.StartupPath) + "\"",
                FileName = exeAppLauncher,
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process processApp = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo = startInfo
            };
            processApp.Exited += delegate
            {
                return;
            };

            try
            {
                processApp.Start();
                processApp.WaitForExit();
            }
            catch (Win32Exception exception)
            {
                MessageBox.Show(exception.Message, "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
