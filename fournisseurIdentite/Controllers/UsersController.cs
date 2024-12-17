using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;
using fournisseurIdentite.Models;
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
    private readonly User? _users;

    [HttpPost("inscription")]
    public async Task<IActionResult> Inscription([FromBody] UsersRequest user){
    
        Users users= new Users();
        users.Id = 1;    
        users.Username = user.Username;
        users.Pass = _passwordService.HashPassword(user.Password);
        users.Email = user.Email;
        
        // TO DO : save to database 

        await _emailService.SendEmailAsync(users.Email, "Validation du compte", EmailBuilder.buildValidationMail(users.Id, users.Username));
        return Ok("Compte créé");
    }
    [HttpGet("valider")]
    public IActionResult ValiderForm([FromQuery] int id)
    {
        return Content($@"
            <html>
            <body>
                <form action='/api/users/valider' method='post'>
                    <input type='hidden' name='id' value='{id}' />
                    <p>Cliquez sur le bouton pour valider votre compte :</p>
                    <button type='submit'>Valider mon compte</button>
                </form>
            </body>
            </html>", "text/html");
    }

    [HttpPost("valider")]
    public async Task<IActionResult> ValiderUtilisateur([FromForm] int id)
    {
        Users user = new Users();
        // TODO: Get user by id 
        // var user = await _context.Users.FindAsync(id);

        // if (user == null)
        // {
        //     return NotFound(new { message = "Utilisateur non trouvé." });
        // }

        // if (user.IsValidated)
        // {
        //     return BadRequest(new { message = "Utilisateur déjà validé." });
        // }

        // Fonction 
        user.EstValide = true;
        // Valider changement
        // await _context.SaveChangesAsync();

        return Ok(new { message = "Compte validé avec succès." });
    }


}