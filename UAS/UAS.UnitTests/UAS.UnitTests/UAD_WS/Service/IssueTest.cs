using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityIssue = FrameworkUAD.Entity.Issue;
using EntityIssueCloseSubGenMap = FrameworkUAD.Entity.IssueCloseSubGenMap;
using ServiceIssue = UAD_WS.Service.Issue;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimIssue;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueTest : Fakes
    {
        private const int SampleProductId = 100;
        private const int SampleIssueId = 200;
        private const int SampleId = 300;
        private const int SavedCount = 1;

        private ServiceIssue _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceIssue();
        }

        [Test]
        public void ArchiveAll_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var imbDictionary = new Dictionary<int, string>();
            var compareDictionary = new Dictionary<int, string>();
            ShimWorker.AllInstances.ArchiveAllInt32Int32DictionaryOfInt32StringDictionaryOfInt32StringClientConnections
                = (a, b, c, d, e, f) => false;

            // Act
            var result = _testEntity.ArchiveAll(
                Guid.Empty,
                SampleProductId,
                SampleIssueId,
                imbDictionary,
                compareDictionary,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, false);
        }

        [Test]
        public void BulkInsertSubGenIDs_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityIssueCloseSubGenMap>();
            ShimWorker.AllInstances.BulkInsertSubGenIDsListOfIssueCloseSubGenMapClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.BulkInsertSubGenIDs(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void BulkInsertSubGenIDs_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssueCloseSubGenMap>();
            ShimWorker.AllInstances.BulkInsertSubGenIDsListOfIssueCloseSubGenMapClientConnections = (_, entities, ___) =>
            {
                entities.Add(new EntityIssueCloseSubGenMap());
                return true;
            };

            // Act
            var result = _testEntity.BulkInsertSubGenIDs(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void ValidateArchive_ByPubIdAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ValidateArchiveInt32Int32ClientConnections = (a, b, c, d) => true;

            // Act
            var result = _testEntity.ValidateArchive(Guid.Empty, SampleProductId, SampleIssueId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void RollBackIssue_ByPubIdAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.RollBackIssueInt32Int32Int32ClientConnections = (a, b, c, d, e) => true;

            // Act
            var result = _testEntity.RollBackIssue(Guid.Empty, SampleProductId, SampleIssueId, 0, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SelectForPublication_ByPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssue>();
            ShimWorker.AllInstances.SelectPublicationInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectForPublication(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForPublisher_ByPublisherId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssue>();
            ShimWorker.AllInstances.SelectPublisherInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectForPublisher(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityIssue>();
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
            var entity = new EntityIssue();
            ShimWorker.AllInstances.SaveIssueClientConnections = (a, b, c) => SavedCount;
            ShimForJsonFunction<EntityIssue>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, SavedCount);
        }
    }
}
