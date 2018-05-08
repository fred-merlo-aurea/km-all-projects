using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueCompError
    {
        public static List<Entity.IssueCompError> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompError> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompError_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.IssueCompError> SelectProcessCode(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompError> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueCompError_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.IssueCompError Get(SqlCommand cmd)
        {
            Entity.IssueCompError retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueCompError();
                        DynamicBuilder<Entity.IssueCompError> builder = DynamicBuilder<Entity.IssueCompError>.CreateBuilder(rdr);
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
        private static List<Entity.IssueCompError> GetList(SqlCommand cmd)
        {
            List<Entity.IssueCompError> retList = new List<Entity.IssueCompError>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueCompError retItem = new Entity.IssueCompError();
                        DynamicBuilder<Entity.IssueCompError> builder = DynamicBuilder<Entity.IssueCompError>.CreateBuilder(rdr);
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
    }
}
