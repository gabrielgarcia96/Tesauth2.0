using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using teste.Domain.Interfaces;
using teste.Domain.Models;
using teste.Infrastructure.Data;

namespace teste.Infrastructure.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly ConfigMongoDb _configMongoDb;
    private readonly IConfiguration _configuration;

    public AccountRepository(ConfigMongoDb configMongoDb, IConfiguration configuration)
    {
        _configMongoDb = configMongoDb;
        _configuration = configuration;
    }

    public async Task CreateAsync(User user)
    {
       await _configMongoDb.UserCollection.InsertOneAsync(user);
    }

    public Task<User> GetUserAsync(string username)
    {
        return _configMongoDb.UserCollection.Find
            (u => u.Username == username).FirstOrDefaultAsync();
    }

    public Task<User> GetEmailAsync(string email)
    {
        return _configMongoDb.UserCollection.Find
            (e => e.Email == email).FirstOrDefaultAsync(); 
    }

    public Task<User> GetPasswordAsync(string password)
    {
        return _configMongoDb.UserCollection.Find
            (p => p.Password == password).FirstOrDefaultAsync();
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _configMongoDb.UserCollection.Find(u => true)
            .Project(u => new User
            {
                Username = u.Username,
                Email = u.Email,
                Roles = u.Roles
            })
            .ToListAsync();
    }
   
}
