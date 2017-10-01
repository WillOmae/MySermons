using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Updater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1) { return; }
            /* Only one instance of this process is allowed at a time */
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1) { return; }

            for (int i = 0; i < args.Length / 2; i++)
            {
                int j = i * 2;
                DownloadFile(args[j], args[j + 1]);
            }
        }
        private static void DownloadFile(string uri, string location)
        {
            WebClient downloader = new WebClient();
            try
            {
                byte[] latestAppUI = downloader.DownloadData(uri);
                File.Create(location).Close();
                File.WriteAllBytes(location, latestAppUI);
            }
            catch (WebException)
            {
                MessageBox.Show("Please check your internet connection.", "MySermons Updater Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "MySermons Updater Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                string newPath = location.Replace("_t", "");
                File.Move(location, newPath);
            }
            catch {; }
        }
    }
}
