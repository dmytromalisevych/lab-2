using System.Collections.Generic;

namespace HospitalAppointmentSystem.Models.ViewModels
{
    public class DoctorsListViewModel
    {
        public IEnumerable<Doctor> Doctors { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string SearchString { get; set; }
    }
}