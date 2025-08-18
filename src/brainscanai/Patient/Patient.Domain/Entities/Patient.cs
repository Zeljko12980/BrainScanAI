namespace Patient.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Fullname FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Jmbg Jmbg { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public ContactInformation Contact { get; set; } = null!;
        public BloodType BloodType { get; set; } = null!;
        public EmergencyContact EmergencyContact { get; set; } = null!;
        public MedicalHistory MedicalHistory { get; private set; } = new MedicalHistory();


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid DoctorId { get; set; }

        public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
        public ICollection<ChronicDisease> ChronicDiseases { get; set; } = new List<ChronicDisease>();
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
        public ICollection<ScanImage> ScanImages { get; set; } = new List<ScanImage>();

        public void AddAllergy(string name, Severity severity, string? notes = null)
        {
            var allergy = Allergy.Create(Id, name, severity, notes);
            Allergies.Add(allergy);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddChronicDisease(string name, Severity severity, string? description = null)
        {
            var disease = new ChronicDisease(Id, name, severity);
            if (!string.IsNullOrWhiteSpace(description))
                disease.UpdateDescription(description);

            ChronicDiseases.Add(disease);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddScanImage(string imageType, string url, DateTime takenAt)
        {
            var scanImage = new ScanImage(Id, imageType, url, takenAt);
            ScanImages.Add(scanImage);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddMedication(string name, string dosage)
        {
            var medication = new Medication(Id, name, dosage);
            Medications.Add(medication);
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
