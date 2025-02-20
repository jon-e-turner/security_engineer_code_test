using ConfigChecker.Dtos;
using System.Text.Json;

namespace ConfigCheckerTests.Dtos
{
    [TestClass]
    public class ResourceDtoTests
    {
        [TestMethod]
        public void ResourceDtoShouldParseDatabaseJsonIntoCorrectResourceDto()
        {
            ResourceDto? testResource = JsonSerializer.Deserialize<ResourceDto>("""
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

            Assert.IsNotNull(testResource);

            Assert.AreEqual("database", testResource.Type);
            Assert.AreEqual("db1", testResource.Name);
            
            Assert.IsNotEmpty(testResource.AzureSpecific);
            Assert.HasCount(3, testResource.AzureSpecific);
            Assert.AreEqual("rg2", testResource.AzureSpecific["resource_group"]);
            Assert.AreEqual("westus", testResource.AzureSpecific["location"]);
            Assert.AreEqual("Azure SQL Database", testResource.AzureSpecific["db_service"]);
            
            Assert.IsNotNull(testResource.SecuritySettings);
            Assert.IsNotEmpty(testResource.SecuritySettings);
            Assert.HasCount(4, testResource.SecuritySettings);

            var ports = testResource.SecuritySettings["open_ports"].Deserialize<int[]>();
            Assert.IsNotNull(ports);
            Assert.Contains(22, ports);
            Assert.Contains(80, ports);

            Assert.AreEqual("supersecurepassword", testResource.SecuritySettings["password"].GetString());
            Assert.IsTrue(testResource.SecuritySettings["encryption"].GetBoolean());
            Assert.IsTrue(testResource.SecuritySettings["mfa_enabled"].GetBoolean());
        }
    }
}
