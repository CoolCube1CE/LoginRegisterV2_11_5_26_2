using System;
using System.Security.Cryptography;

namespace LoginRegisterV2
{
    /// <summary>
    /// Turns passwords into stored hashes and checks a typed password against one.
    /// Moved here from App_Code. The algorithm is unchanged (PBKDF2, 100000
    /// iterations, 16-byte salt, 32-byte hash, stored as "salt:hash"), so every
    /// password already in your database still works.
    /// </summary>
    public static class PasswordHelper
    {
        private const int Iterations = 100000;

        // Hashes a plain-text password into a "salt:hash" string safe to store in the DB
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        // Compares a plain-text password against a stored "salt:hash" string
        public static bool VerifyPassword(string password, string storedValue)
        {
            if (string.IsNullOrEmpty(storedValue) || !storedValue.Contains(":"))
                return false;

            string[] parts = storedValue.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] computedHash = pbkdf2.GetBytes(32);
                return ConstantTimeEquals(storedHash, computedHash);
            }
        }

        // Constant-time comparison so login timing can't leak info about the password
        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
