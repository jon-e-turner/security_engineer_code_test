using ConfigChecker.Abstractions.Dtos;
using ConfigChecker.Abstractions.Models;

namespace ConfigChecker.Abstractions
{
    public interface IReportStore
    {
        public IAsyncEnumerable<FindingDto> GetReportAsync(string reportId);

        public ValueTask AppendToReportAsync(List<Finding> findings);

        public ValueTask DeleteReportAsync(string reportId);
    }
}
