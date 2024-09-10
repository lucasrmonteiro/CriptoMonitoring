using CriptMonitoring.Domain.Entity;

namespace CriptMonitoring.Data.Interfaces;

public interface IBitstampRepository : IRepositoryBase<OrderBookEntity>
{
    OrderBookEntity GetLast(string cripto);
    IEnumerable<OrderBookEntity> GetByTime(string cripto, decimal time);
}