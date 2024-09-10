using CriptMonitoring.Application.Dto;
using CriptMonitoring.Application.Enum;
using CriptMonitoring.Application.Interfaces;
using CriptMonitoring.Data.Interfaces;
using CriptMonitoring.Domain.Entity;

namespace CriptMonitoring.Application.Services;

public class OperationService : IOperationService
{
    private readonly IBitstampRepository _bitstampRepository;
    
    public OperationService(IBitstampRepository bitstampRepository)
    {
        _bitstampRepository = bitstampRepository;
    }
    public async Task<OperationResultDto> DoOperation(OperationRequestDto operationRequestDto)
    {
        var operationeResult = new OperationResultDto();
        operationeResult.Id = operationRequestDto.Id;
        var last = _bitstampRepository.GetLast(operationRequestDto.Cripto);
        
        if (last is not null)
        {
            var operationsDb = operationRequestDto.Operation switch
            {
                OperationEnum.Buy => last.asks.OrderBy(it => it.usd).ToList(),
                OperationEnum.Sell => last.bids.OrderByDescending(it => it.usd).ToList(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            operationeResult.data = ProcessOperation(operationsDb ,operationRequestDto);
        }

        await SaveNewData(operationRequestDto, operationeResult);

        return operationeResult;
    }

    private async Task SaveNewData(OperationRequestDto operationRequestDto, OperationResultDto operationeResult)
    {
        var newOperationDb = new OrderBookEntity()
        {
            cripto = operationRequestDto.Cripto,
        };

        if (operationRequestDto.Operation == OperationEnum.Buy)
            newOperationDb.asks = operationeResult.data;
        
        if (operationRequestDto.Operation == OperationEnum.Sell)
            newOperationDb.bids = operationeResult.data;
        
        await _bitstampRepository.InsertAsync(newOperationDb);
    }

    private List<PriceQuantityPairEntity> ProcessOperation(List<PriceQuantityPairEntity> operationsDb, 
        OperationRequestDto operationRequestDto)
    {
        decimal remainingQuantity = operationRequestDto.Amount;
        decimal totalPrice = 0;
        
        var operationeResult = new List<PriceQuantityPairEntity>();
        
        foreach (var item in operationsDb)
        {
            if (remainingQuantity <= 0) 
                break;
                
            decimal quantityToUse = Math.Min((decimal)item.cripto, remainingQuantity);
            totalPrice += quantityToUse * (decimal)item.usd;
            remainingQuantity -= quantityToUse;
                
            operationeResult.Add(new PriceQuantityPairEntity
            {
                cripto =quantityToUse,
                usd = remainingQuantity
            });
        }

        return operationeResult;
    }
}