using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceSubscriberFinal = UAD_WS.Service.SubscriberFinal;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberFinal;
using EntitySubscriberFinal = FrameworkUAD.Entity.SubscriberFinal;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberFinalTest : Fakes
    {
        private const int SourceFileId = 100;
        private const string ProcessCode = "Code1";
        private const string FileType = "type1";
        private const string SampleSearch = "address1";
        private const int AffectedCountPositive = 1;

        private ServiceSubscriberFinal _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberFinal();
        }

        [Test]
        public void SaveDQMClean_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveDQMCleanClientConnectionsStringString = (a, b, c, d) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveDQMClean(Guid.Empty, new ClientConnections(), SourceFileId, ProcessCode, FileType);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void SaveDQMClean_BySourceFileIdAndProcessCodeAndFileType_ReturnsSuccessResponse()
        {
            // Arrange
            string parameterCode = null;
            string parameterType = null;
            ShimWorker.AllInstances.SaveDQMCleanClientConnectionsStringString = (_, __, code, type) =>
            {
                parameterCode = code;
                parameterType = type;
                return false;
            };

            // Act
            var result = _testEntity.SaveDQMClean(Guid.Empty, new ClientConnections(), SourceFileId, ProcessCode, FileType);

            // Assert
            VerifySuccessResponse(result, false);
            parameterCode.ShouldBe(ProcessCode);
            parameterType.ShouldBe(FileType);
        }

        [Test]
        public void SetOneMaster_WithAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SetOneMasterClientConnections = (_, __) => false;

            // Act
            var result = _testEntity.SetOneMaster(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SetMissingMaster_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SetMissingMasterClientConnections = (_, __) => false;

            // Act
            var result = _testEntity.SetMissingMaster(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void AddressSearch_ByAddressDetail_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.AddressSearchStringStringStringStringStringClientConnections = (a, b, c, d, e, f, g) => false;

            // Act
            var result = _testEntity.AddressSearch(
                Guid.Empty,
                SampleSearch,
                SampleSearch,
                SampleSearch,
                SampleSearch,
                SampleSearch,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SelectByAddressValidation_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsStringInt32BooleanBoolean =
                (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(Guid.Empty, new ClientConnections(), ProcessCode, SourceFileId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAddressValidation_ByLatLonValid_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsBoolean = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(Guid.Empty, new ClientConnections(), true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForFileAudit_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            ShimWorker.AllInstances.SelectForFileAuditStringInt32NullableOfDateTimeNullableOfDateTimeClientConnectionsBoolean =
                (a, b, c, d, e, f, g) => list;

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
        public void SaveBulkUpdate_ByEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal> { new EntitySubscriberFinal() };
            ShimWorker.AllInstances.SaveBulkUpdateIListOfSubscriberFinalClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.SaveBulkUpdate(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SaveBulkInsert_ByEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal> { new EntitySubscriberFinal() };
            ShimWorker.AllInstances.SaveBulkInsertIListOfSubscriberFinalClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.SaveBulkInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Save_ByEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberFinal();
            ShimWorker.AllInstances.SaveSubscriberFinalClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntitySubscriberFinal>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberFinal>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, ProcessCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
