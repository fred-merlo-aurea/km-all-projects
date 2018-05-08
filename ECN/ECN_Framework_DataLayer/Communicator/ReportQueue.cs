using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public class ReportQueue
    {
        public static List<ECN_Framework_Entities.Communicator.ReportQueue> GetReportHistory(int ReportScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_Select_ReportScheduleID";
            cmd.Parameters.AddWithValue("@ReportScheduleID", ReportScheduleID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.ReportQueue GetNextPendingForReportSchedule(int ReportScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_SelectNextPending_ReportScheduleID";
            cmd.Parameters.AddWithValue("@ReportScheduleID", ReportScheduleID);

            return Get(cmd);
        }

        public static void ResendReport(int ReportQueueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_Resend";
            cmd.Parameters.AddWithValue("@ReportQueueID", ReportQueueID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete_ReportScheduleID(int ReportScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_Delete_ReportScheduleID";
            cmd.Parameters.AddWithValue("@ReportScheduleID", ReportScheduleID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateStatus(int RQID, string Status,string FailureReason)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_UpdateStatus";
            cmd.Parameters.AddWithValue("@ReportQueueID", RQID);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@FailureReason", FailureReason);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.ReportQueue rq)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_Save";
            if (rq.ReportQueueID > 0)
                cmd.Parameters.AddWithValue("@ReportQueueID", rq.ReportQueueID);
            cmd.Parameters.AddWithValue("@ReportScheduleID", rq.ReportScheduleID);
            cmd.Parameters.AddWithValue("@ReportID", rq.ReportID);
            cmd.Parameters.AddWithValue("@SendTime", rq.SendTime);
            cmd.Parameters.AddWithValue("@Status", rq.Status);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool Exists(ECN_Framework_Entities.Communicator.ReportQueue rq)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_Exists_Date_Report";
            
            cmd.Parameters.AddWithValue("@ReportScheduleID", rq.ReportScheduleID);
            cmd.Parameters.AddWithValue("@ReportID", rq.ReportID);
            cmd.Parameters.AddWithValue("@SendTime", rq.SendTime);
            //cmd.Parameters.AddWithValue("@Status", rq.Status);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.ReportQueue GetNextToSend(int? blastid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportQueue_GetNextToSend";
            if (blastid.HasValue)
                cmd.Parameters.AddWithValue("@BlastID", blastid);

            return Get(cmd);
        }


        private static ECN_Framework_Entities.Communicator.ReportQueue Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ReportQueue retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ReportQueue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ReportQueue>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.ReportQueue> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ReportQueue> retList = new List<ECN_Framework_Entities.Communicator.ReportQueue>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ReportQueue retItem = new ECN_Framework_Entities.Communicator.ReportQueue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ReportQueue>.CreateBuilder(rdr);
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
