using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterModel registerModel);
    }
}