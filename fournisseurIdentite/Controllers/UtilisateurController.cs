using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;
using fournisseurIdentite.Models;
using fournisseurIdentite.src.Utils;

namespace fournisseurIdentite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilisateurController : ControllerBase
{
    private readonly FournisseurIdentiteContext _context;
     private readonly IPasswordService _passwordService;
     private readonly EmailService _emailService;

    // Simuler une base de données (en mémoire)
    private static readonly List<User> _userDatabase = new()
    {
        new User { Id = 1, Username = "JohnDoe", Email = "johndoe@example.com", Pass = "WT6UAQCmn6gjl1u8S6jwCS/ldc1VrA2TjOz/zY8iqcSqyc52W/uuE2/deiZpJVj4" } // Hash simulé
    };
    
    public UtilisateurController(IPasswordService passwordService, EmailService emailService, FournisseurIdentiteContext context) 
    {
        _context = context;
        _emailService = emailService;
        _passwordService = passwordService;
    }
    private readonly User? _users;

    [HttpPost("inscription")]
    public async Task<IActionResult> Inscription([FromBody] UsersRequest user){

        User users = new()
        {
            Id = 1,
            Username = user.Username,
            Pass = _passwordService.HashPassword(user.Password ?? ""),
            Email = user.Email
        };

        // TO DO : save to database 
        await _context.Users.AddAsync(users);
        await _context.SaveChangesAsync();
        await _emailService.SendEmailAsync(users.Email ?? "", "Validation du compte", EmailBuilder.buildValidationMail(users.Id, users.Username ?? ""));
        return Ok("Compte créé");
    }
    [HttpPost("test")]
    public async Task<IActionResult> testBase(){
        
        User users = new()
        {
            Id = 1,
            Username = "rakoto",
            Pass = "123",
            Email = "rakoto",
            NbTentative = 0,
            EstValide = false
            
        };
        
        await _context.Users.AddAsync(users);
        await _context.SaveChangesAsync();
        // await _emailService.SendEmailAsync(users.Email ?? "", "Validation du compte", EmailBuilder.buildValidationMail(users.Id, users.Username ?? ""));
        return Ok("Compte créé");
       
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            return BadRequest("Email ou mot de passe manquant.");

        // Recherche de l'utilisateur dans la base
        var user = _userDatabase.FirstOrDefault(u => u.Email == loginRequest.Email);
        if (user == null)
            return Unauthorized("Utilisateur non trouvé.");

        // Vérification du mot de passe
        var isPasswordValid = _passwordService.VerifyPassword(loginRequest.Password, user.Pass ?? "");
        if (!isPasswordValid)
            return Unauthorized("Mot de passe incorrect.");
        // Retourner les données de l'utilisateur
        var userData = new
        {
            user.Id,
            user.Username,
            user.Email
        };
        await Task.CompletedTask;
        return Ok(userData);
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
        Users user = new ();
        // TODO: Get user by id 
        var utilisateur = await _context.Users.FindAsync(id);

        if (utilisateur == null)
        {
            return NotFound(new { message = "Utilisateur non trouvé." });
        }

        if (utilisateur.EstValide ?? false)
        {
            return BadRequest(new { message = "Utilisateur déjà validé." });
        }


        // Fonction 
        utilisateur.EstValide = true;

        // Valider changement
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
        return Ok(new { message = "Compte validé avec succès." });
    }


}