using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class ReportGroups
    {
        public static bool ExistsByIDName(int reportGroupID, int responseGroupID, string name, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportGroups_Exists_ByIDNameResponseGroupID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@reportGroupID", reportGroupID);
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.Parameters.AddWithValue("@Name", name);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static List<Entity.ReportGroups> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ReportGroups> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportGroups_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ReportGroups Get(SqlCommand cmd)
        {
            Entity.ReportGroups retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ReportGroups();
                        DynamicBuilder<Entity.ReportGroups> builder = DynamicBuilder<Entity.ReportGroups>.CreateBuilder(rdr);
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
        private static List<Entity.ReportGroups> GetList(SqlCommand cmd)
        {
            List<Entity.ReportGroups> retList = new List<Entity.ReportGroups>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ReportGroups retItem = new Entity.ReportGroups();
                        DynamicBuilder<Entity.ReportGroups> builder = DynamicBuilder<Entity.ReportGroups>.CreateBuilder(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections client, Entity.ReportGroups x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportGroups_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ReportGroupID", x.ReportGroupID);
            cmd.Parameters.AddWithValue("@ResponseGroupID", x.ResponseGroupID);
            cmd.Parameters.AddWithValue("@DisplayName", x.DisplayName);
            cmd.Parameters.AddWithValue("@DisplayOrder", x.DisplayOrder);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
