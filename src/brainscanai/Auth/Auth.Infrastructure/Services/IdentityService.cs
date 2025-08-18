using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Services
{
    public class IdentityService
           (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IEmailService emailService, ITokenGenerator tokenGenerator, IPdfService pdfService)
           : IIdentityService
    {
        public async ValueTask<(bool isSucceed, string userId)> RegisterUserAsync(
     Guid userId,
     string username,
     string password,
     string email,
     string firstName,
     string lastName,
     string profilePictureUrl,
     DateTime createdAt,
     DateTime? updatedAt,
     bool isDeleted,
     DateTime? deletedAt,
     string roleName
 )
        {
            var user = new ApplicationUser
            {
                Id = userId.ToString(),
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ProfilePictureUrl = profilePictureUrl,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                IsDeleted = isDeleted,
                DeletedAt = deletedAt,
                TwoFactorEnabled=false
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors);

            await userManager.AddToRoleAsync(user, roleName);

            await emailService.SendEmailAsync(user.Email,
    "Welcome! Your account has been created",
    $"Hello {user.FirstName}, your account is ready. Your password is: {password}");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var url = $"https://192.168.0.13:6064/auth-service/api/auth/confirm-email?token={encodedToken}&userId={user.Id}";

            await emailService.SendEmailAsync(user.Email, "Confirm your email", $"Confirm via: {url}");

            return (true, user.Id);
        }




        public async ValueTask<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User not found");
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await userManager.SetTwoFactorEnabledAsync(user, true);
                return true;
            }
            return false;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) throw new Exception("Invalid credentials");

            var code = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);



            var fullName = $"{user.FirstName} {user.LastName}";
            var pdfBytes = await pdfService.GenerateTwoFactorPdf(fullName, code);

            await emailService.SendEmailWithAttachmentAsync(
                email,
                "2FA Code",
                "<p>Your 2FA code is attached as a PDF.</p>",
                pdfBytes,
               "2fa-code.pdf"
             );

            return true;
        }

        public async Task<(string id, string userName, string token)> VerifyTwoFactorCodeAsync(string email, string code)
        {
            var user = await userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");
            var isValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
            if (!isValid) throw new Exception("Invalid verification code");

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenGenerator.GenerateJWTToken((user.Id, user.UserName, roles,user.ProfilePictureUrl));

            return (user.Id, user.UserName, token);
        }

        public async ValueTask<bool> CreateRoleAsync(string roleName)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }
            return result.Succeeded;
        }
    }
}
