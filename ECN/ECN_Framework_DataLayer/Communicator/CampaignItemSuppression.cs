using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Runtime.Serialization;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemSuppression
    {
        private static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemSuppression>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemSuppression retItem = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemSuppression>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemSuppression Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemSuppression retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemSuppression>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.CampaignItemSuppression GetByCampaignItemSuppressionID(int campaignItemSuppressionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSuppression_Select_CampaignItemSuppressionID";
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", campaignItemSuppressionID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSuppression_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return GetList(cmd);
        }

        public static bool Exists(int campaignItemID, int campaignItemSuppressionID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 cis.CampaignItemSuppressionID) FROM CampaignItemSuppression cis WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON cis.CampaignItemID = ci.CampaignItemID " +
                              "JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID WHERE cis.CampaignItemID = @CampaignItemID and cis.CampaignItemSuppressionID = @CampaignItemSuppressionID AND c.CustomerID = @CustomerID and cis.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", campaignItemSuppressionID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int campaignItemID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 cis.CampaignItemSuppressionID FROM CampaignItemSuppression cis WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON cis.CampaignItemID = ci.CampaignItemID " +
                              "JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID WHERE cis.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID and cis.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", campaignItemID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemSuppression suppression)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSuppression_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemSuppressionID", (object)suppression.CampaignItemSuppressionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)suppression.CampaignItemID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)suppression.GroupID ?? DBNull.Value));
            if (suppression.CampaignItemSuppressionID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)suppression.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)suppression.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()); 
        }

        public static void Delete(int campaignItemID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSuppression_Delete_All";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int campaignItemID, int campaignItemSuppressionID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSuppression_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", campaignItemSuppressionID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }


    }
}
