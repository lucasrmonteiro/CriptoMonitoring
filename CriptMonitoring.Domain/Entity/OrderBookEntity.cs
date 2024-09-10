using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CriptMonitoring.Domain.Entity;

[BsonIgnoreExtraElements]
public class OrderBookEntity
{
    [BsonElement("timestamp")]
    public BsonTimestamp  Timestamp { get; set; }
    [BsonElement("microtimestamp")]
    public BsonTimestamp  Microtimestamp { get; set; }
    public List<PriceQuantityPairEntity> bids { get; set; }
    public List<PriceQuantityPairEntity> asks { get; set; }
    public string cripto { get; set; }
}