using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        
        [Required(ErrorMessage = "Введіть ім'я")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Введіть прізвище")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Введіть спеціалізацію")]
        [Display(Name = "Спеціалізація")]
        public string Specialization { get; set; }
        
        public string FullName => $"{LastName} {FirstName}";
        
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorAvailability> Availabilities { get; set; }
    }
}