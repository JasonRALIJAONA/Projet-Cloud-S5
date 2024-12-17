using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;

namespace fournisseurIdentite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PasswordService _passwordService;

        // Injection du service via le constructeur
        public AuthController(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        // Hacher un mot de passe avec PBKDF2
        [HttpPost("hash")]
        public ActionResult<string> HashPassword([FromBody] string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Le mot de passe ne peut pas être nul ou vide." });
            }

            var hashedPassword = _passwordService.HashPassword(password);
            return Ok(hashedPassword);
        }

        // Vérifier si un mot de passe correspond à un hash
        [HttpPost("verify")]
        public ActionResult<bool> VerifyPassword([FromBody] VerifyPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.StoredHash))
            {
                return BadRequest("Le mot de passe ou le hash stocké ne peut pas être nul ou vide.");
            }

            bool isCorrect = _passwordService.VerifyPassword(request.Password, request.StoredHash);
            return Ok(isCorrect);
        }
    }
}
