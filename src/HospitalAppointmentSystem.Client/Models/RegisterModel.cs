using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Client.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [MinLength(6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}