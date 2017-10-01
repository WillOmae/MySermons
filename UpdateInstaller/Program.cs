using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace UpdateInstaller
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length < 1) { return; }
            /* Only one instance of this process is allowed at a time */
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1) { return; }

            for (int i = 0; i < args.Length / 2; i++)
            {
                int j = i * 2;
                UpdateActuator(args[j], args[j + 1]);
            }
        }
        private static void UpdateActuator(string existingFile, string tempFile)
        {
            try
            {
                if (File.Exists(tempFile))
                {
                    if (File.Exists(existingFile))
                    {
                        File.Delete(existingFile);
                    }
                    File.Move(tempFile, existingFile);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
