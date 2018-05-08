using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberArchive
    {
        public static List<Entity.SubscriberArchive> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberArchive_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberArchive> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberArchive_Select";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberArchive> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberArchive_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        private static Entity.SubscriberArchive Get(SqlCommand cmd)
        {
            Entity.SubscriberArchive retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberArchive();
                        DynamicBuilder<Entity.SubscriberArchive> builder = DynamicBuilder<Entity.SubscriberArchive>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberArchive> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberArchive> retList = new List<Entity.SubscriberArchive>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberArchive retItem = new Entity.SubscriberArchive();
                        DynamicBuilder<Entity.SubscriberArchive> builder = DynamicBuilder<Entity.SubscriberArchive>.CreateBuilder(rdr);
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
        public static bool SaveBulkInsert(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberArchive_SaveBulkInsert";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static int Save(Entity.SubscriberArchive x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberArchive_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriberArchiveID", x.SubscriberArchiveID));
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
            cmd.Parameters.Add(new SqlParameter("@PhoneExists", (object)x.PhoneExists ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", x.Fax));
            cmd.Parameters.Add(new SqlParameter("@FaxExists", (object)x.FaxExists ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)x.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailExists", (object)x.EmailExists ?? DBNull.Value));
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
            cmd.Parameters.Add(new SqlParameter("@IGrp_No", (object)x.IGrp_No ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IGrp_Cnt", (object)x.IGrp_Cnt ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CGrp_No", (object)x.CGrp_No ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CGrp_Cnt", (object)x.CGrp_Cnt ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StatList", (object)x.StatList ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Sic", (object)x.Sic ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SicCode", (object)x.SicCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Gender", (object)x.Gender ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IGrp_Rank", (object)x.IGrp_Rank ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CGrp_Rank", (object)x.CGrp_Rank ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)x.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Home_Work_Address", (object)x.Home_Work_Address ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Demo7", (object)x.Demo7 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)x.Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Latitude", (object)x.Latitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Longitude", (object)x.Longitude ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailStatusID", (object)x.EmailStatusID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsMailable", (object)x.IsMailable ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SARecordIdentifier", (object)x.SARecordIdentifier ?? DBNull.Value));
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
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
