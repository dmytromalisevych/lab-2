using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Doctor>> GetDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<List<Patient>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }
        public async Task<List<Appointment>> GetAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                throw new Exception("Запис не знайдено");

            return appointment;
        }

        public async Task CreateAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(int id, Appointment appointment)
        {
            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
                throw new Exception("Запис не знайдено");

            existingAppointment.DateTime = appointment.DateTime;
            existingAppointment.DoctorId = appointment.DoctorId;
            existingAppointment.PatientId = appointment.PatientId;
            existingAppointment.Description = appointment.Description;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}