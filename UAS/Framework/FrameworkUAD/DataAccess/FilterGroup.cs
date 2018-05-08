using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class FilterGroup
    {
        #region Data
        public static List<Entity.FilterGroup> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            List<Entity.FilterGroup> retList = new List<Entity.FilterGroup>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from filterGroup where FilterID = @FilterID order by sortorder", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterID", FilterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.FilterGroup> builder = DynamicBuilder<Entity.FilterGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.FilterGroup x = builder.Build(rdr);

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
            cmd.Connection =DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterGroup_Delete_ByFilterID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
