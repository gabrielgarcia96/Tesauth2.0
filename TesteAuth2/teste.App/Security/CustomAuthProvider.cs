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

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (OperatingSystem.IsBrowser())
            {
                var username = await _localStorage.GetItemAsync<string>("username");
                var roleString = await _localStorage.GetItemAsync<string>("role");

                Console.WriteLine($"Recuperado do LocalStorage - Username: {username}, Role: {roleString}");

                if (!string.IsNullOrEmpty(username) && Enum.TryParse(roleString, out Role roleValue))
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, roleValue.ToString())
                    }, "apiauth");

                    _principal = new ClaimsPrincipal(identity);
                }
                else
                {
                    _principal = new ClaimsPrincipal(new ClaimsIdentity()); // Usuário não autenticado
                }
            }

            return new AuthenticationState(_principal);
        }

        public async Task MarkUserAsAuthenticated(string username, Role role)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            }, "apiauth");

            _principal = new ClaimsPrincipal(identity);

            Console.WriteLine($"Usuário autenticado: {_principal.Identity.IsAuthenticated}");
            Console.WriteLine($"Definindo no LocalStorage - Username: {username}, Role: {role}");

            await _localStorage.SetItemAsync("username", username);
            await _localStorage.SetItemAsync("role", role.ToString());

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            Console.WriteLine("Usuário deslogando...");

            await _localStorage.RemoveItemAsync("username");
            await _localStorage.RemoveItemAsync("role");

            _principal = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
