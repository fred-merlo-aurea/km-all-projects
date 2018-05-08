using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityMenu = KMPlatform.Entity.Menu;
using ServiceMenu = UAS_WS.Service.Menu;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimMenu;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MenuTest : Fakes
    {
        private const int SampleId = 100;

        private ServiceMenu _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceMenu();
        }

        [Test]
        public void Select_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectInt32BooleanBoolean = (a, b, c, d) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, true);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Select_ByGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityMenu>();
            ShimWorker.AllInstances.SelectInt32BooleanBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForApplication_ByApplicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityMenu>();
            ShimWorker.AllInstances.SelectForApplicationInt32BooleanBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectForApplication(Guid.Empty, SampleId, true, true);

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
