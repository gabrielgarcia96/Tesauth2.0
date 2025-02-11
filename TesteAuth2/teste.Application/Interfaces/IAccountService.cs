using teste.Application.DTOs;
using teste.Domain.Models;

namespace teste.Application.Interfaces;

public interface IAccountService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<User> GetUserAsync(string username);
    Task<User> GetEmailAsync(string email);
    Task<User> ValidadeUserAsync(string username, string password);
    Task<List<User>> GetAllUsersAsync();
}
