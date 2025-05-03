using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Models.ViewModels;

namespace HospitalAppointmentSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly int _pageSize = 10;

        public AppointmentsController(AppDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string searchString, string status, int page = 1)
        {
            try
            {
                var appointmentsQuery = _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .AsQueryable();

                // Фільтрація за статусом
                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<AppointmentStatus>(status, out var appointmentStatus))
                    {
                        appointmentsQuery = appointmentsQuery.Where(a => a.Status == appointmentStatus);
                    }
                }

                // Пошук
                if (!string.IsNullOrEmpty(searchString))
                {
                    appointmentsQuery = appointmentsQuery.Where(a =>
                        a.Doctor.LastName.Contains(searchString) ||
                        a.Doctor.FirstName.Contains(searchString) ||
                        a.Patient.LastName.Contains(searchString) ||
                        a.Patient.FirstName.Contains(searchString) ||
                        a.Notes.Contains(searchString));
                }

                // Отримуємо загальну кількість записів
                var totalItems = await appointmentsQuery.CountAsync();

                // Пагінація
                var appointments = await appointmentsQuery
                    .OrderByDescending(a => a.AppointmentDateTime)
                    .Skip((page - 1) * _pageSize)
                    .Take(_pageSize)
                    .ToListAsync();

                var viewModel = new AppointmentsListViewModel
                {
                    Appointments = appointments,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = _pageSize,
                        TotalItems = totalItems
                    },
                    CurrentStatus = status,
                    SearchString = searchString
                };

                // Додаємо статистику для відображення
                viewModel.TotalAppointments = totalItems;
                viewModel.ScheduledAppointments = await appointmentsQuery.CountAsync(a => a.Status == AppointmentStatus.Scheduled);
                viewModel.CompletedAppointments = await appointmentsQuery.CountAsync(a => a.Status == AppointmentStatus.Completed);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні списку призначень: {ex.Message}");
                TempData["Error"] = "Виникла помилка при завантаженні даних";
                return View(new AppointmentsListViewModel());
            }
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.Doctors = _context.Doctors.OrderBy(d => d.LastName).ToList();
                ViewBag.Patients = _context.Patients.OrderBy(p => p.LastName).ToList();
                return View(new Appointment { AppointmentDateTime = DateTime.Now.AddDays(1) });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при відкритті форми створення: {ex.Message}");
                TempData["Error"] = "Виникла помилка при завантаженні форми";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,PatientId,AppointmentDateTime,Notes")] Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Перевірка на дублікати
                    var existingAppointment = await _context.Appointments
                        .AnyAsync(a => a.DoctorId == appointment.DoctorId &&
                                     a.AppointmentDateTime == appointment.AppointmentDateTime);

                    if (existingAppointment)
                    {
                        ModelState.AddModelError("", "На цей час вже є запис до даного лікаря");
                        ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.LastName).ToListAsync();
                        ViewBag.Patients = await _context.Patients.OrderBy(p => p.LastName).ToListAsync();
                        return View(appointment);
                    }

                    appointment.Status = AppointmentStatus.Scheduled;
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Призначення успішно створено";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при створенні призначення: {ex.Message}");
                ModelState.AddModelError("", "Виникла помилка при створенні призначення");
            }

            ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.LastName).ToListAsync();
            ViewBag.Patients = await _context.Patients.OrderBy(p => p.LastName).ToListAsync();
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.LastName).ToListAsync();
            ViewBag.Patients = await _context.Patients.OrderBy(p => p.LastName).ToListAsync();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,DoctorId,PatientId,AppointmentDateTime,Status,Notes")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Перевірка на дублікати
                    var existingAppointment = await _context.Appointments
                        .AnyAsync(a => a.DoctorId == appointment.DoctorId &&
                                     a.AppointmentDateTime == appointment.AppointmentDateTime &&
                                     a.AppointmentId != appointment.AppointmentId);

                    if (existingAppointment)
                    {
                        ModelState.AddModelError("", "На цей час вже є запис до даного лікаря");
                        ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.LastName).ToListAsync();
                        ViewBag.Patients = await _context.Patients.OrderBy(p => p.LastName).ToListAsync();
                        return View(appointment);
                    }

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Призначення успішно оновлено";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.AppointmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні призначення: {ex.Message}");
                ModelState.AddModelError("", "Виникла помилка при оновленні призначення");
            }

            ViewBag.Doctors = await _context.Doctors.OrderBy(d => d.LastName).ToListAsync();
            ViewBag.Patients = await _context.Patients.OrderBy(p => p.LastName).ToListAsync();
            return View(appointment);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Призначення успішно видалено";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при видаленні призначення: {ex.Message}");
                TempData["Error"] = "Виникла помилка при видаленні призначення";
            }

            return RedirectToAction(nameof(Index));
        }

        // Допоміжні методи
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }

        // GET: Appointments/Calendar
        public IActionResult Calendar()
        {
            return View();
        }

        // API метод для отримання призначень для календаря
        [HttpGet]
        public async Task<JsonResult> GetAppointments(DateTime start, DateTime end)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDateTime >= start && a.AppointmentDateTime <= end)
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.Patient.FullName} - {a.Doctor.FullName}",
                    start = a.AppointmentDateTime,
                    end = a.AppointmentDateTime.AddMinutes(30),
                    status = a.Status.ToString(),
                    className = a.Status == AppointmentStatus.Completed ? "bg-success" :
                               a.Status == AppointmentStatus.Cancelled ? "bg-danger" : "bg-primary"
                })
                .ToListAsync();

            return Json(appointments);
        }
    }
}