using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceSubscriberArchive = UAD_WS.Service.SubscriberArchive;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberArchive;
using EntitySubscriberArchive = FrameworkUAD.Entity.SubscriberArchive;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberArchiveTest : Fakes
    {
        private const int AffectedCount = 1;
        private const string SampleString = "code1";
        private const int SampleId = 100;

        private ServiceSubscriberArchive _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberArchive();
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberArchive();
            ShimWorker.AllInstances.SaveSubscriberArchiveClientConnections = (_, __, ___) => AffectedCount;
            ShimForJsonFunction<EntitySubscriberArchive>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCount);
        }

        [Test]
        public void SaveBulkInsert_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveBulkInsertListOfSubscriberArchiveClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkInsert(Guid.Empty, null, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void SaveBulkInsert_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberArchive>();
            ShimWorker.AllInstances.SaveBulkInsertListOfSubscriberArchiveClientConnections = (_, entities, __) =>
            {
                entities.Add(new EntitySubscriberArchive());
                return false;
            };

            // Act
            var result = _testEntity.SaveBulkInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            list.ShouldNotBeEmpty();
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberArchive>();
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberArchive>();
            ShimWorker.AllInstances.SelectStringClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleString, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForFileAudit_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberArchive>();
            ShimWorker.AllInstances.SelectForFileAuditStringInt32NullableOfDateTimeNullableOfDateTimeClientConnections
                = (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectForFileAudit(
                Guid.Empty,
                SampleString,
                SampleId,
                DateTime.MinValue,
                DateTime.MaxValue,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
