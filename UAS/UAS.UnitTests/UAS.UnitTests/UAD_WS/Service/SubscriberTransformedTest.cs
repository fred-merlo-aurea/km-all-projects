using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntitySubscriberTransformed = FrameworkUAD.Entity.SubscriberTransformed;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using ServiceSubscriberTransformed = UAD_WS.Service.SubscriberTransformed;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberTransformed;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberTransformedTest : Fakes
    {
        private const string SampleProcessCode = "code22";
        private const int SampleFileId = 100;
        private const int SamplePageIndex = 200;
        private const int SamplePageSize = 300;
        private const int SampleCount = 400;
        private const int AffectedCountPositive = 10;

        private ServiceSubscriberTransformed _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberTransformed();
        }

        [Test]
        public void Select_ByAccessKeyAndWorkerReturnsNull_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => null;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null);
        }

        [Test]
        public void Select_ByAccessKeyAndWorkerThrows_ReturnsErrorResponseWithFriendlyMessage()
        {
            // Arrange
            ShimWorker.AllInstances.SelectClientConnections = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
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
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectStringClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleProcessCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForFileAudit_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectForFileAuditStringInt32NullableOfDateTimeNullableOfDateTimeClientConnections
                = (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectForFileAudit(Guid.Empty, SampleProcessCode, SampleFileId, DateTime.MinValue, DateTime.MaxValue, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAddressValidation_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsStringBoolean
                = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(Guid.Empty, new ClientConnections(), SampleProcessCode, false);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAddressValidation_ByGeoNotValid_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsBoolean
                = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(Guid.Empty, new ClientConnections(), false);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForGeoCoding_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectForGeoCodingClientConnections = (_, __) => list;

            // Act
            var result = _testEntity.SelectForGeoCoding(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForGeoCoding_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectForGeoCodingClientConnectionsInt32 = (_, __, ___) => list;

            // Act
            var result = _testEntity.SelectForGeoCoding(Guid.Empty, new ClientConnections(), SampleFileId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void AddressValidation_Paging_ByPageIndex_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.AddressValidation_PagingInt32Int32StringClientConnectionsBooleanInt32
                = (a, b, c, d, e, f, g) => list;

            // Act
            var result = _testEntity.AddressValidation_Paging(Guid.Empty, SamplePageIndex, SamplePageSize, SampleProcessCode, new ClientConnections(), false, SampleFileId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void CountAddressValidation_LatLonValid_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CountAddressValidationClientConnectionsBoolean = (a, b, c) => AffectedCountPositive;

            // Act
            var result = _testEntity.CountAddressValidation(Guid.Empty, new ClientConnections(), true);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void CountForGeoCoding_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CountForGeoCodingClientConnections = (_, __) => SampleCount;

            // Act
            var result = _testEntity.CountForGeoCoding(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, SampleCount);
        }

        [Test]
        public void CountForGeoCoding_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CountForGeoCodingClientConnectionsInt32 = (_, __, ___) => SampleCount;

            // Act
            var result = _testEntity.CountForGeoCoding(Guid.Empty, new ClientConnections(), SampleFileId);

            // Assert
            VerifySuccessResponse(result, SampleCount);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberTransformed();
            ShimWorker.AllInstances.SaveSubscriberTransformedClientConnections = (_, __, ___) => SampleCount;
            ShimForJsonFunction<EntitySubscriberTransformed>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, SampleCount);
        }

        [Test]
        public void SaveBulkInsert_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SaveBulkInsertListOfSubscriberTransformedClientConnections = (_, __, ___) => false;

            // Act
            var result = _testEntity.SaveBulkInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntitiesAndDataCompare_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfSubscriberTransformedClientConnectionsBoolean = (a, b, c, d) => false;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections(), true);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void AddressValidateExisting_BySourceFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.AddressValidateExistingClientConnectionsInt32String = (a, b, c, d) => false;

            // Act
            var result = _testEntity.AddressValidateExisting(Guid.Empty, new ClientConnections(), SampleFileId, SampleProcessCode);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void DataMatching_BySourceFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DataMatchingClientConnectionsInt32String = (a, b, c, d) => false;

            // Act
            var result = _testEntity.DataMatching(Guid.Empty, new ClientConnections(), SampleFileId, SampleProcessCode);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void EnableIndexes_WithAccessId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.EnableIndexes(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void AddressUpdateBulkSql_WithEntities_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.AddressUpdateBulkSqlListOfSubscriberTransformedClientConnections = (_, __, ___) => false;

            // Act
            var result = _testEntity.AddressUpdateBulkSql(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void StandardRollUpToMaster_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.StandardRollUpToMasterClientConnectionsInt32String = (a, b, c, d) => false;

            // Act
            var result = _testEntity.StandardRollUpToMaster(Guid.Empty, new ClientConnections(), SampleFileId, SampleProcessCode);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void DisableIndexes_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.DisableIndexes(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void SelectTopOne_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberTransformed();
            ShimWorker.AllInstances.SelectTopOneStringClientConnections = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectTopOne(Guid.Empty, SampleProcessCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Select_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectClientConnectionsInt32 = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections(), SampleFileId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAddressValidation_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsInt32Boolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(Guid.Empty, new ClientConnections(), SampleFileId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByAddressValidation_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberTransformed>();
            ShimWorker.AllInstances.SelectByAddressValidationClientConnectionsStringInt32Boolean
                = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectByAddressValidation(
                Guid.Empty,
                new ClientConnections(),
                SampleProcessCode,
                SampleFileId,
                true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void CountAddressValidation_BySourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CountAddressValidationClientConnectionsInt32Boolean
                = (a, b, c, d) => AffectedCountPositive;

            // Act
            var result = _testEntity.CountAddressValidation(Guid.Empty, new ClientConnections(), SampleFileId, true);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void CountAddressValidation_ByProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CountAddressValidationClientConnectionsStringBoolean
                = (a, b, c, d) => AffectedCountPositive;

            // Act
            var result = _testEntity.CountAddressValidation(Guid.Empty, new ClientConnections(), SampleProcessCode, true);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
