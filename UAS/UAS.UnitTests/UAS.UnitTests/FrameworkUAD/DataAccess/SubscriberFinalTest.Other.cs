using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KM.Common.Data;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using EntityObject = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using DataAccessFakes = FrameworkUAS.DataAccess.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="SubscriberFinal"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberFinalTests
    {
        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.SubscriberFinal> _list;
        private Entity.SubscriberFinal _objWithRandomValues;
        private Entity.SubscriberFinal _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.SubscriberFinal)
                .CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriberFinal)
                .CreateInstance();

            _list = new List<Entity.SubscriberFinal>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SelectByAddressValidation_WhenCalledWithValidLatLon_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SelectByAddressValidation(Client, ProcessCode, SourceFileId, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => _sqlCommand.Parameters["@IsLatLonValid"].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidationSourceId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByAddressValidation_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SelectByAddressValidation(Client, IsLatLonValid);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@IsLatLonValid"].Value.ShouldBe(IsLatLonValid),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByAddressValidation),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithProcessCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.Select(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProcessCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForFileAudit_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = SubscriberFinal.SelectForFileAudit(ProcessCode, SourceFileId, startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => _sqlCommand.Parameters["@StartDate"].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters["@EndDate"].Value.ShouldBe(endDate),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForFileAudit),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForFieldUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SelectForFieldUpdate(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForFieldUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForIgnoredReport_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SelectForIgnoredReport(ProcessCode, IsIgnored, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@isIgnored"].Value.ShouldBe(IsIgnored),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForIgnoredReport),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForProcessedReport_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SelectForProcessedReport(ProcessCode, IsIgnored, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@isIgnored"].Value.ShouldBe(IsIgnored),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForIgnoredReport),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectRecordIdentifiers_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var recordIdentifiers = new List<EntityObject.RecordIdentifier>
            {
                typeof(EntityObject.RecordIdentifier)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return recordIdentifiers.GetSqlDataReader();
            };

            // Act
            var result = SubscriberFinal.SelectRecordIdentifiers(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectRecordIdentifiers),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(recordIdentifiers).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectResultCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var resultCounts = new List<EntityObject.AdmsResultCount>
            {
                typeof(EntityObject.AdmsResultCount).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return resultCounts.GetSqlDataReader();
            };

            // Act
            var result = SubscriberFinal.SelectResultCount(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectResultCount),
                () => result.ShouldNotBeNull(),
                () => result.IsContentMatched(resultCounts.First()).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectResultCountAfterProcessToLive_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var resultCounts = new List<EntityObject.AdmsResultCount>
            {
                typeof(EntityObject.AdmsResultCount).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return resultCounts.GetSqlDataReader();
            };

            // Act
            var result = SubscriberFinal.SelectResultCountAfterProcessToLive(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectResultCountAfterProcessToLive),
                () => result.ShouldNotBeNull(),
                () => result.IsContentMatched(resultCounts.First()).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SaveBulkUpdate(Xml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkInsert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SaveBulkInsert(Xml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveDQMClean_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SaveDQMClean(Client, ProcessCode, FileType);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@FileType"].Value.ShouldBe(FileType),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveDqmClean),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void NullifyKMPSGroupEmails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.NullifyKMPSGroupEmails(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcNullifyKmpsGroupEmails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SetMissingMaster_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SetMissingMaster(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSetMissingMaster),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SetOneMaster_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.SetOneMaster(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSetOneMaster),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void AddressSearch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.AddressSearch(Address, Mailstop, City, State, Zip, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Address"].Value.ShouldBe(Address),
                () => _sqlCommand.Parameters["@Mailstop"].Value.ShouldBe(Mailstop),
                () => _sqlCommand.Parameters["@City"].Value.ShouldBe(City),
                () => _sqlCommand.Parameters["@State"].Value.ShouldBe(State),
                () => _sqlCommand.Parameters["@Zip"].Value.ShouldBe(Zip),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcAddressSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SetPubCode_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            SubscriberFinal.SetPubCode(Client, ProcessCode, ProductId, BrandId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["ProductId"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["BrandId"].Value.ShouldBe(BrandId),
                () => _sqlCommand.CommandText.ShouldBe(ProcSetPubCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetEmailListFromEcn_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberFinal.GetEmailListFromEcn(GroupId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@groupID"].Value.ShouldBe(GroupId),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetEmailListFromEcn),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ECN_ThirdPartySuppresion_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var xml = new StringBuilder(Xml);

            // Arrange, Act
            SubscriberFinal.ECN_ThirdPartySuppresion(xml, ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(xml.ToString()),
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcEcnThirdPartySuppresion),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ECN_OtherProductsSuppression_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var xml = new StringBuilder(Xml);

            // Act
            SubscriberFinal.ECN_OtherProductsSuppression(xml, ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(xml.ToString()),
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcEcnOtherProductsSuppression),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            DataAccessFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommandString = (cmd, connString) =>
            {
                _sqlCommand = cmd;
                connString.ShouldBe(ConnectionString.ECN_Communicator.ToString());
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}