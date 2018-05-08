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
    public class CampaignItemOptOutGroup
    {
        public static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetByCampaignItemID(int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemOptOutGroup_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            return GetList(cmd);
            
        }

        public static void Save(ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup CampaignItemOptOutGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemOptOutGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", CampaignItemOptOutGroup.CampaignItemID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", CampaignItemOptOutGroup.GroupID));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object)CampaignItemOptOutGroup.CreatedUserID ?? DBNull.Value));
            DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int CampaignItemID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemOptOutGroup_Delete_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int CampaignItemOptOutID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemOptOutGroup_Delete";
            cmd.Parameters.AddWithValue("@CampaignItemOptOutID", CampaignItemOptOutID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup retItem = new ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>.CreateBuilder(rdr);
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
