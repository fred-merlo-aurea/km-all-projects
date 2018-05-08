using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using NUnit.Framework;
using UAS.UnitTests.UAS_WS.Service.Common;
using EntityProfile = KMPlatform.Entity.Profile;
using ServiceProfile = UAS_WS.Service.Profile;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimProfile;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProfileTest : Fakes
    {
        private const string SampleString = "field1";
        private const int SampleId = 10;
        private const int AffectedCountPositive = 2;
        private const int AffectedCountNegative = -1;

        private ServiceProfile _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceProfile();
        }

        [Test]
        public void Search_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SearchStringStringString = (a, b, c, d) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Search(Guid.Empty, SampleString, SampleString, SampleString);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Select_ByStringValues_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SearchStringStringString = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Search(Guid.Empty, SampleString, SampleString, SampleString);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForProfile_ByProfileId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProfile();
            ShimWorker.AllInstances.SelectInt32 = (a, b) => entity;

            // Act
            var result = _testEntity.SelectForProfile(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SelectForPublication_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SelectPublicationInt32 = (a, b) => list;

            // Act
            var result = _testEntity.SelectForPublication(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForPublisher_ByPublisherId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SelectPublisherInt32 = (a, b) => list;

            // Act
            var result = _testEntity.SelectForPublisher(Guid.Empty, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForPublicationSubscribed_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SelectPublicationSubscribedInt32Boolean = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectForPublicationSubscribed(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForPublicationProspect_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SelectPublicationProspectInt32Boolean = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectForPublicationProspect(Guid.Empty, SampleId, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SelectPublicationInt32BooleanBoolean = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, true, true);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void BindPublicationList_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProfile();
            ShimWorker.AllInstances.BindPublicationListProfile = (a, b) => entity;
            ShimForJsonFunction<EntityProfile>();

            // Act
            var result = _testEntity.BindPublicationList(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Search_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProfile>();
            ShimWorker.AllInstances.SearchOf1StringListOfM0<EntityProfile>((a, b, c) => list);

            // Act
            var result = _testEntity.Search(Guid.Empty, SampleString, list);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProfile();
            ShimWorker.AllInstances.SaveProfile = (a, b) => AffectedCountPositive;
            ShimForJsonFunction<EntityProfile>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void Save_WorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityProfile();
            ShimWorker.AllInstances.SaveProfile = (a, b) => AffectedCountNegative;
            ShimForJsonFunction<EntityProfile>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, AffectedCountNegative);
        }
    }
}
