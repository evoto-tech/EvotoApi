using System;
using System.Linq;
using System.Security.Cryptography;

namespace Common
{
    public sealed class Passwords
    {
        private const int SALT_SIZE_IN_BYTES = 128;
        private const int HASH_SIZE_IN_BYTES = 128;
        private const int PASSWORD_DIFFICULTY = 10000;

        private static readonly RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();

        public static string HashPassword(string password)
        {
            var saltBytes = GenerateRandomBytes(SALT_SIZE_IN_BYTES);
            var hashed = new Rfc2898DeriveBytes(password, saltBytes, PASSWORD_DIFFICULTY).GetBytes(HASH_SIZE_IN_BYTES);

            var hashBytes = new byte[SALT_SIZE_IN_BYTES + HASH_SIZE_IN_BYTES];
            Array.Copy(saltBytes, 0, hashBytes, 0, SALT_SIZE_IN_BYTES);
            Array.Copy(hashed, 0, hashBytes, SALT_SIZE_IN_BYTES, HASH_SIZE_IN_BYTES);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);
            if (hashBytes.Length != SALT_SIZE_IN_BYTES + HASH_SIZE_IN_BYTES) {
                throw new NotSupportedException($"Invalid Hash size: {HASH_SIZE_IN_BYTES + SALT_SIZE_IN_BYTES}");
            }


            var salt = hashBytes.Take(SALT_SIZE_IN_BYTES).ToArray();
            var hashedPasswordBytes = new Rfc2898DeriveBytes(password, salt, PASSWORD_DIFFICULTY).GetBytes(HASH_SIZE_IN_BYTES);

            return hashedPasswordBytes.SequenceEqual(hashBytes.Skip(SALT_SIZE_IN_BYTES));
        }

        public static byte[] GenerateRandomBytes(int size)
        {
            var bytes = new byte[size];
            RngCsp.GetBytes(bytes);
            return bytes;
        }
    }
}