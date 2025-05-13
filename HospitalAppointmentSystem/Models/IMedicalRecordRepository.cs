namespace HospitalAppointmentSystem.Models
{
    public interface IMedicalRecordRepository
    {
        IQueryable<MedicalRecord> MedicalRecords { get; }
        void CreateMedicalRecord(MedicalRecord medicalRecord);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(MedicalRecord medicalRecord);
        MedicalRecord GetMedicalRecord(int id);
    }
}