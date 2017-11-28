namespace AppEngine
{
    public class LauncherUI : AppLauncher.ILauncher
    {
        public void Launch()
        {
            System.Windows.Forms.Application.Run(new AppUI.ParentForm());
        }
    }
}
