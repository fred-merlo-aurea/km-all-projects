using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntitySubscriberAddKill = FrameworkUAD.Entity.SubscriberAddKill;
using EntitySubscriberAddKillDetail = FrameworkUAD.Entity.SubscriberAddKillDetail;
using ServiceSubscriberAddKill = UAD_WS.Service.SubscriberAddKill;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimSubscriberAddKill;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberAddKillTest : Fakes
    {
        private const int ProductId = 100;
        private const int SampleId = 200;
        private const int AffectedCountPositive = 1;
        private const string SampleString = "123";

        private ServiceSubscriberAddKill _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSubscriberAddKill();
        }

        [Test]
        public void ClearDetails_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ClearDetailsInt32ClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.ClearDetails(Guid.Empty, ProductId, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void ClearDetails_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.ClearDetailsInt32ClientConnections = (_, id, ___) =>
            {
                parameterId = id;
                return true;
            };

            // Act
            var result = _testEntity.ClearDetails(Guid.Empty, ProductId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterId.ShouldBe(ProductId);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberAddKill>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySubscriberAddKill();
            ShimWorker.AllInstances.SaveSubscriberAddKillClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntitySubscriberAddKill>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void UpdateSubscription_WithEntityDetails_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateSubscriptionInt32Int32StringBooleanClientConnections
                = (a, b, c, d, e, f) => AffectedCountPositive;

            // Act
            var result = _testEntity.UpdateSubscription(
                Guid.Empty,
                SampleId,
                SampleId,
                SampleString,
                true,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void BulkInsertDetails_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntitySubscriberAddKillDetail>();
            ShimWorker.AllInstances.BulkInsertDetailListOfSubscriberAddKillDetailInt32ClientConnections
                = (a, b, c, d) => true;

            // Act
            var result = _testEntity.BulkInsertDetails(Guid.Empty, list, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }
    }
}
