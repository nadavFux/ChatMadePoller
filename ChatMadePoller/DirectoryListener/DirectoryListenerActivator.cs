using MongoDB.Bson;
using OpenTracing;
using Mongo;
using Messaging;
using Models;
using Microsoft.Extensions.Logging;

namespace DirectoryListener
{
    public interface IDirectoryListener
    {
        void StartListening(string directoryPath, Action<DirectoryInfo> onDirectoryCreated);
    }

    public class DirectoryListenerActivator
    {
        private readonly IDirectorySearcher directorySearcher;
        private readonly IDirectoryProcessor directoryProcessor;
        private readonly IDirectoryListener directoryListener;
        private readonly IDirectoryDocumentRepository repository;
        private readonly IMessageSender messageSender;
        private readonly ILogger logger;
        private readonly ITracer tracer;

        public DirectoryListenerActivator(IDirectoryListener directoryListener,
                                           IDirectorySearcher directorySearcher,
                                           IDirectoryProcessor directoryProcessor,
                                           IDirectoryDocumentRepository repository,
                                           IMessageSender messageSender,
                                           ILogger logger, ITracer tracer)
        {
            this.directorySearcher = directorySearcher;
            this.directoryProcessor = directoryProcessor;
            this.repository = repository;
            this.directoryListener = directoryListener;
            this.messageSender = messageSender;
            this.logger = logger;
            this.tracer = tracer;
        }

        public void Activate(string directoryPath)
        {
            if (directoryPath.StartsWith("s3://"))
            {
                // Listen to object storage using the Amazon SDK
                // TODO: Implement this
            }
            else
            {
                directoryListener.StartListening(directoryPath, OnDirectoryCreated);
            }
        }
        private async void OnDirectoryCreated(DirectoryInfo directory)
        {
            using (IScope scope = tracer.BuildSpan("OnDirectoryCreated").StartActive(finishSpanOnDispose: true))
            {
                scope.Span.SetTag("DirectoryPath", directory.FullName);

                if (directorySearcher.IsWantedDirectory(directory))
                {
                    try
                    {
                        // Save "found" status in repository
                        DirectoryDocument doc = new DirectoryDocument
                        {
                            Id = ObjectId.GenerateNewId(),
                            DirectoryPath = directory.FullName,
                            Status = ProcessStatus.Found
                        };
                        repository.Upsert(doc);

                        await directoryProcessor.ProcessDirectory(directory);

                        // Update status to "success" in repository
                        doc.Status = ProcessStatus.Success;
                        repository.Upsert(doc);

                        messageSender.SendMessage(directory.FullName);

                        logger.LogInformation($"Successfully processed directory {directory.FullName} and sent message.");
                    }
                    catch (Exception ex)
                    {
                        // Update status to "failed" in repository
                        DirectoryDocument doc = new DirectoryDocument
                        {
                            Id = ObjectId.GenerateNewId(),
                            DirectoryPath = directory.FullName,
                            Status = ProcessStatus.Failed,
                            ErrorMessage = ex.Message
                        };
                        repository.Upsert(doc);

                        logger.LogError(ex, $"Failed to process directory {directory.FullName}.");
                    }
                }
            }
        }
    }
}
