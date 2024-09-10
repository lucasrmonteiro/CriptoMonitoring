using System.Text.Json;
using System.Text.Json.Serialization;
using CriptMonitoring.Domain.Entity;

namespace CriptMonitoring.Infra.Converter;

public class PriceQuantityPairConverter: JsonConverter<PriceQuantityPairEntity>
{
    public override PriceQuantityPairEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token");
        }

        reader.Read();
        var usd = reader.GetString();

        reader.Read();
        var btc = reader.GetString();

        reader.Read(); 
        return new PriceQuantityPairEntity
        {
            cripto = Convert.ToDecimal(btc),
            usd = Convert.ToDecimal(usd)
        };
    }

    public override void Write(Utf8JsonWriter writer, PriceQuantityPairEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue((decimal)value.cripto);
        writer.WriteNumberValue((decimal)value.usd);
        writer.WriteEndArray();
    }
}