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
    public class CampaignItemBlastRefBlast
    {
        private static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast retItem = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast GetByCampaignItemBlastRefBlastID(int campaignItemBlastRefBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "e_CampaignItemBlastRefBlast_Select_CampaignItemBlastRefBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemBlastRefBlastID", campaignItemBlastRefBlastID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> GetByCampaignItemBlastID(int campaignItemBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastRefBlast_Select_CampaignItemBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            return GetList(cmd);
        }

        public static bool Exists(int campaignItemBlastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 cibrb.CampaignItemBlastRefBlastID FROM CampaignItemBlastRefBlast cibrb WITH (NOLOCK) " +
                               "JOIN CampaignItemBlast cib WITH (NOLOCK) ON cibrb.CampaignItemBlastID = cib.CampaignItemBlastID " +              
                               "JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID " +
                               "JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID WHERE cibrb.CampaignItemBlastID = @CampaignItemBlastID AND c.CustomerID = @CustomerID and cibrb.IsDeleted = 0 and cib.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int campaignItemBlastID, int campaignItemBlastRefBlastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 cibrb.CampaignItemBlastRefBlastID FROM CampaignItemBlastRefBlast cibrb WITH (NOLOCK) " +
                               "JOIN CampaignItemBlast cib WITH (NOLOCK) ON cibrb.CampaignItemBlastID = cib.CampaignItemBlastID " +
                               "JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID " +
                               "JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID WHERE cibrb.CampaignItemBlastID = @CampaignItemBlastID and cibrb.CampaignItemBlastRefBlastID = @CampaignItemBlastRefBlastID AND c.CustomerID = @CustomerID and cibrb.IsDeleted = 0 and cib.IsDeleted = 0 and ci.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CampaignItemBlastRefBlastID", campaignItemBlastRefBlastID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemBlastID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast refBlast)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastRefBlast_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemBlastRefBlastID", (object)refBlast.CampaignItemBlastRefBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemBlastID", (object)refBlast.CampaignItemBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RefBlastID", (object)refBlast.RefBlastID ?? DBNull.Value));
            if (refBlast.CampaignItemBlastRefBlastID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)refBlast.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)refBlast.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int campaignItemBlastID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastRefBlast_Delete_All";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int campaignItemBlastID, int campaignItemBlastRefBlastID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastRefBlast_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            cmd.Parameters.AddWithValue("@CampaignItemBlastRefBlastID", campaignItemBlastRefBlastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }


    }
}
