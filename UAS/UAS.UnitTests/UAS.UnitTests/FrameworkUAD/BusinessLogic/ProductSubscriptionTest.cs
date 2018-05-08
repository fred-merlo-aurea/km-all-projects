using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Entities = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage()]
    public partial class ProductSubscriptionTest
    {
        private const string SomeResponse = "SomeResponse";
        private const string SaveResponsesMethodName = "SaveResponses";

        private IDisposable _shimContext;
        private ProductSubscription _testEntity;
        private PrivateObject _privateTestObject;
        private bool _isUserLoggedSaved;
        private bool _isHistoryReponseMapSaved;

        [SetUp]
        public void SetUp()
        {
            _shimContext = ShimsContext.Create();
            _testEntity = new ProductSubscription();
            _privateTestObject = new PrivateObject(_testEntity);
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }

        [Test]
        public void SaveResponses_WhenOnlyChangedList_SavesResponseAndReturnsUserLogId()
        {
            // Arrange
            var productSubDetails = GetProductSubscriptionDetails();
            var answers = new List<Entities.ProductSubscriptionDetail>
            {
                productSubDetails
            };
            var client = new Client();
            InitializeFakesForSaveResponses();
            
            // Act
            var userlogList = _privateTestObject.Invoke(SaveResponsesMethodName, answers, 1, 1, 1, 1, 1, client, 1) as List<int>;

            // Assert
            userlogList.ShouldSatisfyAllConditions(
                () => userlogList.ShouldNotBeNull(),
                () => userlogList.ShouldNotBeEmpty(),
                () => userlogList.Count.ShouldBe(1),
                () => userlogList.ShouldBe(new int[] { 1 }),
                () => _isHistoryReponseMapSaved.ShouldBeTrue(),
                () => _isUserLoggedSaved.ShouldBeTrue());
        }

        [Test]
        public void SaveResponses_WhenOnlyRemovedAndAddedList_SavesResponseAndReturnsUserLogId()
        {
            // Arrange
            var currentProduct = GetProductSubscriptionDetails();
            currentProduct.CodeSheetID = 2;
            var answers = new List<Entities.ProductSubscriptionDetail>
            {
                currentProduct
            };
            var originalProduct = GetProductSubscriptionDetails();
            originalProduct.CodeSheetID = 3;
            originalProduct.ResponseOther = SomeResponse;
            ShimProductSubscriptionDetail.AllInstances.SelectInt32ClientConnections = (p, i, c) => new List<Entities.ProductSubscriptionDetail>
            {
                originalProduct
            };
            var client = new Client();
            InitializeFakesForSaveResponses();

            // Act
            var userlogList = _privateTestObject.Invoke(SaveResponsesMethodName, answers, 1, 1, 1, 1, 1, client, 1) as List<int>;

            // Assert
            userlogList.ShouldSatisfyAllConditions(
                () => userlogList.ShouldNotBeNull(),
                () => userlogList.ShouldNotBeEmpty(),
                () => userlogList.Count.ShouldBe(1),
                () => userlogList.ShouldBe(new int[] { 1 }),
                () => _isHistoryReponseMapSaved.ShouldBeTrue(),
                () => _isUserLoggedSaved.ShouldBeTrue());
        }

        private void InitializeFakesForSaveResponses()
        {
            _isHistoryReponseMapSaved = false;
            _isUserLoggedSaved = false;
            var originalProduct = GetProductSubscriptionDetails();
            originalProduct.CodeSheetID = 1;
            originalProduct.ResponseOther = SomeResponse;

            ShimProductSubscriptionDetail.AllInstances.SelectInt32ClientConnections = (p, i, c) => new List<Entities.ProductSubscriptionDetail>
            {
                originalProduct
            };
            ShimProductSubscriptionDetail.AllInstances.ProductSubscriptionDetailUpdateBulkSqlClientConnectionsListOfProductSubscriptionDetail =
                (p, conn, q) => new List<Entities.ProductSubscriptionDetail>
                {
                    originalProduct
                };

            ShimCodeSheet.AllInstances.SelectInt32ClientConnections = (q, a, s) => new List<Entities.CodeSheet>
            {
                new Entities.CodeSheet
                {
                    CodeSheetID = 1,
                    PubID = 1,
                    IsActive = true,
                    ResponseGroupID = 1
                }
            };
            ShimResponseGroup.AllInstances.SelectInt32ClientConnections = (t, u, v) => new List<Entities.ResponseGroup>
            {
                new Entities.ResponseGroup
                {
                    ResponseGroupID = 1,
                    IsActive = true,
                    PubID = 1,
                    IsMultipleValue = false
                }
            };
            ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (c, e, s) => new FrameworkUAD_Lookup.Entity.Code
            {
                CodeId = 1
            };
            ShimUserLog.AllInstances.SaveBulkInsertListOfUserLogClient = (u, p, q) =>
            {
                _isUserLoggedSaved = true;
                return new List<UserLog>
                {
                    new UserLog
                    {
                        ClientID = 1,
                        ApplicationID = 1,
                        UserID = 1,
                        UserLogID = 1,
                    }
                };
            };
            ShimHistoryResponseMap.AllInstances.SaveBulkUpdateListOfHistoryResponseMapClientConnections = (h, mp, conn) =>
            {
                _isHistoryReponseMapSaved = true;
                return new List<Entities.HistoryResponseMap>
                {
                    new Entities.HistoryResponseMap
                    {
                        CodeSheetID = 1,
                        HistoryResponseMapID = 1,
                        HistorySubscriptionID = 1
                    }
                };
            };
        }

        private static Entities.ProductSubscriptionDetail GetProductSubscriptionDetails()
        {
            return new Entities.ProductSubscriptionDetail
            {
                CodeSheetID = 1,
                PubSubscriptionDetailID = 1,
                PubSubscriptionID = 1,
                SubscriptionID = 1
            };
        }
    }
}
