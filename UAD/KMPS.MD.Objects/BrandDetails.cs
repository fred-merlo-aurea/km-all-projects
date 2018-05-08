using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class BrandDetails
    {
        public BrandDetails() { }

        #region Properties
        [DataMember]
        public int BrandDetailsID { get; set; }
        [DataMember]
        public int BrandID  { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public int GroupsBrandID { get; set; }
        #endregion

        #region Data
        public static List<BrandDetails> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<BrandDetails> retList = new List<BrandDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from BrandDetails where BrandID = @brandID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@brandID", brandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<BrandDetails> builder = DynamicBuilder<BrandDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    BrandDetails x = builder.Build(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, BrandDetails b)
        {
            SqlCommand cmd = new SqlCommand("e_BrandDetails_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", b.BrandID));
            cmd.Parameters.Add(new SqlParameter("@PubID", b.PubID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            MasterGroup.DeleteCacheByBrandID(clientconnection, brandID);
            MasterCodeSheet.DeleteCacheByBrandID(clientconnection, brandID);
            Pubs.DeleteCacheByBrandID(clientconnection, brandID);
            PubTypes.DeleteCacheByBrandID(clientconnection, brandID);

            SqlCommand cmd = new SqlCommand("e_BrandDetails_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", brandID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void DeleteByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            SqlCommand cmd = new SqlCommand("e_BrandDetails_Delete_ByPubID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
