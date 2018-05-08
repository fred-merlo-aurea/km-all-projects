using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityIssueArchiveProductSubscription = FrameworkUAD.Entity.IssueArchiveProductSubscription;
using ServiceIssueArchiveProductSubscription = UAD_WS.Service.IssueArchiveProductSubscription;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimIssueArchiveProductSubscription;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueArchiveProductSubscriptionTest : Fakes
    {
        private const int AffectedCountPositive = 2;
        private const int SampleId = 100;
        private const int SampleSize = 200;

        private ServiceIssueArchiveProductSubscription _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceIssueArchiveProductSubscription();
        }

        [Test]
        public void SaveAll_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveAllIssueArchiveProductSubscriptionClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveAll(Guid.Empty, null, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SaveAll_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityIssueArchiveProductSubscription();
            ShimWorker.AllInstances.SaveAllIssueArchiveProductSubscriptionClientConnections = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.SaveAll(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscription>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfIssueArchiveProductSubscriptionClientConnections
                = (_, __, ___) => true;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityIssueArchiveProductSubscription();
            ShimWorker.AllInstances.SaveIssueArchiveProductSubscriptionClientConnections = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectIssue_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscription>();
            ShimWorker.AllInstances.SelectIssueInt32ClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectIssue(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectPaging_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscription>();
            ShimWorker.AllInstances.SelectPagingInt32Int32Int32ClientConnections = (a, b, d, e, f) => list;

            // Act
            var result = _testEntity.SelectPaging(
                Guid.Empty,
                SampleId,
                SampleSize,
                SampleId,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForUpdate_ByProductIdAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscription>();
            ShimWorker.AllInstances.SelectForUpdateInt32Int32ListOfInt32ClientConnections = (a, b, d, e, f) => list;

            // Act
            var result = _testEntity.SelectForUpdate(
                Guid.Empty,
                SampleId,
                SampleId,
                new List<int>(), 
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectCount_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectCountInt32ClientConnections = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.SelectCount(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
