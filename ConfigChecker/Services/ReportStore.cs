using ConfigChecker.Abstractions;
using ConfigChecker.Dtos;
using ConfigChecker.Models;
using ConfigChecker.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ConfigChecker.Services
{
  public class ReportStore : IReportStore
  {
    private readonly FindingsDbContext _dbContext;

    public ReportStore(FindingsDbContext dbContext) 
    { 
      _dbContext = dbContext;
    }

    public async ValueTask AppendToReportAsync(List<Finding> findings)
    {
      await _dbContext.AddRangeAsync(findings);
      await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DeleteReportAsync(string reportId)
    {
      var findingsToDelete = await _dbContext.Findings
        .Where(f => f.ReportId.Equals(reportId, StringComparison.InvariantCultureIgnoreCase))
        .ToListAsync();

      if (findingsToDelete.Count > 0)
      {
        _dbContext.RemoveRange(findingsToDelete);
        await _dbContext.SaveChangesAsync();
      }
    }

    public async IAsyncEnumerable<FindingDto> GetReportAsync(string reportId)
    {
      var findings = _dbContext.Findings
        .Where(f => f.ReportId.Equals(reportId, StringComparison.InvariantCultureIgnoreCase))
        .AsAsyncEnumerable();

      await foreach (var f in findings) {
        yield return new FindingDto(f.ResourceName, f.Name, f.Description, f.Mitigation, f.Severity, f.CweId);
      }
    }
  }
}
