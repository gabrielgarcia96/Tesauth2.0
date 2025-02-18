using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using teste.Domain.Models;
using teste.Infrastructure.Data;
using teste.Infrastructure.Interfaces;

namespace teste.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ConfigMongoDb _configMongoDb;
    private readonly IConfiguration _configuration;

    public ProductRepository(ConfigMongoDb configMongoDb, IConfiguration configuration)
    {
        _configMongoDb = configMongoDb;
        _configuration = configuration;
    }

    public async Task AddProductAsync(Product product)
    {
        await _configMongoDb.ProductCollection.InsertOneAsync(product);
    }

    public async Task DeleteAsync(string code)
    {
        await _configMongoDb.ProductCollection.DeleteOneAsync(code);
    }

    public  Task<List<Product>> GetAllProductsAsync()
    {
        return _configMongoDb.ProductCollection.Find(p => true)
            .Project(p => new Product
            {
                Code = p.Code,
                NameProduct = p.NameProduct,
                Price = p.Price,
                CodBarras = p.CodBarras,
                Active = p.Active
            }).ToListAsync();
    }

    public  Task<Product> GetByCodeAsync(string code)
    {
        return _configMongoDb.ProductCollection.Find(c => c.Code == code).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string code, Product product)
    {
       var filter = Builders<Product>.Filter.Eq(p => p.Code, code);
        var update = Builders<Product>.Update
             .Set(p => p.Code, product.Code)
             .Set(p => p.NameProduct, product.NameProduct)
             .Set(p => p.Price, product.Price)
             .Set(p => p.CodBarras, product.CodBarras)
             .Set(p => p.Active, product.Active);

        await _configMongoDb.ProductCollection.UpdateOneAsync(filter, update);
    }

    public Task<Product> GetProductNameAsync(string nameProduct)
    {
        return _configMongoDb.ProductCollection.Find(n => n.NameProduct == nameProduct).FirstOrDefaultAsync();
    }
}
