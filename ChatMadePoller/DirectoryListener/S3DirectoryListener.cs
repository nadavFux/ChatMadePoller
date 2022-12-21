using Amazon.S3;
using Amazon.S3.Model;

namespace DirectoryListener
{
    public class S3DirectoryListener : IDirectoryListener
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly HashSet<string> _processedDirectories;

        public S3DirectoryListener(string bucketName, string prefix)
        {
            _s3Client = new AmazonS3Client();
            _bucketName = bucketName;
            _processedDirectories = new HashSet<string>();
        }

        public void StartListening(string directoryPath, Action<DirectoryInfo> onDirectoryCreated)
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = directoryPath
            };

            ListObjectsV2Response response;
            do
            {
                response = _s3Client.ListObjectsV2Async(request).Result;

                foreach (S3Object entry in response.S3Objects)
                {
                    string directoryKey = entry.Key;
                    if (IsDirectory(directoryKey) && !_processedDirectories.Contains(directoryKey))
                    {
                        _processedDirectories.Add(directoryPath);
                        /* 
                         * TODO- rework so that it will work with s3,
                         Alternatively, you could create your own custom class that represents an S3 directory
                        and pass an instance of this class to the onDirectoryCreated callback 
                        will require a FS,S3 Storage Classes and while i can let chatGPT write it for me this was supposed to be a fun short thing and its consuming me.... by taking 2 hours and a half
                         */
                        throw new Exception("see comment in S3DirectoryListener");
                        onDirectoryCreated(new DirectoryInfo(directoryPath));
                    }
                }

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
        }

        private bool IsDirectory(string path)
        {
            return path.EndsWith("/");
        }
    }
}
