using System.Security.Cryptography;
using System.Text;

namespace PaymentGateway.Helpers
{
    public static class EncryptionHelper
    {
        public static string GenerateKey() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        public static string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

            return Convert.ToBase64String(iv.Concat(encrypted).ToArray());
        }

        public static string Decrypt(string cipherText, string key)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = fullCipher.Take(16).ToArray();

            using var decryptor = aes.CreateDecryptor();
            var decrypted = decryptor.TransformFinalBlock(fullCipher.Skip(16).ToArray(), 0, fullCipher.Length - 16);

            return Encoding.UTF8.GetString(decrypted);
        }
    }

}
