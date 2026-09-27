using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace JWTDemo.Models;

public sealed class DemoUser
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public sealed class LoginViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
}

public sealed class UserStore
{
    private readonly IWebHostEnvironment environment;

    public UserStore(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    public async Task<DemoUser?> ValidateAsync(string username, string password)
    {
        var path = Path.Combine(environment.ContentRootPath, "Data", "users.json");
        await using var stream = File.OpenRead(path);
        var users = await JsonSerializer.DeserializeAsync<List<DemoUser>>(stream) ?? [];
        return users.FirstOrDefault(user =>
            string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase) &&
            user.Password == password);
    }
}
