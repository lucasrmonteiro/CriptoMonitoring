using CriptMonitoring.Data.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CriptMonitoring.Data.Repository;

public class BaseRepository<T> : IRepositoryBase<T> where T : class
{
    protected readonly IMongoCollection<T?> _collection;
    private readonly ILogger<BaseRepository<T>> _logger;

    public BaseRepository(IMongoClient client, string databaseName, string collectionName)
    {
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        try
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
      
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        try
        {
            FilterDefinition<T?> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }

    public async Task<T?> InsertAsync(T? entity)
    {
        try
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(string id, T? entity)
    {
        try
        {
            FilterDefinition<T?> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            await _collection.ReplaceOneAsync(filter, entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            FilterDefinition<T?> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            await _collection.DeleteOneAsync(filter);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}