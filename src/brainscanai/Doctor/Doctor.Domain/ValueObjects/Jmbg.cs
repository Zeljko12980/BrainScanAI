namespace Doctor.Domain.ValueObjects
{
    public class Jmbg
    {
        public string Value { get; private set; }


        private Jmbg() { }

        public Jmbg(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsValidJmbg(value))
                throw new ArgumentException("Invalid JMBG.", nameof(value));

            Value = value;
        }

        private static bool IsValidJmbg(string jmbg)
        {
            if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
                return false;

            int day = int.Parse(jmbg.Substring(0, 2));
            int month = int.Parse(jmbg.Substring(2, 2));
            int yearPart = int.Parse(jmbg.Substring(4, 3));

            int year = yearPart >= 900 ? 1000 + yearPart : 2000 + yearPart;

            if (!IsValidDate(year, month, day))
                return false;

            int[] weights = { 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 6; i++)
            {
                sum += weights[i] * ((jmbg[i] - '0') + (jmbg[i + 6] - '0'));
            }

            int mod = sum % 11;
            int controlDigit = mod == 0 ? 0 : 11 - mod;
            if (controlDigit == 10)
                controlDigit = 0;

            return controlDigit == (jmbg[12] - '0');
        }

        private static bool IsValidDate(int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Jmbg other) return false;
            return Value == other.Value;
        }

        public override int GetHashCode() => HashCode.Combine(Value);
    }
}
