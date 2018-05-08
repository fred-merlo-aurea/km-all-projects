using System;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceCodeSheetMasterCodeSheetBridge = UAD_WS.Service.CodeSheetMasterCodeSheetBridge;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimCodeSheetMasterCodeSheetBridge;
using EntityCodeSheetMasterCodeSheetBridge = FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CodeSheetMasterCodeSheetBridgeTest : Fakes
    {
        private const int CodeSheetId = 100;
        private const int MasterId = 200;
        private const int AffectedCount = 1;

        private ServiceCodeSheetMasterCodeSheetBridge _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceCodeSheetMasterCodeSheetBridge();
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityCodeSheetMasterCodeSheetBridge();
            ShimWorker.AllInstances.SaveCodeSheetMasterCodeSheetBridgeClientConnections = (_, __, ___) => AffectedCount;
            ShimForJsonFunction<EntityCodeSheetMasterCodeSheetBridge>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCount);
        }

        [Test]
        public void DeleteCodeSheetID_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteCodeSheetID(Guid.Empty, CodeSheetId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void DeleteCodeSheetID_ByCodeSheetId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterCodeSheetId = 0;
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, id) =>
            {
                parameterCodeSheetId = id;
                return true;
            };

            // Act
            var result = _testEntity.DeleteCodeSheetID(Guid.Empty, CodeSheetId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterCodeSheetId.ShouldBe(CodeSheetId);
        }

        [Test]
        public void DeleteMasterID_ByMasterId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, id) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.DeleteMasterID(Guid.Empty, MasterId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(MasterId);
        }
    }
}
