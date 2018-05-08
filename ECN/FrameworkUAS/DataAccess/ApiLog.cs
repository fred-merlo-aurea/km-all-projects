using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ApiLog
    {
        public static int Save(Entity.ApiLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApiLog_Save";
            cmd.Parameters.AddWithValue("@ApiLogId", x.ApiLogId);
            cmd.Parameters.AddWithValue("@ClientID", x.ClientID);
            cmd.Parameters.AddWithValue("@AccessKey", x.AccessKey);
            cmd.Parameters.AddWithValue("@RequestFromIP", x.RequestFromIP);
            cmd.Parameters.AddWithValue("@ApiId", x.ApiId);
            cmd.Parameters.AddWithValue("@Entity", x.Entity);
            cmd.Parameters.AddWithValue("@Method", x.Method);
            cmd.Parameters.AddWithValue("@ErrorMessage", x.ErrorMessage);
            cmd.Parameters.AddWithValue("@RequestData", x.RequestData);
            cmd.Parameters.AddWithValue("@ResponseData", x.ResponseData);
            cmd.Parameters.AddWithValue("@RequestStartDate", x.RequestStartDate);
            cmd.Parameters.AddWithValue("@RequestStartTime", x.RequestStartTime);
            cmd.Parameters.Add(new SqlParameter("@RequestEndDate", (object)x.RequestEndDate ?? DBNull.Value));//null
            cmd.Parameters.Add(new SqlParameter("@RequestEndTime", (object)x.RequestEndTime ?? DBNull.Value));//null

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
