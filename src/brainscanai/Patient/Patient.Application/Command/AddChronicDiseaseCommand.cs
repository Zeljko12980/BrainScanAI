using Patient.Application.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Command
{
    public class AddChronicDiseaseCommand:ICommand<bool>
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; } = null!;
        public Severity Severity { get; set; }
        public string? Notes { get; set; }
    }

    public class AddChronicDiseaseCommandHandler
        (IPatientService patientService)
        : ICommandHandler<AddChronicDiseaseCommand, bool>
    {
        public async Task<bool> Handle(AddChronicDiseaseCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.AddChronicDiseaseAsync(command.PatientId, new Dtos.CreateChronicDiseaseDto()
            {
                Description=command.Notes,
                Name=command.Name,
                Severity=command.Severity,
            });
            return result;
        }
    }
}
