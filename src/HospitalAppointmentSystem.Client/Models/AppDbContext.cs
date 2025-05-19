using Microsoft.EntityFrameworkCore;
using HospitalAppointmentSystem.Client.Models;

namespace HospitalAppointmentSystem.Client.Models
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
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<AppointmentDraft> AppointmentDrafts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфігурація для Doctor
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Specialization).IsRequired();
            });

            // Конфігурація для Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });

            // Конфігурація для Appointment
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DateTime).IsRequired();
                
                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId);
                
                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId);
            });

            // Конфігурація для MedicalRecord
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                
                entity.HasOne(m => m.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(m => m.PatientId);
            });

            // Конфігурація для AppointmentDraft
            modelBuilder.Entity<AppointmentDraft>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DateTime).IsRequired();
            });
        }
    }
}