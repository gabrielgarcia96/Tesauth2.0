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

                Console.WriteLine($"LocalStorage - Username: {username}, Role: {roleString}");

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
                    identity = new ClaimsIdentity(); // Usuário não autenticado
                }

                _principal = new ClaimsPrincipal(identity);

                // 🔹 Chama a notificação somente após garantir que os dados foram carregados corretamente
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));

                return new AuthenticationState(_principal);
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }


        public async Task MarkUserAsAuthenticated(string username, Role role)
        {
            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role.ToString())
    }, "apiauth");

            _principal = new ClaimsPrincipal(identity);

            Console.WriteLine($"User auth: {_principal.Identity.IsAuthenticated}");
            Console.WriteLine($"LocalStorage - Username: {username}, Role: {role}");

            await _localStorage.SetItemAsync("username", username);
            await _localStorage.SetItemAsync("role", role.ToString());

            // 🔹 Aguarda a persistência no LocalStorage antes de notificar mudanças
            await Task.Delay(100);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));
        }

        public async Task Logout()
        {
            Console.WriteLine("Logof user...");

            await _localStorage.RemoveItemAsync("username");
            await _localStorage.RemoveItemAsync("role");

            _principal = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
