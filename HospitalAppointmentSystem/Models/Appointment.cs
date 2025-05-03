using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required(ErrorMessage = "Дата прийому обов'язкова")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата прийому")]
        public DateTime AppointmentDateTime { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Примітки")]
        public string Notes { get; set; }
        
        [Required]
        [Display(Name = "Статус")]
        public AppointmentStatus Status { get; set; }
        
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        
    }
    
    public enum AppointmentStatus
    {
        [Display(Name = "Заплановано")]
        Scheduled,
        
        [Display(Name = "Завершено")]
        Completed,
        
        [Display(Name = "Скасовано")]
        Cancelled,
        
        [Display(Name = "Оновлено")]
        UpdatedAt
    }
}