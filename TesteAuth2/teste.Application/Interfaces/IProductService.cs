using teste.Domain.Models;

namespace teste.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetByCodeAsync(int code);
    Task AddProductAsync(Product product);
    Task UpdateAsync(int code, Product product);
    Task DeleteAsync(int code);
    Task<Product> GetProductNameAsync(string productName);
}
