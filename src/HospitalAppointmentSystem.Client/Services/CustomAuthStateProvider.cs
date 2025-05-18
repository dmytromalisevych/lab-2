using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace HospitalAppointmentSystem.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthService _authService;

    public CustomAuthStateProvider(ILocalStorageService localStorage, AuthService authService)
    {
        _localStorage = localStorage;
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Створіть claims на основі токену
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "user"),
            // Додайте інші claims за потреби
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}