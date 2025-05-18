using System.Net.Http.Json;
using HospitalAppointmentSystem.Client.Models;
using Microsoft.AspNetCore.Components.Authorization; // Додайте цей рядок// Важливо використовувати правильний namespace

namespace HospitalAppointmentSystem.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, 
        AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> Login(LoginModel model)
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
    public async Task<bool> Register(RegisterModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        (_authStateProvider as CustomAuthStateProvider)?.NotifyAuthenticationStateChanged();
    }
}