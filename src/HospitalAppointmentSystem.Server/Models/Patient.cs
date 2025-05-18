using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Server.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}