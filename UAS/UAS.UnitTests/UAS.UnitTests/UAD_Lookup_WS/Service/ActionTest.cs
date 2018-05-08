using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityAction = FrameworkUAD_Lookup.Entity.Action;
using ServiceAction = UAD_Lookup_WS.Service.Action;
using ShimWorker = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimAction;

namespace UAS.UnitTests.UAD_Lookup_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActionTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int AffectedCountNegative = -1;

        private ServiceAction _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceAction();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAction();
            ShimWorker.AllInstances.SaveAction = (_, __) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityAction>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAction();
            ShimWorker.AllInstances.SaveAction = (_, entityToSave) => AffectedCountPositive;
            ShimForJsonFunction<EntityAction>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityAction();
            ShimWorker.AllInstances.SaveAction = (_, entityToSave) => AffectedCountNegative;
            ShimForJsonFunction<EntityAction>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }

        [Test]
        public void Validate_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityAction();
            ShimWorker.AllInstances.ValidateAction = (_, __) => true;

            // Act
            var result = _testEntity.Validate(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityAction>();
            ShimWorker.AllInstances.Select = _ => list;

            // Act
            var result = _testEntity.Select(Guid.Empty);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
