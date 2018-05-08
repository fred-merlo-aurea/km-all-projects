using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace FrameworkUAD.DataAccess
{
    public class Table
    {
        public static List<Object.Table> Select(string dbName)
        {
            List<Object.Table> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TABLE_NAME as 'TableName' " +
                              "FROM INFORMATION_SCHEMA.TABLES With(NoLock) " +
                              "WHERE TABLE_TYPE = 'BASE TABLE'";
            retItem = GetList(cmd, dbName);
            return retItem;
        }

        public static List<Object.Table> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.Table> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Table_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static DataTable Select(string dbName, string table, string pubCode)
        {
            string sql = "DECLARE @Table varchar(100) = '" + table + "', @PubCode varchar(50) = '" + pubCode + "' " +
                            @"IF EXISTS(SELECT * FROM sys.columns 
                                    WHERE (UPPER([name]) = 'PUBCODE' OR UPPER([name]) = 'PUBID') AND [object_id] = OBJECT_ID(@Table))
                            BEGIN    
                            IF (@PubCode != '')
	                            BEGIN
		                            DECLARE @PubID int = (SELECT PubID FROM Pubs With(NoLock) WHERE PubCode = @PubCode)
		                            IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubID%')
		                            BEGIN
			                            EXEC('SELECT * FROM ' + @Table + ' WHERE PubID = ' + @PubID);
		                            END
		                            ELSE
		                            BEGIN
			                            IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubCode%')
			                            BEGIN
				                            EXEC('SELECT * FROM ' + @Table + ' WHERE PubCode = ' + @PubCode);
			                            END
			                            ELSE
			                            BEGIN
				                            EXEC('SELECT * FROM ' + @Table);
			                            END
		                            END
	                            END
	                            ELSE
	                            BEGIN
		                            EXEC('SELECT * FROM ' + @Table);
	                            END
                            END
                            ELSE
                            BEGIN
	                            BEGIN
		                            EXEC('SELECT * FROM ' + @Table);
	                            END
                            END";
            SqlConnection c = KM.Common.DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAD_Master.ToString());
            string conn = c.ConnectionString.Replace("master", dbName);
            SqlConnection sqlConn = new SqlConnection(conn);
            //SqlConnection conn = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
            return KMCommonDataFunctions.GetDataTable(sql, sqlConn);
        }

        public static DataTable Select(KMPlatform.Object.ClientConnections client, string dbName, string table, string pubCode)
        {
            DataTable retItem = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UADTable_ExportData";
            cmd.Parameters.Add(new SqlParameter("@Table", table));
            cmd.Parameters.Add(new SqlParameter("@PubCode", pubCode));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }

        private static Object.Table Get(SqlCommand cmd, string dbName)
        {
            Object.Table retItem = null;
            try
            {
                using (var rdr = KMCommonDataFunctions.ExecuteReader(
                    cmd, DataFunctions.ConnectionString.UAD_Master.ToString(), dbName))
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
        private static List<Object.Table> GetList(SqlCommand cmd, string dbName)
        {
            List<Object.Table> retList = new List<Object.Table>();
            try
            {
                using (var rdr = KMCommonDataFunctions.ExecuteReader(
                    cmd, DataFunctions.ConnectionString.UAD_Master.ToString(), dbName))
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
        private static List<Object.Table> GetList(SqlCommand cmd)
        {
            List<Object.Table> retList = new List<Object.Table>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
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
