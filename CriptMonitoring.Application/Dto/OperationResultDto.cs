using CriptMonitoring.Domain.Entity;

namespace CriptMonitoring.Application.Dto;

public class OperationResultDto
{
    public Guid Id { get; set; }
    public string Cripto { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public List<PriceQuantityPairEntity> data { get; set; }
}