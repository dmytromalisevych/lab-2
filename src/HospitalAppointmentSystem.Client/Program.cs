using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Client;
using HospitalAppointmentSystem.Client.Services;
using HospitalAppointmentSystem.Client.Models;
using HospitalAppointmentSystem.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Додаємо сервіси авторизації
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthService>();

// Додаємо сервіси для роботи з даними
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<IDataService, DataService>();

await builder.Build().RunAsync();