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
}