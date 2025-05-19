using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Client.Models;

public class Appointment
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public virtual Doctor? Doctor { get; set; }
    public virtual Patient? Patient { get; set; }
}