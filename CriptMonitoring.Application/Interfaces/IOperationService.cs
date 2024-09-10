using CriptMonitoring.Application.Dto;

namespace CriptMonitoring.Application.Interfaces;

public interface IOperationService
{
    Task<OperationResultDto> DoOperation(OperationRequestDto operationRequestDto);
}