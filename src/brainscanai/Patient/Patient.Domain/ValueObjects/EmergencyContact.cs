namespace Patient.Domain.ValueObjects
{
    public class EmergencyContact
    {
        public string Name { get; private set; }
        public string Phone { get; private set; }


        public EmergencyContact(string name, string phone)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Name is required.", nameof(name))
                : name;

            Phone = string.IsNullOrWhiteSpace(phone)
                ? throw new ArgumentException("Phone is required.", nameof(phone))
                : phone;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not EmergencyContact other) return false;
            return Name == other.Name && Phone == other.Phone;
        }

        public override int GetHashCode()
            => HashCode.Combine(Name, Phone);
    }
}
