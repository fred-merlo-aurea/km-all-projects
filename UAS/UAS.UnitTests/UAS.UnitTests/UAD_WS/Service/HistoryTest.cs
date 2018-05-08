using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityHistory = FrameworkUAD.Entity.History;
using ShimWorkerHistory = FrameworkUAD.BusinessLogic.Fakes.ShimHistory;
using ServiceHistory = UAD_WS.Service.History;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryTest : Fakes
    {
        private const int BatchId = 100;
        private const int UserId = 200;
        private const int PublicationId = 300;
        private const int HistoryId = 400;
        private const int LogId = 500;
        private const int MapId = 600;

        private ServiceHistory _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceHistory();
        }

        [Test]
        public void Select_IfInternalWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorkerHistory.AllInstances.SelectClientConnections = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Select_ByClient_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityHistory>();
            ShimWorkerHistory.AllInstances.SelectClientConnections = (_, __) =>
            {
                list.Add(new EntityHistory());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void Select_ByBatchId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterBatchId = 0;
            var list = new List<EntityHistory>();
            ShimWorkerHistory.AllInstances.SelectInt32ClientConnections = (_, id, __) =>
            {
                parameterBatchId = id;
                list.Add(new EntityHistory());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, BatchId, new ClientConnections());

            // Assert
            parameterBatchId.ShouldBe(BatchId);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void Select_ByStartDateAndEndDate_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityHistory>();
            var startDate = DateTime.Now;
            var endDate = startDate.AddSeconds(1);
            var parameterEndDate = DateTime.MinValue;
            ShimWorkerHistory.AllInstances.SelectDateTimeDateTimeClientConnections = (a, b, end, d) =>
            {
                parameterEndDate = end;
                list.Add(new EntityHistory());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, startDate, endDate, new ClientConnections());

            // Assert
            parameterEndDate.ShouldBe(endDate);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void Select_ByUserIdAndPublicationId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityHistory>();
            var parameterUserId = 0;
            var parameterPublicationId = 0;
            ShimWorkerHistory.AllInstances.SelectBatchInt32Int32ClientConnections = (a, user, publication, d) =>
            {
                parameterUserId = user;
                parameterPublicationId = publication;
                list.Add(new EntityHistory());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, UserId, PublicationId, new ClientConnections());

            // Assert
            parameterUserId.ShouldBe(UserId);
            parameterPublicationId.ShouldBe(PublicationId);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void UserLogList_ByHistoryId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            var parameterHistoryId = 0;
            ShimWorkerHistory.AllInstances.UserLogListInt32ClientConnections = (a, history, d) =>
            {
                parameterHistoryId = history;
                list.Add(0);
                return list;
            };

            // Act
            var result = _testEntity.UserLogList(Guid.Empty, HistoryId, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void HistoryResponseList_ByHistoryId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            var parameterHistoryId = 0;
            ShimWorkerHistory.AllInstances.HistoryResponseListInt32ClientConnections = (a, history, d) =>
            {
                parameterHistoryId = history;
                list.Add(0);
                return list;
            };

            // Act
            var result = _testEntity.HistoryResponseList(Guid.Empty, HistoryId, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void HistoryMarketingMapList_ByHistoryId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            var parameterHistoryId = 0;
            ShimWorkerHistory.AllInstances.HistoryMarketingMapListInt32ClientConnections = (a, history, d) =>
            {
                parameterHistoryId = history;
                list.Add(0);
                return list;
            };

            // Act
            var result = _testEntity.HistoryMarketingMapList(Guid.Empty, HistoryId, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void AddHistoryEntry_ByProperties_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterBatchId = 0;
            var parameterUserId = 0;
            ShimWorkerHistory.AllInstances.AddHistoryEntryClientConnectionsInt32Int32Int32Int32Int32Int32Int32Int32Int32ListOfInt32ListOfInt32ListOfInt32
                = (history, connections, batch, a, b, c, d, e, f, user, h, i, j, k) => 
            {
                parameterBatchId = batch;
                parameterUserId = user;
                return new EntityHistory();
            };

            // Act
            var result = _testEntity
                .AddHistoryEntry(Guid.Empty, new ClientConnections(), BatchId, 0, 0, 0, 0, 0, 0, UserId);

            // Assert
            parameterBatchId.ShouldBe(BatchId);
            parameterUserId.ShouldBe(UserId);
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void InsertHistoryToUserLog_ByHistoryAndLog_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterHistoryId = 0;
            var parameterLogId = 0;
            ShimWorkerHistory.AllInstances.Insert_History_To_UserLogInt32Int32ClientConnections
                = (history, id, log, connections) => 
            {
                parameterHistoryId = id;
                parameterLogId = log;
                return true;
            };

            // Act
            var result = _testEntity.Insert_History_To_UserLog(Guid.Empty, HistoryId, LogId, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            parameterLogId.ShouldBe(LogId);
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void InsertHistoryToHistoryMarketingMapList_ByHistoryIdAndMapList_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterHistoryId = 0;
            var list = new List<FrameworkUADEntity.HistoryMarketingMap>();
            ShimWorkerHistory.AllInstances.Insert_History_To_HistoryMarketingMap_ListInt32ListOfHistoryMarketingMapClientConnections
                = (history, id, _, __) => 
            {
                parameterHistoryId = id;
                return true;
            };

            // Act
            var result = _testEntity.Insert_History_To_HistoryMarketingMap_List(Guid.Empty, HistoryId, list, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void InsertHistoryToHistoryMarketingMap_ByHistoryIdAndMapId_ReturnsSuccessResponse()
        {
            // Arrange
            var parameterHistoryId = 0;
            var parameterMapId = 0;
            ShimWorkerHistory.AllInstances.Insert_History_To_HistoryMarketingMapInt32Int32ClientConnections
                = (history, id, map, __) => 
            {
                parameterHistoryId = id;
                parameterMapId = map;
                return true;
            };

            // Act
            var result = _testEntity.Insert_History_To_HistoryMarketingMap(Guid.Empty, HistoryId, MapId, new ClientConnections());

            // Assert
            parameterHistoryId.ShouldBe(HistoryId);
            parameterMapId.ShouldBe(MapId);
            result.Status.ShouldBe(ResponseStatus.Success);
        }

        [Test]
        public void Save_ByHistory_ReturnsSuccessResponse()
        {
            // Arrange
            var history = new EntityHistory();
            EntityHistory parameterHistory = null;
            ShimWorkerHistory.AllInstances.SaveHistoryClientConnections = (_, entity, __) => 
            {
                parameterHistory = entity;
                return 0;
            };

            // Act
            var result = _testEntity.Save(Guid.Empty, history, new ClientConnections());

            // Assert
            parameterHistory.ShouldBeSameAs(history);
            result.Status.ShouldBe(ResponseStatus.Success);
        }
    }
}
