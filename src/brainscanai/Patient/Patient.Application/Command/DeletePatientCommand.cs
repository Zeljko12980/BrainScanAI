

using Patient.Application.Common.Interface;

namespace Patient.Application.Command
{
    public class DeletePatientCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeletePatientCommandHandler 
        (IPatientService patientService)
        : ICommandHandler<DeletePatientCommand, bool>
    {
        public async Task<bool> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
        {
            var result= await patientService.DeleteAsync(command.Id);

            return result;
        }
    }
}
