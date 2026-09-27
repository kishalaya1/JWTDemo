using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key is not configured. Set Jwt:Key (for example via environment variable Jwt__Key).");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok(new { Message = "JWT authentication demo is running." }));

app.MapPost("/login", (LoginRequest request) =>
{
    if (request is not { Username: "demo", Password: "password123" })
    {
        return Results.Unauthorized();
    }

    var expiresAt = DateTime.UtcNow.AddMinutes(30);
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, request.Username),
        new Claim(ClaimTypes.Role, "DemoUser")
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = expiresAt,
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Results.Ok(new TokenResponse(tokenHandler.WriteToken(token), expiresAt));
}).AllowAnonymous();

app.MapGet("/secure", (ClaimsPrincipal user) =>
{
    return Results.Ok(new
    {
        Message = "This endpoint is protected by JWT authentication.",
        User = user.Identity?.Name
    });
}).RequireAuthorization();

app.Run();

record LoginRequest(string Username, string Password);
record TokenResponse(string AccessToken, DateTime ExpiresAt);
