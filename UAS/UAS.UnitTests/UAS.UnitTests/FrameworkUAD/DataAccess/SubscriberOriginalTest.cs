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
    ///     Unit tests for <see cref="SubscriberOriginal"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberOriginalTest
    {
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 25;

        private ClientConnections _client;
        private SqlConnection _connection;
        private Dictionary<string, string> _bulkCopyColumns;
        private bool _bulkCopyClosed;
        private int _saveLogCount;
        private IDisposable _context;
        private List<Entity.SubscriberOriginal> _actualList;
        private List<Entity.SubscriberOriginal> _subscriberOriginals;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _client = new ClientConnections();
            _bulkCopyClosed = false;
            _saveLogCount = 0;
            _bulkCopyColumns = new Dictionary<string, string>();

            _subscriberOriginals = new List<Entity.SubscriberOriginal>
            {
                new Entity.SubscriberOriginal()
                {
                    ProcessCode = ProcessCode,
                    SourceFileID = SourceFileId
                },
                new Entity.SubscriberOriginal(1, 1, ProcessCode),
                typeof(Entity.SubscriberOriginal).CreateInstance()
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
            SubscriberOriginal.SaveBulkSqlInsert(_subscriberOriginals, _client);

            // Assert
            _bulkCopyClosed.ShouldBeTrue();
            _saveLogCount.ShouldBe(2);
            _bulkCopyColumns.Keys.ToArray().ShouldBe(Columns);
            _bulkCopyColumns.Values.ToArray().ShouldBe(Columns);

            for (var index = 0; index < _actualList.Count; index++)
            {
                Entity.SubscriberOriginal subscriberOriginal = _actualList[index];
                subscriberOriginal.IsContentMatched(_subscriberOriginals[index]);
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
                SubscriberOriginal.SaveBulkSqlInsert(_subscriberOriginals, _client));
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
                SubscriberOriginal.SaveBulkSqlInsert(_subscriberOriginals, _client));

            // Assert
            sourceFileId.ShouldBe(-99);
            fileStatusTypeId.ShouldBe(-99);
            processCode.ShouldBe("SubscriberOriginal.SaveBulkSqlInsert");
        }

        private static string[] Columns => new[]
        {
            "SubscriberOriginalID", "SourceFileID", "PubCode", "Sequence", "FName", "LName", "Title", "Company",
            "Address", "MailStop", "City", "State", "Zip", "Plus4", "ForZip", "County", "Country", "CountryID", "Phone",
            "Fax", "Email", "CategoryID", "TransactionID", "TransactionDate", "QDate", "QSourceID", "RegCode",
            "Verified", "SubSrc", "OrigsSrc", "Par3C", "MailPermission", "FaxPermission", "PhonePermission",
            "OtherProductsPermission", "ThirdPartyPermission", "EmailRenewPermission", "TextPermission", "Source",
            "Priority", "Sic", "SicCode", "Gender", "Address3", "Home_Work_Address", "Demo7", "Mobile", "Latitude",
            "Longitude", "Score", "EmailStatusID", "SORecordIdentifier", "ImportRowNumber", "DateCreated",
            "DateUpdated", "CreatedByUserID", "UpdatedByUserID", "ProcessCode", "IsActive", "ExternalKeyId",
            "AccountNumber", "EmailID", "Copies", "GraceIssues", "IsComp", "IsPaid", "IsSubscribed", "Occupation",
            "SubscriptionStatusID", "SubsrcID", "Website"
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
                bc.DestinationTableName.ShouldBe($"[{_connection.Database.ToString()}].[dbo].[SubscriberOriginal]");
                bc.BatchSize.ShouldBe(0);
                bc.BulkCopyTimeout.ShouldBe(0);

                foreach (SqlBulkCopyColumnMapping mapping in bc.ColumnMappings)
                {
                    _bulkCopyColumns.Add(mapping.SourceColumn, mapping.DestinationColumn);
                }

                _actualList = table.ConvertDataTable<Entity.SubscriberOriginal>();
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
        }
    }
}