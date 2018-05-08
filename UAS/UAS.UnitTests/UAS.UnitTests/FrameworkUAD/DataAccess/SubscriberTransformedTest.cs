using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    ///     Unit tests for <see cref="SubscriberTransformed"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberTransformedTest
    {
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 25;
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.SubscriberTransformed> _list;
        private Entity.SubscriberTransformed _objWithRandomValues;
        private Entity.SubscriberTransformed _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private SqlConnection _connection;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private int _saveLogCount;
        private IDisposable _context;
        private List<Entity.SubscriberTransformed> _actualList;
        private List<Entity.SubscriberTransformed> _subscriberTransformeds;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _bulkCopyClosed = false;
            _saveLogCount = 0;
            _bulkCopyColumns = new Dictionary<string, string>();

            _subscriberTransformeds = new List<Entity.SubscriberTransformed>
            {
                new Entity.SubscriberTransformed()
                {
                    ProcessCode = ProcessCode,
                    SourceFileID = SourceFileId
                },
                new Entity.SubscriberTransformed(1, Guid.Empty, ProcessCode),
                typeof(Entity.SubscriberTransformed).CreateInstance()
            };

            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.SubscriberTransformed).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriberTransformed).CreateInstance();

            _list = new List<Entity.SubscriberTransformed>
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
        public void SaveBulkSqlInsert_WhenCalledWithData_WriteDataTableToTheServer()
        {
            // Arrange, Act
            SubscriberTransformed.SaveBulkSqlInsert(_subscriberTransformeds, Client);

            // Assert
            _bulkCopyClosed.ShouldBeTrue();
            _saveLogCount.ShouldBe(2);
            _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns);
            _bulkCopyColumns.Values.ToArray().ShouldBe(Columns);

            for (var index = 0; index < _actualList.Count; index++)
            {
                Entity.SubscriberTransformed subscriberTransformed = _actualList[index];
                subscriberTransformed.IsContentMatched(_subscriberTransformeds[index]);
            }
        }

        [Test]
        public void SaveBulkSqlInsert_WhenColumnLengthIsInvalid_ThrowException()
        {
            // Arrange
            int sourceFileId = 0;
            int fileStatusTypeId = 0;
            var processCode = string.Empty;

            ShimSqlBulkCopy.AllInstances.Close = bc => throw new Exception("Received an invalid column length from the bcp client for colid[1]");
            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                sourceFileId = fileLog.SourceFileID;
                fileStatusTypeId = fileLog.FileStatusTypeID;
                processCode = fileLog.ProcessCode;

                return true;
            };

            // Act
            Should.Throw<Exception>(() =>
                SubscriberTransformed.SaveBulkSqlInsert(_subscriberTransformeds, Client));

            // Assert
            sourceFileId.ShouldBe(-99);
            fileStatusTypeId.ShouldBe(-99);
            processCode.ShouldBe("SubscriberTransformed.SaveBulkSqlInsert");
        }

        [Test]
        public void SaveBulkSqlInsert_WhenThrowException_SaveFileLogAndThrowException()
        {
            // Arrange
            int sourceFileId = 0;
            int fileStatusTypeId = 0;
            var processCode = string.Empty;

            _connection = new ShimSqlConnection
            {
                Open = () => throw new Exception("unknown-exception")
            }.Instance;

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                sourceFileId = fileLog.SourceFileID;
                fileStatusTypeId = fileLog.FileStatusTypeID;
                processCode = fileLog.ProcessCode;

                return true;
            };

            // Act
            Should.Throw<Exception>(() =>
                SubscriberTransformed.SaveBulkSqlInsert(_subscriberTransformeds, Client));

            // Assert
            sourceFileId.ShouldBe(-99);
            fileStatusTypeId.ShouldBe(-99);
            processCode.ShouldBe("SubscriberTransformed.SaveBulkSqlInsert");
        }

        private static string[] Columns => new[]
        {
            "SubscriberTransformedID", "SORecordIdentifier", "SourceFileID", "PubCode", "Sequence", "FName", "LName",
            "Title", "Company", "Address", "MailStop", "City", "State", "Zip", "Plus4", "ForZip", "County", "Country",
            "CountryID", "Phone", "Fax", "Email", "CategoryID", "TransactionID", "TransactionDate", "QDate",
            "QSourceID", "RegCode", "Verified", "SubSrc", "OrigsSrc", "Par3C", "MailPermission", "FaxPermission",
            "PhonePermission", "OtherProductsPermission", "ThirdPartyPermission", "EmailRenewPermission",
            "TextPermission", "Source", "Priority", "Sic", "SicCode", "Gender", "Address3", "Home_Work_Address",
            "Demo7", "Mobile", "Latitude", "Longitude", "IsLatLonValid", "LatLonMsg", "EmailStatusID",
            "STRecordIdentifier", "DateCreated", "DateUpdated", "CreatedByUserID", "UpdatedByUserID", "ImportRowNumber",
            "ProcessCode", "IsActive", "ExternalKeyId", "AccountNumber", "EmailID", "Copies", "GraceIssues", "IsComp",
            "IsPaid", "IsSubscribed", "Occupation", "SubscriptionStatusID", "SubsrcID", "Website", "SubGenSubscriberID",
            "SubGenSubscriptionID", "SubGenPublicationID", "SubGenMailingAddressId", "SubGenBillingAddressId",
            "IssuesLeft", "UnearnedReveue", "SubGenIsLead", "SubGenRenewalCode", "SubGenSubscriptionRenewDate",
            "SubGenSubscriptionExpireDate", "SubGenSubscriptionLastQualifiedDate"
        };

        private void SetupFakes()
        {
            _connection = new ShimSqlConnection
            {
                DatabaseGet = () => "data-base"
            }.Instance;

            ShimDataFunctions.GetClientSqlConnectionClientConnections = client =>
            {
                client.ShouldBe(Client);

                return _connection;
            };

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bc, table) =>
            {
                bc.DestinationTableName.ShouldBe($"[{_connection.Database.ToString()}].[dbo].[SubscriberTransformed]");
                bc.BatchSize.ShouldBe(0);
                bc.BulkCopyTimeout.ShouldBe(0);

                foreach (SqlBulkCopyColumnMapping mapping in bc.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _actualList = table.ConvertDataTable<Entity.SubscriberTransformed>();
            };

            ShimSqlBulkCopy.AllInstances.Close = bc => { _bulkCopyClosed = true; };

            ShimFileLog.AllInstances.SaveFileLog = (log, fileLog) =>
            {
                fileLog.SourceFileID.ShouldBe(SourceFileId);
                fileLog.FileStatusTypeID.ShouldBe(-99);
                fileLog.ProcessCode.ShouldBe(ProcessCode);

                _saveLogCount++;

                return true;
            };

            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
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