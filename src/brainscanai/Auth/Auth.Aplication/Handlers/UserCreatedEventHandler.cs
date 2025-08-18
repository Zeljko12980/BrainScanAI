using Auth.Application.Common.Interface;
using Auth.Application.Services;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Handlers
{
    public class UserCreatedEventHandler
        (IIdentityService identityService,
        IPasswordGenerator passwordGenerator
        )
        : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;
            var generatedPassword = passwordGenerator.Generate(12);

            // Osiguraj da su datumi UTC
            var createdAtUtc = DateTime.SpecifyKind(message.CreatedAt, DateTimeKind.Utc);
            DateTime? updatedAtUtc = message.UpdatedAt.HasValue
                ? DateTime.SpecifyKind(message.UpdatedAt.Value, DateTimeKind.Utc)
                : null;
            DateTime? deletedAtUtc = message.DeletedAt.HasValue
                ? DateTime.SpecifyKind(message.DeletedAt.Value, DateTimeKind.Utc)
                : null;

            await identityService.RegisterUserAsync(
                message.UserId,
                message.Username,
                generatedPassword,
                message.Email,
                message.FirstName,
                message.LastName,
                message.ProfilePictureUrl,
                createdAtUtc,
                updatedAtUtc,
                message.IsDeleted,
                deletedAtUtc,
                message.RoleName
            );
        }

    }
}
