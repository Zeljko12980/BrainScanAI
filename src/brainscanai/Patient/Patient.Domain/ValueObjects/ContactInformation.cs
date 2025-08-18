namespace Patient.Domain.ValueObjects
{
    public class ContactInformation
    {
        public string PhoneNumber { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Address { get; private set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj is not ContactInformation other) return false;
            return PhoneNumber == other.PhoneNumber &&
                   Email == other.Email &&
                   Address == other.Address;
        }
        public ContactInformation(string phoneNumber, string email, string address)
        {
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber)
                ? throw new ArgumentException("Phone number is required.", nameof(phoneNumber))
                : phoneNumber;

            Email = string.IsNullOrWhiteSpace(email)
                ? throw new ArgumentException("Email is required.", nameof(email))
                : email;

            Address = string.IsNullOrWhiteSpace(address)
                ? throw new ArgumentException("Address is required.", nameof(address))
                : address;
        }
        public override int GetHashCode()
            => HashCode.Combine(PhoneNumber, Email, Address);
    }

}
