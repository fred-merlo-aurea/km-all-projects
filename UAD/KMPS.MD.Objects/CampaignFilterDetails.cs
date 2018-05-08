using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class CampaignFilterDetails
    {
        public CampaignFilterDetails() { }
        #region Properties
        public int CampaignFilterID { get; set; }
        public int CampaignID { get; set; }
        public string FilterName { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        #endregion

        #region Data
        public static List<CampaignFilterDetails> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<CampaignFilterDetails> retList = new List<CampaignFilterDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from CampaignFilterDetails", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CampaignFilterDetails> builder = DynamicBuilder<CampaignFilterDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    CampaignFilterDetails x = builder.Build(rdr);
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
            cmd.Parameters.AddWithValue("@CampaignFilterID", CampaignFilterID);
            cmd.Parameters.AddWithValue("@xmlSubscriber", xmlSubscriber);
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}