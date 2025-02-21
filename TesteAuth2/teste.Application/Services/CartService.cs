using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using teste.Domain.Models;

public class CartService
{
    private const string CartStorageKey = "cartItems";
    private readonly ProtectedLocalStorage _localStorage;

    public event Action OnChange;
    private List<CartItem> cartItems = new();

    public CartService(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeCart()
    {
        var storedCart = await _localStorage.GetAsync<string>(CartStorageKey);
        if (storedCart.Success && storedCart.Value != null)
        {
            cartItems = JsonSerializer.Deserialize<List<CartItem>>(storedCart.Value) ?? new List<CartItem>();
            NotifyStateChanged();
        }
    }

    public async Task AddToCart(Product product)
    {
        var item = cartItems.FirstOrDefault(p => p.Product.Code == product.Code);
        if (item != null)
        {
            item.Quantity++;
        }
        else
        {
            cartItems.Add(new CartItem { Product = product, Quantity = 1 });
        }

        await SaveCartToStorage();
        NotifyStateChanged();
    }

    public async Task RemoveFromCart(Product product)
    {
        var item = cartItems.FirstOrDefault(p => p.Product.Code == product.Code);
        if (item != null)
        {
            cartItems.Remove(item);
            await SaveCartToStorage();
            NotifyStateChanged();
        }
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        return cartItems;
    }

    public decimal GetTotalPrice()
    {
        return cartItems.Sum(item => item.Product.Price * item.Quantity);
    }

    private async Task SaveCartToStorage()
    {
        var jsonCart = JsonSerializer.Serialize(cartItems);
        await _localStorage.SetAsync(CartStorageKey, jsonCart);
    }

    public int GetCartItemCount()
    {
        return cartItems.Sum(item => item.Quantity);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
