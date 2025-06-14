namespace Contracts.IRepositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IClientRepository Client { get; }
    IPaymentRepository Payment { get; }
    IExchangeRateRepository ExchangeRate { get; }
    Task SaveAsync();
}