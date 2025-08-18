namespace Doctor.Domain.ValueObjects
{
    public class Email
    {
        public string Address { get; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address is required.", nameof(address));

            if (!IsValidEmail(address))
                throw new ArgumentException("Email address format is invalid.", nameof(address));

            Address = address;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Address;

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
                return Address == other.Address;
            return false;
        }

        public override int GetHashCode() => Address.GetHashCode();
    }

}
