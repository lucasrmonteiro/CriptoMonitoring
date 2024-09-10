using CriptMonitoring.Application.Enum;

namespace CriptMonitoring.Application.Dto;

public class OperationRequestDto
{
    public Guid Id { get; set; }
    public OperationEnum Operation { get; set; }
    public string Cripto { get; set; }
    public int Amount { get; set; }
}