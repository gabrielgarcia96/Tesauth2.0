using System.Threading.Tasks;
using teste.Domain.Models;

namespace teste.Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetByCodeAsync(int code);
    Task<Product> GetProductNameAsync(string nameProduct);
    Task AddProductAsync(Product product);
    Task UpdateAsync(int code, Product product);
    Task DeleteAsync(int code);
}
