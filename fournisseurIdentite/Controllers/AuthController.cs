using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;

namespace fournisseurIdentite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public AuthController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("hash")]
        public ActionResult<string> HashPassword([FromBody] string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Le mot de passe ne peut pas être vide.");

            var hashedPassword = _passwordService.HashPassword(password);
            return Ok(hashedPassword);
        }

        [HttpPost("verify")]
        public ActionResult<bool> VerifyPassword([FromBody] VerifyPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.StoredHash))
                return BadRequest("Mot de passe ou hash non fourni.");

            var isCorrect = _passwordService.VerifyPassword(request.Password, request.StoredHash);
            return Ok(isCorrect);
        }
    }

}
