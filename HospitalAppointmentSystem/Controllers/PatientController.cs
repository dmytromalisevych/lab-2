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
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly int _pageSize = 5;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {

            var patientsQuery = _context.Patients
                .OrderBy(p => p.LastName);
            
            int totalItems = patientsQuery.Count();
            
            var patients = patientsQuery
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
            
            var viewModel = new PatientsListViewModel
            {
                Patients = patients,
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
            var patient = _context.Patients.Find(id);
                
            if (patient == null)
            {
                return NotFound();
            }
            
            var appointments = _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == id)
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToList();
                
            ViewBag.Appointments = appointments;
            
            return View(patient);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }
        
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);
            
            if (patient == null)
            {
                return NotFound();
            }
            
            return View(patient);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Patient patient)
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
                    _context.SaveChanges();
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
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }
        
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.Find(id);
                
            if (patient == null)
            {
                return NotFound();
            }
            
            return View(patient);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var patient = _context.Patients.Find(id);
            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}