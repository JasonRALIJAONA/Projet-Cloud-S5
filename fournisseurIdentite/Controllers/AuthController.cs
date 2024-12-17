using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace fournisseurIdentite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Hacher un mot de passe avec PBKDF2
        [HttpPost("hash")]
        public ActionResult<object> HashPassword([FromBody] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return BadRequest(new { message = "Le mot de passe ne peut pas être nul ou vide." });
                }

                var hashedPassword = PasswordUtils.HashPassword(password);
                Console.WriteLine(hashedPassword);
                return Ok(new { hashedPassword });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erreur lors du hachage du mot de passe: {ex.Message}" });
            }
        }

        // Vérifier si un mot de passe correspond à un hash
        [HttpPost("verify")]
        public ActionResult<bool> VerifyPassword([FromBody] VerifyPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.StoredHash))
                {
                    return BadRequest("Le mot de passe ou le hash stocké ne peut pas être nul ou vide.");
                }

                bool isCorrect = PasswordUtils.VerifyPassword(request.Password, request.StoredHash);
                return Ok(isCorrect);  // Retourne si le mot de passe est correct
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la vérification du mot de passe: {ex.Message}");
            }
        }
    }

    // Classe de requête pour vérifier le mot de passe
    public class VerifyPasswordRequest
    {
        public string? Password { get; set; }
        public string? StoredHash { get; set; }
    }

    // Classe utilitaire pour gérer le hachage des mots de passe avec PBKDF2
    public class PasswordUtils
    {
        // Hacher un mot de passe avec PBKDF2
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                // Générer un salt aléatoire
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt);  // Utilisation de RandomNumberGenerator

                // Hacher le mot de passe avec PBKDF2
                byte[] hash = PBKDF2(password, salt);
                byte[] hashWithSalt = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, hashWithSalt, 0, salt.Length);
                Array.Copy(hash, 0, hashWithSalt, salt.Length, hash.Length);

                return Convert.ToBase64String(hashWithSalt);
            }
        }

        // Vérifier si un mot de passe correspond à un hash
        public static bool VerifyPassword(string password, string storedHash)
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

        // Implémentation de PBKDF2 avec SHA-256 et un nombre sécurisé d'itérations
        private static byte[] PBKDF2(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // Taille du hash
            }
        }
    }
}
