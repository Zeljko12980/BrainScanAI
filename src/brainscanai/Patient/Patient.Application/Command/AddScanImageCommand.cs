using Patient.Application.Common.Interface;

namespace Patient.Application.Command
{
    public class AddScanImageCommand : ICommand<bool>
    {
        public Guid PatientId { get; set; }
        public string ImageType { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime TakenAt { get; set; }
    }

    public class AddScanImageCommandHandler
        (IPatientService patientService)
        : ICommandHandler<AddScanImageCommand, bool>
    {
        public async Task<bool> Handle(AddScanImageCommand command, CancellationToken cancellationToken)
        {
            var result = await patientService.AddScanImageAsync(command.PatientId, new Dtos.CreateScanImageDto()
            {
                ImageType = command.ImageType,
                Url = command.Url,
                TakenAt = command.TakenAt
            });

            return result;
        }
    }
}
