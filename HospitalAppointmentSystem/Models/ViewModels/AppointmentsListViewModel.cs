namespace HospitalAppointmentSystem.Models.ViewModels
{
    public class AppointmentsListViewModel
    {
        public IEnumerable<Appointment> Appointments { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentStatus { get; set; }
        public string SearchString { get; set; }
        public int TotalAppointments { get; set; }
        public int ScheduledAppointments { get; set; }
        public int CompletedAppointments { get; set; }
    }
}