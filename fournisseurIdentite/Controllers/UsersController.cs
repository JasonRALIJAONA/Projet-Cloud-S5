using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;
using fournisseurIdentite.src.Utils;
namespace fournisseurIdentite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
     private readonly IPasswordService _passwordService;
     private readonly EmailService _emailService;
    public UsersController(IPasswordService passwordService, EmailService emailService) 
    {
        _emailService = emailService;
        _passwordService = passwordService;
    }
    private readonly Users _users;

    [HttpPost("inscription")]
    public async Task<IActionResult> Inscription([FromBody] UsersRequest user){
        Users users= new Users();
        users.Username = user.Username;
        users.Pass = _passwordService.HashPassword(user.Password);
        users.Email = user.Email;
        
        // TO DO : save to database 

        await _emailService.SendEmailAsync(users.Email, "Validation du compte", EmailBuilder.buildValidationMail(users.Id, users.Username));
        return Ok("Compte créé");
    }
}