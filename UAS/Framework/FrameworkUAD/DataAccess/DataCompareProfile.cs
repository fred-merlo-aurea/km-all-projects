using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class DataCompareProfile
    {
        public static Entity.DataCompareProfile Get(SqlCommand cmd)
        {
            Entity.DataCompareProfile retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareProfile();
                        DynamicBuilder<Entity.DataCompareProfile> builder = DynamicBuilder<Entity.DataCompareProfile>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareProfile> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareProfile> retList = new List<Entity.DataCompareProfile>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareProfile retItem = new Entity.DataCompareProfile();
                        DynamicBuilder<Entity.DataCompareProfile> builder = DynamicBuilder<Entity.DataCompareProfile>.CreateBuilder(rdr);
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
        public static void InsertFromSubscriberFinal(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareProfile_InsertFromSubFinal_ProcessCode";
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static List<Entity.DataCompareProfile> Select(KMPlatform.Object.ClientConnections client, string processCode)
        {
            List<Entity.DataCompareProfile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareProfile_Select_ProcessCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            retItem = GetList(cmd);
            return retItem;
        }
        public static int GetDataCompareCount(KMPlatform.Object.ClientConnections client, string processCode, int dcTargetCodeId, int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dc_GetDataCompareCount";
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Parameters.AddWithValue("@dcTargetCodeId", dcTargetCodeId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static DataTable GetDataCompareData(KMPlatform.Object.ClientConnections client, string processCode, int dcTargetCodeId, string matchType, int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dc_GetDataCompareData";
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Parameters.AddWithValue("@dcTargetCodeId", dcTargetCodeId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@MatchType", matchType);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }

        public static DataTable GetDataCompareSummary(KMPlatform.Object.ClientConnections client, string processCode, int dcTargetCodeId, string matchType, int id = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dc_GetDataCompareSummary";
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Parameters.AddWithValue("@dcTargetCodeId", dcTargetCodeId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@MatchType", matchType);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }
    }
}
