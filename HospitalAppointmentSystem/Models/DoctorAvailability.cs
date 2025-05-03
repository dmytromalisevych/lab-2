using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class DoctorAvailability
    {
        public int DoctorAvailabilityId { get; set; }
        
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        
        [Required]
        [Display(Name = "День тижня")]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        [Display(Name = "Час початку")]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        [Display(Name = "Час закінчення")]
        public TimeSpan EndTime { get; set; }
    }
}