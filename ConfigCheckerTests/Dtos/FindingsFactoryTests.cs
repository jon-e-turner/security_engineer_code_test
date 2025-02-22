using ConfigChecker.Dtos;
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
        """
    ,
    TestHarness.Options.SerializerOptions);

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

    [TestMethod]
    public void CreateExposedRcePortFindingReturnsFindingDto()
    {
        if (testResource is not null)
        {
            var finding = testResource.CreateExposedRcePortFinding(22);

            Assert.IsNotNull(finding);
            Assert.AreEqual(testResource.Name, finding.ResourceName);
            Assert.AreEqual(FindingName.OpenRcePort, finding.Name);
            Assert.Contains("Port 22 ", finding.Description);
            Assert.Contains(" network groups ", finding.Mitigation);
            Assert.AreEqual(FindingSeverity.Critical, finding.Severity);
            Assert.AreEqual("862", finding.CweId);
        }
        else
        {
            Assert.Fail("Could not parse test resource");
        }
    }

    [TestMethod]
    public void CreateWeakPasswordFindingReturnsFindingDto()
    {
        if (testResource is not null)
        {
            var finding = testResource.CreateWeakPasswordFinding();

            Assert.IsNotNull(finding);
            Assert.AreEqual(testResource.Name, finding.ResourceName);
            Assert.AreEqual(FindingName.WeakPassword, finding.Name);
            Assert.Contains(" minimum requirements.", finding.Description);
            Assert.Contains(" length and complexity.", finding.Mitigation);
            Assert.AreEqual(FindingSeverity.Critical, finding.Severity);
            Assert.AreEqual("1391", finding.CweId);
        }
        else
        {
            Assert.Fail("Could not parse test resource");
        }
    }

    [TestMethod]
    public void CreateMfaDisabledFindingReturnsFindingDto()
    {
        if (testResource is not null)
        {
            var finding = testResource.CreateMfaDisabledFinding();

            Assert.IsNotNull(finding);
            Assert.AreEqual(testResource.Name, finding.ResourceName);
            Assert.AreEqual(FindingName.MfaDisabled, finding.Name);
            Assert.Contains("Multi-factor authentication ", finding.Description);
            Assert.Contains("Require multi-factor ", finding.Mitigation);
            Assert.AreEqual(FindingSeverity.High, finding.Severity);
            Assert.AreEqual("308", finding.CweId);
        }
        else
        {
            Assert.Fail("Could not parse test resource");
        }
    }

    [TestMethod]
    public void CreateEncryptionDisabledFindingReturnsFindingDto()
    {
        if (testResource is not null)
        {
            var finding = testResource.CreateEncryptionDisabledFinding();

            Assert.IsNotNull(finding);
            Assert.AreEqual(testResource.Name, finding.ResourceName);
            Assert.AreEqual(FindingName.EncryptionDisabled, finding.Name);
            Assert.Contains("Encryption-at-rest ", finding.Description);
            Assert.Contains("Require encryption ", finding.Mitigation);
            Assert.AreEqual(FindingSeverity.High, finding.Severity);
            Assert.AreEqual("312", finding.CweId);
        }
        else
        {
            Assert.Fail("Could not parse test resource");
        }
    }
}
