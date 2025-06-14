using MirraApi.Domain.Entities;

namespace Contracts.IRepositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges);

    Task<Client?> GetClientByIdAsync(int clientId, bool trackChanges);
    
    void CreateClient(Client client);

    void UpdateClient(Client client);

    void DeleteClient(Client client);
}