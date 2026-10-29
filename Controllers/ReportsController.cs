using System.Security.Claims;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ReportsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<MonthlySummaryRow>> GetMonthly([FromQuery] int year, [FromQuery] int month)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Unauthorized();

        if (year < 1 || year > 9999 || month < 1 || month > 12)
        {
            return BadRequest("Invalid year or month.");
        }

        var results = await _db.MonthlySummaries
            .FromSqlInterpolated($@"
                EXEC dbo.GetMonthlySummary
                    @UserId={userId},
                    @Year={year},
                    @Month={month}")
            .ToListAsync();

        return Ok(results.Single());
    }
}