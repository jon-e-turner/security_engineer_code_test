using ConfigChecker.Models;

namespace ConfigChecker
{
  internal interface IReportStore
  {
    public ValueTask<List<Finding>> GetReport(string reportId);
  }
}
