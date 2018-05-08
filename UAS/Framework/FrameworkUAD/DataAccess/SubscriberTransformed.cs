using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using KM.Common;
using BuildingFileLog = FrameworkUAS.BusinessLogic.FileLog;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class SubscriberTransformed
    {
        private const string TableName = "SubscriberTransformed";
        private const string BuilInsertStartMessage = "DBCall:Bulk Insert SubscriberTransformed";
        private const string BulkInsertEndMessage = "DBCall End:Bulk Insert SubscriberTransformed";
        private const string MethodName = "SubscriberTransformed.SaveBulkSqlInsert";
        private const string InvalidColumnExceptionMessage = "Received an invalid column length from the bcp client for colid";
        private const string ErrorPattern = @"\d+";

        #region selects
        public static List<Entity.SubscriberTransformed> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static Entity.SubscriberTransformed SelectTopOne(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_ProcessCode_TopOne";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Get(cmd);
        }
        public static List<Entity.SubscriberTransformed> Select(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static Object.DimensionErrorCount SelectDimensionCount(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberTransformed_Select_DimensionCount";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            Object.DimensionErrorCount retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.DimensionErrorCount();
                        DynamicBuilder<Object.DimensionErrorCount> builder = DynamicBuilder<Object.DimensionErrorCount>.CreateBuilder(rdr);
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
        public static List<FrameworkUAD.Object.ImportRowNumber> SelectImportRowNumbers(KMPlatform.Object.ClientConnections client, string ProcessCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberTransformed_ImportRowNumber";
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetCustomList(cmd);
        }
        #endregion

        #region Address validation and geocoding selects
        public static List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_SourceFileID_IsLatLonValid";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_ProcessCode_SourceFileID_IsLatLonValid";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_ProcessCode_IsLatLonValid";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Select_IsLatLonValid";
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> AddressValidation_Paging(int currentPage, int pageSize, string processCode, KMPlatform.Object.ClientConnections client, bool isLatLonValid = false, int sourceFileID = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_AddressValidation_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectForGeoCoding(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_SelectForGeoCoding";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberTransformed> SelectForGeoCoding(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_SelectForGeoCoding_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        #endregion

        #region Geocode counts
        public static int CountAddressValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CountAddressValidation_SourceFileID_IsLatLonValid";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int CountAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CountAddressValidation_ProcessCode_IsLatLonValid";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int CountAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CountAddressValidation_IsLatLonValid";
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        /// <summary>
        /// return INVALID count
        /// </summary>
        /// <param name="client"></param>
        /// <returns>int</returns>
        public static int CountForGeoCoding(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CountForGeoCoding";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        /// <summary>
        /// reutrn INVALID count
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sourceFileID"></param>
        /// <returns>int </returns>
        public static int CountForGeoCoding(KMPlatform.Object.ClientConnections client, int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CountForGeoCoding_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        #endregion

        #region distinct PubCode list
        public static List<string> GetDistinctPubCodes(KMPlatform.Object.ClientConnections client, string processCode)
        {
            List<string> pubs = new List<string>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberTransformed_GetPubCodes";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        pubs.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            return pubs;
        }
        #endregion

        private static Entity.SubscriberTransformed Get(SqlCommand cmd)
        {
            Entity.SubscriberTransformed retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberTransformed();
                        DynamicBuilder<Entity.SubscriberTransformed> builder = DynamicBuilder<Entity.SubscriberTransformed>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberTransformed> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberTransformed> retList = new List<Entity.SubscriberTransformed>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberTransformed retItem = new Entity.SubscriberTransformed();
                        DynamicBuilder<Entity.SubscriberTransformed> builder = DynamicBuilder<Entity.SubscriberTransformed>.CreateBuilder(rdr);
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
        private static List<FrameworkUAD.Object.ImportRowNumber> GetCustomList(SqlCommand cmd)
        {
            List<FrameworkUAD.Object.ImportRowNumber> retList = new List<FrameworkUAD.Object.ImportRowNumber>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        FrameworkUAD.Object.ImportRowNumber retItem = new FrameworkUAD.Object.ImportRowNumber();
                        DynamicBuilder<FrameworkUAD.Object.ImportRowNumber> builder = DynamicBuilder<FrameworkUAD.Object.ImportRowNumber>.CreateBuilder(rdr);
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
            catch(Exception ex) { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        #region Save / Update methods
        public static bool SaveBulkInsert(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_SaveBulkInsert";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool SaveBulkSqlInsert(List<Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            var dataTable = Core_AMS.Utilities.BulkDataReader<Entity.SubscriberTransformed>.ToDataTable(list);
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

                ExecuteBulkCopy(list, bulkCopy, dataTable);
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

        public static int Save(Entity.SubscriberTransformed x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriberTransformedID", x.SubscriberTransformedID));
            cmd.Parameters.Add(new SqlParameter("@SORecordIdentifier", x.SORecordIdentifier));
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
            cmd.Parameters.Add(new SqlParameter("@IsLatLonValid", (object)x.IsLatLonValid ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LatLonMsg", (object)x.LatLonMsg ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", (object)x.EmailStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@STRecordIdentifier", (object)x.STRecordIdentifier ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ImportRowNumber", (object)x.ImportRowNumber ?? DBNull.Value));
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
            cmd.Parameters.AddWithValue("SubGenSubscriberID", x.SubGenSubscriberID);
            cmd.Parameters.AddWithValue("SubGenSubscriptionID", x.SubGenSubscriptionID);
            cmd.Parameters.AddWithValue("SubGenPublicationID", x.SubGenPublicationID);
            cmd.Parameters.AddWithValue("SubGenMailingAddressId", x.SubGenMailingAddressId);
            cmd.Parameters.AddWithValue("SubGenBillingAddressId", x.SubGenBillingAddressId);
            cmd.Parameters.AddWithValue("IssuesLeft", x.IssuesLeft);
            cmd.Parameters.AddWithValue("UnearnedReveue", x.UnearnedReveue);
            cmd.Parameters.AddWithValue("@SubGenIsLead", (object)x.SubGenIsLead ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubGenRenewalCode", (object)x.SubGenRenewalCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubGenSubscriptionRenewDate", (object)x.SubGenSubscriptionRenewDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubGenSubscriptionExpireDate", (object)x.SubGenSubscriptionExpireDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubGenSubscriptionLastQualifiedDate", (object)x.SubGenSubscriptionLastQualifiedDate ?? DBNull.Value);

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool AddressUpdateBulkSql(List<Entity.SubscriberTransformed> list, KMPlatform.Object.ClientConnections client)
        {
            //int sourceFileID = list.FirstOrDefault().SourceFileID;
            //string processCode = list.FirstOrDefault().ProcessCode;
            bool done = true;
            try
            {
                StringBuilder sbXML = new StringBuilder();
                sbXML.AppendLine("<XML>");
                foreach (Entity.SubscriberTransformed x in list)
                {
                    sbXML.AppendLine("<Subscriber>");

                    sbXML.AppendLine("<SORecordIdentifier>" + x.SORecordIdentifier.ToString() + "</SORecordIdentifier>");
                    sbXML.AppendLine("<Address>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Address.Trim()) + "</Address>");
                    sbXML.AppendLine("<MailStop>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.MailStop.Trim()) + "</MailStop>");
                    sbXML.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.City.Trim()) + "</City>");
                    sbXML.AppendLine("<State>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.State.Trim()) + "</State>");
                    sbXML.AppendLine("<Zip>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Zip.Trim()) + "</Zip>");
                    sbXML.AppendLine("<Plus4>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Plus4.Trim()) + "</Plus4>");
                    sbXML.AppendLine("<Latitude>" + x.Latitude.ToString() + "</Latitude>");
                    sbXML.AppendLine("<Longitude>" + x.Longitude.ToString() + "</Longitude>");
                    sbXML.AppendLine("<IsLatLonValid>" + x.IsLatLonValid.ToString() + "</IsLatLonValid>");
                    //sbXML.AppendLine("<IsMailable>" + x.IsMailable.ToString() + "</IsMailable>");
                    sbXML.AppendLine("<LatLongMsg>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.LatLonMsg.Trim()) + "</LatLongMsg>");
                    sbXML.AppendLine("<Country>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.Country.Trim()) + "</Country>");
                    sbXML.AppendLine("<County>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.County.Trim()) + "</County>");

                    sbXML.AppendLine("</Subscriber>");
                }
                sbXML.AppendLine("</XML>");

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_SubscriberTransformed_SaveAddressValidation";
                cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);

                done = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "SubscriberTransformed.AddressUpdateBulkSql"));
                throw ex;
            }
            return done;
        }
        #endregion

        #region Jobs / Operations
        public static bool StandardRollUpToMaster(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Standard_RollUpToMaster";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@MailPermissionOverRide", mailPermissionOverRide);
            cmd.Parameters.AddWithValue("@FaxPermissionOverRide", faxPermissionOverRide);
            cmd.Parameters.AddWithValue("@PhonePermissionOverRide", phonePermissionOverRide);
            cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", otherProductsPermissionOverRide);
            cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", thirdPartyPermissionOverRide);
            cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", emailRenewPermissionOverRide);
            cmd.Parameters.AddWithValue("@TextPermissionOverRide", textPermissionOverRide);

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool StandardRollUpToMaster(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode,
            bool mailPermissionOverRide = false, bool faxPermissionOverRide = false, bool phonePermissionOverRide = false,
            bool otherProductsPermissionOverRide = false, bool thirdPartyPermissionOverRide = false, bool emailRenewPermissionOverRide = false,
            bool textPermissionOverRide = false, bool updateEmail = true, bool updatePhone = true, bool updateFax = true, bool updateMobile = true)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Standard_RollUpToMaster";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@MailPermissionOverRide", mailPermissionOverRide);
            cmd.Parameters.AddWithValue("@FaxPermissionOverRide", faxPermissionOverRide);
            cmd.Parameters.AddWithValue("@PhonePermissionOverRide", phonePermissionOverRide);
            cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", otherProductsPermissionOverRide);
            cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", thirdPartyPermissionOverRide);
            cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", emailRenewPermissionOverRide);
            cmd.Parameters.AddWithValue("@TextPermissionOverRide", textPermissionOverRide);
            cmd.Parameters.AddWithValue("@UpdateEmail", updateEmail);
            cmd.Parameters.AddWithValue("@UpdatePhone", updatePhone);
            cmd.Parameters.AddWithValue("@UpdateFax", updateFax);
            cmd.Parameters.AddWithValue("@UpdateMobile", updateMobile);

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool DataMatching(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_DataMatching";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SequenceDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SequenceDataMatching";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool DataMatching_multiple(KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode, string matchFields)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_DataMatch_multiple";
            cmd.Parameters.AddWithValue("@matchFields", matchFields);
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool DataMatching_single(KMPlatform.Object.ClientConnections client, string processCode, string matchField)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_DataMatch_single";
            cmd.Parameters.AddWithValue("@matchField", matchField);
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool AddressValidExisting(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_AddressValidateExisting";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        //public static bool DisableIndexes(KMPlatform.Object.ClientConnections client)
        //{
        //    //--Disable Index
        //    //ALTER INDEX [IXYourIndex] ON YourTable DISABLE
        //    //GO
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SubscriberTransformed_DisableIndexes";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return DataFunctions.ExecuteNonQuery(cmd);

        //}
        //public static bool EnableIndexes(KMPlatform.Object.ClientConnections client)
        //{
        //    //--Enable Index
        //    //ALTER INDEX [IXYourIndex] ON YourTable REBUILD
        //    //GO
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SubscriberTransformed_EnableIndexes";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    return DataFunctions.ExecuteNonQuery(cmd);
        //}
        public static bool RevertXmlFormattingAfterBulkInsert(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberTransformed_RevertXmlFormattingAfterBulkInsert";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion

        private static void LogException(Exception ex, SqlBulkCopy bulkCopy)
        {
            DataAccessExceptionBase.LogException(ex, bulkCopy, MethodName, InvalidColumnExceptionMessage, ErrorPattern);
        }

        private static void ExecuteBulkCopy(IReadOnlyCollection<Entity.SubscriberTransformed> list, SqlBulkCopy bulkCopy, DataTable dt)
        {
            var sourceFileId = list.First().SourceFileID;
            var processCode = list.First().ProcessCode;
            var fileLog = new BuildingFileLog();
            fileLog.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, BuilInsertStartMessage, processCode));
            bulkCopy.WriteToServer(dt);
            fileLog.Save(new FrameworkUAS.Entity.FileLog(sourceFileId, -99, BulkInsertEndMessage, processCode));
            bulkCopy.Close();
        }

        private static void InitializeColumnMappings(SqlBulkCopy bulkCopy)
        {
            bulkCopy.ColumnMappings.Add("SubscriberTransformedID", "SubscriberTransformedID");
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
            bulkCopy.ColumnMappings.Add("Sic", "Sic");
            bulkCopy.ColumnMappings.Add("SicCode", "SicCode");
            bulkCopy.ColumnMappings.Add("Gender", "Gender");
            bulkCopy.ColumnMappings.Add("Address3", "Address3");
            bulkCopy.ColumnMappings.Add("Home_Work_Address", "Home_Work_Address");
            bulkCopy.ColumnMappings.Add("Demo7", "Demo7");
            bulkCopy.ColumnMappings.Add("Mobile", "Mobile");
            bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
            bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
            bulkCopy.ColumnMappings.Add("IsLatLonValid", "IsLatLonValid");
            bulkCopy.ColumnMappings.Add("LatLonMsg", "LatLonMsg");
            bulkCopy.ColumnMappings.Add("EmailStatusID", "EmailStatusID");
            bulkCopy.ColumnMappings.Add("STRecordIdentifier", "STRecordIdentifier");
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

            bulkCopy.ColumnMappings.Add("SubGenSubscriberID", "SubGenSubscriberID");
            bulkCopy.ColumnMappings.Add("SubGenSubscriptionID", "SubGenSubscriptionID");
            bulkCopy.ColumnMappings.Add("SubGenPublicationID", "SubGenPublicationID");
            bulkCopy.ColumnMappings.Add("SubGenMailingAddressId", "SubGenMailingAddressId");
            bulkCopy.ColumnMappings.Add("SubGenBillingAddressId", "SubGenBillingAddressId");
            bulkCopy.ColumnMappings.Add("IssuesLeft", "IssuesLeft");
            bulkCopy.ColumnMappings.Add("UnearnedReveue", "UnearnedReveue");

            bulkCopy.ColumnMappings.Add("SubGenIsLead", "SubGenIsLead");
            bulkCopy.ColumnMappings.Add("SubGenRenewalCode", "SubGenRenewalCode");
            bulkCopy.ColumnMappings.Add("SubGenSubscriptionRenewDate", "SubGenSubscriptionRenewDate");
            bulkCopy.ColumnMappings.Add("SubGenSubscriptionExpireDate", "SubGenSubscriptionExpireDate");
            bulkCopy.ColumnMappings.Add("SubGenSubscriptionLastQualifiedDate", "SubGenSubscriptionLastQualifiedDate");
        }
    }
}
