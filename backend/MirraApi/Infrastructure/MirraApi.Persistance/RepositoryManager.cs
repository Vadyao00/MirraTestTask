using Contracts.IRepositories;
using MirraApi.Persistance.Repositories;

namespace MirraApi.Persistance;

public class RepositoryManager(AppDbContext context) : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _userRepository = new(() => new UserRepository(context));
    private readonly Lazy<IClientRepository> _clientRepository = new(() => new ClientRepository(context));
    private readonly Lazy<IExchangeRateRepository> _exchangeRateRepository = new(() => new ExchangeRateRepository(context));
    private readonly Lazy<IPaymentRepository> _paymentRepository = new(() => new PaymentRepository(context));

    public IUserRepository User => _userRepository.Value;
    public IClientRepository Client => _clientRepository.Value;
    public IExchangeRateRepository ExchangeRate => _exchangeRateRepository.Value;
    public IPaymentRepository Payment => _paymentRepository.Value;
    
    public async Task SaveAsync() => await context.SaveChangesAsync();
}