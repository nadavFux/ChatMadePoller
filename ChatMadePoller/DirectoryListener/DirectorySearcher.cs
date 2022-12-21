namespace DirectoryListener
{
    public interface IDirectorySearcher
    {
        bool IsWantedDirectory(DirectoryInfo directory);
    }

    public class DirectorySearcher : IDirectorySearcher
    {
        private readonly string? RequiredFilePrefix;

        public DirectorySearcher(string? requiredFilePrefix)
        {
            this.RequiredFilePrefix = requiredFilePrefix;
        }

        public bool IsWantedDirectory(DirectoryInfo directory)
        {
            // Implement using requiredFilePrefix
            return true;
        }
    }
}
