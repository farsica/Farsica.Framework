﻿namespace Farsica.Framework.Security
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public static class Cryptography
    {
        private const int Iterations = 1024;

        public static async Task<string?> EncryptAesAsync(string plainText, string password, string salt)
        {
            return Convert.ToBase64String(await EncryptAesAsync(Encoding.UTF8.GetBytes(plainText), password, salt));
        }

        public static async Task<byte[]> EncryptAesAsync([NotNull] byte[] plainText, string password, string salt)
        {
            var aesProvider = Aes.Create();
            aesProvider.KeySize = 256;
            aesProvider.Padding = PaddingMode.PKCS7;
            aesProvider.Mode = CipherMode.CBC;

            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var derivedBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var derivedKey = derivedBytes.GetBytes(32); // 256 bits
            var derivedInitVector = derivedKey.Take(16).ToArray(); // 128 bits

            var encryptor = aesProvider.CreateEncryptor(derivedKey, derivedInitVector);
            using var memStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(plainText);
            await cryptoStream.FlushFinalBlockAsync();
            var cipherTextBytes = memStream.ToArray();

            memStream.Close();
            cryptoStream.Close();

            return cipherTextBytes;
        }

        public static async Task<string?> DecryptAesAsync(string? base64EncryptedText, string password, string salt)
        {
            if (string.IsNullOrEmpty(base64EncryptedText))
            {
                return base64EncryptedText;
            }

            return await DecryptAesAsync(Convert.FromBase64String(base64EncryptedText), password, salt);
        }

        public static async Task<string?> DecryptAesAsync(byte[]? encryptedText, string password, string salt)
        {
            if (encryptedText is null)
            {
                return string.Empty;
            }

            var aesProvider = Aes.Create();
            aesProvider.KeySize = 256;
            aesProvider.Padding = PaddingMode.PKCS7;
            aesProvider.Mode = CipherMode.CBC;

            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var derivedBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var derivedKey = derivedBytes.GetBytes(32); // 256 bits
            var derivedInitVector = derivedKey.Take(16).ToArray(); // 128 bits

            var decryptor = aesProvider.CreateDecryptor(derivedKey, derivedInitVector);
            using var memStream = new MemoryStream(encryptedText);
            using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
            using var plainTextReader = new StreamReader(cryptoStream);
            var decryptedText = await plainTextReader.ReadToEndAsync();
            memStream.Close();
            cryptoStream.Close();

            return decryptedText;
        }
    }
}
