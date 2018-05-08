using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberDemographicFinal
    {
        public static List<Entity.SubscriberDemographicFinal> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicFinal_Select_PubID";
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicFinal> SelectSubscriberOriginal(Guid SFRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicFinal_Select_SORecordIdentifier";
            cmd.Parameters.AddWithValue("@SFRecordIdentifier", SFRecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicFinal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicFinal_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicFinal> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicFinal_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        private static Entity.SubscriberDemographicFinal Get(SqlCommand cmd)
        {
            Entity.SubscriberDemographicFinal retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberDemographicFinal();
                        DynamicBuilder<Entity.SubscriberDemographicFinal> builder = DynamicBuilder<Entity.SubscriberDemographicFinal>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberDemographicFinal> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberDemographicFinal> retList = new List<Entity.SubscriberDemographicFinal>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberDemographicFinal retItem = new Entity.SubscriberDemographicFinal();
                        DynamicBuilder<Entity.SubscriberDemographicFinal> builder = DynamicBuilder<Entity.SubscriberDemographicFinal>.CreateBuilder(rdr);
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

        public static int Save(Entity.SubscriberDemographicFinal x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicFinal_Save";
            cmd.Parameters.Add(new SqlParameter("@SDFinalID", x.SDFinalID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@SFRecordIdentifier", x.SFRecordIdentifier));
            cmd.Parameters.Add(new SqlParameter("@MAFField", x.MAFField));
            cmd.Parameters.Add(new SqlParameter("@Value", x.Value));
            cmd.Parameters.Add(new SqlParameter("@NotExists", x.NotExists));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DemographicUpdateCodeId", x.DemographicUpdateCodeId));
            cmd.Parameters.Add(new SqlParameter("@IsAdhoc", x.IsAdhoc));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));
            cmd.Parameters.Add(new SqlParameter("@IsDemoDate", x.IsDemoDate));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
