using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        
        [Required(ErrorMessage = "Ім'я пацієнта обов'язкове")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Прізвище пацієнта обов'язкове")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Телефон обов'язковий")]
        [Phone(ErrorMessage = "Некоректний формат телефону")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        public DateTime DateOfBirth { get; set; }
        
        public ICollection<Appointment> Appointments { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}