using Microsoft.AspNetCore.Mvc;
using HospitalAppointmentSystem.Server.Models;

namespace HospitalAppointmentSystem.Server
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}