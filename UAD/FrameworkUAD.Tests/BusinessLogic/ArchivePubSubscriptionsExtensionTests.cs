using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.BusinessLogic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using KmCommonFake = KM.Common.Fakes;
using UadDataAccess = FrameworkUAD.DataAccess.Fakes;

namespace FrameworkUAD.Tests.BusinessLogic
{
    /// <summary>
    /// Unit test for <see cref="ArchivePubSubscriptionsExtension"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ArchivePubSubscriptionsExtensionTests
    {
        private const string TestColumn = "TestColumn";
        private const string TestRow = "TestRow";
        private const string TestField1 = "TestField1";
        private const int TestUser = 1;
        private const int TestOne = 1;
        private const int ProductId = 1;
        private const int IssueId = 1;
        private static readonly DateTime _testDate = new DateTime(2018,1,1);
        private ArchivePubSubscriptionsExtension _archivePubSubscriptionsExtension;
        private IDisposable _shimContext;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _archivePubSubscriptionsExtension = new ArchivePubSubscriptionsExtension();
            UadDataAccess.ShimArchivePubSubscriptionsExtension.GetSqlCommand = (x) => CreateEntity();
            UadDataAccess.ShimArchivePubSubscriptionsExtension.GetListSqlCommand = (x) => CreateEntityList();
            KmCommonFake.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = (x) => CreateAdHocTable();
            UadDataAccess.ShimDataFunctions.GetClientSqlConnectionClient = (x) => new SqlConnection();
            KmCommonFake.ShimDataFunctions.ExecuteNonQuerySqlCommand = (x) => true;
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void SelectForUpdate_ReturnsObjectList()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var pubsubs = new List<int> { 1 };

            // Act
            var result = _archivePubSubscriptionsExtension.SelectForUpdate(ProductId, IssueId, pubsubs, client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result[0].CreatedByUserID.ShouldBe(TestUser),
                () => result[0].UpdatedByUserID.ShouldBe(TestUser),
                () => result[0].IssueArchiveSubscriptionID.ShouldBe(TestOne),
                () => result[0].DateUpdated.ShouldBe(_testDate),
                () => SatisfyFields(result[0]).ShouldBeTrue());
        }

        [Test]
        public void GetArchiveAdhocs_ReturnsObjectList()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var pubSubID = 1;

            // Act
            var result = _archivePubSubscriptionsExtension.GetArchiveAdhocs(client, pubSubID, ProductId, IssueId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result[0].AdHocField.ShouldBe(TestColumn),
                () => result[0].Value.ShouldBe(TestRow));
        }

        [Test]
        public void Save_Success_ReturnsBoolean()
        {
            var client = new KMPlatform.Object.ClientConnections();
            var pubSubscriptionAdHoc = new List<Object.PubSubscriptionAdHoc> { new Object.PubSubscriptionAdHoc { } };
            var issueArchiveSubscriptionID = 1;
            var pubID = 1;

            // Act
            var result = _archivePubSubscriptionsExtension.Save(pubSubscriptionAdHoc, issueArchiveSubscriptionID, pubID, client);

            // Assert
            result.ShouldBeTrue();
        }
        private static bool SatisfyFields(Entity.ArchivePubSubscriptionsExtension entity)
        {
            var privateObject = new PrivateObject(entity);
            for (int i = 1; i <= 100; i++)
            {
                if ((string)privateObject.GetProperty("Field" + i.ToString()) != i.ToString())
                {
                    return false;
                }
            }
            return true;
        }

        private static Entity.ArchivePubSubscriptionsExtension CreateEntity()
        {
            var entity = new Entity.ArchivePubSubscriptionsExtension
            {              
                 CreatedByUserID = TestUser,
                 UpdatedByUserID = TestUser,
                 Field1 = TestField1,
                 IssueArchiveSubscriptionID = TestOne,
                 DateCreated  = _testDate,
                 DateUpdated = _testDate
            };
            var privateObject = new PrivateObject(entity);
            for (int i = 1; i <= 100; i++)
            {
                privateObject.SetProperty("Field" + i.ToString(), i.ToString());
            }
            return entity;
        }

        private static List<Entity.ArchivePubSubscriptionsExtension> CreateEntityList()
        {
            return new List<Entity.ArchivePubSubscriptionsExtension>
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
