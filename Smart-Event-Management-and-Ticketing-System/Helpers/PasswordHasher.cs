using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Smart_Event_Management_and_Ticketing_System.Helpers
{
    /// <summary>
    /// Helper class for password hashing and verification
    /// Uses PBKDF2 with HMACSHA256
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hash a password using PBKDF2
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password with salt (Base64 encoded)</returns>
        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt using a cryptographically strong random sequence
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash password with PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Combine salt and hash for storage
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        /// <summary>
        /// Verify a password against a hash
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="hashedPassword">Stored hashed password (with salt)</param>
        /// <returns>True if password matches, false otherwise</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Split the stored hash into salt and hash parts
                var parts = hashedPassword.Split('.');
                if (parts.Length != 2)
                {
                    return false; // Invalid format
                }

                var salt = Convert.FromBase64String(parts[0]);
                var storedHash = parts[1];

                // Hash the provided password with the same salt
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                // Compare hashes
                return hashed == storedHash;
            }
            catch
            {
                return false;
            }
        }
    }
}
