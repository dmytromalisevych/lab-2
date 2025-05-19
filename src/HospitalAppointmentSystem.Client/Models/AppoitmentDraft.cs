namespace HospitalAppointmentSystem.Client.Models;

public class AppointmentDraft
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
}