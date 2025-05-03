using System.Collections.Generic;

namespace HospitalAppointmentSystem.Models.ViewModels
{
    public class AppointmentsListViewModel
    {
        public IEnumerable<Appointment> Appointments { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int TotalAppointments { get; set; }
        public int DoctorsCount { get; set; }
        public int PatientsCount { get; set; }
    }
}