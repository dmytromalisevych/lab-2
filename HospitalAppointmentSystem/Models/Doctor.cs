using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        
        [Required(ErrorMessage = "Ім'я лікаря обов'язкове")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Прізвище лікаря обов'язкове")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Спеціальність обов'язкова")]
        [Display(Name = "Спеціальність")]
        public string Specialization { get; set; }
        
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Телефон обов'язковий")]
        [Phone(ErrorMessage = "Некоректний формат телефону")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Availability> Availabilities { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}