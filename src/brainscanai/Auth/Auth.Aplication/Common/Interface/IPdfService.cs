namespace Auth.Aplication.Common.Interface
{
    public interface IPdfService
    {
        Task<byte[]> GenerateTwoFactorPdf(string fullName, string code);
    }
}
