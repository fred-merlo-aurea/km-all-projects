using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Adhoc
    {
        public Adhoc() { }

        #region Properties
        [DataMember]
        public int AdhocID { get; set; }
        [DataMember]
        public string AdhocName { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string ColumnValue { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ColumnType { get; set; }
        [DataMember]
        public string ColumnName { get; set; }
        #endregion

        #region Data
        public static List<Adhoc> GetByCategoryID(KMPlatform.Object.ClientConnections clientconnection, int categoryID, int brandID, int pubID)
        {
            List<Adhoc> retList = new List<Adhoc>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_Adhoc_Select_CategoryID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Adhoc> builder = DynamicBuilder<Adhoc>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Adhoc x = builder.Build(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Adhoc adhoc)
        {
            SqlCommand cmd = new SqlCommand("sp_Adhoc_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@AdhocName", adhoc.AdhocName));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", adhoc.CategoryID));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", adhoc.SortOrder));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int categoryID)
        {
            SqlCommand cmd = new SqlCommand("sp_Adhoc_Delete_CategoryID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CategoryID", categoryID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
