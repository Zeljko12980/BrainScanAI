namespace Patient.Domain.ValueObjects
{
    public class Fullname
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;

    
        private Fullname() { }

        public Fullname(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
        }

        public override bool Equals(object? obj) =>
            obj is Fullname other &&
            FirstName == other.FirstName &&
            LastName == other.LastName;

        public override int GetHashCode() =>
            HashCode.Combine(FirstName, LastName);
    }
}
