using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class PubCode
    {
        private static string PubCodesQuery = "SELECT pubcode From pubs With(NoLock)";

        public static List<Object.PubCode> Select(string dbName)
        {
            List<Object.PubCode> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = PubCodesQuery;
            retItem = GetList(cmd, dbName);
            return retItem;
        }
        public static List<Object.PubCode> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.PubCode> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubCode_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        private static Object.PubCode Get(SqlCommand cmd, string dbName)
        {
            Object.PubCode retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, dbName))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.PubCode();
                        DynamicBuilder<Object.PubCode> builder = DynamicBuilder<Object.PubCode>.CreateBuilder(rdr);
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
        private static List<Object.PubCode> GetList(SqlCommand cmd, string dbName)
        {
            List<Object.PubCode> retList = new List<Object.PubCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, dbName))
                {
                    if (rdr != null)
                    {
                        Object.PubCode retItem = new Object.PubCode();
                        DynamicBuilder<Object.PubCode> builder = DynamicBuilder<Object.PubCode>.CreateBuilder(rdr);
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
        private static List<Object.PubCode> GetList(SqlCommand cmd)
        {
            List<Object.PubCode> retList = new List<Object.PubCode>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Object.PubCode retItem = new Object.PubCode();
                    DynamicBuilder<Object.PubCode> builder = DynamicBuilder<Object.PubCode>.CreateBuilder(rdr);
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
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
