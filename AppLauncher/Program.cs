using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AppLauncher
{
    public static class Program
    {
        static public string StartupFolder;
        static public Assembly AssemblyUI = null;
        static public Assembly AssemblyEngine = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1) { return; }

            StartupFolder = args[0];
            string DllAppUI = args[0] + @"\bin\" + "AppUI.dll";
            string DllAppEngine = args[0] + @"\bin\" + "AppEngine.dll";

            try
            {
                AssemblyUI = LoadAssembly(DllAppUI);
                AssemblyEngine = LoadAssembly(DllAppEngine);

                if (AssemblyUI != null || AssemblyEngine != null)
                {
                    if (AssemblyEngine.CreateInstance("AppLauncher.LauncherEngine", true) is ILauncher launcher)
                    {
                        launcher.Launch();
                    }
                    else
                    {
                        MessageBox.Show("Engine launcher returned null", "Unresolved error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Couldn't load assemblies", "Unresolved error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("An unresolved error was encountered. Please reinstall the program.", "Unresolved error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Loads the local assembly from the specified file name
        /// </summary>
        /// <returns></returns>
        private static Assembly LoadAssembly(string location)
        {
            Assembly assembly = null;

            if (File.Exists(location))
            {
                try
                {
                    assembly = Assembly.LoadFrom(location);

                }
                catch
                {
                    assembly = null;
                }
            }
            else
            {
                assembly = null;
            }
            return assembly;
        }
    }
}
