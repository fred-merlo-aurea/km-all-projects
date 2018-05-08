using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using AcsFileDetail = FrameworkUAD.DataAccess.AcsFileDetail;
using AcsFileDetailEntity = FrameworkUAD.Entity.AcsFileDetail;
using FileLog = FrameworkUAS.Entity.FileLog;
using ShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;
using ShimFileLog = FrameworkUAS.BusinessLogic.Fakes.ShimFileLog;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    [TestFixture]
    public class AcsFileDetailTest
    {
        private const string DestinationTableNameKey = "DestinationTableName";
        private const string BatchSizeKey = "BatchSize";
        private const string BulkCopyTimeoutKey = "TimeOut";
        private const string DatabaseName = "MyDatabase";
        private const string DestinationTableName = "[MyDatabase].[dbo].[AcsFileDetail]";
        private const int ExpectedFileStatusTypeId = -99;
        private const int ExpectedSourceFileId = -99;
        private const int NumberOfColumns = 86;
        private const int DefaultBatchSize = 0;
        private const string ExceptionMessage = "MyExceptionMessage";
        private const string ExpectedProcessCode = "AcsFileDetail.Save";
        private const int NumberOfEntities = 10; 

        private static readonly string[] Columns =
        {
            "RecordType",
            "FileVersion",
            "SequenceNumber",
            "AcsMailerId",
            "KeylineSequenceSerialNumber",
            "MoveEffectiveDate",
            "MoveType",
            "DeliverabilityCode",
            "UspsSiteID",
            "LastName",
            "FirstName",
            "Prefix",
            "Suffix",
            "OldAddressType",
            "OldUrb",
            "OldPrimaryNumber",
            "OldPreDirectional",
            "OldStreetName",
            "OldSuffix",
            "OldPostDirectional",
            "OldUnitDesignator",
            "OldSecondaryNumber",
            "OldCity",
            "OldStateAbbreviation",
            "OldZipCode",
            "NewAddressType",
            "NewPmb",
            "NewUrb",
            "NewPrimaryNumber",
            "NewPreDirectional",
            "NewStreetName",
            "NewSuffix",
            "NewPostDirectional",
            "NewUnitDesignator",
            "NewSecondaryNumber",
            "NewCity",
            "NewStateAbbreviation",
            "NewZipCode",
            "Hyphen",
            "NewPlus4Code",
            "NewDeliveryPoint",
            "NewAbbreviatedCityName",
            "NewAddressLabel",
            "FeeNotification",
            "NotificationType",
            "IntelligentMailBarcode",
            "IntelligentMailPackageBarcode",
            "IdTag",
            "HardcopyToElectronicFlag",
            "TypeOfAcs",
            "FulfillmentDate",
            "ProcessingType",
            "CaptureType",
            "MadeAvailableDate",
            "ShapeOfMail",
            "MailActionCode",
            "NixieFlag",
            "ProductCode1",
            "ProductCodeFee1",
            "ProductCode2",
            "ProductCodeFee2",
            "ProductCode3",
            "ProductCodeFee3",
            "ProductCode4",
            "ProductCodeFee4",
            "ProductCode5",
            "ProductCodeFee5",
            "ProductCode6",
            "ProductCodeFee6",
            "Filler",
            "EndMarker",
            "ProductCode",
            "OldAddress1",
            "OldAddress2",
            "OldAddress3",
            "NewAddress1",
            "NewAddress2",
            "NewAddress3",
            "SequenceID",
            "TransactionCodeValue",
            "CategoryCodeValue",
            "IsIgnored",
            "AcsActionId",
            "CreatedDate",
            "CreatedTime",
            "ProcessCode"
        };

        private IDisposable _context;

        public static IEnumerable<IList<AcsFileDetailEntity>> InvalidTestData
        {
            get
            {
                yield return null;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(InvalidTestData))]
        public void Insert_InvalidInput_NothingWrittenArgumentException(List<AcsFileDetailEntity> entities)
        {
            // Act, Assert
            Should.Throw<ArgumentNullException>(
                () => AcsFileDetail.Insert(entities, new ClientConnections()));
        }

        [Test]
        public void Insert_ValidInput_DataWritten()
        {
            // Arrange
            Dictionary<string, string> actualColumnMappings = null;
            Dictionary<string, object> actualBulkCopySettings = null;
            var actualNumberOfRows = 0;
            var actualFileLogs = new List<FileLog>();

            var entities = Enumerable.Range(1, NumberOfEntities)
                .Select(x => new AcsFileDetailEntity())
                .ToList();

            ShimsForDataAccess();

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
            {
                actualNumberOfRows = dataTable.Rows.Count;
                actualBulkCopySettings = new Dictionary<string, object>
                {
                    [DestinationTableNameKey] = sqlBulkCopy.DestinationTableName,
                    [BatchSizeKey] = sqlBulkCopy.BatchSize,
                    [BulkCopyTimeoutKey] = sqlBulkCopy.BulkCopyTimeout
                };

                actualColumnMappings = new Dictionary<string, string>();
                foreach (SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings)
                {
                    actualColumnMappings.Add(columnMapping.SourceColumn, columnMapping.DestinationColumn);
                }
            };

            ShimFileLog.AllInstances.SaveFileLog = (fileLog, fileLogEntity) =>
            {
                actualFileLogs.Add(fileLogEntity);
                return true;
            };

            // Act
            var success = AcsFileDetail.Insert(entities, new ClientConnections());

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualNumberOfRows.ShouldBe(NumberOfEntities),
                () => actualBulkCopySettings[DestinationTableNameKey].ShouldBe(DestinationTableName),
                () => actualBulkCopySettings[BatchSizeKey].ShouldBe(DefaultBatchSize),
                () => actualBulkCopySettings[BulkCopyTimeoutKey].ShouldBe(0),
                () => actualFileLogs.Count.ShouldBe(0),
                () => actualColumnMappings.Count.ShouldBe(NumberOfColumns),
                () => actualColumnMappings.All(mapping => mapping.Key == mapping.Value).ShouldBeTrue(),
                () => actualColumnMappings
                    .Keys
                    .ToList()
                    .ForEach(columnName => Columns.Contains(columnName).ShouldBeTrue()));
        }

        [Test]
        public void Insert_ExceptionThrown_LogWrittenExceptionReThrown()
        {
            // Arrange
            var actualFileLogs = new List<FileLog>();
            var expectedException = new InvalidOperationException(ExceptionMessage);
            var entities = Enumerable.Range(1, 10)
                .Select(x => new AcsFileDetailEntity())
                .ToList();

            ShimsForDataAccess();

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) => throw expectedException;

            ShimFileLog.AllInstances.SaveFileLog = (fileLog, fileLogEntity) =>
            {
                actualFileLogs.Add(fileLogEntity);
                return true;
            };

            // Act, Assert
            Should.Throw<InvalidOperationException>(
                () => AcsFileDetail.Insert(entities, new ClientConnections()));

            actualFileLogs.ShouldSatisfyAllConditions(
                () => actualFileLogs.Count.ShouldBe(1),
                () => actualFileLogs.First().FileStatusTypeID.ShouldBe(ExpectedFileStatusTypeId),
                () => actualFileLogs.First().SourceFileID.ShouldBe(ExpectedSourceFileId),
                () => actualFileLogs.First().ProcessCode.ShouldBe(ExpectedProcessCode));
        }

        private static void ShimsForDataAccess()
        {
            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.AllInstances.DatabaseGet = connection => DatabaseName;

            ShimDataFunctions.GetClientSqlConnectionClient = client => new SqlConnection();
        }
    }
}
