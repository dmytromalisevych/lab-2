using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Server.Models;

namespace HospitalAppointmentSystem.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Додаємо початкові дані
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Email = "doctor@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doctor123!"),
                    Role = "Doctor"
                },
                new User 
                { 
                    Id = 2, 
                    Email = "patient@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Patient123!"),
                    Role = "Patient"
                }
            );
        }
    }
}