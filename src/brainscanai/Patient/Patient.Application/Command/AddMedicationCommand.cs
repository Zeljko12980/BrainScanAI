using Patient.Application.Common.Interface;

namespace Patient.Application.Command
{
    public class AddMedicationCommand : ICommand<bool>
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string Dosage { get; set; } = null!;
    }

    public class AddMedicationCommandHandler
        (IPatientService patientService)
        : ICommandHandler<AddMedicationCommand, bool>
    {
        public async Task<bool> Handle(AddMedicationCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.AddMedicationAsync(command.PatientId, new Dtos.CreateMedicationDto()
            {
                Name = command.Name,
                Dosage = command.Dosage
            });

            return result;
        }
    }
}
