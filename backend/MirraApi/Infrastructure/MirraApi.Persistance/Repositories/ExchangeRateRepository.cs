using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Repositories;

public class ExchangeRateRepository(AppDbContext dbContext) : RepositoryBase<ExchangeRate>(dbContext), IExchangeRateRepository
{
    public async Task<ExchangeRate?> GetLatestRateAsync()
    {
        return await FindAll(trackChanges: false)
            .OrderByDescending(r => r.UpdatedAt)
            .FirstOrDefaultAsync();
    }

   
    public void CreateRate(ExchangeRate rate)
    {
        Create(rate);
    }
}