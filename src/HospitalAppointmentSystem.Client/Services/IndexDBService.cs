using Microsoft.JSInterop;
using System.Text.Json;

namespace HospitalAppointmentSystem.Client.Services
{
    public class IndexedDBService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string DbName = "HospitalAppointmentSystem.db";
        private const int DbVersion = 1;

        public IndexedDBService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            await _jsRuntime.InvokeVoidAsync("initializeIndexedDB", DbName, DbVersion);
        }

        public async Task<T> GetDataAsync<T>(string storeName, int id)
        {
            var json = await _jsRuntime.InvokeAsync<string>("getFromIndexedDB", DbName, storeName, id);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task<List<T>> GetAllDataAsync<T>(string storeName)
        {
            var jsonArray = await _jsRuntime.InvokeAsync<string[]>("getAllFromIndexedDB", DbName, storeName);
            return jsonArray?.Select(json => JsonSerializer.Deserialize<T>(json)).ToList() ?? new List<T>();
        }

        public async Task SaveDataAsync<T>(string storeName, T data)
        {
            var json = JsonSerializer.Serialize(data);
            await _jsRuntime.InvokeVoidAsync("saveToIndexedDB", DbName, storeName, json);
        }

        public async Task DeleteDataAsync(string storeName, int id)
        {
            await _jsRuntime.InvokeVoidAsync("deleteFromIndexedDB", DbName, storeName, id);
        }
    }
}