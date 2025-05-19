using Microsoft.JSInterop;
using System.Text.Json;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services
{
    public class IndexedDbService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _dbName = "HospitalAppointmentSystem";

        public IndexedDbService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            await _jsRuntime.InvokeVoidAsync("initDatabase");
        }

        public async Task<T> GetByIdAsync<T>(string storeName, int id)
        {
            return await _jsRuntime.InvokeAsync<T>("getFromDb", _dbName, storeName, id);
        }

        public async Task<List<T>> GetAllAsync<T>(string storeName)
        {
            return await _jsRuntime.InvokeAsync<List<T>>("getAllFromDb", _dbName, storeName);
        }

        public async Task<int> AddAsync<T>(string storeName, T item)
        {
            return await _jsRuntime.InvokeAsync<int>("addToDb", _dbName, storeName, item);
        }

        public async Task UpdateAsync<T>(string storeName, T item)
        {
            await _jsRuntime.InvokeVoidAsync("updateInDb", _dbName, storeName, item);
        }

        public async Task DeleteAsync(string storeName, int id)
        {
            await _jsRuntime.InvokeVoidAsync("deleteFromDb", _dbName, storeName, id);
        }
    }
}