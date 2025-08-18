namespace Doctor.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public FullName Name { get; set; } = null!;
        public Email Email { get; set; } = null!;
        public PhoneNumber Phone { get; set; } = null!;
        public LicenseNumber License { get; set; } = null!;
        public Jmbg Jmbg { get; set; } = null!;

        public string Specialization { get; set; } = "Oncologist";


        public List<Guid> PatientIds { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
