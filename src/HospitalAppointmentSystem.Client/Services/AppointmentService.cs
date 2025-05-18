using System.Net.Http.Json;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services;

public class AppointmentService
{
    private readonly HttpClient _httpClient;

    public AppointmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Appointment>> GetAppointments()
    {
        return await _httpClient.GetFromJsonAsync<List<Appointment>>("api/appointments") ?? new List<Appointment>();
    }

    public async Task<Appointment> GetAppointment(int id)
    {
        return await _httpClient.GetFromJsonAsync<Appointment>($"api/appointments/{id}")
               ?? throw new Exception("Запис не знайдено");
    }

    public async Task<Appointment> CreateAppointment(Appointment appointment)
    {
        var response = await _httpClient.PostAsJsonAsync("api/appointments", appointment);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Appointment>()
               ?? throw new Exception("Помилка при створенні запису");
    }

    public async Task<Appointment> UpdateAppointment(int id, Appointment appointment)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/appointments/{id}", appointment);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Appointment>()
               ?? throw new Exception("Помилка при оновленні запису");
    }

    public async Task DeleteAppointment(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/appointments/{id}");
        response.EnsureSuccessStatusCode();
    }
}