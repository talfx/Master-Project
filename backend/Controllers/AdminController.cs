using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("query")]
    public async Task<IActionResult> ExecuteQuery([FromBody] QueryRequest request)
    {
        try
        {
            // Only allow SELECT queries for safety
            if (!request.Query.Trim().ToUpper().StartsWith("SELECT"))
            {
                return BadRequest(new { message = "Only SELECT queries are allowed" });
            }

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = request.Query;

            using var reader = await command.ExecuteReaderAsync();
            var results = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class QueryRequest
{
    public string Query { get; set; } = string.Empty;
}