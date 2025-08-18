namespace Auth.Aplication.Common.Interface
{
    public interface ITokenGenerator
    {
        public string GenerateJWTToken((string userId, string userName, IList<string> roles, string profileImage) userDetails);
    }
}
