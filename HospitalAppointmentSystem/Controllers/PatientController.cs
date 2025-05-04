using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(AppDbContext context, ILogger<PatientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await _context.Patients
                    .Include(p => p.Appointments)
                    .OrderBy(p => p.LastName)
                    .ToListAsync();
                return View(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patients: {ex.Message}");
                return View(new List<Patient>());
            }
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var patient = await _context.Patients
                    .Include(p => p.Appointments)
                    .ThenInclude(a => a.Doctor)
                    .FirstOrDefaultAsync(p => p.PatientId == id);

                if (patient == null)
                {
                    return NotFound();
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient details: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Пацієнта успішно додано";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating patient: {ex.Message}");
                    ModelState.AddModelError("", "Помилка при створенні пацієнта");
                }
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient for edit: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,DateOfBirth")] Patient patient)
        {
            if (id != patient.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Дані пацієнта оновлено";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientId))
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
                    _logger.LogError($"Error updating patient: {ex.Message}");
                    ModelState.AddModelError("", "Помилка при оновленні даних пацієнта");
                }
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var patient = await _context.Patients
                    .Include(p => p.Appointments)
                    .FirstOrDefaultAsync(p => p.PatientId == id);

                if (patient == null)
                {
                    return NotFound();
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting patient for delete: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.Appointments)
                    .FirstOrDefaultAsync(p => p.PatientId == id);

                if (patient == null)
                {
                    return NotFound();
                }

                if (patient.Appointments != null && patient.Appointments.Any(a => a.Status == AppointmentStatus.Scheduled))
                {
                    TempData["Error"] = "Неможливо видалити пацієнта з активними призначеннями";
                    return RedirectToAction(nameof(Index));
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Пацієнта видалено";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting patient: {ex.Message}");
                TempData["Error"] = "Помилка при видаленні пацієнта";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}