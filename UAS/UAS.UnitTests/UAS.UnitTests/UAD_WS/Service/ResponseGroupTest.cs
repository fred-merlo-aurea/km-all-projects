using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceResponseGroup = UAD_WS.Service.ResponseGroup;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimResponseGroup;
using EntityResponseGroup = FrameworkUAD.Entity.ResponseGroup;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ResponseGroupTest : Fakes
    {
        private const int GroupId = 100;
        private const int AffectedCountPositive = 1;

        private ServiceResponseGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceResponseGroup();
        }

        [Test]
        public void Delete_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.Delete(Guid.Empty, GroupId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Copy_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterId = 0;
            ShimWorker.AllInstances.CopyClientConnectionsInt32String = (a, b, id, d) =>
            {
                parameterId = id;
                return false;
            };

            // Act
            var result = _testEntity.Copy(Guid.Empty, GroupId, string.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
            parameterId.ShouldBe(GroupId);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityResponseGroup>();
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
            var list = new List<EntityResponseGroup>();
            ShimWorker.AllInstances.SelectInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections(), GroupId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityResponseGroup();
            ShimWorker.AllInstances.SaveResponseGroupClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityResponseGroup>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }
    }
}
