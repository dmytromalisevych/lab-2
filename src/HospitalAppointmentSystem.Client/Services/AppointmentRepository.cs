using HospitalAppointmentSystem.Client.Models;
using HospitalAppointmentSystem.Client.Interfaces;

namespace HospitalAppointmentSystem.Client.Services;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly IndexedDbService _dbService;
    private const string StoreName = "appointments";

    public AppointmentRepository(IndexedDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _dbService.GetAllAsync<Appointment>(StoreName);
    }

    public async Task<Appointment> GetByIdAsync(int id)
    {
        return await _dbService.GetByIdAsync<Appointment>(StoreName, id);
    }

    public async Task<int> CreateAsync(Appointment appointment)
    {
        return await _dbService.AddAsync(StoreName, appointment);
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        await _dbService.UpdateAsync(StoreName, appointment);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbService.DeleteAsync(StoreName, id);
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        var allAppointments = await GetAllAsync();
        return allAppointments.Where(a => a.DoctorId == doctorId).ToList();
    }

    public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
    {
        var allAppointments = await GetAllAsync();
        return allAppointments.Where(a => a.PatientId == patientId).ToList();
    }
}