using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HospitalAppointmentSystem.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            // Додаємо логування
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => 
                new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();
            
            Console.WriteLine("Starting the application...");
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Application start failed: {ex}");
            throw;
        }
    }
}