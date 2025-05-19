using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Client.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Виберіть дату")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Виберіть лікаря")]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Виберіть пацієнта")]
        public int PatientId { get; set; }
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Додайте опис")]
        public string Description { get; set; }
    }
}