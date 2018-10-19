using System;
using System.Security.Cryptography;
using System.Text;

namespace AlarmClock.Misc
{
    public static class Encrypter
    {
        private const int SaltSize = 16;

        public static (string hash, string salt) Encode(string password)
        {
            var saltBytes = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(saltBytes);

            var salt = Hash(saltBytes);

            return (Hash(salt + password), salt);
        }

        private static string Hash(byte[] bytes)
        {
            var hash = new SHA256Managed().ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string Hash(string text) => Hash(Encoding.Default.GetBytes(text));
    }
}
