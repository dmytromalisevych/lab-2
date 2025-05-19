using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using HospitalAppointmentSystem.Client.Interfaces;
using HospitalAppointmentSystem.Client.Models;
using HospitalAppointmentSystem.Client.Auth;

namespace HospitalAppointmentSystem.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
                if (!response.IsSuccessStatusCode) return false;

                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (result?.Successful != true) return false;

                await _localStorage.SetItemAsync("authToken", result.Token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
                if (!response.IsSuccessStatusCode) return false;

                var result = await response.Content.ReadFromJsonAsync<RegisterResult>();
                return result?.Successful ?? false;
            }
            catch
            {
                return false;
            }
        }
    }
}