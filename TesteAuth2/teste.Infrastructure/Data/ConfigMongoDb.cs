using MongoDB.Driver;
using teste.Domain.Models;

namespace teste.Infrastructure.Data;

public class ConfigMongoDb
{
    private readonly IMongoDatabase _database;
    
    public ConfigMongoDb(string stringConnection, string databaseName)
    {
        var configConnection = new MongoClient(stringConnection);
        _database = configConnection.GetDatabase(databaseName);
    }

    public IMongoCollection<User> UserCollection => _database.GetCollection<User>("Users");
    public IMongoCollection<Product> ProductCollection => _database.GetCollection<Product>("Products");
}
