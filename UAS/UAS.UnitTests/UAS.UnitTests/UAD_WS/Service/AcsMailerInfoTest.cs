using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityAcsMailerInfo = FrameworkUAD.Entity.AcsMailerInfo;
using ServiceAcsMailerInfo = UAD_WS.Service.AcsMailerInfo;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimAcsMailerInfo;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AcsMailerInfoTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;

        private ServiceAcsMailerInfo _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAcsMailerInfo();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAcsMailerInfo();
            ShimWorker.AllInstances.SaveAcsMailerInfoClientConnections = (a, b, c) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityAcsMailerInfo>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAcsMailerInfo();
            ShimWorker.AllInstances.SaveAcsMailerInfoClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityAcsMailerInfo>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAcsMailerInfo>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectByID_ByEntityId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAcsMailerInfo();
            ShimWorker.AllInstances.SelectByIDInt32ClientConnections = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectByID(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, entity);
        }
    }
}
