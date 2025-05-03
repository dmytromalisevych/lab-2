using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.Models.ViewModels;

namespace HospitalAppointmentSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly int _pageSize = 5;

        public DoctorsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var doctorsQuery = _context.Doctors
                .OrderBy(d => d.LastName);
            
            int totalItems = doctorsQuery.Count();
            
            var doctors = doctorsQuery
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
            
            var viewModel = new DoctorsListViewModel
            {
                Doctors = doctors,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = totalItems
                }
            };
            
            return View(viewModel);
        }
        
        public IActionResult Details(int id)
        {
            var doctor = _context.Doctors
                .Include(d => d.Availabilities)
                .FirstOrDefault(d => d.DoctorId == id);
                
            if (doctor == null)
            {
                return NotFound();
            }
            
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == id && a.Status == AppointmentStatus.Scheduled && a.AppointmentDateTime > DateTime.Now)
                .OrderBy(a => a.AppointmentDateTime)
                .ToList();
                
            ViewBag.Appointments = appointments;
            
            return View(doctor);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }
        
        public IActionResult Edit(int id)
        {
            var doctor = _context.Doctors.Find(id);
            
            if (doctor == null)
            {
                return NotFound();
            }
            
            return View(doctor);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorId))
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
            return View(doctor);
        }
        
        public IActionResult Delete(int id)
        {
            var doctor = _context.Doctors.Find(id);
                
            if (doctor == null)
            {
                return NotFound();
            }
            
            return View(doctor);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var doctor = _context.Doctors.Find(id);
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
}