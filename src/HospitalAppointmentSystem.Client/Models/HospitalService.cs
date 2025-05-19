using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Client.Models
{
    public class HospitalService
    {
        private readonly AppDbContext _context;

        public HospitalService(AppDbContext context)
        {
            _context = context;
        }

        // Методи для роботи з лікарями
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        // Методи для роботи з пацієнтами
        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        // Методи для роботи з записами
        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Методи для роботи з медичними записами
        public async Task<List<MedicalRecord>> GetMedicalRecordsAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
        }
    }
}