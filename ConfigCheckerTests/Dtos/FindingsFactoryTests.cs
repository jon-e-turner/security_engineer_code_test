using ConfigChecker.Dtos;
using ConfigChecker.Models;
using System.Text.Json;

namespace ConfigCheckerTests.Dtos;

[TestClass]
public class FindingsFactoryTests
{
    private readonly ResourceDto? testResource = JsonSerializer.Deserialize<ResourceDto>("""
        {
          "type": "database",
          "name": "db1",
          "open_ports": [22, 80],
          "password": "supersecurepassword",
          "encryption": true,
          "mfa_enabled": true,
          "azure_specific": {
            "resource_group": "rg2",
            "location": "westus",
            "db_service": "Azure SQL Database"
          }
        }
        """,
        JsonSerializerOptions.Web);

    [TestMethod]
    public void CreateExposedPortFindingReturnsFindingDto()
    {
        if (testResource is not null)
        {
            var finding = testResource.CreateExposedPortFinding(80);

            Assert.IsNotNull(finding);
            Assert.AreEqual(testResource.Name, finding.ResourceName);
            Assert.AreEqual(FindingName.OpenPort, finding.Name);
            Assert.Contains("Port 80 ", finding.Description);
            Assert.Contains(" network groups ", finding.Mitigation);
            Assert.AreEqual(FindingSeverity.High, finding.Severity);
            Assert.AreEqual("862", finding.CweId);
        }
        else
        {
            Assert.Fail("Could not parse test resource");
        }
    }
}
