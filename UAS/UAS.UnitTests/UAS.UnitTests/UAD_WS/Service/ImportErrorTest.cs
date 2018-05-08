using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KM.Common;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityImportError = FrameworkUAD.Entity.ImportError;
using ServiceImportError = UAD_WS.Service.ImportError;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimImportError;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImportErrorTest : Fakes
    {
        private const int SampleId = 100;
        private const int SavedCount = 1;
        private const string SampleString = "name1";

        private ServiceImportError _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceImportError();
        }

        [Test]
        public void SaveBulkSqlInsert_IfWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var list = new List<EntityImportError>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfImportErrorClientConnections = (_, __, ___) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, false, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntityList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityImportError>();
            ShimWorker.AllInstances.SaveBulkSqlInsertListOfImportErrorClientConnections = (_, __, ___) =>
            {
                list.Add(new EntityImportError());
                return true;
            };

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, list, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void SaveBulkSqlInsert_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityImportError();
            EntityImportError parameterEntity = null;
            ShimWorker.AllInstances.SaveBulkSqlInsertImportErrorClientConnections = (_, entityToSave, ___) =>
            {
                parameterEntity = entityToSave;
                return true;
            };

            // Act
            var result = _testEntity.SaveBulkSqlInsert(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
            parameterEntity.ShouldBeSameAs(entity);
        }

        [Test]
        public void Select_ByProcessCodeAndSourceFileId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityImportError>();
            ShimWorker.AllInstances.SelectStringInt32ClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleString, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityImportError();
            ShimWorker.AllInstances.SaveImportErrorClientConnections = (a, b, c) => SavedCount;
            ShimForJsonFunction<EntityImportError>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, SavedCount);
        }
    }
}
