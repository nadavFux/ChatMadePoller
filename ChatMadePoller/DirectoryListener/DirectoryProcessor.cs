namespace DirectoryListener
{
    public interface IDirectoryProcessor
    {
        Task ProcessDirectory(DirectoryInfo directory);
    }


    internal class DirectoryProcessor : IDirectoryProcessor
    {
        public Task ProcessDirectory(DirectoryInfo directory)
        {
            //TODO actully implement a process- also- the most dangerous stab ever made
            directory.Delete();
            return Task.CompletedTask;
        }
    }
}
