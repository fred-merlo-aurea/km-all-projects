using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class Table
    {
        public static List<Object.Table> Select()
        {
            List<Object.Table> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Table_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static DataTable Select(string table, int client, int file)
        {
            DataTable retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Table_ExportData";
            cmd.Parameters.Add(new SqlParameter("@Table", table));
            cmd.Parameters.Add(new SqlParameter("@ClientID", client));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", file));

            retItem = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.UAS.ToString());
            return retItem;
        }

        private static Object.Table Get(SqlCommand cmd)
        {
            Object.Table retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.Table();
                        DynamicBuilder<Object.Table> builder = DynamicBuilder<Object.Table>.CreateBuilder(rdr);
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
        private static List<Object.Table> GetList(SqlCommand cmd)
        {
            List<Object.Table> retList = new List<Object.Table>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.Table retItem = new Object.Table();
                        DynamicBuilder<Object.Table> builder = DynamicBuilder<Object.Table>.CreateBuilder(rdr);
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
