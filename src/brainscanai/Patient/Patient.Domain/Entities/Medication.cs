namespace Patient.Domain.Entities
{
    public class Medication
    {
        public Medication(Guid patientId, string name, string dosage)
        {
            Id = Guid.NewGuid();

            PatientId = patientId;

            Name = !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new ArgumentException("Medication name is required.", nameof(name));

            Dosage = !string.IsNullOrWhiteSpace(dosage)
                ? dosage
                : throw new ArgumentException("Dosage is required.", nameof(dosage));
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Dosage { get; private set; }

        public Guid PatientId { get; private set; }

        public Patient Patient { get; private set; } = null!;

        public void UpdateDosage(string newDosage)
        {
            if (string.IsNullOrWhiteSpace(newDosage))
                throw new ArgumentException("New dosage is required.", nameof(newDosage));

            Dosage = newDosage;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Medication other) return false;
            return Id == other.Id;
        }

        public static Medication Create(Guid patientId, string name, string dosage)
        {
            return new Medication(patientId, name, dosage);
        }


        public override int GetHashCode() => Id.GetHashCode();
    }
}
