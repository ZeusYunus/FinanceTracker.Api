using System;

namespace FinanceTracker.Api.Models;

public class MonthlySummaryRow
{
    public decimal Income { get; set; }

    public decimal Expenses { get; set; }

    public decimal Balance { get; set; }
}
