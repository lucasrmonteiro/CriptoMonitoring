using CriptMonitoring.Application.Dto;
using CriptMonitoring.Domain.Entity;

namespace CriptMonitoring.Application.Interfaces;

public interface IBitstampService
{
    Task SaveAsync(OrderBookEntity entity);
    PricesDto GetStatics(string cripto);
    Task ProcessCripto(string message, string cripto);
}