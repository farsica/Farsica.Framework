namespace Farsica.Framework.Captcha
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal static class SecurityExtensions
    {
        internal const string Captcha = "Captcha";

        internal static string Encrypt(string stringToHash)
        {
            const string Salt = @"F@rA80o|\/|";
            var saltByte = Encoding.UTF8.GetBytes(Salt);
            var passwordTextByte = Encoding.UTF8.GetBytes(stringToHash);
            var passwordTextWithSaltByte = new byte[passwordTextByte.Length + saltByte.Length];
            for (var i = 0; i < passwordTextByte.Length; i++)
            {
                passwordTextWithSaltByte[i] = passwordTextByte[i];
            }

            for (var i = 0; i < Salt.Length; i++)
            {
                passwordTextWithSaltByte[passwordTextByte.Length + i] = saltByte[i];
            }

            HashAlgorithm hash = SHA256.Create();
            var hashByte = hash.ComputeHash(passwordTextWithSaltByte);
            var hashWithSaltByte = new byte[hashByte.Length + saltByte.Length];
            for (var i = 0; i < hashByte.Length; i++)
            {
                hashWithSaltByte[i] = hashByte[i];
            }

            for (var i = 0; i < saltByte.Length; i++)
            {
                hashWithSaltByte[hashByte.Length + i] = saltByte[i];
            }

            return Convert.ToBase64String(hashWithSaltByte);
        }
    }
}
