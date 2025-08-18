namespace Patient.Domain.Entities
{

    public class ChronicDisease(Guid patientId, string name, Severity severity)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; } =
            !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new ArgumentException("Disease name is required.", nameof(name));

        public string? Description { get; private set; }

        public Severity Severity { get; private set; } = severity;

        public DateTime DiagnosedOn { get; private set; } = DateTime.UtcNow;

        public Guid PatientId { get; private set; } = patientId;

        public Patient Patient { get; private set; } = null!;

        public void UpdateSeverity(Severity severity)
        {
            Severity = severity;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public override bool Equals(object? obj)
        {
            return obj is ChronicDisease other && Id == other.Id;
        }

        public static ChronicDisease Create(Guid patientId, string name, Severity severity, string? description = null)
        {
            var disease = new ChronicDisease(patientId, name, severity);

            if (!string.IsNullOrWhiteSpace(description))
                disease.UpdateDescription(description);

            return disease;
        }


        public override int GetHashCode() => Id.GetHashCode();
    }
}