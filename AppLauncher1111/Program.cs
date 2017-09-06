using System;
using System.Windows.Forms;

namespace AppLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                (UpdaterClass.AssemblyUI, UpdaterClass.AssemblyEngine) = UpdaterClass.LoadAssemblies();
                UpdaterClass.GetUpdatedFilesExist();
                if (UpdaterClass.AssemblyUI != null && UpdaterClass.AssemblyEngine != null)
                {
                    if (UpdaterClass.AssemblyEngine.CreateInstance("AppLauncher.LauncherEngine", true) is ILauncher launcher)
                    {
                        launcher.Launch();
                    }
                    else
                    {
                        MessageBox.Show("Engine launcher returned null");
                    }
                }
                else
                {
                    MessageBox.Show("Couldn't load assemblies");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
