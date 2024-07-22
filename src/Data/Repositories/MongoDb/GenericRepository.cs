using Data.Interfaces.MongoDb;
using Data.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repositories.MongoDb
{
    public class GenericRepository<T> : IMongoDbContext<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private IClientSessionHandle? Session { get; set; }
        private readonly MongoClient _mongoClient;

        public GenericRepository(MongoDbSettings mongoDbSettings)
        {
            _mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var database = _mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);

        }

        public async Task UpdateAsync(string id, T entity)
        {
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)), entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
        }

    }
}
