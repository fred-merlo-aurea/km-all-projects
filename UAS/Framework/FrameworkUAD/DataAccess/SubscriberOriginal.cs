using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Core_AMS.Utilities;
using KM.Common;
using FrameworkUAS.Entity;
using BusinessFileLog = FrameworkUAS.BusinessLogic.FileLog;
using StringFunctions = KM.Common.StringFunctions;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class SubscriberOriginal
    {
        private const string StandardTableFormat = "[{0}].[dbo].[{1}]";
        private const string TableName = "SubscriberOriginal";
        private const string InvalidColumnLengthErrorMessage = "Received an invalid column length from the bcp client for colid";
        private const string ErrorPattern = @"\d+";
        private const string BulkInsertStartMessage = "DBCall:Bulk Insert SubscriberOriginal";
        private const string BulkInsertEndMessage = "DBCall End:Bulk Insert SubscriberOriginal";
        private const string MethodName = "SubscriberOriginal.SaveBulkSqlInsert";

        public static List<Entity.SubscriberOriginal> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberOriginal> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberOriginal> Select(int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_Select_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberOriginal> Select(string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_Select_SourceFileID_ProcessCode";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberOriginal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        private static Entity.SubscriberOriginal Get(SqlCommand cmd)
        {
            Entity.SubscriberOriginal retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberOriginal();
                        DynamicBuilder<Entity.SubscriberOriginal> builder = DynamicBuilder<Entity.SubscriberOriginal>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberOriginal> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberOriginal> retList = new List<Entity.SubscriberOriginal>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberOriginal retItem = new Entity.SubscriberOriginal();
                        DynamicBuilder<Entity.SubscriberOriginal> builder = DynamicBuilder<Entity.SubscriberOriginal>.CreateBuilder(rdr);
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
        public static bool SaveBulkUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_SaveBulkUpdate";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SaveBulkInsert(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_SaveBulkInsert";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool SaveBulkSqlInsert(List<Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            var bulkCopy = default(SqlBulkCopy);
            var dataTable = BulkDataReader<Entity.SubscriberOriginal>.ToDataTable(list);
            using (var conn = DataFunctions.GetClientSqlConnection(client))
            {
                try
                {
                    conn.Open();
                    bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                    {
                        DestinationTableName = string.Format(StandardTableFormat, conn.Database, TableName),
                        BatchSize = 0,
                        BulkCopyTimeout = 0
                    };
                    InitializeColumnMappings(bulkCopy);

                    ExecuteBulkCopy(list, bulkCopy, dataTable);
                    return true;
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
            }
        }

        private static void ExecuteBulkCopy(IReadOnlyCollection<Entity.SubscriberOriginal> list, SqlBulkCopy bulkCopy, DataTable dt)
        {
            var processCode = list.First().ProcessCode;
            var sourceFileId = list.First().SourceFileID;

            var fileLog = new BusinessFileLog();
            fileLog.Save(new FileLog(sourceFileId, -99, BulkInsertStartMessage, processCode));
            bulkCopy.WriteToServer(dt);
            fileLog.Save(new FileLog(sourceFileId, -99, BulkInsertEndMessage, processCode));
            bulkCopy.Close();
        }

        private static void LogException(Exception ex, SqlBulkCopy bulkCopy)
        {
            DataAccessExceptionBase.LogException(ex, bulkCopy, MethodName, InvalidColumnLengthErrorMessage, ErrorPattern);
        }

        private static void InitializeColumnMappings(SqlBulkCopy bc)
        {
            bc.ColumnMappings.Add("SubscriberOriginalID", "SubscriberOriginalID");
            bc.ColumnMappings.Add("SourceFileID", "SourceFileID");
            bc.ColumnMappings.Add("PubCode", "PubCode");
            bc.ColumnMappings.Add("Sequence", "Sequence");
            bc.ColumnMappings.Add("FName", "FName");
            bc.ColumnMappings.Add("LName", "LName");
            bc.ColumnMappings.Add("Title", "Title");
            bc.ColumnMappings.Add("Company", "Company");
            bc.ColumnMappings.Add("Address", "Address");
            bc.ColumnMappings.Add("MailStop", "MailStop");
            bc.ColumnMappings.Add("City", "City");
            bc.ColumnMappings.Add("State", "State");
            bc.ColumnMappings.Add("Zip", "Zip");
            bc.ColumnMappings.Add("Plus4", "Plus4");
            bc.ColumnMappings.Add("ForZip", "ForZip");
            bc.ColumnMappings.Add("County", "County");
            bc.ColumnMappings.Add("Country", "Country");
            bc.ColumnMappings.Add("CountryID", "CountryID");
            bc.ColumnMappings.Add("Phone", "Phone");
            bc.ColumnMappings.Add("Fax", "Fax");
            bc.ColumnMappings.Add("Email", "Email");
            bc.ColumnMappings.Add("CategoryID", "CategoryID");
            bc.ColumnMappings.Add("TransactionID", "TransactionID");
            bc.ColumnMappings.Add("TransactionDate", "TransactionDate");
            bc.ColumnMappings.Add("QDate", "QDate");
            bc.ColumnMappings.Add("QSourceID", "QSourceID");
            bc.ColumnMappings.Add("RegCode", "RegCode");
            bc.ColumnMappings.Add("Verified", "Verified");
            bc.ColumnMappings.Add("SubSrc", "SubSrc");
            bc.ColumnMappings.Add("OrigsSrc", "OrigsSrc");
            bc.ColumnMappings.Add("Par3C", "Par3C");
            bc.ColumnMappings.Add("MailPermission", "MailPermission");
            bc.ColumnMappings.Add("FaxPermission", "FaxPermission");
            bc.ColumnMappings.Add("PhonePermission", "PhonePermission");
            bc.ColumnMappings.Add("OtherProductsPermission", "OtherProductsPermission");
            bc.ColumnMappings.Add("ThirdPartyPermission", "ThirdPartyPermission");
            bc.ColumnMappings.Add("EmailRenewPermission", "EmailRenewPermission");
            bc.ColumnMappings.Add("TextPermission", "TextPermission");
            bc.ColumnMappings.Add("Source", "Source");
            bc.ColumnMappings.Add("Priority", "Priority");
            bc.ColumnMappings.Add("Sic", "Sic");
            bc.ColumnMappings.Add("SicCode", "SicCode");
            bc.ColumnMappings.Add("Gender", "Gender");
            bc.ColumnMappings.Add("Address3", "Address3");
            bc.ColumnMappings.Add("Home_Work_Address", "Home_Work_Address");
            bc.ColumnMappings.Add("Demo7", "Demo7");
            bc.ColumnMappings.Add("Mobile", "Mobile");
            bc.ColumnMappings.Add("Latitude", "Latitude");
            bc.ColumnMappings.Add("Longitude", "Longitude");
            bc.ColumnMappings.Add("Score", "Score");
            bc.ColumnMappings.Add("EmailStatusID", "EmailStatusID");
            bc.ColumnMappings.Add("SORecordIdentifier", "SORecordIdentifier");
            bc.ColumnMappings.Add("ImportRowNumber", "ImportRowNumber");
            bc.ColumnMappings.Add("DateCreated", "DateCreated");
            bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
            bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
            bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");
            bc.ColumnMappings.Add("ProcessCode", "ProcessCode");
            bc.ColumnMappings.Add("IsActive", "IsActive");
            bc.ColumnMappings.Add("ExternalKeyId", "ExternalKeyId");
            bc.ColumnMappings.Add("AccountNumber", "AccountNumber");
            bc.ColumnMappings.Add("EmailID", "EmailID");
            bc.ColumnMappings.Add("Copies", "Copies");
            bc.ColumnMappings.Add("GraceIssues", "GraceIssues");
            bc.ColumnMappings.Add("IsComp", "IsComp");
            bc.ColumnMappings.Add("IsPaid", "IsPaid");
            bc.ColumnMappings.Add("IsSubscribed", "IsSubscribed");
            bc.ColumnMappings.Add("Occupation", "Occupation");
            bc.ColumnMappings.Add("SubscriptionStatusID", "SubscriptionStatusID");
            bc.ColumnMappings.Add("SubsrcID", "SubsrcID");
            bc.ColumnMappings.Add("Website", "Website");
        }

        public static int Save(Entity.SubscriberOriginal x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOriginal_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriberOriginalID", x.SubscriberOriginalID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", (object)x.SourceFileID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubCode", (object)x.PubCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Sequence", (object)x.Sequence ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FName", (object)x.FName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LName", (object)x.LName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Title", (object)x.Title ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Company", (object)x.Company ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address", (object)x.Address ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailStop", (object)x.MailStop ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)x.City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@State", (object)x.State ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Zip", (object)x.Zip ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Plus4", (object)x.Plus4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ForZip", (object)x.ForZip ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@County", (object)x.County ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Country", (object)x.Country ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)x.CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)x.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", x.Fax));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)x.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", (object)x.CategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TransactionID", (object)x.TransactionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TransactionDate", (object)x.TransactionDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QDate", (object)x.QDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QSourceID", (object)x.QSourceID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RegCode", (object)x.RegCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Verified", (object)x.Verified ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubSrc", (object)x.SubSrc ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OrigsSrc", (object)x.OrigsSrc ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Par3C", (object)x.Par3C ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailPermission", (object)x.MailPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FaxPermission", (object)x.FaxPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhonePermission", (object)x.PhonePermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OtherProductsPermission", (object)x.OtherProductsPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ThirdPartyPermission", (object)x.ThirdPartyPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailRenewPermission", (object)x.EmailRenewPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TextPermission", (object)x.TextPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Source", (object)x.Source ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Priority", (object)x.Priority ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Sic", (object)x.Sic ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SicCode", (object)x.SicCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Gender", (object)x.Gender ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)x.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Home_Work_Address", (object)x.Home_Work_Address ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Demo7", (object)x.Demo7 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)x.Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Latitude", (object)x.Latitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Longitude", (object)x.Longitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Score", (object)x.Score ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", (object)x.EmailStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SORecordIdentifier", (object)x.SORecordIdentifier ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ImportRowNumber", (object)x.ImportRowNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", (object)x.ProcessCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)x.IsActive ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExternalKeyId", (object)x.ExternalKeyId ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AccountNumber", (object)x.AccountNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailID", (object)x.EmailID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Copies", (object)x.Copies ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GraceIssues", (object)x.GraceIssues ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsComp", (object)x.IsComp ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsPaid", (object)x.IsPaid ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsSubscribed", (object)x.IsSubscribed ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Occupation", (object)x.Occupation ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionStatusID", (object)x.SubscriptionStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SubsrcID", (object)x.SubsrcID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Website", (object)x.Website ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
