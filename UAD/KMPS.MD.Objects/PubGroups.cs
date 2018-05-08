using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class PubGroups
    {
        #region Properties
        public int PubID { get; set; }
        public int GroupID { get; set; }
        #endregion

        #region Data
        public static List<PubGroups> Get(KMPlatform.Object.ClientConnections clientconnection, int PubID)
        {
            List<PubGroups> retList = new List<PubGroups>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from PubGroups Where PubId = @PubID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubGroups> builder = DynamicBuilder<PubGroups>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    PubGroups x = builder.Build(rdr);
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
        public void Save(KMPlatform.Object.ClientConnections clientconnection)
        {
            SqlCommand cmd = new SqlCommand("sp_SavePubGroups");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", GroupID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int PubID)
        {
            SqlCommand cmd = new SqlCommand("Delete from PubGroups where PubID = @PubID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}