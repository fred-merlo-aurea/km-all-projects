using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FrameworkUAS.BusinessLogic;
using static Core_AMS.Utilities.BulkDataReader<FrameworkUAD.Entity.FileValidator_Transformed>;
using EntityFileLog = FrameworkUAS.Entity.FileLog;

namespace FrameworkUAD.DataAccess
{
    public class FileValidator_Transformed
    {
        private const string DatabaseNamePostFix = "[dbo].[FileValidator_Transformed]";
        private const string BulkCopyStartedMessage = "DBCall:Bulk Insert FileValidator_Transformed";
        private const string BulkCopyFininshedMessage = "DBCall End:Bulk Insert FileValidator_Transformed";
        private const int DefaultBatchSize = 1000;
        private const int BulkCopyTimeout = 0;
        private const int FileStatusTypeId = -99;

        private static readonly string[] Columns =
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

        public static bool SaveBulkSqlInsert(List<Entity.FileValidator_Transformed> list, KMPlatform.Object.ClientConnections client)
        {
            if (list == null || !list.Any())
            {
                throw new ArgumentException(nameof(list));
            }

            var headTransformed = list.First();
            var sourceFileId = headTransformed.SourceFileID;
            var processCode = headTransformed.ProcessCode;
            var dataTable = ToDataTable(list);
            var done = false;
            using (var sqlConnection = DataFunctions.GetClientSqlConnection(client))
            {
                sqlConnection.Open();
                try
                {
                    using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.TableLock, null))
                    {
                        sqlBulkCopy.DestinationTableName = GetDestinationTableName(sqlConnection);
                        sqlBulkCopy.BatchSize = DefaultBatchSize;
                        sqlBulkCopy.BulkCopyTimeout = BulkCopyTimeout;

                        foreach (var columnName in Columns)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
                        }

                        var fileLog = new FileLog();
                        SaveToFileLog(fileLog, sourceFileId, processCode, BulkCopyStartedMessage);
                        sqlBulkCopy.WriteToServer(dataTable);
                        SaveToFileLog(fileLog, sourceFileId, processCode, BulkCopyFininshedMessage);
                        done = true;
                    }
                }
                catch (Exception exception) 
                    when (exception is SqlException
                          || exception is IOException)
                {
                    // BUG: exception "swallowed"
                }
            }

            return done;
        }

        private static void SaveToFileLog(FileLog fileLog, int sourceFileId, string processCode, string message)
        {
            fileLog.Save(
                new EntityFileLog(
                    sourceFileId,
                    FileStatusTypeId,
                    message,
                    processCode));
        }

        private static string GetDestinationTableName(SqlConnection sqlConnection)
        {
            return $"[{sqlConnection.Database}].{DatabaseNamePostFix}";
        }
    }
}
