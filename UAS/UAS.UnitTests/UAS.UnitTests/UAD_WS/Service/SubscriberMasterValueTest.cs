using System;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceSubscriberMasterValue = UAD_WS.Service.SubscriberMasterValue;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberMasterValue;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberMasterValueTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceSubscriberMasterValue _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberMasterValue();
        }

        [Test]
        public void DeleteMasterID_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteMasterID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void DeleteMasterID_ByMasterId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, ___) => true;

            // Act
            var result = _testEntity.DeleteMasterID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
