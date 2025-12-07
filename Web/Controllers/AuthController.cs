using Core.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[AllowAnonymous]
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

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userAuth.Password);

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(new { message = "Username or password is incorrect" });
    }
}

public record UserAuthDto(string Email, string Password);

public record UserSignUpDto(string Username, string Email, string Password);