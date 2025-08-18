using BuildingBlocks.Messaging.Events;
using MassTransit;
using Patient.Application.Common.Interface;
using Patient.Application.Dtos;

namespace Patient.Application.Handlers
{
    public class TumorAnalysisCompletedEventHandler
        (IPatientService patientService)
        : IConsumer<TumorAnalysisCompletedEvent>
    {
        public async Task Consume(ConsumeContext<TumorAnalysisCompletedEvent> context)
        {
            var @event = context.Message;


          
            var newMedicalEvent = new MedicalEventDto
            {
                Date = @event.AnalysisDate,
                Type = "Tumor Analysis Completed",
                Description = $"Tumor type: {@event.TumorType}, Confidence: {@event.Confidence:P2}"
            };


            if (!string.IsNullOrEmpty(@event.ImageBase64))
            {
                await patientService.AddScanImageAsync(@event.PatientId, new CreateScanImageDto
                {
                    ImageType = ".jpg",
                    TakenAt = DateTime.UtcNow,
                    Url = @event.ImageBase64
                });
            }

            var success = await patientService.AddMedicalEventAsync(@event.PatientId, newMedicalEvent);

            if (!success)
            {
               
                throw new ApplicationException($"Failed to add medical event for patient {@event.PatientId}");
            }
        }
    }
}
