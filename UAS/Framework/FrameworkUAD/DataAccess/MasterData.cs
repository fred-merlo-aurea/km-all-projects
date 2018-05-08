using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class MasterData
    {
        public static List<Object.MasterData> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.MasterData> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_MasterData_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Object.MasterData Get(SqlCommand cmd)
        {
            Object.MasterData retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.MasterData();
                        DynamicBuilder<Object.MasterData> builder = DynamicBuilder<Object.MasterData>.CreateBuilder(rdr);
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
        public static List<Object.MasterData> GetList(SqlCommand cmd)
        {
            List<Object.MasterData> retList = new List<Object.MasterData>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.MasterData retItem = new Object.MasterData();
                        DynamicBuilder<Object.MasterData> builder = DynamicBuilder<Object.MasterData>.CreateBuilder(rdr);
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
