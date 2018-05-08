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
    public class CampaignItemLinkTracking
    {
        public static ECN_Framework_Entities.Communicator.CampaignItemLinkTracking GetByCILTID(int CILTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Select_CILTID";
            cmd.Parameters.AddWithValue("@CILTID", CILTID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetByLinkTrackingParamOptionID(int ltpoid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Select_LTPOID";
            cmd.Parameters.AddWithValue("@LTPOID", ltpoid);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemLinkTracking retItem = new ECN_Framework_Entities.Communicator.CampaignItemLinkTracking();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemLinkTracking Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemLinkTracking retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemLinkTracking();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemLinkTracking campaignItemLinkTracking)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Save";
            cmd.Parameters.Add(new SqlParameter("@CILTID", (object)campaignItemLinkTracking.CILTID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)campaignItemLinkTracking.CampaignItemID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LTPID", (object)campaignItemLinkTracking.LTPID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LTPOID", (object)campaignItemLinkTracking.LTPOID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomValue", (object)campaignItemLinkTracking.CustomValue ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void DeleteByCampaignItemID(int campaignItemID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Delete_ByCampaignItemID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteByLTID(int campaignItemID, int LTID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemLinkTracking_Delete_CampaignItemID_LTID";
            cmd.Parameters.AddWithValue("@LTID", LTID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetParamInfo(int blastID, int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItemLinkTracking_GetParamInfo";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
