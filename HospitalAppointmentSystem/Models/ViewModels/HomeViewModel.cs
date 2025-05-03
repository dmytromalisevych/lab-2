namespace HospitalAppointmentSystem.Models.ViewModels
{
    public class HomeViewModel
    {
        public int DoctorsCount { get; set; }
        public int PatientsCount { get; set; }
        public int TotalAppointments { get; set; }
        public int ScheduledAppointments { get; set; }
        public List<Appointment> UpcomingAppointments { get; set; }
    }
}