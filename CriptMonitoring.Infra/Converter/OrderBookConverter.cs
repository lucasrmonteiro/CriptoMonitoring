using System.Text.Json;
using System.Text.Json.Serialization;
using CriptMonitoring.Domain.Entity;
using MongoDB.Bson;

namespace CriptMonitoring.Infra.Converter;

public class OrderBookConverter : JsonConverter<OrderBookEntity>
{
    public override OrderBookEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }
        
        var optionsPrice = new JsonSerializerOptions
        {
            Converters = { new PriceQuantityPairConverter() }
        };

        var orderBook = new OrderBookEntity();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return orderBook;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "timestamp":
                        orderBook.Timestamp = new BsonTimestamp(long.Parse(reader.GetString()));
                        break;
                    case "microtimestamp":
                        orderBook.Microtimestamp = new BsonTimestamp(long.Parse(reader.GetString()));
                        break;
                    case "bids":
                        orderBook.bids = JsonSerializer.Deserialize<List<PriceQuantityPairEntity>>(ref reader, optionsPrice);
                        break;
                    case "asks":
                        orderBook.asks = JsonSerializer.Deserialize<List<PriceQuantityPairEntity>>(ref reader, optionsPrice);
                        break;
                }
            }
        }

        throw new JsonException("Invalid JSON format for OrderBookEntity");
    }

    public override void Write(Utf8JsonWriter writer, OrderBookEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("timestamp", value.Timestamp.Timestamp.ToString());
        writer.WriteString("microtimestamp", value.Microtimestamp.Timestamp.ToString());
        writer.WritePropertyName("bids");
        JsonSerializer.Serialize(writer, value.bids, options);
        writer.WritePropertyName("asks");
        JsonSerializer.Serialize(writer, value.asks, options);
        writer.WriteEndObject();
    }
}