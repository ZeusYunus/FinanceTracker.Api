using System;

namespace FinanceTracker.Api.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    // "Income" or "Expense"
    public string Type { get; set; } = "Expense";

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}
