using Core.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AuthController(ILogger<AuthController> logger, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] UserAuthDto userAuth)
    {
        var user = await _userManager.FindByEmailAsync(userAuth.Email)
                   ?? await _userManager.FindByNameAsync(userAuth.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.PasswordSignInAsync(user, userAuth.Password, true, false);

        if (result.Succeeded)
        {
            return Ok(new
            {
                user = new
                {
                    username = user.UserName,
                    email = user.Email
                }
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpDto userAuth)
    {
        var user = new User()
        {
            UserName = userAuth.Username,
            Email = userAuth.Email,
        };

        var result = await _userManager.CreateAsync(user, userAuth.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
            return Ok(new { user = new { username = user.UserName, email = user.Email } });
        }

        return BadRequest(new { message = "Проверьте данные регистрации.", errors = result.Errors.Select(x => x.Description) });
    }

    [HttpGet, Authorize]
    public IActionResult Me() => Ok(new { user = new { username = User.Identity!.Name, email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value } });

    [HttpPost]
    public async Task<IActionResult> Logout() { await _signInManager.SignOutAsync(); return NoContent(); }
}

public record UserAuthDto(string Email, string Password);

public record UserSignUpDto(string Username, string Email, string Password);
