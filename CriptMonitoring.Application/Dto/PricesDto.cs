namespace CriptMonitoring.Application.Dto;

public class PricesDto
{
    public decimal AvgPrice { get; set; }
    public decimal MaxPrice { get; set; } 
    public decimal MinPrice { get; set; } 
    public decimal AvgLastFiveSeconds { get; set; }
}