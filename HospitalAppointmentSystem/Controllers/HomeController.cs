using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Models.ViewModels;

namespace HospitalAppointmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 10; // Кількість записів на сторінці

            int doctorsCount = _context.Doctors.Count();
            int patientsCount = _context.Patients.Count();
            int appointmentsCount = _context.Appointments.Count();
            
            // Зберігаємо статистику у ViewBag для зручності використання у представленні
            ViewBag.DoctorsCount = doctorsCount;
            ViewBag.PatientsCount = patientsCount;
            ViewBag.AppointmentsCount = appointmentsCount;
            
            var upcomingAppointments = _context.Appointments
                .Where(a => a.Status == AppointmentStatus.Scheduled && a.AppointmentDateTime > DateTime.Now)
                .OrderBy(a => a.AppointmentDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                
            foreach (var appointment in upcomingAppointments)
            {
                appointment.Doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == appointment.DoctorId);
                appointment.Patient = _context.Patients.FirstOrDefault(p => p.PatientId == appointment.PatientId);
            }
            
            // Створюємо об'єкт PagingInfo для пагінації
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = appointmentsCount
            };
            
            // Створюємо об'єкт ViewModel і заповнюємо його даними
            var viewModel = new AppointmentsListViewModel
            {
                Appointments = upcomingAppointments,
                PagingInfo = pagingInfo,
                TotalAppointments = appointmentsCount,
                DoctorsCount = doctorsCount,
                PatientsCount = patientsCount
            };
            
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}