using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using teste.Domain.Models.Enums;

namespace teste.App.Security
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _principal = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly ILocalStorageService _localStorage;

        public CustomAuthProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }


        public async Task InitializeAuthState()
        {
            var username = await _localStorage.GetItemAsync<string>("username");
            var roleString = await _localStorage.GetItemAsync<string>("role");

            Console.WriteLine($"Recuperado do LocalStorage - Username: {username}, Role: {roleString}");

            ClaimsIdentity identity;
            if (!string.IsNullOrEmpty(username) && Enum.TryParse(roleString, out Role roleValue))
            {
                identity = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, roleValue.ToString())
        }, "apiauth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            _principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));
        }


        public async Task MarkUserAsAuthenticated(string username, Role role)
        {
            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role.ToString())
    }, "apiauth");

            _principal = new ClaimsPrincipal(identity);

            Console.WriteLine($"Autenticando usuário: {username}, Papel: {role}");
            Console.WriteLine($"Identity IsAuthenticated: {_principal.Identity.IsAuthenticated}");

            await _localStorage.SetItemAsync("username", username);
            await _localStorage.SetItemAsync("role", role.ToString());

            await Task.Delay(100);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));
        }


        public async Task Logout()
        {
            
            await _localStorage.RemoveItemAsync("username");
            await _localStorage.RemoveItemAsync("role");
            await _localStorage.RemoveItemAsync("cart");

            _principal = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
