using System.Text.Json.Serialization;

namespace CriptMonitoring.Domain.Entity;

public class BitstampEntity
{
    public OrderBookEntity data { get; set; }
    public string channel { get; set; }
    public string @event { get; set; }
}