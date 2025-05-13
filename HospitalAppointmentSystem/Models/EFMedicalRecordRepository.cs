namespace HospitalAppointmentSystem.Models
{
    public class EFMedicalRecordRepository : IMedicalRecordRepository
    {
        private AppDbContext context;

        public EFMedicalRecordRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<MedicalRecord> MedicalRecords => context.MedicalRecords;

        public void CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            context.Add(medicalRecord);
            context.SaveChanges();
        }

        public void UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            context.Update(medicalRecord);
            context.SaveChanges();
        }

        public void DeleteMedicalRecord(MedicalRecord medicalRecord)
        {
            context.Remove(medicalRecord);
            context.SaveChanges();
        }

        public MedicalRecord GetMedicalRecord(int id)
        {
            return context.MedicalRecords.Find(id);
        }
    }
}