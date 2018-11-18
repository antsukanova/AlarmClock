using System;
using System.Security.Cryptography;
using System.Text;

namespace AlarmClock.Tools
{
    public static class Encrypter
    {
        private const int SaltSize = 16;

        public static HashObject Encode(string password)
        {
            var saltBytes = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(saltBytes);

            var salt = Hash(saltBytes);

            return new HashObject(Hash(salt + password), salt);
        }

        private static string Hash(byte[] bytes)
        {
            var hash = new SHA256Managed().ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string Hash(string text) => Hash(Encoding.Default.GetBytes(text));
    }

    public class HashObject
    {
        public string Hash { get; }
        public string Salt { get; }

        public HashObject(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}
