using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using MirraApi.Domain.Dtos;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Repositories;

public class PaymentRepository(AppDbContext dbContext) : RepositoryBase<Payment>(dbContext), IPaymentRepository
{
    public async Task<IEnumerable<PaymentDto>> GetRecentPaymentsAsync(int take)
    {
        
        var paymentsQuery = FindAll(trackChanges: false)
            .Include(p => p.Client)
            .OrderByDescending(p => p.Date)
            .Take(take);

        return await paymentsQuery.Select(p => new PaymentDto
        {
            Id = p.Id,
            Amount = p.Amount,
            Date = p.Date,
            Description = p.Description,
            ClientName = p.Client.Name,
            ClientEmail = p.Client.Email
        })
        .ToListAsync();
    }

    public async Task<IEnumerable<PaymentDto>> GetClientPaymentsAsync(int clientId)
    {
        var paymentsQuery = FindByCondition(p => p.ClientId == clientId, trackChanges: false)
            .Include(p => p.Client)
            .OrderByDescending(p => p.Date);

        return await paymentsQuery.Select(p => new PaymentDto
        {
            Id = p.Id,
            Amount = p.Amount,
            Date = p.Date,
            Description = p.Description,
            ClientName = p.Client.Name,
            ClientEmail = p.Client.Email
        })
        .ToListAsync();
    }
}