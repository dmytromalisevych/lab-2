using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Client.Models;

public class Appointment
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Дата є обов'язковою")]
    public DateTime Date { get; set; }
    
    [Required(ErrorMessage = "Ім'я пацієнта є обов'язковим")]
    [StringLength(100, ErrorMessage = "Ім'я пацієнта не може перевищувати 100 символів")]
    public string? PatientName { get; set; }
    
    [Required(ErrorMessage = "Ім'я лікаря є обов'язковим")]
    [StringLength(100, ErrorMessage = "Ім'я лікаря не може перевищувати 100 символів")]
    public string? DoctorName { get; set; }
    
    [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів")]
    public string? Description { get; set; }
    
    public string? Status { get; set; } = "Заплановано";
}