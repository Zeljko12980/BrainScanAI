using BuildingBlocks.Messaging.Events;
using MassTransit;
using Patient.Application.Common.Interface;
using Patient.Application.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patient.Application.Command
{
    public class CreatePatientCommand : ICommand<PatientDto>
    {
        public CreatePatientDto PatientDto { get; set; }

        public Guid DoctorId { get; set; }
        public CreatePatientCommand(CreatePatientDto patientDto, Guid doctorId)
        {
            PatientDto = patientDto;
            DoctorId = doctorId;
        }
    }

    public class CreatePatientCommandHandler
        (IPatientService patientService,
         IPublishEndpoint publishEndpoint)
        : ICommandHandler<CreatePatientCommand, PatientDto>
    {
        public async Task<PatientDto> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
        {
    
            var patient = await patientService.CreateAsync(command.PatientDto, command.DoctorId);

            
            var eventMessage = new UserCreatedEvent()
            {
                UserId = patient.Id,  
                Username = command.PatientDto.Username,
                FirstName = command.PatientDto.FirstName,
                LastName = command.PatientDto.LastName,
                Email = command.PatientDto.Email,
                CreatedAt = DateTime.UtcNow,
                ProfilePictureUrl = command.PatientDto.ProfilePictureUrl ?? string.Empty,
                RoleName="Patient"
                
            };

            await publishEndpoint.Publish(eventMessage, cancellationToken);

            return patient;
        }
    }
}
