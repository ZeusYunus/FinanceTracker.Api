using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Api.DTOs;

public class CreateTransactionDto
{
    [Range(typeof(decimal), "0.01", "9999999999999999")]
    public decimal Amount { get; set; }

    [Required]
    [RegularExpression("^(Income|Expense)$")]
    public string Type { get; set; } = "Expense";

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int? CategoryId { get; set; }
}
