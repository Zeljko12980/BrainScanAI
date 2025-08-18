namespace Patient.Domain.ValueObjects
{
    public class MedicalHistory
    {
        public List<MedicalEvent> Events { get; private set; } = new();


       
        public MedicalHistory()
        {
            Events = new List<MedicalEvent>();
        }


        public MedicalHistory(List<MedicalEvent> events)
        {
            Events = events ?? new List<MedicalEvent>();
        }

        public void AddEvent(MedicalEvent medicalEvent)
        {
            if (medicalEvent == null)
                throw new ArgumentNullException(nameof(medicalEvent));

            Events.Add(medicalEvent);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not MedicalHistory other)
                return false;

            return Events.SequenceEqual(other.Events);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var medicalEvent in Events)
            {
                hash = hash * 31 + medicalEvent.GetHashCode();
            }
            return hash;
        }

       
        public class MedicalEvent
        {
            public Guid Id { get; private set; }
            public DateTime Date { get; set; }
            public string EventType { get; set; }
            public string Description { get; set; }

            private MedicalEvent() { }

            public MedicalEvent(DateTime date, string eventType, string description)
            {
                Id =  Guid.NewGuid();

                Date = date;
                EventType = string.IsNullOrWhiteSpace(eventType)
                    ? throw new ArgumentException("Event type is required.", nameof(eventType))
                    : eventType;

                Description = string.IsNullOrWhiteSpace(description)
                    ? throw new ArgumentException("Description is required.", nameof(description))
                    : description;
            }

            public override bool Equals(object? obj) => obj is MedicalEvent other &&
                Id == other.Id &&
                Date == other.Date &&
                EventType == other.EventType &&
                Description == other.Description;

            public override int GetHashCode() => HashCode.Combine(Date, EventType, Description);
        }

    }
}