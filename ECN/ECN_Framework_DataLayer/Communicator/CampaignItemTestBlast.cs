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
    public class CampaignItemTestBlast
    {
        private static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTestBlast retItem = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemTestBlast Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemTestBlast retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.CampaignItemTestBlast GetByCampaignItemTestBlastID(int CampaignItemTestBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Select_CampaignItemTestBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", CampaignItemTestBlastID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemTestBlast GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Get(cmd);
        }

        public static bool Exists(int campaignItemID, int CampaignItemTestBlastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 citb.CampaignItemTestBlastID FROM CampaignItemTestBlast citb WITH (NOLOCK) " +
                              "JOIN CampaignItem ci WITH (NOLOCK) ON citb.CampaignItemID = ci.CampaignItemID " +
                              "JOIN Campaign ca WITH (NOLOCK) ON ci.CampaignID = ca.CampaignID " +
                              "JOIN ecn5_accounts..Customer c with (nolock) on ca.CustomerID = c.CustomerID " +
                              "WHERE citb.CampaignItemID = @CampaignItemID AND citb.CampaignItemTestBlastID = @CampaignItemTestBlastID AND c.CustomerID = @CustomerID and citb.IsDeleted = 0 and ci.IsDeleted = 0 and ca.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", CampaignItemTestBlastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemTestBlastID", (object)ciBlast.CampaignItemTestBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)ciBlast.CampaignItemID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)ciBlast.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)ciBlast.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", ciBlast.HasEmailPreview));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)ciBlast.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemTestBlastType", (object)ciBlast.CampaignItemTestBlastType ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)ciBlast.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", (object)ciBlast.EmailSubject ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromName", (object)ciBlast.FromName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", (object)ciBlast.FromEmail ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", (object)ciBlast.ReplyTo ?? DBNull.Value));
            if (ciBlast.CampaignItemTestBlastID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ciBlast.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ciBlast.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int campaignItemID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Delete_All";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int campaignItemID, int CampaignItemTestBlastID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", CampaignItemTestBlastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateBlastID(int CampaignItemTestBlastID, int blastID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTestBlast_UpdateBlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", CampaignItemTestBlastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }


    }
}

