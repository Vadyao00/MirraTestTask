using MirraApi.Domain.Dtos;

namespace Contracts.IRepositories;

public interface IPaymentRepository
{
    Task<IEnumerable<PaymentDto>> GetRecentPaymentsAsync(int take);
    Task<IEnumerable<PaymentDto>> GetClientPaymentsAsync(int clientId);
}