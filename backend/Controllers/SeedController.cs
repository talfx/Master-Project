using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    public SeedController(AppDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpPost("create-admin")]
    public async Task<IActionResult> CreateAdmin()
    {
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = _passwordService.HashPassword("admin123"),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Admin user created", username = "admin", password = "admin123" });
    }
}