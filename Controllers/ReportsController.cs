using System.Security.Claims;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController(IReportRepository _reportRepository) : ControllerBase
{
    [HttpGet("monthly")]
    public async Task<ActionResult<MonthlySummaryRow>> GetMonthly([FromQuery] int year, [FromQuery] int month)
    {
        if (year < 1 || year > 9999 || month < 1 || month > 12)
        {
            return BadRequest("Invalid year or month.");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Unauthorized();

        var summary = await _reportRepository.GetMonthlySummaryAsync(userId, year, month);

        if (summary is null) return NotFound();

        return Ok(summary);
    }
}