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
    public class AdhocCategory
    {
        public AdhocCategory() { }

        #region Properties
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion

        #region Data
        public static bool Exists(KMPlatform.Object.ClientConnections clientconnection, string categoryName)
        {
            SqlCommand cmd = new SqlCommand("sp_AdhocCategory_Exists_ByName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CategoryName", categoryName));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static List<AdhocCategory> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<AdhocCategory> retList = new List<AdhocCategory>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_AdhocCategory_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<AdhocCategory> builder = DynamicBuilder<AdhocCategory>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    AdhocCategory x = builder.Build(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, int categoryID, string categoryName, int sortOrder)
        {
            SqlCommand cmd = new SqlCommand("sp_AdhocCategory_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CategoryID", categoryID));
            cmd.Parameters.Add(new SqlParameter("@CategoryName", categoryName));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion
    }
}