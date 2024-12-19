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
    private const string PinSessionKey = "Pin";
    private const string PinExpirationSessionKey = "PinExpiration";
    private readonly FournisseurIdentiteContext _context;
     private readonly IPasswordService _passwordService;
     private readonly EmailService _emailService;
     private readonly PINService _pinService;
    private readonly UtilisateurService _service;
    // Simuler une base de données (en mémoire)

    
    public UtilisateurController(IPasswordService passwordService, EmailService emailService, FournisseurIdentiteContext context, PINService pinservice, UtilisateurService service) 
    {
        _context = context;
        _passwordService = passwordService;
        _pinService = pinservice;
        _emailService = emailService;
        _service = service;
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
        var user = _context.Utilisateurs.FirstOrDefault(u => u.Email == loginRequest.Email);

        if (user == null)
            return Unauthorized("Utilisateur non trouvé.");

        // Vérification du mot de passe
        var isPasswordValid = _passwordService.VerifyPassword(loginRequest.Password, user.MotDePasse ?? "");
        if (!isPasswordValid)
            return Unauthorized("Mot de passe incorrect.");

        // Génération du PIN
        string pin = _pinService.CreatePIN(5);
        HttpContext.Session.SetString(PinSessionKey, pin);
        HttpContext.Session.SetString(PinExpirationSessionKey, DateTime.UtcNow.AddSeconds(90).ToString("o"));

        // Envoi de l'e-mail avec le PIN
        await _emailService.SendEmailAsync(user.Email ?? "", "Validation du compte", EmailBuilder.buildPINMail(pin, user.NomUtilisateur ?? ""));

        return Ok(new { message = "Un PIN a été envoyé pour validation. Vous avez 90 secondes pour le valider." });
    }


    [HttpPost("validerPin")]
    public IActionResult ValiderPin([FromBody] PinValidationRequest request)
    {
        // Vérifier si le PIN existe dans la session
        string? sessionPin = HttpContext.Session.GetString(PinSessionKey);
        string? sessionPinExpiration = HttpContext.Session.GetString(PinExpirationSessionKey);

        if (string.IsNullOrEmpty(sessionPin) || string.IsNullOrEmpty(sessionPinExpiration))
        {
            return Unauthorized("PIN non trouvé ou session expirée.");
        }

        // Vérifier si le PIN correspond
        if (sessionPin != request.Pin)
        {
            return Unauthorized("PIN incorrect.");
        }

        // Vérifier si le PIN a expiré
        if (DateTime.TryParse(sessionPinExpiration, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime expirationDate))
        {
            if (DateTime.UtcNow > expirationDate)
            {
                return Unauthorized("Le PIN a expiré.");
            }
        }
        else
        {
            return BadRequest("Erreur dans le format d'expiration du PIN.");
        }


        // PIN validé, récupérer les données de l'utilisateur
        var user = _context.Utilisateurs.FirstOrDefault(u => u.Id == request.UserId);
        if (user == null)
        {
            return NotFound("Utilisateur non trouvé.");
        }

        var userData = new
        {
            user.Id,
            user.NomUtilisateur,
            user.Email
        };

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

    [HttpPost("inserer")]
     public IActionResult CreateUtilisateur([FromBody] Utilisateur utilisateur)
    {
        
        try
        {
            var insertedUser = _service.CreateUtilisateur(utilisateur);
            return Ok(insertedUser);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
        }
    }

   [HttpPut("update-info")]
    public IActionResult UpdateUtilisateur([FromBody] UpdateUtilisateurDto dto)
    {
        if (dto == null)
        {
            return BadRequest("dto object is null.");
        }

        try
        {
           var utilisateur = _service.UpdateUtilisateur(dto);
        return Ok(utilisateur);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

   [HttpPut("update-tentative")]
    public IActionResult AddTentative([FromBody] UtilisateurTentativeDto dto)
    {
        if (dto == null)
        {
            return BadRequest("dto object is null.");
        }

        try
        {
           var utilisateur = _service.AddTentative(dto);
        return Ok(utilisateur);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
