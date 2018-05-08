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
    public class UploadLog
    {

        public static int Save(ECN_Framework_Entities.Communicator.UploadLog log)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UploadLog_Save";
            cmd.Parameters.Add(new SqlParameter("@UploadID", log.UploadID));
            cmd.Parameters.Add(new SqlParameter("@UserID", log.UserID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", log.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Path", log.Path));
            cmd.Parameters.Add(new SqlParameter("@FileName", log.FileName));
            cmd.Parameters.Add(new SqlParameter("@PageSource", log.PageSource));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
