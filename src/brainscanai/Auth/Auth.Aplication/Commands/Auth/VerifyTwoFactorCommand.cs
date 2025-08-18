namespace Auth.Aplication.Commands.Auth
{
    public record VerifyTwoFactorCommand(string Email, string Code) : ICommand<AuthResponseDto>;

    public class VerifyTwoFactorCommandValidator : AbstractValidator<VerifyTwoFactorCommand>
    {
        public VerifyTwoFactorCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Verification code must be 6 characters long.");
        }
    }
    public class VerifyTwoFactorCommandHandler
        (IIdentityService identityService)
        : ICommandHandler<VerifyTwoFactorCommand, AuthResponseDto>
    {

        public async Task<AuthResponseDto> Handle(VerifyTwoFactorCommand request, CancellationToken cancellationToken)
        {
            var (userId, userName, token) = await identityService.VerifyTwoFactorCodeAsync(request.Email, request.Code);

            return new AuthResponseDto
            {
                UserId = userId,
                Name = userName,
                Token = token
            };
        }
    }
}
