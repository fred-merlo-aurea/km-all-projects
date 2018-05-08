using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DQMQue
    {
        public static List<Entity.DQMQue> Select(int clientID, bool isDemo, bool isADMS)
        {
            List<Entity.DQMQue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@IsDemo", isDemo);
            cmd.Parameters.AddWithValue("@IsADMS", isADMS);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.DQMQue Select(string processCode)
        {
            Entity.DQMQue retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);

            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.DQMQue> Select(bool isDemo, bool isADMS, bool isQued)
        {
            List<Entity.DQMQue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Select_IsQued";
            cmd.Parameters.AddWithValue("@IsDemo", isDemo);
            cmd.Parameters.AddWithValue("@IsADMS", isADMS);
            cmd.Parameters.AddWithValue("@IsQued", isQued);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DQMQue> Select(int clientID, bool isDemo, bool isADMS, bool isQued)
        {
            List<Entity.DQMQue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Select_ClientID_IsQued";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@IsDemo", isDemo);
            cmd.Parameters.AddWithValue("@IsADMS", isADMS);
            cmd.Parameters.AddWithValue("@IsQued", isQued);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.DQMQue> Select(bool isDemo, bool isADMS, bool isQued, bool isCompleted)
        {
            List<Entity.DQMQue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Select_IsQued_IsCompleted";
            cmd.Parameters.AddWithValue("@IsDemo", isDemo);
            cmd.Parameters.AddWithValue("@IsADMS", isADMS);
            cmd.Parameters.AddWithValue("@IsQued", isQued);
            cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);

            retItem = GetList(cmd);
            return retItem;
        }
        private static List<Entity.DQMQue> GetList(SqlCommand cmd)
        {
            List<Entity.DQMQue> retList = new List<Entity.DQMQue>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DQMQue retItem = new Entity.DQMQue();
                        DynamicBuilder<Entity.DQMQue> builder = DynamicBuilder<Entity.DQMQue>.CreateBuilder(rdr);
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
        private static Entity.DQMQue Get(SqlCommand cmd)
        {
            Entity.DQMQue retItem = new Entity.DQMQue();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        DynamicBuilder<Entity.DQMQue> builder = DynamicBuilder<Entity.DQMQue>.CreateBuilder(rdr);
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

        public static bool Save(Entity.DQMQue x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_Save";
            cmd.Parameters.AddWithValue("@ProcessCode", x.ProcessCode);
            cmd.Parameters.AddWithValue("@ClientID", x.ClientID);
            cmd.Parameters.AddWithValue("@IsDemo", x.IsDemo);
            cmd.Parameters.AddWithValue("@IsADMS", x.IsADMS);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@IsQued", x.IsQued);
            cmd.Parameters.AddWithValue("@DateQued", (object)x.DateQued ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsCompleted", x.IsCompleted);
            cmd.Parameters.AddWithValue("@DateCompleted", (object)x.DateCompleted ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SourceFileId", x.SourceFileId);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateComplete(string processCode, string msg = "", bool createLog = false)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DQMQue_UpdateComplete";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
    }
}
