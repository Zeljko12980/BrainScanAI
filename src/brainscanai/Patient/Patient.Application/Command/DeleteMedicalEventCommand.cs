


using Patient.Application.Common.Interface;

namespace Patient.Application.Command
{
    public class DeleteMedicalEventCommand:ICommand<bool>
    {
        public Guid PatientId { get; set; }
        public Guid EventId { get; set; }
    }

    public class DeleteMedicalEventCommandHandler
        (IPatientService patientService)
        : ICommandHandler<DeleteMedicalEventCommand, bool>
    {
        public async Task<bool> Handle(DeleteMedicalEventCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.DeleteMedicalEventAsync(command.PatientId, command.EventId);

            return result;
        }
    }
}
