using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Client.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Email є обов'язковим")]
    [EmailAddress(ErrorMessage = "Неправильний формат email адреси")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Пароль є обов'язковим")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен бути від 6 до 100 символів")]
    public string? Password { get; set; }
}