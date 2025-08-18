namespace Doctor.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Number { get; }

        public PhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Phone number is required.", nameof(number));

            
            if (!System.Text.RegularExpressions.Regex.IsMatch(number, @"^[\d\+\-\s]+$"))
                throw new ArgumentException("Phone number format is invalid.", nameof(number));

            Number = number;
        }

        public override string ToString() => Number;

        public override bool Equals(object? obj)
        {
            if (obj is PhoneNumber other)
                return Number == other.Number;
            return false;
        }

        public override int GetHashCode() => Number.GetHashCode();
    }

}
