namespace Patient.Domain.ValueObjects
{
    public class BloodType
    {
        private static readonly HashSet<string> ValidTypes = new()
        {
            "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };

        public string Type { get; }

        public BloodType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Blood type is required.", nameof(type));

            if (!ValidTypes.Contains(type.ToUpperInvariant()))
                throw new ArgumentException($"Invalid blood type: {type}.", nameof(type));

            Type = type.ToUpperInvariant();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BloodType other) return false;
            return Type == other.Type;
        }

        public override int GetHashCode() => HashCode.Combine(Type);
    }
}
