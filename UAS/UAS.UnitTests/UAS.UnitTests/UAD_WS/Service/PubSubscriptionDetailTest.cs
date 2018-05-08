using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityProductSubscriptionDetail = FrameworkUAD.Entity.ProductSubscriptionDetail;
using ServicePubSubscriptionDetail = UAD_WS.Service.PubSubscriptionDetail;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimProductSubscriptionDetail;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PubSubscriptionDetailTest : Fakes
    {
        private const int SampleId = 100;
        private const int SampleSize = 200;
        private const int AffectedCountPositive = 1;

        private ServicePubSubscriptionDetail _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServicePubSubscriptionDetail();
        }

        [Test]
        public void DeleteCodeSheetID_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.DeleteCodeSheetID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void DeleteCodeSheetID_ByCodeSheetId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => true;

            // Act
            var result = _testEntity.DeleteCodeSheetID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Select_ByPubSubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscriptionDetail>();
            ShimWorker.AllInstances.SelectInt32ClientConnections = (_, __, ___) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SaveBulkUpdate_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscriptionDetail>();
            ShimWorker.AllInstances.ProductSubscriptionDetailUpdateBulkSqlClientConnectionsListOfProductSubscriptionDetail
                = (_, __, ___) => list;

            // Act
            var result = _testEntity.SaveBulkUpdate(Guid.Empty, new ClientConnections(), list);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectCount_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectCountInt32ClientConnections = (_, __, ___) => AffectedCountPositive;

            // Act
            var result = _testEntity.SelectCount(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectPaging_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscriptionDetail>();
            ShimWorker.AllInstances.SelectPagingInt32Int32Int32ClientConnections
                = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectPaging(
                Guid.Empty,
                SampleId,
                SampleSize,
                SampleId,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
