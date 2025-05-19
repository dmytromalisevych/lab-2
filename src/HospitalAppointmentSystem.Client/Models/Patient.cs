namespace HospitalAppointmentSystem.Client.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Appointment> Appointments { get; set; }
    public List<MedicalRecord> MedicalRecords { get; set; }
}