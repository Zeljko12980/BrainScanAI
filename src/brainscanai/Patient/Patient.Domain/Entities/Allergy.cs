namespace Patient.Domain.Entities
{
    public class Allergy(Guid patientId, string name, Severity severity)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid PatientId { get; private set; } = patientId;

        public Patient Patient { get; private set; } = null!;

        public string Name { get; private set; } =
            !string.IsNullOrWhiteSpace(name) ? name
            : throw new ArgumentException("Allergy name is required.", nameof(name));

        public Severity Severity { get; private set; } = severity;

        public string? Notes { get; private set; }

        public DateTime ReportedAt { get; private set; } = DateTime.UtcNow;

        
        public static Allergy Create(Guid patientId, string name, Severity severity, string? notes = null)
        {
            var allergy = new Allergy(patientId, name, severity);
            allergy.Notes = notes;
            return allergy;
        }

        public void UpdateSeverity(Severity newSeverity)
        {
            Severity = newSeverity;
        }

        public void UpdateNotes(string? newNotes)
        {
            Notes = newNotes;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Allergy other) return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }


}
