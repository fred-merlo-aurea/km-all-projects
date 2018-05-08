using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ImportErrorSummary
    {
        public static List<Object.ImportErrorSummary> Select(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.ImportErrorSummary> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ImportErrorSummary_Select_SourceFileID_ProcessCode";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Object.ImportErrorSummary> FileValidatorSelect(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.ImportErrorSummary> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ImportErrorSummary_FileValidatorSelect_SourceFileID_ProcessCode";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Object.ImportErrorSummary Get(SqlCommand cmd)
        {
            Object.ImportErrorSummary retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.ImportErrorSummary();
                        DynamicBuilder<Object.ImportErrorSummary> builder = DynamicBuilder<Object.ImportErrorSummary>.CreateBuilder(rdr);
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
        public static List<Object.ImportErrorSummary> GetList(SqlCommand cmd)
        {
            List<Object.ImportErrorSummary> retList = new List<Object.ImportErrorSummary>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.ImportErrorSummary retItem = new Object.ImportErrorSummary();
                        DynamicBuilder<Object.ImportErrorSummary> builder = DynamicBuilder<Object.ImportErrorSummary>.CreateBuilder(rdr);
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
