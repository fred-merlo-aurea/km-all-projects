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
    public class FilterGroup
    {
        public FilterGroup() { }

        #region Properties
        [DataMember]
        public int FilterGroupID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion

        #region Data
        public static List<FilterGroup> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            List<FilterGroup> retList = new List<FilterGroup>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from filterGroup where FilterID = @FilterID order by sortorder", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterID", FilterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterGroup> builder = DynamicBuilder<FilterGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterGroup x = builder.Build(rdr);

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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, int filterID, int sortOrder)
        {
            SqlCommand cmd = new SqlCommand("e_FilterGroup_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        public static void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterGroup_Delete_ByFilterID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
