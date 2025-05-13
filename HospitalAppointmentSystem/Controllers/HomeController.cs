using Microsoft.AspNetCore.Mvc;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                DoctorsCount = await _context.Doctors.CountAsync(),
                PatientsCount = await _context.Patients.CountAsync(),
                ScheduledAppointments = await _context.Appointments
                    .CountAsync(a => a.Status == AppointmentStatus.Scheduled),
                TotalAppointments = await _context.Appointments.CountAsync(),
                UpcomingAppointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .Where(a => a.AppointmentDateTime.Date >= DateTime.Today)
                    .OrderBy(a => a.AppointmentDateTime)
                    .ToListAsync(),
                DoctorsBySpecialization = await _context.Doctors
                    .GroupBy(d => d.Specialization)
                    .ToDictionaryAsync(g => g.Key, g => g.Count())
            };

            return View(viewModel);
        }
    }
}