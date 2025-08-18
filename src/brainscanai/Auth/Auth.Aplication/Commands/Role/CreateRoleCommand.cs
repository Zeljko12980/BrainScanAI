namespace Auth.Application.Commands.Role
{
    public class RoleCreateCommand : ICommand<int>
    {
        public string RoleName { get; set; }
    }

    public class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
    {
        public RoleCreateCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
        }
    }
    public class RoleCreateCommandHandler
        (IIdentityService identityService)
        : ICommandHandler<RoleCreateCommand, int>
    {
     
        public async Task<int> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.CreateRoleAsync(request.RoleName);
            return result ? 1 : 0;
        }
    }
}
