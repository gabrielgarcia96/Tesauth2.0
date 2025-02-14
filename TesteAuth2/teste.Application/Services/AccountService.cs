using teste.Application.DTOs;
using teste.Application.Interfaces;
using teste.Domain.Interfaces;
using teste.Domain.Models;
using teste.Domain.Models.Enums;
using teste.Infrastructure.Repository;

namespace teste.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }


    public Task<User> GetEmailAsync(string email)
    {
       return _accountRepository.GetEmailAsync(email);
    }

    public Task<User> GetUserAsync(string username)
    {
       return _accountRepository.GetUserAsync(username);
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        var existUser = await _accountRepository.GetUserAsync(registerDto.Username);
        var existEmail = await _accountRepository.GetEmailAsync(registerDto.Email);

        if (existUser != null)
        {
            Console.WriteLine("Username already exists!");
            return;
        }
        if (existEmail != null)
        {
            Console.WriteLine("Email already exists!");
            return;
        }

        var newUser = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Roles =  Role.User

        };

        await  _accountRepository.CreateAsync(newUser);
       
    }

    public async Task<LoginDto> ValidadeUserAsync(string username, string password)
    {
        var getUser = await _accountRepository.GetUserAsync(username);
       

        if (getUser == null)
        {
            throw new UnauthorizedAccessException("User not found!");
        }

        var storedPassword = getUser.Password;

        if (string.IsNullOrEmpty(storedPassword))
        {
            throw new UnauthorizedAccessException("Stored password is null or empty!");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, storedPassword))
        {
            throw new UnauthorizedAccessException("Invalid username or password!");
        }


        return new LoginDto
        {
            Username = username,
            Roles = getUser.Roles
        };

    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _accountRepository.GetAllUsersAsync();
    }

}
