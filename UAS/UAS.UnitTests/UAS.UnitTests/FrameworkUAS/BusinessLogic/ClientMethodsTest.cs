using System;
using System.Data;
using System.Collections.Generic;
using System.Transactions.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using FrameworkUAS.BusinessLogic;
using KMPlatform.Entity;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using ShimClientMethodsData = FrameworkUAS.DataAccess.Fakes.ShimClientMethods;

namespace UAS.UnitTests.FrameworkUAS.BusinessLogic
{
    [TestFixture]
    public class ClientMethodsTest
    {
        private IDisposable shims;
        private bool taScopeCompleteCalled;

        [SetUp]
        public void SetUp()
        {
            shims = ShimsContext.Create();
            taScopeCompleteCalled = false;
            ShimTransactionScope.AllInstances.Complete = scope => { taScopeCompleteCalled = true; };            
        }

        [TearDown]
        public void TearDown()
        {
            shims?.Dispose();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GLM_Relational_Update_Call_ClientCalled(bool returns)
        {
            // Arrange
            var updateCalled = false;
            ShimClientMethodsData.GLM_Relational_Update = () =>
                {
                    updateCalled = true;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.GLM_Relational_Update();

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BriefMedia_Relational_CleanUpData_Call_ClientCalled(bool returns)
        {
            // Arrange
            var sameClient = false;
            var client = new KMPlatform.Entity.Client();
            ShimClientMethodsData.BriefMedia_Relational_CleanUpDataClient = cl =>
                {
                    sameClient = client == cl;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.BriefMedia_Relational_CleanUpData(client);

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(sameClient);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GLM_CreateTempCMSTables_Call_ClientCalled(bool returns)
        {
            // Arrange
            var updateCalled = false;
            ShimClientMethodsData.GLM_CreateTempCMSTables = () =>
                {
                    updateCalled = true;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.GLM_CreateTempCMSTables();

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GLM_DropTempCMSTables_Call_ClientCalled(bool returns)
        {
            // Arrange
            var updateCalled = false;
            ShimClientMethodsData.GLM_DropTempCMSTables = () =>
                {
                    updateCalled = true;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.GLM_DropTempCMSTables();

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WATT_CreateTempCMSTables_Call_ClientCalled(bool returns)
        {
            // Arrange
            var updateCalled = false;
            ShimClientMethodsData.WATT_CreateTempCMSTables = () =>
                {
                    updateCalled = true;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.WATT_CreateTempCMSTables();

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WATT_DropTempCMSTables_Call_ClientCalled(bool returns)
        {
            // Arrange
            var updateCalled = false;
            ShimClientMethodsData.WATT_DropTempCMSTables = () =>
                {
                    updateCalled = true;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.WATT_DropTempCMSTables();

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(updateCalled);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WATT_Relational_Process_ECN_GROUPID_PUBCODE_Call_ClientCalled(bool returns)
        {
            // Arrange
            var sameTable = false;
            var dataTable = new DataTable();
            ShimClientMethodsData.WATT_Relational_Process_ECN_GROUPID_PUBCODEDataTable = table => 
                {
                    sameTable = table == dataTable;
                    return returns;
                };

            var clientMethods = new ClientMethods();

            // Act
            var done = clientMethods.WATT_Relational_Process_ECN_GROUPID_PUBCODE(dataTable);

            // Assert
            Assert.AreEqual(returns, done);
            Assert.IsTrue(sameTable);
            Assert.IsTrue(taScopeCompleteCalled);
        }

        [Test]
        public void Haymarket_CMS_Insert_Subscriber_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string>
                {
                    "YearOfBirth",
                    "PrimaryEmailAddress"
                },
                new List<string[]>
                {
                    new []
                    {
                        "row1Value1",
                        "row1Value2"
                    }
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Insert_SubscriberDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Insert_Subscriber(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Haymarket_CMS_Update_Address_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Update_AddressDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Update_Address(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Haymarket_CMS_Insert_PublicationPubCode_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Insert_PublicationPubCodeDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Insert_PublicationPubCode(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Haymarket_CMS_Insert_Activity_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Insert_ActivityDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Insert_Activity(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Haymarket_CMS_Insert_Ecomm_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Insert_EcommDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Insert_Ecomm(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Haymarket_CMS_Update_Medical_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Haymarket_CMS_Update_MedicalDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Haymarket_CMS_Update_Medical(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Insert_Access_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Insert_AccessClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Insert_Access(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Update_Users_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Update_UsersClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Update_Users(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Update_TaxBehavior_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Update_TaxBehaviorClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Update_TaxBehavior(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Update_PageBehavior_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Update_PageBehaviorClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Update_PageBehavior(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Update_SearchBehavior_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Update_SearchBehaviorClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Update_SearchBehavior(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void BriefMedia_Relational_Update_TopicCode_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var client = new Client();
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.BriefMedia_Relational_Update_TopicCodeClientDataTable = (client1, table) =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.BriefMedia_Relational_Update_TopicCode(client, dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_PersonID_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> {"col1"},
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_PersonIDDataTable = table =>
            {
                numberOfBulkInserts++;
            };
            
            var clientMethods = new ClientMethods();
            
            // Act
            clientMethods.Advanstar_Insert_PersonID(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void GLM_Relational_InsertData_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.GLM_Relational_InsertDataDataTable = table =>
            {
                numberOfBulkInserts++;
                return true;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.GLM_Relational_InsertData(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_RegCodeCompare_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_RegCodeCompareDataTable = table =>
            {
                numberOfBulkInserts++;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Advanstar_Insert_RegCodeCompare(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_RegCode_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_RegCodeDataTable = table =>
            {
                numberOfBulkInserts++;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Advanstar_Insert_RegCode(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_SourceCode_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_SourceCodeDataTable = table =>
            {
                numberOfBulkInserts++;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Advanstar_Insert_SourceCode(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_PriCode_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_PriCodeDataTable = table =>
            {
                numberOfBulkInserts++;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Advanstar_Insert_PriCode(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        [Test]
        public void Advanstar_Insert_RefreshDupes_ValidDataTable_PerformsBulkUpdate()
        {
            // Arrange
            var dataTable = CreateDataTable(
                new List<string> { "col1" },
                new List<string[]>
                {
                    new [] {"row1Value1"}
                });
            var numberOfBulkInserts = 0;
            var expectedResults = 1;

            ShimClientMethodsData.Advanstar_Insert_RefreshDupesDataTable = table =>
            {
                numberOfBulkInserts++;
            };

            var clientMethods = new ClientMethods();

            // Act
            clientMethods.Advanstar_Insert_RefreshDupes(dataTable);

            // Assert
            Assert.AreEqual(expectedResults, numberOfBulkInserts);
        }

        private static DataTable CreateDataTable(List<string> columns, List<string[]> rows)
        {
            var dataTable = new DataTable();
            if (columns == null)
            {
                return dataTable;
            }

            var numberOfColumns = columns.Count;
            foreach (var column in columns)
            {
                dataTable.Columns.Add(column);
            }
            
            foreach (var row in rows)
            {
                if (row == null)
                {
                    continue;
                }

                var dataRow = dataTable.NewRow();
                int numberOfRowValues = row.Length;
                for (int i = 0; i < numberOfColumns && i < numberOfRowValues; i++)
                {
                    dataRow[i] = row[i];
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
