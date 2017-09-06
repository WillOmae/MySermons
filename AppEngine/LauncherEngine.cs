namespace AppLauncher
{
    public class LauncherEngine : ILauncher
    {
        public void Launch()
        {
            if (Program.AssemblyUI.CreateInstance("AppEngine.LauncherUI", true) is ILauncher launcher)
            {
                launcher.Launch();
            }
        }
    }
}
