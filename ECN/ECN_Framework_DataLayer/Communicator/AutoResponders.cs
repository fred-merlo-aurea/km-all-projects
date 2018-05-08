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
    public class AutoResponders
    {
        public static int Save(ECN_Framework_Entities.Communicator.AutoResponders responder)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AutoResponders_Save";
            cmd.Parameters.Add(new SqlParameter("@AutoResponderID", responder.AutoResponderID));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)responder.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailServer", responder.MailServer));
            cmd.Parameters.Add(new SqlParameter("@AccountName", responder.AccountName));
            cmd.Parameters.Add(new SqlParameter("@AccountPasswd", responder.AccountPasswd));
            cmd.Parameters.Add(new SqlParameter("@ForwardTo", responder.ForwardTo));
            if (responder.AutoResponderID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)responder.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)responder.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool Exists(int autoResponderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AutoResponders_Exists";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@AutoResponderID", autoResponderID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
    }
}
