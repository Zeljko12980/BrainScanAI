namespace BuildingBlocks.Messaging.Events
{
    public record TumorAnalysisCompletedEvent : IntegrationEvent
    {
        public Guid PatientId { get; init; }
        public string TumorType { get; init; } = null!;
        public double Confidence { get; init; }
        public DateTime AnalysisDate { get; init; }
        public string? ImageBase64 { get; init; } // nullable

        public TumorAnalysisCompletedEvent(
            Guid patientId,
            string tumorType,
            double confidence,
            DateTime analysisDate,
            string? imageBase64 = null) // default null
        {
            PatientId = patientId;
            TumorType = tumorType;
            Confidence = confidence;
            AnalysisDate = analysisDate;
            ImageBase64 = imageBase64;
        }
    }
}
