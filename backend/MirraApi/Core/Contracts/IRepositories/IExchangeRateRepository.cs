using MirraApi.Domain.Entities;

namespace Contracts.IRepositories;

public interface IExchangeRateRepository
{
    Task<ExchangeRate?> GetLatestRateAsync();
    void CreateRate(ExchangeRate rate);
}