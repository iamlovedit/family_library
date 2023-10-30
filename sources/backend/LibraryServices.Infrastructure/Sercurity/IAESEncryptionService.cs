namespace LibraryServices.Infrastructure.Sercurity
{
    public interface IAESEncryptionService
    {
        string Encrypt(string plain, string? aesKey = null);

        string Decrypt(string cipher, string? aesKey = null);
    }
}
