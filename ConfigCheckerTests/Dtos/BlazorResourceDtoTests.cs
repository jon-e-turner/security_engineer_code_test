using System.Text.Json;

using ConfigCheckerBlazor.Models;

using ConfigCheckerTests.TestHarness;

namespace ConfigCheckerTests.Dtos
{
    [ TestClass ]
    public class BlazorResourceDtoTests
    {
        [ TestMethod ]
        public void ResourceDtoShouldParseDatabaseJsonIntoCorrectResourceDto()
        {
            var testResource = JsonSerializer.Deserialize<ResourceDto>("""
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
                Options.SerializerOptions);

            Assert.IsNotNull(testResource);

            Assert.AreEqual("database", testResource.Type);
            Assert.AreEqual("db1", testResource.Name);

            Assert.IsNotEmpty(testResource.AzureSpecific);
            Assert.HasCount(3, testResource.AzureSpecific);
            Assert.AreEqual("rg2", testResource.AzureSpecific[ "resource_group" ]);
            Assert.AreEqual("westus", testResource.AzureSpecific[ "location" ]);
            Assert.AreEqual("Azure SQL Database", testResource.AzureSpecific[ "db_service" ]);

            Assert.IsNotNull(testResource.SecuritySettings);
            Assert.IsNotEmpty(testResource.SecuritySettings);
            Assert.HasCount(4, testResource.SecuritySettings);

            var ports = testResource.SecuritySettings[ "open_ports" ].Deserialize<int[ ]>();
            Assert.IsNotNull(ports);
            Assert.Contains(22, ports);
            Assert.Contains(80, ports);

            Assert.AreEqual("supersecurepassword", testResource.SecuritySettings[ "password" ].GetString());
            Assert.IsTrue(testResource.SecuritySettings[ "encryption" ].GetBoolean());
            Assert.IsTrue(testResource.SecuritySettings[ "mfa_enabled" ].GetBoolean());
        }

        [ TestMethod ]
        public void ResourceDtoShouldParseVirtualMachineJsonIntoCorrectResourceDto()
        {
            var testResource = JsonSerializer.Deserialize<ResourceDto>("""
                                                                       {
                                                                         "type": "virtual_machine",
                                                                         "name": "vm1",
                                                                         "open_ports": [22, 80, 443],
                                                                         "password": "weakpassword",
                                                                         "encryption": false,
                                                                         "mfa_enabled": false,
                                                                         "azure_specific": {
                                                                           "resource_group": "rg1",
                                                                           "location": "eastus",
                                                                           "vm_size": "Standard_DS1_v2"
                                                                         }
                                                                       }
                                                                       """,
                Options.SerializerOptions);

            Assert.IsNotNull(testResource);

            Assert.AreEqual("virtual_machine", testResource.Type);
            Assert.AreEqual("vm1", testResource.Name);

            Assert.IsNotEmpty(testResource.AzureSpecific);
            Assert.HasCount(3, testResource.AzureSpecific);
            Assert.AreEqual("rg1", testResource.AzureSpecific[ "resource_group" ]);
            Assert.AreEqual("eastus", testResource.AzureSpecific[ "location" ]);
            Assert.AreEqual("Standard_DS1_v2", testResource.AzureSpecific[ "vm_size" ]);

            Assert.IsNotNull(testResource.SecuritySettings);
            Assert.IsNotEmpty(testResource.SecuritySettings);
            Assert.HasCount(4, testResource.SecuritySettings);

            var ports = testResource.SecuritySettings[ "open_ports" ].Deserialize<int[ ]>();
            Assert.IsNotNull(ports);
            Assert.Contains(22, ports);
            Assert.Contains(80, ports);
            Assert.Contains(443, ports);

            Assert.AreEqual("weakpassword", testResource.SecuritySettings[ "password" ].GetString());
            Assert.IsFalse(testResource.SecuritySettings[ "encryption" ].GetBoolean());
            Assert.IsFalse(testResource.SecuritySettings[ "mfa_enabled" ].GetBoolean());
        }

        [ TestMethod ]
        public void ResourceDtoShouldParseStorageAccountJsonIntoCorrectResourceDto()
        {
            var testResource = JsonSerializer.Deserialize<ResourceDto>("""
                                                                       {
                                                                         "type": "storage_account",
                                                                         "name": "storage1",
                                                                         "encryption": false,
                                                                         "azure_specific": {
                                                                           "resource_group": "rg1",
                                                                           "location": "eastus",
                                                                           "account_tier": "Standard",
                                                                           "replication": "LRS"
                                                                         }
                                                                       }
                                                                       """,
                Options.SerializerOptions);

            Assert.IsNotNull(testResource);

            Assert.AreEqual("storage_account", testResource.Type);
            Assert.AreEqual("storage1", testResource.Name);

            Assert.IsNotEmpty(testResource.AzureSpecific);
            Assert.HasCount(4, testResource.AzureSpecific);
            Assert.AreEqual("rg1", testResource.AzureSpecific[ "resource_group" ]);
            Assert.AreEqual("eastus", testResource.AzureSpecific[ "location" ]);
            Assert.AreEqual("Standard", testResource.AzureSpecific[ "account_tier" ]);
            Assert.AreEqual("LRS", testResource.AzureSpecific[ "replication" ]);

            Assert.IsNotNull(testResource.SecuritySettings);
            Assert.IsNotEmpty(testResource.SecuritySettings);
            Assert.HasCount(1, testResource.SecuritySettings);

            Assert.IsFalse(testResource.SecuritySettings[ "encryption" ].GetBoolean());
        }
    }
}
