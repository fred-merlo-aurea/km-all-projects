using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class FilterScheduleLog
    {
        public FilterScheduleLog() { }

        #region Properties
        [DataMember]
        public int FilterScheduleLogID { get; set; }
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int DownloadCount { get; set; }
        #endregion

        #region Data

        public static List<FilterScheduleLog> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<FilterScheduleLog> retList = new List<FilterScheduleLog>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterScheduleLog_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterScheduleLog> builder = DynamicBuilder<FilterScheduleLog>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    FilterScheduleLog x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static DataTable GetByDate(KMPlatform.Object.ClientConnections clientconnection, string dt)
        {
            SqlCommand cmd = new SqlCommand("e_FilterScheduleLog_Select_ByDate");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.CommandTimeout = 0;

            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnection)); 
        }

        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterScheduleLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterScheduleLog_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterScheduleID", x.FilterScheduleID));
            cmd.Parameters.Add(new SqlParameter("@StartDate", x.StartDate));
            cmd.Parameters.Add(new SqlParameter("@StartTime", x.StartTime));
            cmd.Parameters.Add(new SqlParameter("@FileName", x.FileName));
            cmd.Parameters.Add(new SqlParameter("@DownloadCount", x.DownloadCount));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))); 
        }
        #endregion
    }
}
