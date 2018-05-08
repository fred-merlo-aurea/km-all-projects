using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.BusinessLogic;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KmCommonFake = KM.Common.Fakes;
using UadDataAccess = FrameworkUAD.DataAccess.Fakes;

namespace FrameworkUAD.Tests.BusinessLogic
{
    /// <summary>
    /// Unit test for <see cref="IssueArchiveProductSubscription"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueArchiveProductSubscriptionTests
    {
        private const string TestColumn = "TestColumn";
        private const string TestRow = "TestRow";
        private const string TestAccountNumber = "TestField1";
        private const int TestUser = 1;
        private const int TestOne = 1;
        private const int ProductId = 1;
        private const int IssueId = 1;
        private IssueArchiveProductSubscription _issueArchiveProductSubscription;
        private IDisposable _shimContext;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _issueArchiveProductSubscription = new IssueArchiveProductSubscription();
            UadDataAccess.ShimIssueArchiveProductSubscription.GetSqlCommand = (x) => CreateEntity();
            UadDataAccess.ShimIssueArchiveProductSubscription.GetListSqlCommand = (x) => CreateEntityList();
            KmCommonFake.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = (x) => CreateAdHocTable();
            UadDataAccess.ShimDataFunctions.GetClientSqlConnectionClient = (x) => new SqlConnection();
            KmCommonFake.ShimDataFunctions.ExecuteNonQuerySqlCommand = (x) => true;
            UadDataAccess.ShimDataFunctions.ExecuteScalarSqlCommand = (x) => TestOne;
            KmCommonFake.ShimDataFunctions.GetSqlConnectionString = (x) => new SqlConnection();
            ShimSqlConnection.AllInstances.Open = (p) => { };
            ShimSqlConnection.AllInstances.Close = (p) => { };
            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (p1, p2) => { };
            ShimSqlBulkCopy.AllInstances.Close = (p) => { };
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void SelectIssue_ReturnsObjectList()
        {
            var client = new KMPlatform.Object.ClientConnections();

            // Act
            var result = _issueArchiveProductSubscription.SelectIssue(IssueId, client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result[0].CreatedByUserID.ShouldBe(TestUser),
                () => result[0].AccountNumber.ShouldBe(TestAccountNumber));
        }

        [Test]
        public void SelectPaging_ReturnsObjectList()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var page = 1;
            var pageSize = 10;

            // Act
            var result = _issueArchiveProductSubscription.SelectPaging(page, pageSize, IssueId, client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result[0].CreatedByUserID.ShouldBe(TestUser),
                () => result[0].AccountNumber.ShouldBe(TestAccountNumber));
        }

        [Test]
        public void SelectForUpdate_ReturnsObjectList()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            var pubsubs = new List<int> { 1 };

            // Act
            var result = _issueArchiveProductSubscription.SelectForUpdate(ProductId, IssueId, pubsubs, client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result[0].CreatedByUserID.ShouldBe(TestUser),
                () => result[0].AccountNumber.ShouldBe(TestAccountNumber));
        }

        [Test]
        public void SelectCount_Success_ReturnsInt()
        {
            var client = new KMPlatform.Object.ClientConnections();

            // Act
            var result = _issueArchiveProductSubscription.SelectCount(IssueId, client);

            // Assert
            result.ShouldBe(TestOne);
        }

        [Test]
        public void SaveBulkSqlInsert_Success_ReturnsBoolean()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var entityListToSave = CreateEntityList();

            // Act
            var result = _issueArchiveProductSubscription.SaveBulkSqlInsert(entityListToSave, client);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void Save_Success_ReturnsInt()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var entityToSave = CreateEntityFromSubcscription();

            // Act
            var result = _issueArchiveProductSubscription.Save(entityToSave, client);

            // Assert
            result.ShouldBe(TestOne);
        }

        [Test]
        public void SaveAll_Success_ReturnsInt()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var entityToSave = CreateEntityFromSubcscription();

            // Act
            var result = _issueArchiveProductSubscription.SaveAll(entityToSave, client);

            // Assert
            result.ShouldBe(TestOne);
        }

        private static Entity.IssueArchiveProductSubscription CreateEntity()
        {
            return new Entity.IssueArchiveProductSubscription
            {
                CreatedByUserID = TestUser,
                AccountNumber = TestAccountNumber
            };
        }

        private static Entity.IssueArchiveProductSubscription CreateEntityFromSubcscription()
        {
            return new Entity.IssueArchiveProductSubscription(
                new Entity.ProductSubscription
                {
                    CreatedByUserID = TestUser,
                    AccountNumber = TestAccountNumber
                });
        }

        private static List<Entity.IssueArchiveProductSubscription> CreateEntityList()
        {
            return new List<Entity.IssueArchiveProductSubscription>
            {
                CreateEntity()
            };
        }

        private static DataTable CreateAdHocTable()
        {
            return new DataTable
            {
                Columns = { TestColumn },
                Rows = { { TestRow } }
            };
        }     
    }
}
