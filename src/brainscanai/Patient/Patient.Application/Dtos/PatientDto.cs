namespace Patient.Application.Dtos
{
    public class PatientDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Jmbg { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;

        public string BloodType { get; set; } = null!;

        public string EmergencyContactName { get; set; } = null!;
        public string EmergencyContactPhone { get; set; } = null!;
        public string EmergencyContactRelation { get; set; } = null!;

        public List<string> Allergies { get; set; } = new();
        public List<string> ChronicDiseases { get; set; } = new();
        public List<string> Medications { get; set; } = new();
        public List<string> ScanImages { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public MedicalHistoryDto MedicalHistory { get; set; }

    }
}
