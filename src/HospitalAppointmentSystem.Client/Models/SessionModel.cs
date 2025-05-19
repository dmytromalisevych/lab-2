namespace HospitalAppointmentSystem.Client.Models;

public class SessionModel
{
    public int Id { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}