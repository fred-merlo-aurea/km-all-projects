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
    [Serializable]
    public class ReportSchedule
    {
        public static ECN_Framework_Entities.Communicator.ReportSchedule GetByReportScheduleID(int ReportScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Select_ReportScheduleID";
            cmd.Parameters.AddWithValue("@ReportScheduleID", ReportScheduleID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetReportsToSend(DateTime dateToSend)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Select_ToSend";
            cmd.Parameters.AddWithValue("@timeToSend", dateToSend.ToString());
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Save";
            cmd.Parameters.Add(new SqlParameter("@ReportScheduleID", ReportSchedule.ReportScheduleID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)ReportSchedule.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReportID", (object)ReportSchedule.ReportID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)ReportSchedule.StartTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartDate", (object)ReportSchedule.StartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EndDate", (object)ReportSchedule.EndDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ScheduleType", (object)ReportSchedule.ScheduleType ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RecurrenceType", (object)ReportSchedule.RecurrenceType ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunSunday", (object)ReportSchedule.RunSunday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunMonday", (object)ReportSchedule.RunMonday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunTuesday", (object)ReportSchedule.RunTuesday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunWednesday", (object)ReportSchedule.RunWednesday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunThursday", (object)ReportSchedule.RunThursday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunFriday", (object)ReportSchedule.RunFriday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RunSaturday", (object)ReportSchedule.RunSaturday ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MonthScheduleDay", (object)ReportSchedule.MonthScheduleDay ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MonthLastDay", (object)ReportSchedule.MonthLastDay ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", (object)ReportSchedule.FromEmail ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromName", (object)ReportSchedule.FromName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", (object)ReportSchedule.EmailSubject ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ToEmail", (object)ReportSchedule.ToEmail ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReportParameters", (object)ReportSchedule.ReportParameters ?? DBNull.Value));
            if (ReportSchedule.ReportScheduleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ReportSchedule.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ReportSchedule.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)ReportSchedule.BlastID));
            cmd.Parameters.Add(new SqlParameter("@ExportFormat", (object)ReportSchedule.ExportFormat ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ReportSchedule> retList = new List<ECN_Framework_Entities.Communicator.ReportSchedule>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ReportSchedule retItem = new ECN_Framework_Entities.Communicator.ReportSchedule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ReportSchedule>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.ReportSchedule Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ReportSchedule retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ReportSchedule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ReportSchedule>.CreateBuilder(rdr);
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

        public static void Delete(int ReportScheduleID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Delete";
            cmd.Parameters.AddWithValue("@ReportScheduleID", ReportScheduleID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static void DeleteByBlastId(int blastId, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Delete_BlastId";
            cmd.Parameters.AddWithValue("@BlastId", blastId);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetByBlastId(int blastId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Select_BlastId";
            cmd.Parameters.AddWithValue("@BlastId", blastId);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ReportSchedule> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ReportSchedule_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }
    }
}
