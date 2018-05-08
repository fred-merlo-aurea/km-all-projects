using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.DataAccess
{
    public class CampaignFilterDetail
    {
        #region Data
        public static List<Entity.CampaignFilterDetail> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.CampaignFilterDetail> retList = new List<Entity.CampaignFilterDetail>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from CampaignFilterDetails", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.CampaignFilterDetail> builder = DynamicBuilder<Entity.CampaignFilterDetail>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Entity.CampaignFilterDetail x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }
        #endregion

        #region CRUD
        public static void saveCampaignDetails(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID, string xmlSubscriber)
        {
            SqlCommand cmd = new SqlCommand("sp_saveCampaignDetails");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.AddWithValue("@CampaignFilterID", CampaignFilterID);
            cmd.Parameters.AddWithValue("@xmlSubscriber", xmlSubscriber);
            DataFunctions.Execute(cmd);
        }
        #endregion
    }
}
