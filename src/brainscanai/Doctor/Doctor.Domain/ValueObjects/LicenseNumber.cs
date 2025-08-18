namespace Doctor.Domain.ValueObjects
{
    public class LicenseNumber
    {
        public string Number { get; }

        public LicenseNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("License number is required.", nameof(number));

         

            Number = number;
        }

        public override string ToString() => Number;

        public override bool Equals(object? obj)
        {
            if (obj is LicenseNumber other)
                return Number == other.Number;
            return false;
        }

        public override int GetHashCode() => Number.GetHashCode();
    }

}
