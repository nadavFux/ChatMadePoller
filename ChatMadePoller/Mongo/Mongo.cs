using MongoDB.Driver;
using Models;

namespace Mongo
{
    public interface IDirectoryDocumentRepository
    {
        void Upsert(DirectoryDocument document);
    }
    public class MongoDirectoryDocumentRepository : IDirectoryDocumentRepository
    {
        private readonly IMongoCollection<DirectoryDocument> _collection;

        public MongoDirectoryDocumentRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<DirectoryDocument>(collectionName);
        }

        public void Upsert(DirectoryDocument document)
        {
            _collection.ReplaceOne(d => d.DirectoryPath == document.DirectoryPath, document, new ReplaceOptions { IsUpsert = true });
        }
    }

}
