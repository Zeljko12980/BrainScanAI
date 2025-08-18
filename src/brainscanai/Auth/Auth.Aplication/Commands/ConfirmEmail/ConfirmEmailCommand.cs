namespace Auth.Aplication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : ICommand<bool>
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public ConfirmEmailCommand(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Confirmation token is required.");
        }
    }

    public class ConfirmEmailCommandHandler
        (IIdentityService identityService)
        : ICommandHandler<ConfirmEmailCommand, bool>
    {


        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {

            var result = await identityService.ConfirmEmailAsync(request.UserId, request.Token);
            return result;
        }
    }
}
