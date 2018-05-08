using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FrameworkUAS.DataAccess;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberFinal
    {
        private static Entity.SubscriberFinal Get(SqlCommand cmd)
        {
            Entity.SubscriberFinal retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberFinal();
                        DynamicBuilder<Entity.SubscriberFinal> builder = DynamicBuilder<Entity.SubscriberFinal>.CreateBuilder(rdr);
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

        private static List<Entity.SubscriberFinal> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberFinal> retList = new List<Entity.SubscriberFinal>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberFinal retItem = new Entity.SubscriberFinal();
                        DynamicBuilder<Entity.SubscriberFinal> builder = DynamicBuilder<Entity.SubscriberFinal>.CreateBuilder(rdr);
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
        public static List<Entity.SubscriberFinal> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select_ProcessCode_SourceFileID_IsLatLonValid";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> SelectByAddressValidation(KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select_IsLatLonValid";
            cmd.Parameters.AddWithValue("@IsLatLonValid", isLatLonValid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> SelectForFieldUpdate(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SelectForFieldUpdate";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> SelectForIgnoredReport(string processCode, bool isIgnored, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select_ProcessCode_Ignored";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@isIgnored", isIgnored); 
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberFinal> SelectForProcessedReport(string processCode, bool isIgnored, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_Select_ProcessCode_Ignored";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@isIgnored", isIgnored);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        /// <summary>
        /// will return a data table with SORecordIdentifier, STRecordIdentifier, SFRecordIdentifier
        /// </summary>
        /// <param name="processCode"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<Object.RecordIdentifier> SelectRecordIdentifiers(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberFinal_Select_RecordIdentifiers";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            List<Object.RecordIdentifier> retList = new List<Object.RecordIdentifier>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.RecordIdentifier retItem = new Object.RecordIdentifier();
                        DynamicBuilder<Object.RecordIdentifier> builder = DynamicBuilder<Object.RecordIdentifier>.CreateBuilder(rdr);
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
        public static Object.AdmsResultCount SelectResultCount(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberFinal_Select_AdmsResultCount";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            Object.AdmsResultCount retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.AdmsResultCount();
                        DynamicBuilder<Object.AdmsResultCount> builder = DynamicBuilder<Object.AdmsResultCount>.CreateBuilder(rdr);
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
        public static Object.AdmsResultCount SelectResultCountAfterProcessToLive(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberFinal_Select_AdmsResultCount_AfterProcessToLive";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            Object.AdmsResultCount retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.AdmsResultCount();
                        DynamicBuilder<Object.AdmsResultCount> builder = DynamicBuilder<Object.AdmsResultCount>.CreateBuilder(rdr);
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
        public static bool SaveBulkUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SaveBulkUpdate";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SaveBulkInsert(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SaveBulkInsert";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SaveDQMClean(KMPlatform.Object.ClientConnections client, string processCode, string fileType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SaveDQMClean";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileType", fileType);
            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool NullifyKMPSGroupEmails(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_NullifyKMPSGroupEmails";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SetMissingMaster(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SubscriberFinal_SetMissingMaster";
            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SetOneMaster(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SetOneMaster";
            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static int Save(Entity.SubscriberFinal subscriber, KMPlatform.Object.ClientConnections client)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "e_SubscriberFinal_Save"
            };

            var nonNullFields = new[]
            {
                "SubGenSubscriberID",
                "SubGenSubscriptionID",
                "SubGenPublicationID",
                "SubGenMailingAddressId",
                "SubGenBillingAddressId",
                "IssuesLeft",
                "UnearnedReveue",
            };

            foreach (var prop in typeof(Entity.SubscriberFinal).GetProperties())
            {
                if (nonNullFields.Contains(prop.Name))
                {
                    cmd.Parameters.AddWithValue($"{prop.Name}", prop.GetValue(subscriber));
                }
                else
                {
                    cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(subscriber) ?? DBNull.Value);
                }
            }

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool AddressSearch(string address, string mailstop, string city, string state, string zip, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_AddressSearch";
            cmd.Parameters.Add(new SqlParameter("@Address", address));
            cmd.Parameters.Add(new SqlParameter("@Mailstop", mailstop));
            cmd.Parameters.Add(new SqlParameter("@City", city));
            cmd.Parameters.Add(new SqlParameter("@State", state));
            cmd.Parameters.Add(new SqlParameter("@Zip", zip));
            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #region DataCompare
        public static void SetPubCode(KMPlatform.Object.ClientConnections client, string processCode, int productId, int brandId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberFinal_SetPubCode";
            cmd.Parameters.AddWithValue("ProcessCode", processCode);
            cmd.Parameters.AddWithValue("ProductId", productId);
            cmd.Parameters.AddWithValue("BrandId", brandId);

            cmd.CommandTimeout = 0;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion
        public static DataTable GetEmailListFromEcn(int groupID)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", groupID);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());

            return dt;
        }
        public static void ECN_ThirdPartySuppresion(StringBuilder xml, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Meister_Demo35OptOuts";
            cmd.Parameters.AddWithValue("@xml", xml.ToString());
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
    
            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            //LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts - ccp_Meister_Demo35OptOuts");
        }
        public static void ECN_OtherProductsSuppression(StringBuilder xml, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Meister_Demo34OptOuts";
            cmd.Parameters.AddWithValue("@xml", xml.ToString());
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            //LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts - ccp_Meister_Demo35OptOuts");
        }
    }
}
