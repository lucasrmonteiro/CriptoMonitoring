using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CriptMonitoring.Domain.Entity;

public class PriceQuantityPairEntity
{
    [BsonRepresentation(BsonType.Decimal128)]
    public Decimal128 cripto { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public Decimal128 usd { get; set; }
}