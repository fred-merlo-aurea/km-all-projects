using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class Campaign
    {
        public static List<Entity.Campaign> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Campaign> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static int GetCountByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            SqlCommand cmd = new SqlCommand("select count(distinct subscriptionID) as count from Campaigns p left join Campaignfilter pf on p.CampaignID = pf.CampaignID  left join Campaignfilterdetails pfd on pfd.CampaignfilterID = pf.CampaignfilterID where p.campaignID = @CampaignID group by p.CampaignID, CampaignName");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int CampaignExists(KMPlatform.Object.ClientConnections clientconnection, string Campaignname)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Campaigns WHERE CampaignName=@CampaignName");
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignName", Campaignname));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static Entity.Campaign Get(SqlCommand cmd)
        {
            Entity.Campaign retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Campaign();
                        DynamicBuilder<Entity.Campaign> builder = DynamicBuilder<Entity.Campaign>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static List<Entity.Campaign> GetList(SqlCommand cmd)
        {
            List<Entity.Campaign> retList = new List<Entity.Campaign>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Campaign retItem = new Entity.Campaign();
                        DynamicBuilder<Entity.Campaign> builder = DynamicBuilder<Entity.Campaign>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static int Save(Entity.Campaign x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CampaignID", x.CampaignID);
            cmd.Parameters.AddWithValue("@CampaignName", x.CampaignName);            
            cmd.Parameters.Add(new SqlParameter("@AddedBy", x.AddedBy));
            cmd.Parameters.Add(new SqlParameter("@DateAdded", x.DateAdded));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));            
            cmd.Parameters.Add(new SqlParameter("@UpdatedBy", x.UpdatedBy));
            cmd.Parameters.Add(new SqlParameter("@BrandID", x.BrandID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
