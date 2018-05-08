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
    public class CampaignFilter
    {
        #region Data
        public static List<Entity.CampaignFilter> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.CampaignFilter> retList = new List<Entity.CampaignFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from CampaignFilter", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.CampaignFilter> builder = DynamicBuilder<Entity.CampaignFilter>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Entity.CampaignFilter x = builder.Build(rdr);
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

        public static Entity.CampaignFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
            Entity.CampaignFilter cf = Get(clientconnection).Find(x => x.CampaignFilterID == CampaignFilterID);
            return cf;
        }

        public static List<Entity.CampaignFilter> GetByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            List<Entity.CampaignFilter> retList = new List<Entity.CampaignFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select pf.CampaignFilterID, filtername, pf.CampaignID, count(CampaignFilterDetailsID) as count, promocode from CampaignFilter pf left join CampaignFilterdetails pfd on pfd.CampaignFilterID = pf.CampaignFilterID 	where pf.CampaignID = " + CampaignID + " group by pf.CampaignFilterID,  filtername, pf.CampaignID, promocode order by filtername", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.CampaignFilter> builder = DynamicBuilder<Entity.CampaignFilter>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Entity.CampaignFilter x = builder.Build(rdr);
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
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@FilterName", filtername));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        #endregion

        #region CRUD
        public static int Insert(KMPlatform.Object.ClientConnections clientconnection, string filtername, int UserID, int CampaignID, string PromoCode)
        {
            SqlCommand cmd = new SqlCommand("e_CampaignFilter_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@FilterName", filtername));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Parameters.Add(new SqlParameter("@PromoCode", PromoCode));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int CampaignFilterID)
        {
            SqlCommand cmd = new SqlCommand("delete from CampaignFilterdetails where CampaignFilterID=@CampaignFilterID; delete from CampaignFilter where CampaignFilterID=@CampaignFilterID");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@CampaignFilterID", CampaignFilterID));
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
