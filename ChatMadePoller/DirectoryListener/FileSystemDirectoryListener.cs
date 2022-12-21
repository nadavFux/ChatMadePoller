using Microsoft.Extensions.Logging;

namespace DirectoryListener
{
    public class FileSystemDirectoryListener : IDirectoryListener
    {
        private readonly ILogger logger;

        public FileSystemDirectoryListener(ILogger logger)
        {
            this.logger = logger;
        }

        public void StartListening(string directoryPath, Action<DirectoryInfo> onDirectoryCreated)
        {
            // Use FileSystemWatcher to listen for changes in the directory
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directoryPath;
            watcher.IncludeSubdirectories = true;
            watcher.Created += (sender, e) => onDirectoryCreated(new DirectoryInfo(e.FullPath));

            // Check for directories that already exist
            foreach (string dir in Directory.EnumerateDirectories(directoryPath, "*", SearchOption.AllDirectories))
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                onDirectoryCreated(directory);
            }

            watcher.EnableRaisingEvents = true;
        }
    }
}
