using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceSubscriberOriginal = UAD_WS.Service.SubscriberOriginal;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberOriginal;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntitySubscriberOriginal = FrameworkUAD.Entity.SubscriberOriginal;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberOriginalTest : Fakes
    {
        private const string ProcessCode = "code11";
        private const int SourceFileId = 100;
        private const int SavedCount = 1;

        private ServiceSubscriberOriginal _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberOriginal();
        }

        [Test]
        public void SaveBulkUpdate_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SaveBulkUpdateListOfSubscriberOriginalClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkUpdate(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SaveBulkUpdate_ByEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SaveBulkUpdateListOfSubscriberOriginalClientConnections = (_, __, ___) => false;

            // Act
            var result = _testEntity.SaveBulkUpdate(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SaveBulkSqlInsert_ByEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = (_, __, ___) => false;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SaveBulkInsert_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SaveBulkInsertListOfSubscriberOriginalClientConnections = (a, entities, c) => false;

            // Act
            var result = _testEntity.SaveBulkInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SelectStringClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, ProcessCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SelectStringInt32ClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, ProcessCode, SourceFileId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SelectInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SourceFileId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForFileAudit_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberOriginal>();
            ShimWorker.AllInstances.SelectForFileAuditStringInt32NullableOfDateTimeNullableOfDateTimeClientConnections
                = (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectForFileAudit(
                Guid.Empty,
                ProcessCode,
                SourceFileId,
                DateTime.MinValue,
                DateTime.MaxValue,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberOriginal();
            ShimWorker.AllInstances.SaveSubscriberOriginalClientConnections = (a, b, c) => SavedCount;
            ShimForJsonFunction<EntitySubscriberOriginal>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, SavedCount);
        }
    }
}
