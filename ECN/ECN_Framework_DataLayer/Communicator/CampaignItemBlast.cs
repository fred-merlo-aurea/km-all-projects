using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class CampaignItemBlast
    {
        private const string ProcedureCampaignItemBlastUpdateBlastId = "e_CampaignItemBlast_UpdateBlastID";
        private const string ProcedureCampaignItemBlastDeleteSingle = "e_CampaignItemBlast_Delete_Single";
        private const string ProcedureCampaignItemBlastDeleteAll = "e_CampaignItemBlast_Delete_All";
        private const string ProcedureSelectCampaignItemBlastId = "e_CampaignItemBlast_Select_CampaignItemBlastID";
        private const string ProcedureSelectCampaignItemId = "e_CampaignItemBlast_Select_CampaignItemID";
        private const string ProcedureSelectBlastId = "e_CampaignItemBlast_Select_BlastID";

        private static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlast retItem = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemBlast>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemBlast Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemBlast retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemBlast>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.CampaignItemBlast GetByCampaignItemBlastID(int campaignItemBlastID)
        {
            var cmd = CommunicatorMethodsHelper.GetCampaignItemBlast(campaignItemBlastID, null, null, ProcedureSelectCampaignItemBlastId);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlast> GetByCampaignItemID(int campaignItemID)
        {
            var cmd = CommunicatorMethodsHelper.GetCampaignItemBlast(null, campaignItemID, null, ProcedureSelectCampaignItemId);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemBlast GetByBlastID(int blastID)
        {
            var cmd = CommunicatorMethodsHelper.GetCampaignItemBlast(null, null, blastID, ProcedureSelectBlastId);
            return Get(cmd);
        }

        public static bool Exists(int campaignItemID, int campaignItemBlastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 cib.CampaignItemBlastID FROM CampaignItemBlast cib WITH (NOLOCK) " +
                              "JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID " +
                              "JOIN Campaign ca WITH (NOLOCK) ON ci.CampaignID = ca.CampaignID " +
                              "JOIN ecn5_accounts..Customer c with (nolock) on ca.CustomerID = c.CustomerID " +
                              "WHERE cib.CampaignItemID = @CampaignItemID AND cib.CampaignItemBlastID = @CampaignItemBlastID AND c.CustomerID = @CustomerID and cib.IsDeleted = 0 and ci.IsDeleted = 0 and ca.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlast_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemBlastID", (object)ciBlast.CampaignItemBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)ciBlast.CampaignItemID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", ciBlast.EmailSubject));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", ciBlast.DynamicFromName));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", ciBlast.DynamicFromEmail));
            cmd.Parameters.Add(new SqlParameter("@DynamicReplyTo", ciBlast.DynamicReplyTo));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)ciBlast.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)ciBlast.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SocialMediaID", (object)ciBlast.SocialMediaID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)ciBlast.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)ciBlast.AddOptOuts_to_MS ?? DBNull.Value));
            if (ciBlast.CampaignItemBlastID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ciBlast.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)ciBlast.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailFrom", (object)ciBlast.EmailFrom ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromName", (object)ciBlast.FromName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", (object)ciBlast.ReplyTo ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int campaignItemID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryCampaignItemBlast(
                campaignItemID, null, null, customerID, userID, ProcedureCampaignItemBlastDeleteAll);
        }

        public static void Delete(int campaignItemID, int campaignItemBlastID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryCampaignItemBlast(
                campaignItemID, campaignItemBlastID, null, customerID, userID, ProcedureCampaignItemBlastDeleteSingle);
        }

        public static void UpdateBlastID(int campaignItemBlastID, int blastID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryCampaignItemBlast(
                null, campaignItemBlastID, blastID, null, userID, ProcedureCampaignItemBlastUpdateBlastId);
        }


    }
}
