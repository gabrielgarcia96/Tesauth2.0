using teste.Domain.Models;

namespace teste.Domain.Interfaces;

public interface IAccountRepository
{
    Task CreateAsync(User user);
    Task<User> GetUserAsync(string username);
    Task<User> GetEmailAsync(string email);
    Task<User> GetPasswordAsync(string password);
    Task<List<User>> GetAllUsersAsync();
}
