using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Patients/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: /Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: /Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Пацієнта успішно додано";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Не вдалося створити запис. Спробуйте ще раз.");
            }
            return View(patient);
        }

        // GET: /Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: /Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,DateOfBirth")] Patient patient)
        {
            if (id != patient.PatientId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Дані пацієнта оновлено";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                if (!PatientExists(patient.PatientId))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "Не вдалося оновити запис. Спробуйте ще раз.");
            }
            
            return View(patient);
        }

        // POST: /Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient != null)
                {
                    _context.Patients.Remove(patient);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Пацієнта видалено";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Не вдалося видалити запис";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}