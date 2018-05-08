using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityCodeSheet = FrameworkUAD.Entity.CodeSheet;
using ServiceCodeSheet = UAD_WS.Service.CodeSheet;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimCodeSheet;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodeSheetTest : Fakes
    {
        private const string SampleCode = "code1";
        private const int SampleId = 111;
        private const int AffectedCountPositive = 1;

        private ServiceCodeSheet _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceCodeSheet();
        }

        [Test]
        public void CodeSheetValidation_Delete_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeSheetValidation_DeleteInt32StringClientConnections = (a, b, c, d) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.CodeSheetValidation_Delete(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void CodeSheetValidation_Delete_WithFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            var parameterCode = string.Empty;
            ShimWorker.AllInstances.CodeSheetValidation_DeleteInt32StringClientConnections = (_, id, code, __) =>
            {
                parameterId = id;
                parameterCode = code;
                return false;
            };

            // Act
            var result = _testEntity.CodeSheetValidation_Delete(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(SampleId);
            parameterCode.ShouldBe(SampleCode);
        }

        [Test]
        public void DeleteCodeSheetID_WithCodeSheetId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, id) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.DeleteCodeSheetID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(SampleId);
        }

        [Test]
        public void CodeSheetValidation_WithSourceFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.CodeSheetValidationInt32StringClientConnections = (a, b, c, d) => false;

            // Act
            var result = _testEntity.CodeSheetValidation(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void FileValidator_CodeSheetValidation_WithSourceFileIdAndProcessCode_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.FileValidator_CodeSheetValidationInt32StringClientConnections = (a, b, c, d) => false;

            // Act
            var result = _testEntity.FileValidator_CodeSheetValidation(Guid.Empty, SampleId, SampleCode, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Save_ByEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCodeSheet();
            ShimWorker.AllInstances.SaveCodeSheetClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityCodeSheet>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCodeSheet>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByPubId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCodeSheet>();
            ShimWorker.AllInstances.SelectInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
