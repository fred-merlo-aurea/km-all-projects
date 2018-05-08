using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Core_AMS.Utilities;
using KM.Common;
using BusinessFileLog = FrameworkUAS.BusinessLogic.FileLog;
using CoreAmsStringFunctions = Core_AMS.Utilities.StringFunctions;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberInvalid
    {
        private const string TableName = "SubscriberInvalid";
        private const string InvalidColumnLengthErrorMessage = "Received an invalid column length from the bcp client for colid";
        private const string ErrorPattern = @"\d+";
        private const string MethodName = "SubscriberInvalid.SaveBulkSqlInsert";

        public static List<Entity.SubscriberInvalid> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberInvalid_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberInvalid> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberInvalid_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberInvalid> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberInvalid_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        private static Entity.SubscriberInvalid Get(SqlCommand cmd)
        {
            Entity.SubscriberInvalid retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberInvalid();
                        DynamicBuilder<Entity.SubscriberInvalid> builder = DynamicBuilder<Entity.SubscriberInvalid>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.SubscriberInvalid> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberInvalid> retList = new List<Entity.SubscriberInvalid>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberInvalid retItem = new Entity.SubscriberInvalid();
                        DynamicBuilder<Entity.SubscriberInvalid> builder = DynamicBuilder<Entity.SubscriberInvalid>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static bool SaveBulkSqlInsert(List<Entity.SubscriberInvalid> list, KMPlatform.Object.ClientConnections client)
        {
            var dt = BulkDataReader<Entity.SubscriberInvalid>.ToDataTable(list);
            var conn = DataFunctions.GetClientSqlConnection(client);
            var bulkCopy = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                {
                    DestinationTableName = $"[{conn.Database}].[dbo].[{TableName}]",
                    BatchSize = 0,
                    BulkCopyTimeout = 0
                };

                InitializeColumnMappings(bulkCopy);

                bulkCopy.WriteToServer(dt);
                bulkCopy.Close();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                LogException(ex, bulkCopy);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return true;
        }

        private static void LogException(Exception ex, SqlBulkCopy bulkCopy)
        {
            DataAccessExceptionBase.LogException(ex, bulkCopy, MethodName, InvalidColumnLengthErrorMessage, ErrorPattern);
        }

        private static void InitializeColumnMappings(SqlBulkCopy bulkCopy)
        {
            bulkCopy.ColumnMappings.Add("SubscriberInvalidID", "SubscriberInvalidID");
            bulkCopy.ColumnMappings.Add("SORecordIdentifier", "SORecordIdentifier");
            bulkCopy.ColumnMappings.Add("SourceFileID", "SourceFileID");
            bulkCopy.ColumnMappings.Add("PubCode", "PubCode");
            bulkCopy.ColumnMappings.Add("Sequence", "Sequence");
            bulkCopy.ColumnMappings.Add("FName", "FName");
            bulkCopy.ColumnMappings.Add("LName", "LName");
            bulkCopy.ColumnMappings.Add("Title", "Title");
            bulkCopy.ColumnMappings.Add("Company", "Company");
            bulkCopy.ColumnMappings.Add("Address", "Address");
            bulkCopy.ColumnMappings.Add("MailStop", "MailStop");
            bulkCopy.ColumnMappings.Add("City", "City");
            bulkCopy.ColumnMappings.Add("State", "State");
            bulkCopy.ColumnMappings.Add("Zip", "Zip");
            bulkCopy.ColumnMappings.Add("Plus4", "Plus4");
            bulkCopy.ColumnMappings.Add("ForZip", "ForZip");
            bulkCopy.ColumnMappings.Add("County", "County");
            bulkCopy.ColumnMappings.Add("Country", "Country");
            bulkCopy.ColumnMappings.Add("CountryID", "CountryID");
            bulkCopy.ColumnMappings.Add("Phone", "Phone");
            bulkCopy.ColumnMappings.Add("Fax", "Fax");
            bulkCopy.ColumnMappings.Add("Email", "Email");
            bulkCopy.ColumnMappings.Add("CategoryID", "CategoryID");
            bulkCopy.ColumnMappings.Add("TransactionID", "TransactionID");
            bulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
            bulkCopy.ColumnMappings.Add("QDate", "QDate");
            bulkCopy.ColumnMappings.Add("QSourceID", "QSourceID");
            bulkCopy.ColumnMappings.Add("RegCode", "RegCode");
            bulkCopy.ColumnMappings.Add("Verified", "Verified");
            bulkCopy.ColumnMappings.Add("SubSrc", "SubSrc");
            bulkCopy.ColumnMappings.Add("OrigsSrc", "OrigsSrc");
            bulkCopy.ColumnMappings.Add("Par3C", "Par3C");
            bulkCopy.ColumnMappings.Add("MailPermission", "MailPermission");
            bulkCopy.ColumnMappings.Add("FaxPermission", "FaxPermission");
            bulkCopy.ColumnMappings.Add("PhonePermission", "PhonePermission");
            bulkCopy.ColumnMappings.Add("OtherProductsPermission", "OtherProductsPermission");
            bulkCopy.ColumnMappings.Add("ThirdPartyPermission", "ThirdPartyPermission");
            bulkCopy.ColumnMappings.Add("EmailRenewPermission", "EmailRenewPermission");
            bulkCopy.ColumnMappings.Add("TextPermission", "TextPermission");
            bulkCopy.ColumnMappings.Add("Source", "Source");
            bulkCopy.ColumnMappings.Add("Priority", "Priority");
            bulkCopy.ColumnMappings.Add("StatList", "StatList");
            bulkCopy.ColumnMappings.Add("Sic", "Sic");
            bulkCopy.ColumnMappings.Add("SicCode", "SicCode");
            bulkCopy.ColumnMappings.Add("Gender", "Gender");
            bulkCopy.ColumnMappings.Add("Address3", "Address3");
            bulkCopy.ColumnMappings.Add("Home_Work_Address", "Home_Work_Address");
            bulkCopy.ColumnMappings.Add("Demo7", "Demo7");
            bulkCopy.ColumnMappings.Add("Mobile", "Mobile");
            bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
            bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
            bulkCopy.ColumnMappings.Add("EmailStatusID", "EmailStatusID");
            bulkCopy.ColumnMappings.Add("SIRecordIdentifier", "SIRecordIdentifier");
            bulkCopy.ColumnMappings.Add("DateCreated", "DateCreated");
            bulkCopy.ColumnMappings.Add("DateUpdated", "DateUpdated");
            bulkCopy.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
            bulkCopy.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");
            bulkCopy.ColumnMappings.Add("ImportRowNumber", "ImportRowNumber");
            bulkCopy.ColumnMappings.Add("ProcessCode", "ProcessCode");
            bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
            bulkCopy.ColumnMappings.Add("ExternalKeyId", "ExternalKeyId");
            bulkCopy.ColumnMappings.Add("AccountNumber", "AccountNumber");
            bulkCopy.ColumnMappings.Add("EmailID", "EmailID");
            bulkCopy.ColumnMappings.Add("Copies", "Copies");
            bulkCopy.ColumnMappings.Add("GraceIssues", "GraceIssues");
            bulkCopy.ColumnMappings.Add("IsComp", "IsComp");
            bulkCopy.ColumnMappings.Add("IsPaid", "IsPaid");
            bulkCopy.ColumnMappings.Add("IsSubscribed", "IsSubscribed");
            bulkCopy.ColumnMappings.Add("Occupation", "Occupation");
            bulkCopy.ColumnMappings.Add("SubscriptionStatusID", "SubscriptionStatusID");
            bulkCopy.ColumnMappings.Add("SubsrcID", "SubsrcID");
            bulkCopy.ColumnMappings.Add("Website", "Website");
        }
    }
}
