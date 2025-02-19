using teste.Domain.Models;

namespace teste.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetByCodeAsync(string code);
    Task AddProductAsync(Product product);
    Task UpdateAsync(string code, Product product);
    Task DeleteAsync(string code);
    Task<Product> GetProductNameAsync(string productName);
}
