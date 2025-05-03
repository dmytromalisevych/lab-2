using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
    
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    
        [Required(ErrorMessage = "Виберіть дату та час")]
        [Display(Name = "Дата та час")]
        public DateTime AppointmentDateTime { get; set; }
    
        [Display(Name = "Примітки")]
        public string Notes { get; set; }
    
        [Display(Name = "Статус")]
        public AppointmentStatus Status { get; set; }
    }
}