using Microsoft.AspNetCore.Mvc;
using HospitalAppointmentSystem.Server.Models;

namespace HospitalAppointmentSystem.Server
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Patient" // За замовчуванням реєструємо як пацієнта
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return BadRequest("Invalid email or password");
            }

            // Тут можна додати генерацію JWT токена

            return Ok("Login successful");
        }
    }
}