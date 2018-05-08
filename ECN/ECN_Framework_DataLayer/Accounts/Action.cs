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
    public class Action
    {
        public static bool Exists(int actionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Exists_ByID";
            cmd.Parameters.AddWithValue("@ActionID", actionID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.Action> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.Action> retItemList = new List<ECN_Framework_Entities.Accounts.Action>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Select";

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Action>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    ECN_Framework_Entities.Accounts.Action retItem = new ECN_Framework_Entities.Accounts.Action();
                    retItem = builder.Build(rdr);
                    retItemList.Add(retItem);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItemList;
        }
    }
}
