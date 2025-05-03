using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Поле Ім'я є обов'язковим")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Прізвище є обов'язковим")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Дата народження є обов'язковим")]
        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string FullName => $"{LastName} {FirstName}";

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}