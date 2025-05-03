using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Models.ViewModels;


namespace HospitalAppointmentSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(AppDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

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

                var totalAppointments = await appointmentsQuery.CountAsync();
            
                var appointments = await appointmentsQuery
                    .OrderByDescending(a => a.AppointmentDateTime)
                    .ToListAsync();

                return View(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні списку призначень: {ex.Message}");
                return View(new List<Appointment>());
            }
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
        
        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Doctors.ToList();
            ViewBag.Patients = _context.Patients.ToList();
            return View();
        }
        
        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,DoctorId,AppointmentDateTime,Notes")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        appointment.Status = AppointmentStatus.Scheduled;
                        _context.Add(appointment);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Помилка при збереженні даних: " + ex.Message);
                    }
                }
            }
    
            ViewBag.Doctors = await _context.Doctors.ToListAsync();
            ViewBag.Patients = await _context.Patients.ToListAsync();
            return View(appointment);
        }
        
        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            
            ViewBag.Doctors = _context.Doctors.ToList();
            ViewBag.Patients = _context.Patients.ToList();
            return View(appointment);
        }
        
        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,PatientId,DoctorId,AppointmentDateTime,Notes,Status")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Doctors = _context.Doctors.ToList();
            ViewBag.Patients = _context.Patients.ToList();
            return View(appointment);
        }
        
        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                if (appointment == null)
                {
                    return NotFound();
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
        
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Додаємо логування помилки
                return RedirectToAction(nameof(Index));
            }
        }
        
        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Appointments/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
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
        
        // POST: Appointments/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            appointment.Status = AppointmentStatus.Cancelled;
            _context.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}