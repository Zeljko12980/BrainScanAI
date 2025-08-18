using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Command
{
    public class CreateDoctorCommand:ICommand<DoctorDto>
    {
        public string UserName { get; set; } = string.Empty; // IdentityUser
        public string Email { get; set; } = string.Empty;    // IdentityUser
        public string PhoneNumber { get; set; } = string.Empty; // IdentityUser
        public string Password { get; set; } = string.Empty; // Za kreiranje naloga
        public string FirstName { get; set; } = string.Empty; // ApplicationUser
        public string LastName { get; set; } = string.Empty;  // ApplicationUser
        public string ProfilePictureUrl { get; set; } = string.Empty;

        // Polja iz Doctor entiteta
        public string LicenseNumber { get; set; } = string.Empty;
        public string Jmbg { get; set; } = string.Empty;
        public string Specialization { get; set; } = "Oncologist";
        public List<Guid> PatientIds { get; set; } = new();
    }

    public class CreateDoctorCommandHandler
        (IDoctorService doctorService, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CreateDoctorCommand, DoctorDto>
    {
        public async Task<DoctorDto> Handle(CreateDoctorCommand command, CancellationToken cancellationToken)
        {
            var result = await doctorService.CreateAsync(new CreateDoctorDto()
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Jmbg = command.Jmbg,
                Specialization = command.Specialization,
                License = command.LicenseNumber,
                PatientIds = command.PatientIds,
                PhoneNumber=command.PhoneNumber,
                
            });

            await publishEndpoint.Publish(new UserCreatedEvent()
            {
                CreatedAt = DateTime.Now,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                ProfilePictureUrl = command.ProfilePictureUrl,
                RoleName="Doctor",
                UserId=result.Id,
         
              
                Username=command.UserName,
            });

            return result;
        }
    }
}
