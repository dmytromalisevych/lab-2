using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Server.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}