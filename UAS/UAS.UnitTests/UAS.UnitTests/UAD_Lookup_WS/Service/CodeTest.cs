using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAD_Lookup_WS.Fakes;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityCode = FrameworkUAD_Lookup.Entity.Code;
using EnumCodeType = FrameworkUAD_Lookup.Enums.CodeType;
using ServiceCode = UAD_Lookup_WS.Service.Code;
using ShimWorker = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode;

namespace UAS.UnitTests.UAD_Lookup_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodeTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;
        private const int SampleId = 100;

        private ServiceCode _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceCode();
            ShimServiceBase.GetCallingIp = () => string.Empty;
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SaveCode = (_, __) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityCode>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SaveCode = (_, entityToSave) => AffectedCountPositive;
            ShimForJsonFunction<EntityCode>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SaveCode = (_, entityToSave) => AffectedCountNegative;
            ShimForJsonFunction<EntityCode>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCode>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByCodeTypeId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCode>();
            ShimWorker.AllInstances.SelectInt32 = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByCodeType_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCode>();
            ShimWorker.AllInstances.SelectEnumsCodeType = (_, __) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, EnumCodeType.ACS_Type);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForDemographicAttribute_ByCodeType_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCode>();
            ShimWorker.AllInstances.SelectForDemographicAttributeEnumsCodeTypeInt32String = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectForDemographicAttribute(
                Guid.Empty,
                EnumCodeType.ACS_Type,
                SampleId,
                string.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectChildren_ByParentId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCode>();
            ShimWorker.AllInstances.SelectChildrenInt32 = (_, __) => list;

            // Act
            var result = _testEntity.SelectChildren(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectCodeId_ByCodeId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SelectCodeIdInt32 = (_, __) => entity;

            // Act
            var result = _testEntity.SelectCodeId(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SelectCodeName_ByCodeName_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SelectCodeNameEnumsCodeTypeString = (_, __, ___) => entity;

            // Act
            var result = _testEntity.SelectCodeName(Guid.Empty, EnumCodeType.ACS_Type, string.Empty);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SelectCodeValue_ByCodeValue_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCode();
            ShimWorker.AllInstances.SelectCodeValueEnumsCodeTypeString = (_, __, ___) => entity;

            // Act
            var result = _testEntity.SelectCodeValue(Guid.Empty, EnumCodeType.ACS_Type, string.Empty);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void CodeExist_ByCodeTypeId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeExistStringInt32 = (_, __, ___) => true;

            // Act
            var result = _testEntity.CodeExist(Guid.Empty, string.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void CodeExist_ByCodeType_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeExistStringEnumsCodeType = (_, __, ___) => true;

            // Act
            var result = _testEntity.CodeExist(Guid.Empty, string.Empty, EnumCodeType.Filter_Type);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void CodeValueExist_ByCodeTypeId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeValueExistStringInt32 = (_, __, ___) => true;

            // Act
            var result = _testEntity.CodeValueExist(Guid.Empty, string.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void CodeValueExist_ByCodeType_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeValueExistStringEnumsCodeType = (_, __, ___) => true;

            // Act
            var result = _testEntity.CodeValueExist(Guid.Empty, string.Empty, EnumCodeType.Filter_Type);

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
