using DirectoryListener;
using Messaging;
using Mongo;
using Logging;
using Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTracing;
using Amazon.S3;

IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

ILogger logger = new NLogLogger();

// Set up tracer
ITracer tracer = new OpenTracer(OpenTracing.Noop.NoopTracerFactory.Create());

// Set up dependencies
IDirectorySearcher directorySearcher = new DirectorySearcher(config["WantedDirectoryName"]);
IDirectoryProcessor directoryProcessor = new DirectoryProcessor();

IDirectoryDocumentRepository repository = new MongoDirectoryDocumentRepository(config["MongoConnectionString"], config["MongoDatabaseName"], config["MongoCollectionName"]);
IMessageSender messageSender = new RabbitMQSender(config["RabbitMQConnectionString"], config["RabbitMQExchange"], logger);

// Set up directory listener
string directoryPath = config["DirectoryPath"];
IDirectoryListener directoryListener = null;
if (directoryPath.StartsWith("s3://"))
{
    string bucketName = directoryPath.Substring(5).Split('/')[0];
    string prefix = directoryPath.Substring(5 + bucketName.Length + 1);
    directoryListener = new S3DirectoryListener(bucketName, prefix);
}
else
{
    directoryListener = new FileSystemDirectoryListener(logger);
}

// Start listening for directories
DirectoryListenerActivator listenerActivator = new DirectoryListenerActivator(directoryListener, directorySearcher, directoryProcessor, repository, messageSender, logger, tracer);
listenerActivator.Activate(directoryPath);

// Keep the program running
Console.ReadLine();
