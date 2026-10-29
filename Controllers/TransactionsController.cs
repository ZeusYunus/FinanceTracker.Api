using System.Security.Claims;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs;
using FinanceTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public TransactionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Unauthorized();

        var transactions = await _db.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> Create(CreateTransactionDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Unauthorized();

        if (dto.Date == default) return BadRequest("A valid date is required.");

        if (dto.CategoryId is int categoryId)
        {
            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId);

            if (!categoryExists) return BadRequest("Invalid category.");
        }

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Type = dto.Type,
            Description = dto.Description,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAll),
            new { id = transaction.Id },
            transaction);
    }
}
