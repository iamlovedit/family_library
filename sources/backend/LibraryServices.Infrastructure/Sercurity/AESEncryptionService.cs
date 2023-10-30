using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class AESEncryptionService : IAESEncryptionService
    {
        private readonly IConfiguration _configuration;

        public AESEncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Decrypt(string cipher, string? aesKey = null)
        {
            if (string.IsNullOrEmpty(cipher))
            {
                throw new ArgumentException($"“{nameof(cipher)}”不能为 null 或空。", nameof(cipher));
            }
            using var aes = CreateAes(aesKey);
            using var decryptor = aes.CreateDecryptor();
            var cipherTextArray = Convert.FromBase64String(cipher);
            var resultArray = decryptor.TransformFinalBlock(cipherTextArray, 0, cipherTextArray.Length);
            var result = Encoding.UTF8.GetString(resultArray);
            Array.Clear(resultArray);
            resultArray = null;
            return result;
        }

        public string Encrypt(string plain, string? aesKey = null)
        {
            if (string.IsNullOrEmpty(plain))
            {
                throw new ArgumentException($"“{nameof(plain)}”不能为 null 或空。", nameof(plain));
            }
            using var aes = CreateAes(aesKey);
            using var encryptor = aes.CreateEncryptor();
            var plainTextArray = Encoding.UTF8.GetBytes(plain);
            var resultArray = encryptor.TransformFinalBlock(plainTextArray, 0, plainTextArray.Length);
            var result = Convert.ToBase64String(resultArray);
            Array.Clear(resultArray);
            resultArray = null;
            return result;
        }

        private Aes CreateAes(string aesKey)
        {
            using var md5 = MD5.Create();
            var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            var key = aesKey ?? _configuration["AES_KEY"];
            aes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key!));
            return aes;
        }
    }
}
