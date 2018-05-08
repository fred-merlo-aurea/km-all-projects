using System;
using System.Collections.Generic;
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

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    ///     Unit tests for <see cref="SubscriberInvalid"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberInvalidTest
    {
        private ClientConnections _client;
        private SqlConnection _connection;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private IDisposable _context;
        private List<Entity.SubscriberInvalid> _actualList;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _client = new ClientConnections();
            _bulkCopyClosed = false;
            _bulkCopyColumns = new Dictionary<string, string>();

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
            // Arrange
            List<Entity.SubscriberInvalid> subscriberInvalids = new List<Entity.SubscriberInvalid>
            {
                new Entity.SubscriberInvalid(new Entity.SubscriberTransformed()),
                new Entity.SubscriberInvalid(1, Guid.Empty, "process-code")
            };

            // Act
            SubscriberInvalid.SaveBulkSqlInsert(subscriberInvalids, _client);

            // Assert
            _bulkCopyClosed.ShouldBeTrue();
            _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns);
            _bulkCopyColumns.Values.ToArray().ShouldBe(Columns);

            for (var index = 0; index < _actualList.Count; index++)
            {
                Entity.SubscriberInvalid subscriberInvalid = _actualList[index];
                subscriberInvalid.IsContentMatched(subscriberInvalids[index]);
            }
        }

        [Test]
        public void SaveBulkSqlInsert_WhenColumnLengthIsInvalid_ThrowException()
        {
            // Arrange
            _connection = new ShimSqlConnection
            {
                DatabaseGet = () => "data-base",
                Close = () => throw new Exception("Received an invalid column length from the bcp client for colid[1]")
            }.Instance;

            // Act, Assert
            Should.Throw<Exception>(() =>
                SubscriberInvalid.SaveBulkSqlInsert(new List<Entity.SubscriberInvalid>(), _client));
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
                SubscriberInvalid.SaveBulkSqlInsert(new List<Entity.SubscriberInvalid>(), _client));

            // Assert
            sourceFileId.ShouldBe(-99);
            fileStatusTypeId.ShouldBe(-99);
            processCode.ShouldBe("SubscriberInvalid.SaveBulkSqlInsert");
        }

        private static string[] Columns => new[]
        {
            "SubscriberInvalidID", "SORecordIdentifier", "SourceFileID", "PubCode", "Sequence", "FName", "LName",
            "Title", "Company", "Address", "MailStop", "City", "State", "Zip", "Plus4", "ForZip", "County",
            "Country", "CountryID", "Phone", "Fax", "Email", "CategoryID", "TransactionID", "TransactionDate",
            "QDate", "QSourceID", "RegCode", "Verified", "SubSrc", "OrigsSrc", "Par3C", "MailPermission",
            "FaxPermission", "PhonePermission", "OtherProductsPermission", "ThirdPartyPermission",
            "EmailRenewPermission", "TextPermission", "Source", "Priority", "StatList", "Sic", "SicCode", "Gender",
            "Address3", "Home_Work_Address", "Demo7", "Mobile", "Latitude", "Longitude", "EmailStatusID",
            "SIRecordIdentifier", "DateCreated", "DateUpdated", "CreatedByUserID", "UpdatedByUserID",
            "ImportRowNumber", "ProcessCode", "IsActive", "ExternalKeyId", "AccountNumber", "EmailID", "Copies",
            "GraceIssues", "IsComp", "IsPaid", "IsSubscribed", "Occupation", "SubscriptionStatusID", "SubsrcID",
            "Website"
        };

        private void SetupFakes()
        {
            _connection = new ShimSqlConnection
            {
                DatabaseGet = () => "data-base"
            }.Instance;

            ShimDataFunctions.GetClientSqlConnectionClientConnections = client =>
            {
                client.ShouldBe(_client);

                return _connection;
            };

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (bc, table) =>
            {
                bc.DestinationTableName.ShouldBe($"[{_connection.Database.ToString()}].[dbo].[SubscriberInvalid]");
                bc.BatchSize.ShouldBe(0);
                bc.BulkCopyTimeout.ShouldBe(0);

                foreach (SqlBulkCopyColumnMapping mapping in bc.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _actualList = table.ConvertDataTable<Entity.SubscriberInvalid>();
            };

            ShimSqlBulkCopy.AllInstances.Close = bc => { _bulkCopyClosed = true; };
        }
    }
}