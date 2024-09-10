using System.Text.Json;
using CriptMonitoring.Application.Dto;
using CriptMonitoring.Application.Enum;
using CriptMonitoring.Application.Interfaces;
using CriptMonitoring.Data.Interfaces;
using CriptMonitoring.Domain.Entity;
using CriptMonitoring.Infra.Converter;

namespace CriptMonitoring.Application.Services;

public class BitstampService : IBitstampService
{
    private readonly IBitstampRepository _bitstampRepository;
    
    public BitstampService(IBitstampRepository bitstampRepository)
    {
        _bitstampRepository = bitstampRepository;
    }
    
    public async Task SaveAsync(OrderBookEntity entity)
    {
        await _bitstampRepository.InsertAsync(entity);
    }
    
    public PricesDto GetStatics(string cripto)
    {
        var dto = new PricesDto();
        var last = _bitstampRepository.GetLast(cripto);
        if (last is not null)
        {
            dto.AvgPrice = Math.Round(last.asks.Average(x => (decimal)x.usd) ,2);
            dto.MaxPrice = Math.Round(last.asks.Max(x => (decimal)x.usd) ,2);
            dto.MinPrice = Math.Round(last.asks.Min(x => (decimal)x.usd) ,2);
            dto.AvgLastFiveSeconds = Math.Round(GetAvgLastFiveSeconds(cripto) ,2);
        }
        
        return dto;

    }

    private decimal GetAvgLastFiveSeconds(string cripto)
    {
        var data = _bitstampRepository.GetByTime(cripto, DateTime.Now.AddSeconds(-5).Second);
        return data.Average(x => x.asks.Average(y => (decimal)y.usd));
    }

    public async Task ProcessCripto(string message ,string cripto)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new OrderBookConverter()}
        };
        var bitstamp = JsonSerializer.Deserialize<BitstampEntity>(message ,options);
        bitstamp.data.cripto = cripto;
        await SaveAsync(bitstamp.data);
    }
}