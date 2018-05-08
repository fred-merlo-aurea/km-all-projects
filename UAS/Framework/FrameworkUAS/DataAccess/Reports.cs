using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class Reports
    {
        #region Client File Log
        public static List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName)
        {
            List<Report.ClientFileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_ClientFileLog_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            retItem = GetClientFileLogList(cmd);
            return retItem;
        }
        public static List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName, DateTime logDate)
        {
            List<Report.ClientFileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_ClientFileLog_ClientID_LogDate";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@LogDate", logDate);
            retItem = GetClientFileLogList(cmd);
            return retItem;
        }
        public static List<Report.ClientFileLog> GetClientFileLog(int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            List<Report.ClientFileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_ClientFileLog_ClientID_StartDate_EndDate";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            retItem = GetClientFileLogList(cmd);
            return retItem;
        }
        private static List<Report.ClientFileLog> GetClientFileLogList(SqlCommand cmd)
        {
            List<Report.ClientFileLog> retList = new List<Report.ClientFileLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Report.ClientFileLog retItem = new Report.ClientFileLog();
                        DynamicBuilder<Report.ClientFileLog> builder = DynamicBuilder<Report.ClientFileLog>.CreateBuilder(rdr);
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
        #region File Count
        public static List<Report.FileCount> GetFileCount(int clientID, string clientName)
        {
            List<Report.FileCount> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_FileCounts_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            retItem = GetFileCountList(cmd);
            return retItem;
        }
        public static List<Report.FileCount> GetFileCount(int clientID, string clientName, DateTime startDate, DateTime endDate)
        {
            List<Report.FileCount> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_FileCounts_ClientID_DateRange";
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            retItem = GetFileCountList(cmd);
            return retItem;
        }
        public static List<Report.FileCount> GetFileCount(DateTime startDate, DateTime endDate)
        {
            List<Report.FileCount> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_FileCounts_DateRange";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            retItem = GetFileCountList(cmd);
            return retItem;
        }
        private static List<Report.FileCount> GetFileCountList(SqlCommand cmd)
        {
            List<Report.FileCount> retList = new List<Report.FileCount>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Report.FileCount retItem = new Report.FileCount();
                        DynamicBuilder<Report.FileCount> builder = DynamicBuilder<Report.FileCount>.CreateBuilder(rdr);
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
        #region TransformationCount
        public static DataTable GetTransformationCount(int clientID, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_TransformationCount_clientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        #endregion
    }
}
