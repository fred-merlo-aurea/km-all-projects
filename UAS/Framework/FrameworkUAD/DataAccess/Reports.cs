using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Reports
    {
        #region rpt_SourceFile_Summary
        public static List<FrameworkUAD.Report.SourceFileSummary> GetSourceFileSummary(int sourceFileID, string processCode, string fileName, KMPlatform.Object.ClientConnections client)
        {
            List<FrameworkUAD.Report.SourceFileSummary> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_SourceFile_Summary";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetListSourceFileSummary(cmd);
            return retItem;
        }
        public static List<FrameworkUAD.Report.SourceFileSummary> GetListSourceFileSummary(SqlCommand cmd)
        {
            List<FrameworkUAD.Report.SourceFileSummary> retList = new List<FrameworkUAD.Report.SourceFileSummary>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        FrameworkUAD.Report.SourceFileSummary retItem = new FrameworkUAD.Report.SourceFileSummary();
                        DynamicBuilder<FrameworkUAD.Report.SourceFileSummary> builder = DynamicBuilder<FrameworkUAD.Report.SourceFileSummary>.CreateBuilder(rdr);
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
        #endregion
        #region rpt_SourceFile_PubCodeSummary
        public static List<FrameworkUAD.Report.SourceFilePubCodeSummary> GetSourceFilePubCodeSummary(int sourceFileID, string processCode, string fileName, KMPlatform.Object.ClientConnections client)
        {
            List<FrameworkUAD.Report.SourceFilePubCodeSummary> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_SourceFile_PubCodeSummary";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetListSourceFilePubCodeSummary(cmd);
            return retItem;
        }
        public static List<FrameworkUAD.Report.SourceFilePubCodeSummary> GetListSourceFilePubCodeSummary(SqlCommand cmd)
        {
            List<FrameworkUAD.Report.SourceFilePubCodeSummary> retList = new List<FrameworkUAD.Report.SourceFilePubCodeSummary>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        FrameworkUAD.Report.SourceFilePubCodeSummary retItem = new FrameworkUAD.Report.SourceFilePubCodeSummary();
                        DynamicBuilder<FrameworkUAD.Report.SourceFilePubCodeSummary> builder = DynamicBuilder<FrameworkUAD.Report.SourceFilePubCodeSummary>.CreateBuilder(rdr);
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
        #endregion
    }
}
