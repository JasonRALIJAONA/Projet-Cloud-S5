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
    private static readonly List<Utilisateur> _userDatabase = new()
    {
        new Utilisateur { Id = 1, NomUtilisateur = "JohnDoe", Email = "johndoe@example.com", MotDePasse = "WT6UAQCmn6gjl1u8S6jwCS/ldc1VrA2TjOz/zY8iqcSqyc52W/uuE2/deiZpJVj4" } // Hash simulé
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

        Console.WriteLine(user+"  h");
        Utilisateur users = new()
        {
            NomUtilisateur = user.Username,
            Email = user.Email,
            MotDePasse = _passwordService.HashPassword(user.Password ?? "")
        };
        Console.WriteLine(users);

        // TO DO : save to database 
        await _context.Utilisateurs.AddAsync(users);
        await _context.SaveChangesAsync();
        await _emailService.SendEmailAsync(users.Email ?? "", "Validation du compte", EmailBuilder.buildValidationMail(users.Id, users.NomUtilisateur ?? ""));
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
        var isPasswordValid = _passwordService.VerifyPassword(loginRequest.Password, user.MotDePasse ?? "");
        if (!isPasswordValid)
            return Unauthorized("Mot de passe incorrect.");
        // Retourner les données de l'utilisateur
        var userData = new
        {
            user.Id,
            user.NomUtilisateur,
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
        Console.WriteLine(id);
        // TODO: Get user by id 
        var utilisateur = await _context.Utilisateurs.FindAsync(id);

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
