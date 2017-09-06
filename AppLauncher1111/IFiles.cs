namespace AppLauncher
{
    public interface IFiles
    {
        string DownloadLocation();
        string ReadFile();
        void DeleteFile();
    }
}
