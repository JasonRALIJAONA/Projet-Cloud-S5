using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fournisseurIdentite.Services;
using fournisseurIdentite.src.DTO;

namespace fournisseurIdentite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
     private readonly IPasswordService _passwordService;
    public UsersController(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }
    private readonly Users _users;

    [HttpPost("inscription")]
    public async Task<IActionResult> Inscription([FromBody] UsersRequest user){
        Users users= new Users();
        users.Username = user.Username;
        users.Pass = _passwordService.HashPassword(user.Password);
        users.Email = user.Email;



    }
}