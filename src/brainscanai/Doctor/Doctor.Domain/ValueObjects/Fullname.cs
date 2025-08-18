namespace Doctor.Domain.ValueObjects
{
    public class FullName
    {
        public string FirstName { get; }
        public string LastName { get; }

        public FullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString() => $"{FirstName} {LastName}";

     
        public override bool Equals(object? obj)
        {
            if (obj is FullName other)
                return FirstName == other.FirstName && LastName == other.LastName;
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName);
    }

}
