using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class SmartFormActivityLog //similar to smartformsTracking
    {
        public static int Insert(ECN_Framework_Entities.Communicator.SmartFormActivityLog sfal)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormActivityLog_Insert";
            cmd.Parameters.Add(new SqlParameter("@SFID", sfal.SFID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", sfal.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", sfal.GroupID));
            cmd.Parameters.Add(new SqlParameter("@EmailID", sfal.EmailID));
            cmd.Parameters.Add(new SqlParameter("@EmailType", sfal.EmailType));
            cmd.Parameters.Add(new SqlParameter("@EmailTo", sfal.EmailTo));
            cmd.Parameters.Add(new SqlParameter("@EmailFrom", sfal.EmailFrom));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", sfal.EmailSubject));
            cmd.Parameters.Add(new SqlParameter("@SendTime", sfal.SendTime));
            cmd.Parameters.Add(new SqlParameter("@UserID", (object)sfal.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
