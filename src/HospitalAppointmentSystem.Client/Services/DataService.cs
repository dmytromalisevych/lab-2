using System.Net.Http.Json;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
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

        public async Task<List<Doctor>> GetDoctors()
        {
            return await _httpClient.GetFromJsonAsync<List<Doctor>>("api/doctors") ?? new List<Doctor>();
        }

        public async Task<List<Patient>> GetPatients()
        {
            return await _httpClient.GetFromJsonAsync<List<Patient>>("api/patients") ?? new List<Patient>();
        }

        public async Task CreateAppointment(Appointment appointment)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appointments", appointment);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAppointment(int id, Appointment appointment)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{id}", appointment);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAppointment(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/appointments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}