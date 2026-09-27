using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTDemo.Controllers;

public sealed class AccountController : Controller
{
    private readonly UserStore userStore;
    private readonly IConfiguration configuration;

    public AccountController(UserStore userStore, IConfiguration configuration)
    {
        this.userStore = userStore;
        this.configuration = configuration;
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userStore.ValidateAsync(model.Username, model.Password);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
            return View(model);
        }

        var issuer = configuration["Jwt:Issuer"] ?? "JWTDemo";
        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return LocalRedirect(string.IsNullOrWhiteSpace(model.ReturnUrl) ? "/" : model.ReturnUrl);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return RedirectToAction(nameof(Login));
    }
}
