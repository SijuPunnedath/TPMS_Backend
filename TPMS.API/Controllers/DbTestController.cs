using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace TPMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DbTestController : ControllerBase
{
    private readonly IConfiguration _config;

    public DbTestController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestDb()
    {
        var connString = _config.GetConnectionString("DefaultConnection");

        try
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            return Ok(new
            {
                message = "Connected successfully!"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Connection failed",
                error = ex.Message,
                details = ex.ToString()
            });
        }

    }
}