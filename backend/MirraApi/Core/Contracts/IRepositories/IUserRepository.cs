
using MirraApi.Domain.Entities;

namespace Contracts.IRepositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    void CreateUser(User user);
    void DeleteUser(User user);
    void UpdateUser(User user);
}