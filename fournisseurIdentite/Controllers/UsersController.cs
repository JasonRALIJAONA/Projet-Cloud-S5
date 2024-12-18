using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;
using fournisseurIdentite.Models;

namespace fournisseurIdentite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
     private readonly IPasswordService _passwordService;
     private readonly EmailService _emailService;

    // Simuler une base de données (en mémoire)
    private static readonly List<Users> _userDatabase = new()
    {
        new Users { Id = 1, Username = "JohnDoe", Email = "johndoe@example.com", Pass = "WT6UAQCmn6gjl1u8S6jwCS/ldc1VrA2TjOz/zY8iqcSqyc52W/uuE2/deiZpJVj4" } // Hash simulé
    };
    
    public UsersController(IPasswordService passwordService, EmailService emailService) 
    {
        _emailService = emailService;
        _passwordService = passwordService;
    }
    private readonly User? _users;

    [HttpPost("inscription")]
    public async Task<IActionResult> Inscription([FromBody] UsersRequest user){
        User users = new()
        {
            Username = user.Username,
            Pass = _passwordService.HashPassword(user.Password ?? ""),
            Email = user.Email
        };

        // Simulate async operation
        await Task.CompletedTask;

        return Ok(users);
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
        var isPasswordValid = _passwordService.VerifyPassword(loginRequest.Password, user.Pass);
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
}