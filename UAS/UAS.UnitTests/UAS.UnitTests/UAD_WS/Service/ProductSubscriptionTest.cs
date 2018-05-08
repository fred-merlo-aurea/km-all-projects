using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityActionProductSubscription = FrameworkUAD.Entity.ActionProductSubscription;
using EntityCopiesProductSubscription = FrameworkUAD.Entity.CopiesProductSubscription;
using EntityProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using EntityPubSubscriptionAdHoc = FrameworkUAD.Object.PubSubscriptionAdHoc;
using ServiceProductSubscription = UAD_WS.Service.ProductSubscription;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimProductSubscription;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductSubscriptionTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;
        private const string SampleName = "name1";
        private const string SampleString = "sample2";
        private const int PageIndex = 1;
        private const int PageSize = 10;

        private ServiceProductSubscription _testEntity;
        private DataTable _dataTable;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceProductSubscription();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeContext();
            _dataTable?.Dispose();
        }

        [Test]
        public void Save_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityProductSubscription();
            ShimWorker.AllInstances.SaveProductSubscriptionClientConnections = (a, b, c) => throw new InvalidOperationException();
            ShimForJsonFunction<EntityProductSubscription>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProductSubscription();
            ShimWorker.AllInstances.SaveProductSubscriptionClientConnections = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityProductSubscription>();

            // Act
            var result = _testEntity.Save(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectProductSubscription_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityProductSubscription();
            ShimWorker.AllInstances.SelectProductSubscriptionInt32ClientConnectionsString = (a, b, c, d) => entity;

            // Act
            var result = _testEntity.SelectProductSubscription(Guid.Empty, SampleId, new ClientConnections(), SampleName);

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void SaveBulkActionIDUpdate_WithXml_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveBulkActionIDUpdateStringClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.SaveBulkActionIDUpdate(Guid.Empty, SampleString, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void UpdateRequesterFlags_ByProductIdAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.Update_Requester_FlagsInt32Int32ClientConnections = (a, b, c, d) => true;

            // Act
            var result = _testEntity.UpdateRequesterFlags(Guid.Empty, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SelectCount_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectCountInt32ClientConnections = (a, b, c) => AffectedCountPositive;

            // Act
            var result = _testEntity.SelectCount(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectPaging_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SelectPagingInt32Int32Int32ClientConnectionsString
                = (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectPaging(
                Guid.Empty,
                PageIndex,
                PageSize,
                SampleId,
                new ClientConnections(),
                SampleName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectPublication_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SelectPublicationInt32ClientConnectionsString = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectPublication(Guid.Empty, SampleId, new ClientConnections(), SampleName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Search_ByName_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SearchClientConnectionsStringStringStringStringStringStringStringStringStringStringStringStringInt32StringInt32Int32Int32
                = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s) => list;

            // Act
            var result = _testEntity.Search(Guid.Empty, new ClientConnections(), SampleName);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SearchSuggestMatch_ByPublisherId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SearchSuggestMatchClientConnectionsInt32Int32StringStringString
                = (a, b, c, d, e, f, g) => list;

            // Act
            var result = _testEntity.SearchSuggestMatch(Guid.Empty, new ClientConnections(), SampleId, SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void UpdateLock_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateLockInt32BooleanInt32ClientConnections = (a, b, c, d, e) => AffectedCountPositive;

            // Act
            var result = _testEntity.UpdateLock(Guid.Empty, SampleId, true, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectSequence_BySequenceId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SelectSequenceInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectSequence(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void UpdateQDate_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.UpdateQDateInt32NullableOfDateTimeInt32ClientConnections = (a, b, c, d, e) => AffectedCountPositive;

            // Act
            var result = _testEntity.UpdateQDate(Guid.Empty, SampleId, DateTime.MinValue, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SearchAddressZip_ByAddress_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SearchAddressZipStringStringClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SearchAddressZip(Guid.Empty, SampleString, SampleString, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SaveBulkWaveMailing_WithXml_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SaveBulkWaveMailingStringInt32ClientConnections = (a, b, c, d) => true;

            // Act
            var result = _testEntity.SaveBulkWaveMailing(Guid.Empty, SampleString, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void ClearWaveMailingInfo_ByWaveMailingId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ClearWaveMailingInfoInt32ClientConnections = (a, b, c) => true;

            // Act
            var result = _testEntity.ClearWaveMailingInfo(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void SelectActionSubscription_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityActionProductSubscription>();
            ShimWorker.AllInstances.SelectProductIDInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectActionSubscription(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectArchiveActionSubscription_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityActionProductSubscription>();
            ShimWorker.AllInstances.SelectProductIDInt32Int32ClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.SelectArchiveActionSubscription(Guid.Empty, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForExport_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            _dataTable = new DataTable();
            ShimWorker.AllInstances.Select_For_ExportInt32Int32StringInt32ClientConnections = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectForExport(Guid.Empty, 0, 10, SampleString, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectForExportStatic_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            _dataTable = new DataTable();
            ShimWorker.AllInstances.Select_For_Export_StaticInt32StringListOfInt32ClientConnections = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.SelectForExportStatic(Guid.Empty, SampleId, SampleString, new List<int>(), new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectForExportStatic_ByProductIdAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            _dataTable = new DataTable();
            ShimWorker.AllInstances.Select_For_Export_StaticInt32Int32StringListOfInt32ClientConnections = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectForExportStatic(Guid.Empty, SampleId, SampleId, SampleString, new List<int>(), new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectAllActiveIDs_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityCopiesProductSubscription>();
            ShimWorker.AllInstances.SelectAllActiveIDsInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectAllActiveIDs(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Get_AdHocs_PubSubscription_ByPubId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityPubSubscriptionAdHoc>();
            ShimWorker.AllInstances.Get_AdHocsInt32Int32ClientConnections = (a, b, c, d) => list;

            // Act
            var result = _testEntity.Get_AdHocs_PubSubscription(Guid.Empty, SampleId, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Get_AdHocs_ByPubId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<string>();
            ShimWorker.AllInstances.Get_AdHocsInt32ClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.Get_AdHocs(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectForUpdate_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SelectForUpdateInt32Int32ListOfInt32ClientConnections = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.SelectForUpdate(Guid.Empty, SampleId, SampleId, new List<int>(), new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void RecordUpdate_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.RecordUpdateHashSetOfInt32StringInt32Int32Int32ClientConnections
                = (a, b, c, d, e, f, g) => true;

            // Act
            var result = _testEntity.RecordUpdate(
                Guid.Empty,
                new HashSet<int>(),
                SampleString,
                SampleId,
                SampleId,
                SampleId,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, true);
        }

        [Test]
        public void Select_BySubscriptionId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityProductSubscription>();
            ShimWorker.AllInstances.SelectInt32ClientConnectionsStringBoolean = (a, b, c, d, e) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }
    }
}
