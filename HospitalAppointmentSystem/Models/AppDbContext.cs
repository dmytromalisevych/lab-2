using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAppointmentSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Налаштування зв'язків між таблицями
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Можна додати початкові дані для тестування, якщо потрібно
            // SeedData(modelBuilder);
        }
        
        /*
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Додавання тестових лікарів
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, FirstName = "Іван", LastName = "Петренко", Specialization = "Терапевт", PhoneNumber = "+380991234567" },
                new Doctor { DoctorId = 2, FirstName = "Олена", LastName = "Коваленко", Specialization = "Кардіолог", PhoneNumber = "+380997654321" }
            );
            
            // Додавання тестових пацієнтів
            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, FirstName = "Михайло", LastName = "Шевченко", DateOfBirth = new DateTime(1985, 5, 15), PhoneNumber = "+380661234567" },
                new Patient { PatientId = 2, FirstName = "Тетяна", LastName = "Іваненко", DateOfBirth = new DateTime(1990, 10, 25), PhoneNumber = "+380667654321" }
            );
        }
        */
    }
}