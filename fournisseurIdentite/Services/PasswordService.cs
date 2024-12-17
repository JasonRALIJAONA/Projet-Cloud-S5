using System;
using System.Security.Cryptography;

namespace fournisseurIdentite.Services
{
    public interface PasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }

    public class PasswordService : PasswordService
    {
        public string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                // Générer un salt aléatoire
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt);

                // Hacher le mot de passe
                byte[] hash = PBKDF2(password, salt);
                byte[] hashWithSalt = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, hashWithSalt, 0, salt.Length);
                Array.Copy(hash, 0, hashWithSalt, salt.Length, hash.Length);

                return Convert.ToBase64String(hashWithSalt);
            }
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashWithSalt = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashWithSalt, 0, salt, 0, salt.Length);

            byte[] hash = PBKDF2(password, salt);

            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != hashWithSalt[salt.Length + i])
                {
                    return false;
                }
            }

            return true;
        }

        private byte[] PBKDF2(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // Taille du hash
            }
        }
    }
}
