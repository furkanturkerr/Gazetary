using Dtos;

namespace Business.Abstract;

public interface IExchangeRateService
{
    Task<ExchangeRateDto> GetRatesAsync();
}