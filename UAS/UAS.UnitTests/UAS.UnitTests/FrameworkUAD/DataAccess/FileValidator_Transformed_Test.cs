using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAS.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using FileLog = FrameworkUAS.Entity.FileLog;
using FileValidator_TransformedEntity = FrameworkUAD.Entity.FileValidator_Transformed;
using ShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    [TestFixture]
    public class FileValidator_Transformed_Test
    {
        private const string DestinationTableNameKey = "DestinationTableName";
        private const string BatchSizeKEy = "BatchSize";
        private const string BulkCopyTimeoutKey = "TimeOut";
        private const string BulkCopyStartedMessage = "DBCall:Bulk Insert FileValidator_Transformed";
        private const string BulkCopyFininshedMessage = "DBCall End:Bulk Insert FileValidator_Transformed";
        private const string DatabaseName = "MyDatabase";
        private const string DestinationTableName = "[MyDatabase].[dbo].[FileValidator_Transformed]";
        private const int FileStatusTypeId = -99;
        private const int SourceFileId = 42;
        private const string ProcessCode = "MyProcessCode";
        private const int DefaultBatchSize = 1000;
        private const int NumberOfColumns = 82;

        private readonly string[] columns =
            {
             "FV_TransformedID",
             "SourceFileID",
             "PubCode",
             "Sequence",
             "FName",
             "LName",
             "Title",
             "Company",
             "Address",
             "MailStop",
             "City",
             "State",
             "Zip",
             "Plus4",
             "ForZip",
             "County",
             "Country",
             "CountryID",
             "Phone",
             "PhoneExists",
             "Fax",
             "FaxExists",
             "Email",
             "EmailExists",
             "CategoryID",
             "TransactionID",
             "TransactionDate",
             "QDate",
             "QSourceID",
             "RegCode",
             "Verified",
             "SubSrc",
             "OrigsSrc",
             "Par3C",
             "Demo31",
             "Demo32",
             "Demo33",
             "Demo34",
             "Demo35",
             "Demo36",
             "Source",
             "Priority",
             "IGrp_No",
             "IGrp_Cnt",
             "CGrp_No",
             "CGrp_Cnt",
             "StatList",
             "Sic",
             "SicCode",
             "Gender",
             "IGrp_Rank",
             "CGrp_Rank",
             "Address3",
             "Home_Work_Address",
             "PubIDs",
             "Demo7",
             "IsExcluded",
             "Mobile",
             "Latitude",
             "Longitude",
             "IsLatLonValid",
             "LatLonMsg",
             "InSuppression",
             "Score",
             "SuppressedDate",
             "SuppressedEmail",
             "EmailStatusID",
             "StatusUpdatedDate",
             "StatusUpdatedReason",
             "IsMailable",
             "Ignore",
             "IsDQMProcessFinished",
             "DQMProcessDate",
             "IsUpdatedInLive",
             "UpdateInLiveDate",
             "STRecordIdentifier",
             "DateCreated",
             "DateUpdated",
             "CreatedByUserID",
             "UpdatedByUserID",
             "ImportRowNumber",
             "ProcessCode"
            };

        private IDisposable context;

        public static IEnumerable<IList<FileValidator_TransformedEntity>> InvalidTestData
        {
            get
            {
                yield return null;
                yield return new List<FileValidator_TransformedEntity>();
            }
        }

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(InvalidTestData))]
        public void SaveBulkSqlInsert_InvalidInput_NothingWrittenArgumentException(List<FileValidator_TransformedEntity> entities)
        {
            // Act, Assert
            Should.Throw<ArgumentException>(
                () => FileValidator_Transformed.SaveBulkSqlInsert(entities, new ClientConnections()));
        }

        [Test]
        public void SaveBulkSqlInsert_ValidInput_DataAndLogWritten()
        {
            // Arrange
            var numberOfEntities = 10;
            Dictionary<string, string> actualColumnMappings = null;
            Dictionary<string, object> actualBulkCopySettings = null;
            var actualNumberOfRows = 0;
            var actualFileLogs = new List<FileLog>();

            var entities = Enumerable.Range(1, 10)
                .Select(x => new FileValidator_TransformedEntity())
                .ToList();

            var headEntity = entities.First();
            headEntity.ProcessCode = ProcessCode;
            headEntity.SourceFileID = SourceFileId;

            ShimsForDataAccess();

            ShimSqlBulkCopy.AllInstances.WriteToServerDataTable = (sqlBulkCopy, dataTable) =>
                {
                    actualNumberOfRows = dataTable.Rows.Count;
                    actualBulkCopySettings = new Dictionary<string, object>
                                         {
                                             [DestinationTableNameKey] = sqlBulkCopy.DestinationTableName,
                                             [BatchSizeKEy] = sqlBulkCopy.BatchSize,
                                             [BulkCopyTimeoutKey] = sqlBulkCopy.BulkCopyTimeout
                                         };

                    actualColumnMappings = new Dictionary<string, string>();
                    foreach (SqlBulkCopyColumnMapping columnMapping in sqlBulkCopy.ColumnMappings)
                    {
                        actualColumnMappings.Add(columnMapping.SourceColumn, columnMapping.DestinationColumn);
                    }
                };

            ShimFileLog.SaveFileLog = fileLog =>
                {
                    actualFileLogs.Add(fileLog);
                    return true;
                };
                  
            // Act
            var success = FileValidator_Transformed.SaveBulkSqlInsert(entities, new ClientConnections());

            // Assert
            success.ShouldSatisfyAllConditions(
                () => success.ShouldBeTrue(),
                () => actualNumberOfRows.ShouldBe(numberOfEntities),
                () => actualBulkCopySettings[DestinationTableNameKey].ShouldBe(DestinationTableName),
                () => actualBulkCopySettings[BatchSizeKEy].ShouldBe(DefaultBatchSize),
                () => actualBulkCopySettings[BulkCopyTimeoutKey].ShouldBe(0),
                () => actualFileLogs.Count.ShouldBe(2),
                () => actualFileLogs.All(x => x.SourceFileID == SourceFileId).ShouldBeTrue(),
                () => actualFileLogs.All(x => x.ProcessCode == ProcessCode).ShouldBeTrue(),
                () => actualFileLogs.All(x => x.FileStatusTypeID == FileStatusTypeId).ShouldBeTrue(),
                () => actualFileLogs.First().Message.ShouldBe(BulkCopyStartedMessage),
                () => actualFileLogs.Last().Message.ShouldBe(BulkCopyFininshedMessage),
                () => actualColumnMappings.Count.ShouldBe(NumberOfColumns),
                () => actualColumnMappings.All(mapping => mapping.Key == mapping.Value).ShouldBeTrue(),
                () => actualColumnMappings
                    .Keys
                    .ToList()
                    .ForEach(columnName => columns.Contains(columnName).ShouldBeTrue()));
        }

        private static void ShimsForDataAccess()
        {
            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.AllInstances.DatabaseGet = connection => DatabaseName;

            ShimDataFunctions.GetClientSqlConnectionClient = client => new SqlConnection();
        }
    }
}
