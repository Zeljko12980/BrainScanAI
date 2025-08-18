using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Doctor.Application.Command
{
    public class AnalyzeTumorImageCommand : ICommand<(string TumorType, double Confidence)>
    {
        public Guid PatientId { get; }
        public string ImageBytes { get; }

        public AnalyzeTumorImageCommand(string imageBytes,Guid patientId)
        {
            ImageBytes = imageBytes ?? throw new ArgumentNullException(nameof(imageBytes));
            PatientId = patientId;
        }
    }

    public class AnalyzeTumorImageCommandHandler
        (IBrainTumorAnalyzer brainTumorAnalyzer,IPublishEndpoint publishEndpoint)
        : ICommandHandler<AnalyzeTumorImageCommand, (string TumorType, double Confidence)>
    {
        public async Task<(string TumorType, double Confidence)> Handle(AnalyzeTumorImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ImageBytes))
                {
                    throw new ArgumentException("Image bytes cannot be null or empty", nameof(request.ImageBytes));
                }

                var (tumorType, confidence) = await brainTumorAnalyzer.AnalyzeAsync(request.ImageBytes);

                var tumorAnalysisCompletedEvent = new TumorAnalysisCompletedEvent(
                    patientId: request.PatientId,
                    tumorType: tumorType,
                    confidence: confidence,
                    analysisDate: DateTime.UtcNow,
                    imageBase64:request.ImageBytes
                );

                await publishEndpoint.Publish(tumorAnalysisCompletedEvent, cancellationToken);

                return (tumorType, confidence);
            }
            catch (Exception ex)
            {
                // Ovdje možeš logirati grešku ili raditi nešto drugo s njom
                Console.WriteLine($"Greška u AnalyzeTumorImageCommandHandler.Handle: {ex}");

                // Po želji možeš baciti izuzetak dalje ili return-ati default ili neki signalni rezultat
                throw; // ili return ("Unknown", 0.0);
            }
        }
    }
}
