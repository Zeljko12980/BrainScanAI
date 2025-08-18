namespace Auth.Aplication.Common.Interface
{
    public interface IIdentityService
    {

        ValueTask<(bool isSucceed, string userId)> RegisterUserAsync(
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
        );



        ValueTask<bool> ConfirmEmailAsync(string userId, string token);



        Task<bool> LoginAsync(string email, string password);
        Task<(string id, string userName, string token)> VerifyTwoFactorCodeAsync(string email, string code);

        ValueTask<bool> CreateRoleAsync(string roleName);
    }
}
