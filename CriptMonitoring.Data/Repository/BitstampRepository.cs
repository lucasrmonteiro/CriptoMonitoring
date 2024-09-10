using CriptMonitoring.Data.Interfaces;
using CriptMonitoring.Domain.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CriptMonitoring.Data.Repository;

public class BitstampRepository : BaseRepository<OrderBookEntity> ,IBitstampRepository
{
    public BitstampRepository(IMongoClient client, string databaseName, string collectionName) : base(client, databaseName, collectionName)
    {
    }
    
    public OrderBookEntity GetLast(string cripto)
    {
        return _collection.Find(it => it.cripto == cripto).SortByDescending(x => x.Timestamp).FirstOrDefault();
    }

    public IEnumerable<OrderBookEntity> GetByTime(string cripto, decimal time)
    {
        var filter = Builders<OrderBookEntity>.Filter.Gt("timestamp", new BsonTimestamp((int)time));
        return _collection.Find(filter).ToList();
    }
}