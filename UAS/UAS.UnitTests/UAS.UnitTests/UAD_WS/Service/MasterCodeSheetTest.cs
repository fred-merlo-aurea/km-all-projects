using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using ServiceMasterCodeSheet = UAD_WS.Service.MasterCodeSheet;
using ShimWorkerMasterCodeSheet = FrameworkUAD.BusinessLogic.Fakes.ShimMasterCodeSheet;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using EntityMasterCodeSheet = FrameworkUAD.Entity.MasterCodeSheet;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MasterCodeSheetTest : Fakes
    {
        private const int MasterGroupId = 100;
        private const int MasterId = 200;
        private const int ImportedCount = 2;
        private const int SavedCount = 1;
        private const int SavedCountForError = -1;

        private ServiceMasterCodeSheet _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceMasterCodeSheet();
        }

        [Test]
        public void Select_IfInternalWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorkerMasterCodeSheet.AllInstances.SelectClientConnections = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Select_ByClient_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityMasterCodeSheet>();
            ShimWorkerMasterCodeSheet.AllInstances.SelectClientConnections = (_, __) =>
            {
                list.Add(new EntityMasterCodeSheet());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void SelectMasterGroupID_ByMasterGroupId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityMasterCodeSheet>();
            var parameterGroupId = 0;
            ShimWorkerMasterCodeSheet.AllInstances.SelectMasterGroupIDClientConnectionsInt32 = (_, __, groupId) =>
            {
                parameterGroupId = groupId;
                list.Add(new EntityMasterCodeSheet());
                return list;
            };

            // Act
            var result = _testEntity.SelectMasterGroupID(Guid.Empty, new ClientConnections(), MasterGroupId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
            parameterGroupId.ShouldBe(MasterGroupId);
        }

        [Test]
        public void Save_IfWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityMasterCodeSheet();
            EntityMasterCodeSheet parameterEntity = null;
            ShimWorkerMasterCodeSheet.AllInstances.SaveMasterCodeSheetClientConnections = (_, entityToSave, __) =>
            {
                parameterEntity = entityToSave;
                return SavedCount;
            };

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            parameterEntity.ShouldBeSameAs(entity);
        }

        [Test]
        public void Save_IfWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityMasterCodeSheet();
            ShimWorkerMasterCodeSheet.AllInstances.SaveMasterCodeSheetClientConnections = (_, entityToSave, __) => SavedCountForError;

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBe(SavedCountForError);
        }

        [Test]
        public void ImportSubscriber_ByMasterId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterMasterId = 0;
            var doc = new XDocument();
            ShimWorkerMasterCodeSheet.AllInstances.ImportSubscriberInt32XDocumentClientConnections = (_, id, __, ___) =>
            {
                parameterMasterId = id;
                return ImportedCount;
            };

            // Act
            var result = _testEntity.ImportSubscriber(Guid.Empty, MasterId, doc, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(ImportedCount);
            parameterMasterId.ShouldBe(MasterId);
        }

        [Test]
        public void DeleteMasterID_ByMasterId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterMasterId = 0;
            ShimWorkerMasterCodeSheet.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, id) =>
            {
                parameterMasterId = id;
                return false;
            };

            // Act
            var result = _testEntity.DeleteMasterID(Guid.Empty, MasterId, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeFalse();
            parameterMasterId.ShouldBe(MasterId);
        }
    }
}
