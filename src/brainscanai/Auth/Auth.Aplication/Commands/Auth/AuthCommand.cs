namespace Auth.Aplication.Commands.Auth
{
    public class AuthCommand : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

    public class AuthCommandHandler
        (IIdentityService identityService)
        : ICommandHandler<AuthCommand>
    {


        public async Task<Unit> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            await identityService.LoginAsync(request.Email, request.Password);
            return Unit.Value;
        }
    }
}
