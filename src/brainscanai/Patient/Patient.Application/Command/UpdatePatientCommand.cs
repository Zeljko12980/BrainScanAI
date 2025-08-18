using Patient.Application.Common.Interface;
using Patient.Application.Dtos;


namespace Patient.Application.Command
{
    public class UpdatePatientCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
        public UpdatePatientDto UpdatedPatientDto { get; set; }

        public UpdatePatientCommand(Guid id, UpdatePatientDto updatedPatientDto)
        {
            Id = id;
            UpdatedPatientDto = updatedPatientDto;
        }
    }

    public class UpdatePatientCommandHandler 
        (IPatientService patientService)
        : ICommandHandler<UpdatePatientCommand, bool>
    {
        public async Task<bool> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.UpdateAsync(command.Id, command.UpdatedPatientDto);
            return result;
        }
    }
}
