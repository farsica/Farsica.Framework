namespace Farsica.Framework.Security
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class Cryptography
    {
        private const int Iterations = 1024;

        public static string Base64EncryptAES(string plainText, string password, string salt)
        {
            return Convert.ToBase64String(EncryptAES(Encoding.UTF8.GetBytes(plainText), password, salt));
        }

        public static byte[] EncryptAES(byte[] plainText, string password, string salt)
        {
            using var aesProvider = new AesCryptoServiceProvider { KeySize = 256, Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC };
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var derivedBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
            var derivedKey = derivedBytes.GetBytes(32); // 256 bits
            var derivedInitVector = derivedKey.Take(16).ToArray(); // 128 bits

            var encryptor = aesProvider.CreateEncryptor(derivedKey, derivedInitVector);
            using var memStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memStream.ToArray();

            memStream.Close();
            cryptoStream.Close();

            return cipherTextBytes;
        }

        public static string Base64DecryptAES(string base64EncryptedText, string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(base64EncryptedText))
            {
                return base64EncryptedText;
            }

            return Encoding.UTF8.GetString(DecryptAES(Convert.FromBase64String(base64EncryptedText), password, salt));
        }

        public static byte[] DecryptAES(byte[] encryptedText, string password, string salt)
        {
            if (encryptedText == null)
            {
                return encryptedText;
            }

            using var aesProvider = new AesCryptoServiceProvider { KeySize = 256, Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC };
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var derivedBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
            var derivedKey = derivedBytes.GetBytes(32); // 256 bits
            var derivedInitVector = derivedKey.Take(16).ToArray(); // 128 bits

            var decryptor = aesProvider.CreateDecryptor(derivedKey, derivedInitVector);
            using var memStream = new MemoryStream(encryptedText);
            using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[encryptedText.Length];
            var byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memStream.Close();
            cryptoStream.Close();

            return plainTextBytes;
        }
    }
}
