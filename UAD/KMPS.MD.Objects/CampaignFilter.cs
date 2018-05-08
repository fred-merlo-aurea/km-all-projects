using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class CampaignFilter
    {
        public CampaignFilter() { }
        #region Properties
        public int CampaignFilterID { get; set; }
        public int CampaignID { get; set; }
        public string FilterName { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Count { get; set; }
        public string PromoCode { get; set; }
        #endregion

        #region Data
        public static List<CampaignFilter> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<CampaignFilter> retList = new List<CampaignFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from CampaignFilter", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CampaignFilter> builder = DynamicBuilder<CampaignFilter>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    CampaignFilter x = builder.Build(rdr);
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

        public static CampaignFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
            CampaignFilter cf = Get(clientconnection).Find(x => x.CampaignFilterID == CampaignFilterID);
            return cf;
        }

        public static List<CampaignFilter> GetByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            List<CampaignFilter> retList = new List<CampaignFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select pf.CampaignFilterID, filtername, pf.CampaignID, count(CampaignFilterDetailsID) as count, promocode from CampaignFilter pf left join CampaignFilterdetails pfd on pfd.CampaignFilterID = pf.CampaignFilterID 	where pf.CampaignID = " + CampaignID + " group by pf.CampaignFilterID,  filtername, pf.CampaignID, promocode order by filtername", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CampaignFilter> builder = DynamicBuilder<CampaignFilter>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    CampaignFilter x = builder.Build(rdr);
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
        
        public static int CampaignFilterExists(KMPlatform.Object.ClientConnections clientconnection, string filtername, int CampaignID)
        {
            SqlCommand cmd = new SqlCommand("SELECT CampaignFilterID FROM CampaignFilter WHERE filterName=@FilterName and CampaignID=@CampaignID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterName", filtername));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion

        #region CRUD
        public static int Insert(KMPlatform.Object.ClientConnections clientconnection, string filtername, int UserID, int CampaignID, string PromoCode)
        {
            SqlCommand cmd = new SqlCommand("e_CampaignFilter_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterName", filtername));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Parameters.Add(new SqlParameter("@PromoCode", PromoCode));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
            SqlCommand cmd = new SqlCommand("delete from CampaignFilterdetails where CampaignFilterID=@CampaignFilterID; delete from CampaignFilter where CampaignFilterID=@CampaignFilterID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignFilterID", CampaignFilterID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}