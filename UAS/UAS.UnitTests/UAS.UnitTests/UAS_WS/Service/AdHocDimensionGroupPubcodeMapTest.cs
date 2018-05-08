using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityAdHocDimensionGroupPubCodeMap = FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap;
using ServiceAdhocDimensionGroupPubCodeMap = UAS_WS.Service.AdHocDimensionGroupPubcodeMap;
using ShimWorker = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimensionGroupPubcodeMap;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdHocDimensionGroupPubcodeMapTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceAdhocDimensionGroupPubCodeMap _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAdhocDimensionGroupPubCodeMap();
        }

        [Test]
        public void SaveBulkSqlInsert_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroupPubCodeMap>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroupPubcodeMap = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.NewGuid(), list);

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SaveBulkSqlInsert_ByClientIdAndUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroupPubCodeMap>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroupPubcodeMap = (_, __) => false;

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.NewGuid(), list);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAdHocDimensionGroupPubCodeMap>();
            ShimWorker.AllInstances.SelectInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.NewGuid(), SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAdHocDimensionGroupPubCodeMap();
            ShimWorker.AllInstances.SaveAdHocDimensionGroupPubcodeMap = (_, __) => true;
            ShimForJsonFunction<EntityAdHocDimensionGroupPubCodeMap>();

            // Act
            var result = _testEntity.Save(Guid.NewGuid(), entity);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
