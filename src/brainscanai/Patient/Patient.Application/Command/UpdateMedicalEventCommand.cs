using Patient.Application.Common.Interface;
using Patient.Application.Dtos;


namespace Patient.Application.Command
{
    public class UpdateMedicalEventCommand:ICommand<bool>
    {
        public Guid PatientId { get; set; }
        public Guid EventId { get; set; }
        public MedicalEventDto UpdatedEvent { get; set; }

        public UpdateMedicalEventCommand(Guid patientId, Guid eventId, MedicalEventDto updatedEvent)
        {
            PatientId = patientId;
            EventId = eventId;
            UpdatedEvent = updatedEvent;
        }
    }

    public class UpdateMedicalEventCommandHandler 
        (IPatientService patientService)
        : ICommandHandler<UpdateMedicalEventCommand, bool>
    {
        public async Task<bool> Handle(UpdateMedicalEventCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.UpdateMedicalEventAsync(command.PatientId, command.EventId, command.UpdatedEvent);
            return result;
        }
    }
}
