using System.Security.Cryptography;
using System.Text;

namespace LibraryServices.Infrastructure.Sercurity
{
    public static class EncryptHelper
    {
        private static string GenerateMD5(byte[] bytes)
        {
            var buffer = MD5.Create().ComputeHash(bytes);
            var strBuilder = new StringBuilder();
            foreach (var item in buffer)
            {
                strBuilder.Append(item.ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string MD5Encrypt32(this string plainText, string salt)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentException($"{nameof(plainText)} is null or empty。", nameof(plainText));
            }

            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException($"{nameof(salt)} is null or empty。", nameof(salt));
            }

            var contentBytes = Encoding.UTF8.GetBytes(plainText + salt);
            return GenerateMD5(contentBytes);
        }
    }
}
