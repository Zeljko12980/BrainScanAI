using Patient.Application.Common.Interface;
using Patient.Application.Dtos;

namespace Patient.Application.Command
{
    public class AddMedicalEventCommand:ICommand<bool>
    {
        public Guid PatientId { get; }
        public MedicalEventDto NewEvent { get; }

        public AddMedicalEventCommand(Guid patientId, MedicalEventDto newEvent)
        {
            PatientId = patientId;
            NewEvent = newEvent;
        }
    }

    public class AddMedicalEventCommandHandler 
        (IPatientService patientService)
        : ICommandHandler<AddMedicalEventCommand, bool>
    {
        public async Task<bool> Handle(AddMedicalEventCommand command, CancellationToken cancellationToken)
        {
          return  await patientService.AddMedicalEventAsync(command.PatientId, command.NewEvent);
        }
    }
}
