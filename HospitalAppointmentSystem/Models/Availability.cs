using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Availability
    {
        public int AvailabilityId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required(ErrorMessage = "День тижня обов'язковий")]
        [Display(Name = "День тижня")]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required(ErrorMessage = "Час початку обов'язковий")]
        [Display(Name = "Час початку")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        
        [Required(ErrorMessage = "Час закінчення обов'язковий")]
        [Display(Name = "Час закінчення")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        
        public Doctor Doctor { get; set; }
    }
}