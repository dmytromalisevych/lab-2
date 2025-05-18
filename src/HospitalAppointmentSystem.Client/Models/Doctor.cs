namespace HospitalAppointmentSystem.Client.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}