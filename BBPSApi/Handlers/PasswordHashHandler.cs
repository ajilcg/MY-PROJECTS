using System.Security.Cryptography;

namespace BBPSApi.Handlers
{
    public class PasswordHashHandler
    {
        // Method to hash a password
        public static string HashPassword(string password)
        {
             int saltSize = 128 / 8;
            var salt=new byte[saltSize];
            // Generate a cryptographically secure salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                salt = new byte[16]; // Salt size is typically 16 bytes
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using PBKDF2
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hashBytes = rfc2898.GetBytes(32); // 32 bytes hash
                byte[] hashWithSalt = new byte[hashBytes.Length + salt.Length];

                // Combine the hash and salt
                Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashBytes, 0, hashWithSalt, salt.Length, hashBytes.Length);

                return Convert.ToBase64String(hashWithSalt);
            }
        }

        // Method to verify a password against a hashed password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert the hashed password back into bytes
            byte[] hashWithSalt = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16]; // The salt size used during hashing
            byte[] storedHash = new byte[hashWithSalt.Length - salt.Length];

            // Extract salt from the hashed password
            Buffer.BlockCopy(hashWithSalt, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(hashWithSalt, salt.Length, storedHash, 0, storedHash.Length);

            // Hash the input password with the extracted salt
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] passwordHash = rfc2898.GetBytes(32);
                // Compare the hashes
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != storedHash[i]) return false;
                }
            }

            return true; // Password is valid
        }
    }
}
