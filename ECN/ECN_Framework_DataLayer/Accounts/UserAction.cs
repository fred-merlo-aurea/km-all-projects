using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{

    [Serializable]
    public class UserAction
    {

        public static List<ECN_Framework_Entities.Accounts.UserAction> GetbyUserID(int userID)
        {
            List<ECN_Framework_Entities.Accounts.UserAction> retItemList = new List<ECN_Framework_Entities.Accounts.UserAction>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserAction_Select";
            cmd.Parameters.AddWithValue("@userID", userID);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.UserAction>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Accounts.UserAction retItem = new ECN_Framework_Entities.Accounts.UserAction();
                        retItem = builder.Build(rdr);
                        retItemList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItemList;
        }

        public static void Save(ECN_Framework_Entities.Accounts.UserAction ua)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserAction_Save";
            cmd.Parameters.AddWithValue("@UserActionID", ua.UserActionID);
            cmd.Parameters.AddWithValue("@UserID", ua.UserID);
            cmd.Parameters.AddWithValue("@ActionID", ua.ActionID);
            cmd.Parameters.AddWithValue("@Active", ua.Active);

            ua.UserActionID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString());   
        }
    }
}
