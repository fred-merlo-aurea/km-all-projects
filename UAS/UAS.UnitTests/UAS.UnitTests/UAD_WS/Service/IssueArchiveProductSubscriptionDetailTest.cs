using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityIssueArchiveProductSubscriptionDetail = FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail;
using ServiceIssueArchiveProductSubscriptionDetail = UAD_WS.Service.IssueArchiveProductSubscriptionDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimIssueArchiveProductSubscriptionDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueArchiveProductSubscriptionDetailTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceIssueArchiveProductSubscriptionDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceIssueArchiveProductSubscriptionDetail();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityIssueArchiveProductSubscriptionDetail();
            ShimWorker.AllInstances.SaveIssueArchiveProductSubscriptionDetailClientConnections = (a, b, c) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Save_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityIssueArchiveProductSubscriptionDetail();
            EntityIssueArchiveProductSubscriptionDetail parameterEntity = null;
            ShimWorker.AllInstances.SaveIssueArchiveProductSubscriptionDetailClientConnections = (a, entityToSave, c) =>
            {
                parameterEntity = entityToSave;
                return true;
            };

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterEntity.ShouldBeSameAs(entity);
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscriptionDetail>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfIssueArchiveProductSubscriptionDetailClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SelectForUpdate_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscriptionDetail>();
            ShimWorker.AllInstances.SelectForUpdateInt32Int32ListOfInt32ClientConnections = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectForUpdate(Guid.Empty, SampleId, SampleId, new List<int>(), new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SaveBulkUpdate_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueArchiveProductSubscriptionDetail>();
            ShimWorker.AllInstances.IssueArchiveProductSubscriptionDetailUpdateBulkSqlClientConnectionsListOfIssueArchiveProductSubscriptionDetail
                = (a, b, c) => list;

            // Act
            var result = _testEntity.SaveBulkUpdate(Guid.Empty, new ClientConnections(), list);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
