using teste.Application.Interfaces;
using teste.Domain.Models;
using teste.Infrastructure.Interfaces;

namespace teste.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task AddProductAsync(Product product)
    {
        var existCode = await _productRepository.GetByCodeAsync(product.Code);
        var existProductName = await _productRepository.GetProductNameAsync(product.NameProduct);

        if (existCode!= null)
        {
            Console.WriteLine("Code Product already exists!");
            return;
        }

        if (existProductName != null)
        {
            Console.WriteLine("Product Name already exists!");
            return;
        }

        await _productRepository.AddProductAsync(product);

    }

    public Task DeleteAsync(int code)
    {
        return _productRepository.DeleteAsync(code);
    }

    public Task<List<Product>> GetAllProductsAsync()
    {
        return _productRepository.GetAllProductsAsync();
    }

    public Task<Product> GetByCodeAsync(int code)
    {
        return _productRepository.GetByCodeAsync(code);
    }

    public Task UpdateAsync(int code, Product product)
    {
       return _productRepository.UpdateAsync(code, product);
    }
   public  Task<Product> GetProductNameAsync(string productName)
    {
       return _productRepository.GetProductNameAsync(productName);
    }


}
