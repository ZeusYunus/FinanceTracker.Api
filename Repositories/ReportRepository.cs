using FinanceTracker.Api.Data;
using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _db;

    public ReportRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<MonthlySummaryRow?> GetMonthlySummaryAsync(string userId, int year, int month)
    {
        var results = await _db.MonthlySummaries
            .FromSqlInterpolated($"""
                EXEC dbo.GetMonthlySummary
                    @UserId={userId},
                    @Year={year},
                    @Month={month}
                """)
            .ToListAsync();

        return results.SingleOrDefault();
    }
}