using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Repositories;

public interface IReportRepository
{
    Task<MonthlySummaryRow?> GetMonthlySummaryAsync(string userId, int year, int month);
}