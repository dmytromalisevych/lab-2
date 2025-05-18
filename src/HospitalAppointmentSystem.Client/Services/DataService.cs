using System.Net.Http.Json;
using System.Net.Http.Headers;
using HospitalAppointmentSystem.Client.Interfaces;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public DataService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<Doctor>>("api/doctors");
                return response ?? new List<Doctor>();
            }
            catch
            {
                return new List<Doctor>();
            }
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<Appointment>>("api/appointments");
                return response ?? new List<Appointment>();
            }
            catch
            {
                return new List<Appointment>();
            }
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/appointments", appointment);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointment.Id}", appointment);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/appointments/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}