using System.Net.Http.Json;
using HospitalAppointmentSystem.Client.Interfaces;
using HospitalAppointmentSystem.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;

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

        public async Task<bool> LoginAsync(LoginModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    await _localStorage.SetItemAsync("authToken", token);
                    (_authStateProvider as CustomAuthStateProvider)?.NotifyAuthenticationStateChanged();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            (_authStateProvider as CustomAuthStateProvider)?.NotifyAuthenticationStateChanged();
        }

        public async Task<User> GetCurrentUserAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrEmpty(token))
                    return null;

                var response = await _httpClient.GetFromJsonAsync<User>("api/auth/user");
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}