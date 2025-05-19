using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services
{
    public interface IDataService
    {
        Task<List<Appointment>> GetAppointments();
        Task<Appointment> GetAppointment(int id);
        Task<List<Doctor>> GetDoctors();
        Task<List<Patient>> GetPatients();
        Task CreateAppointment(Appointment appointment);
        Task UpdateAppointment(int id, Appointment appointment);
        Task DeleteAppointment(int id);
    }
}