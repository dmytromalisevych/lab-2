using System.Collections.Generic;

namespace HospitalAppointmentSystem.Models.ViewModels
{
    public class PatientsListViewModel
    {
        public IEnumerable<Patient> Patients { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}