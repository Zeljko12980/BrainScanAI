namespace Doctor.Application.Common.Interface
{
    public interface IBrainTumorAnalyzer
    {
        Task<(string TumorType, double Confidence)> AnalyzeAsync(string imageBytes);
    }
}
