namespace Auth.Aplication.Commands.Register
{/*
    public class RegisterUserCommand : ICommand<(bool isSucceed, string userId)>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
    
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.ProfilePictureUrl)
                .MaximumLength(200).WithMessage("Profile picture URL must not exceed 200 characters.");
        }
    }
    
    public class RegisterUserCommandHandler
        (IIdentityService identityService)
        : ICommandHandler<RegisterUserCommand, (bool isSucceed, string userId)>
    {

        public async Task<(bool isSucceed, string userId)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
          /*  return await identityService.RegisterUserAsync(
                request.Username,
                request.Password,
                request.Email,
                request.FirstName,
                request.LastName,
                request.ProfilePictureUrl
            );
        }
    }
    */
}
