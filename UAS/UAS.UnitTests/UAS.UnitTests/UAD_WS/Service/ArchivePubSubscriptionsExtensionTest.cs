using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityArchivePubSubscriptionsExtension = FrameworkUAD.Entity.ArchivePubSubscriptionsExtension;
using EntityPubSubscriptionAdHoc = FrameworkUAD.Object.PubSubscriptionAdHoc;
using ServiceArchivePubSubscriptionsExtension = UAD_WS.Service.ArchivePubSubscriptionsExtension;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimArchivePubSubscriptionsExtension;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ArchivePubSubscriptionsExtensionTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;

        private ServiceArchivePubSubscriptionsExtension _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceArchivePubSubscriptionsExtension();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityPubSubscriptionAdHoc>();
            ShimWorker.AllInstances.SaveListOfPubSubscriptionAdHocInt32Int32ClientConnections
                = (a, b, c, d, e) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Save(Guid.Empty, list, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityPubSubscriptionAdHoc>();
            ShimWorker.AllInstances.SaveListOfPubSubscriptionAdHocInt32Int32ClientConnections
                = (a, b, c, d, e) => true;

            // Act
            var result = _testEntity.Save(Guid.Empty, list, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SelectForUpdate_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityArchivePubSubscriptionsExtension>();
            ShimWorker.AllInstances.SelectForUpdateInt32Int32ListOfInt32ClientConnections
                = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectForUpdate(Guid.Empty, SampleId, SampleId, new List<int>(),  new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void GetArchiveAdhocs_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityPubSubscriptionAdHoc>();
            ShimWorker.AllInstances.GetArchiveAdhocsClientConnectionsInt32Int32Int32
                = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.GetArchiveAdhocs(Guid.Empty, SampleId, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
