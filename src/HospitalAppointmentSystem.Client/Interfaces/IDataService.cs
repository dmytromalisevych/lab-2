using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Interfaces
{
    public interface IDataService
    {
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<bool> CreateAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int id);
    }
}