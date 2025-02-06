using ConfigChecker.DTOs;
using ConfigChecker.Models;

namespace ConfigChecker.Services
{
  internal interface IReportStore
  {
    public IAsyncEnumerable<FindingDto> GetReportAsync(string reportId);

    public ValueTask CreateReportAsync(List<Finding> findings);

    public ValueTask DeleteReportAsync(string reportId);
  }
}
