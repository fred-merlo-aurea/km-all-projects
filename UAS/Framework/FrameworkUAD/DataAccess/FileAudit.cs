using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class FileAudit
    {
        public static List<Object.FileAudit> SelectDistinctProcessCodePerSourceFile(KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileAudit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FileAudit_SelectDistinctProcessCodePerSourceFile";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);

            return retItem;
        }
        public static List<Object.FileAudit> Get(SqlCommand cmd)
        {
            List<Object.FileAudit> retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new List<Object.FileAudit>();
                        DynamicBuilder<List<Object.FileAudit>> builder = DynamicBuilder<List<Object.FileAudit>>.CreateBuilder(rdr);
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
        public static List<Object.FileAudit> GetList(SqlCommand cmd)
        {
            List<Object.FileAudit> retList = new List<Object.FileAudit>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.FileAudit retItem = new Object.FileAudit();
                        DynamicBuilder<Object.FileAudit> builder = DynamicBuilder<Object.FileAudit>.CreateBuilder(rdr);
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
