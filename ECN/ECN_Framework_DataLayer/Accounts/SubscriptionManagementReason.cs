using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class SubscriptionManagementReason
    {
        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementReason_Save";

            cmd.Parameters.AddWithValue("@SubscriptionManagementReasonID", smr.SubscriptionManagementReasonID);
            cmd.Parameters.AddWithValue("@SubscriptionManagementID", smr.SubscriptionManagementID);
            cmd.Parameters.AddWithValue("@Reason", smr.Reason);
            cmd.Parameters.AddWithValue("@IsDeleted", smr.IsDeleted.Value);
            cmd.Parameters.AddWithValue("@SortOrder", smr.SortOrder);
            if (smr.SubscriptionManagementReasonID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", smr.UpdatedUserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", smr.CreatedUserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> GetBySMID(int SMID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementReason_Select_SMID";

            cmd.Parameters.AddWithValue("@SMID", SMID);

            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.SubscriptionManagementReason retItem = new ECN_Framework_Entities.Accounts.SubscriptionManagementReason();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SubscriptionManagementReason>.CreateBuilder(rdr);
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
