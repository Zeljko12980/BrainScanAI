


using Patient.Application.Common.Interface;

namespace Patient.Application.Command
{
    public record AddAllergyCommand(Guid PatientId, string Name,  Severity Severity, string? Notes):ICommand<bool>;

    public class AddAllergyCommandHandler
    (IPatientService patientService)
    : ICommandHandler<AddAllergyCommand, bool>
    {
        public async Task<bool> Handle(AddAllergyCommand command, CancellationToken cancellationToken)
        {
         

           
      

            var dto = new Dtos.CreateAllergyDto()
            {
                Name = command.Name,
                Notes = command.Notes,
                Severity = command.Severity
            };

            var result = await patientService.AddAllergyAsync(command.PatientId, dto);
            return result;
        }
    }

}
