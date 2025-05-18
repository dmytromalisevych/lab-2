using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
        Task LogoutAsync();
        Task<User> GetCurrentUserAsync();
    }
}