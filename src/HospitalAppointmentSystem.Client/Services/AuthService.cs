using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalAppointmentSystem.Client.Models;
using BCrypt.Net;

namespace HospitalAppointmentSystem.Client.Services
{
    public class AuthService
    {
        private readonly IIndexedDbService _dbService;
        private const string DOCTORS_STORE = "doctors";
        private const string PATIENTS_STORE = "patients";
        private const string SESSION_STORE = "currentSession";

        public AuthService(IIndexedDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<RegisterResult> Register(RegisterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                // Перевірка чи email вже існує в обох таблицях
                var doctors = await _dbService.GetAllAsync<DoctorModel>(DOCTORS_STORE);
                var patients = await _dbService.GetAllAsync<PatientModel>(PATIENTS_STORE);

                if (doctors.Any(d => d.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)) || 
                    patients.Any(p => p.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    return new RegisterResult 
                    { 
                        Successful = false, 
                        Error = "Користувач з таким email вже існує" 
                    };
                }

                // Хешування пароля
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                if (model.Role == UserRole.Doctor)
                {
                    var doctor = new DoctorModel
                    {
                        Email = model.Email.Trim(),
                        PasswordHash = passwordHash,
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        Specialization = (model.Specialization ?? string.Empty).Trim(),
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _dbService.AddAsync(DOCTORS_STORE, doctor);
                }
                else
                {
                    var patient = new PatientModel
                    {
                        Email = model.Email.Trim(),
                        PasswordHash = passwordHash,
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        CreatedAt = DateTime.UtcNow
                    };
                    await _dbService.AddAsync(PATIENTS_STORE, patient);
                }

                return new RegisterResult { Successful = true };
            }
            catch (Exception ex)
            {
                return new RegisterResult 
                { 
                    Successful = false, 
                    Error = $"Помилка при реєстрації: {ex.Message}" 
                };
            }
        }

        public async Task<LoginResult> Login(LoginModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                // Спочатку шукаємо в лікарях
                var doctors = await _dbService.GetAllAsync<DoctorModel>(DOCTORS_STORE);
                var doctor = doctors.FirstOrDefault(d => 
                    d.Email.Equals(model.Email.Trim(), StringComparison.OrdinalIgnoreCase));
                
                if (doctor != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(model.Password, doctor.PasswordHash))
                    {
                        return new LoginResult 
                        { 
                            Successful = false, 
                            Error = "Невірний пароль" 
                        };
                    }

                    var session = new SessionModel 
                    { 
                        Id = doctor.Id, 
                        Role = UserRole.Doctor,
                        Email = doctor.Email,
                        FirstName = doctor.FirstName,
                        LastName = doctor.LastName
                    };

                    await _dbService.AddAsync(SESSION_STORE, session);
                    return new LoginResult { Successful = true, Role = UserRole.Doctor };
                }

                // Якщо не знайшли в лікарях, шукаємо в пацієнтах
                var patients = await _dbService.GetAllAsync<PatientModel>(PATIENTS_STORE);
                var patient = patients.FirstOrDefault(p => 
                    p.Email.Equals(model.Email.Trim(), StringComparison.OrdinalIgnoreCase));

                if (patient != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(model.Password, patient.PasswordHash))
                    {
                        return new LoginResult 
                        { 
                            Successful = false, 
                            Error = "Невірний пароль" 
                        };
                    }

                    var session = new SessionModel 
                    { 
                        Id = patient.Id, 
                        Role = UserRole.Patient,
                        Email = patient.Email,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName
                    };

                    await _dbService.AddAsync(SESSION_STORE, session);
                    return new LoginResult { Successful = true, Role = UserRole.Patient };
                }

                return new LoginResult 
                { 
                    Successful = false, 
                    Error = "Користувача не знайдено" 
                };
            }
            catch (Exception ex)
            {
                return new LoginResult 
                { 
                    Successful = false, 
                    Error = $"Помилка при вході: {ex.Message}" 
                };
            }
        }

        public async Task Logout()
        {
            try
            {
                await _dbService.DeleteAsync(SESSION_STORE, 1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Помилка при виході: {ex.Message}");
            }
        }
    }
}