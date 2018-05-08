using System;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceActionBackUp = UAD_WS.Service.ActionBackUp;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimActionBackUp;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActionBackUpTest : Fakes
    {
        private const int ProductId = 10;

        private ServiceActionBackUp _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceActionBackUp();
        }

        [Test]
        public void Bulk_Insert_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.Bulk_InsertInt32ClientConnections = (a, b, c) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Bulk_Insert(Guid.Empty, ProductId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Bulk_Insert_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.Bulk_InsertInt32ClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.Bulk_Insert(Guid.Empty, ProductId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Restore_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.RestoreInt32ClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.Restore(Guid.Empty, ProductId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
