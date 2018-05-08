using System;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntitySecurityGroup = KMPlatform.Entity.SecurityGroup;
using ServiceSecurityGroup = UAS_WS.Service.SecurityGroup;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimSecurityGroup;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SecurityGroupTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;
        private const int GroupId = 100;
        private const int UserId = 200;

        private ServiceSecurityGroup _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceSecurityGroup();
        }

        [Test]
        public void Save_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntitySecurityGroup();
            ShimWorker.AllInstances.SaveSecurityGroup = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, GroupId, UserId);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySecurityGroup();
            ShimWorker.AllInstances.SaveSecurityGroup = (_, entityToSave) => AffectedCountPositive;
            ShimForJsonFunction<EntitySecurityGroup>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, GroupId, UserId);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntitySecurityGroup();
            ShimWorker.AllInstances.SaveSecurityGroup = (_, entityToSave) => AffectedCountNegative;

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, GroupId, UserId);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void SecurityGroupNameExists_ByNameAndGroupId_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.SecurityGroupNameExists(Guid.Empty, string.Empty, GroupId);

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void Select_ByUserIdAndClientId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntitySecurityGroup();
            ShimWorker.AllInstances.SelectInt32Int32BooleanBoolean = (a, b, c, d, e) => entity;

            // Act
            var result = _testEntity.Select(Guid.Empty, UserId, GroupId, true);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SelectActiveForClientGroup_ByClientGroupId_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.SelectActiveForClientGroup(Guid.Empty, GroupId);

            // Assert
            VerifyErrorResponse(result, null);
        }
    }
}
