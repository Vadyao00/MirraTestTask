using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Repositories;

public class ClientRepository(AppDbContext dbContext) : RepositoryBase<Client>(dbContext), IClientRepository
{
    
    public async Task<IEnumerable<Client>> GetAllClientsAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int clientId, bool trackChanges)
    {
        return await FindByCondition(c => c.Id == clientId, trackChanges)
            .Include(c => c.Payments)
            .FirstOrDefaultAsync();
    }
    
    public void CreateClient(Client client) => Create(client);
    
    public void UpdateClient(Client client) => Update(client);
    
    public void DeleteClient(Client client) => Delete(client);
}