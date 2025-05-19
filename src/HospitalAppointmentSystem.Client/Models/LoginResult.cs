namespace HospitalAppointmentSystem.Client.Models
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string? Error { get; set; }
        public UserRole Role { get; set; }
    }
}