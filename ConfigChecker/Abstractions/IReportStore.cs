using ConfigChecker.DTOs;
using ConfigChecker.Models;

namespace ConfigChecker.Abstractions
{
  internal interface IReportStore
  {
    public IAsyncEnumerable<FindingDto> GetReportAsync(string reportId);

    public ValueTask AppendToReportAsync(List<Finding> findings);

    public ValueTask DeleteReportAsync(string reportId);
  }
}
