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
    public class FilterDetails
    {
        #region Data
        public static List<Object.FilterDetails> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            List<Object.FilterDetails> retList = new List<Object.FilterDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterDetails where FilterGroupID IN (Select FilterGroupID FROM FilterGroup WHERE FilterID = @FilterID)", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterID", FilterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.FilterDetails> builder = DynamicBuilder<Object.FilterDetails>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Object.FilterDetails x = builder.Build(rdr);

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

        public static List<Object.FilterDetails> getByFilterGroupID(KMPlatform.Object.ClientConnections clientconnection, int FilterGroupID)
        {
            List<Object.FilterDetails> retList = new List<Object.FilterDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterDetails where FilterGroupID = @FilterGroupID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", FilterGroupID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.FilterDetails> builder = DynamicBuilder<Object.FilterDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Object.FilterDetails x = builder.Build(rdr);

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

        private static FrameworkUAD.BusinessLogic.Enums.FiltersType GetFilterType(string FilterType)
        {
            return (FrameworkUAD.BusinessLogic.Enums.FiltersType) Enum.Parse(typeof(FrameworkUAD.BusinessLogic.Enums.FiltersType), FilterType);
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Object.FilterDetails fd)
        {
            SqlCommand cmd = new SqlCommand("e_FilterDetails_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterDetailsID", fd.FilterDetailsID));
            cmd.Parameters.Add(new SqlParameter("@FilterType", fd.FilterType));
            cmd.Parameters.Add(new SqlParameter("@Group", fd.Group));
            cmd.Parameters.Add(new SqlParameter("@Name", fd.Name));
            cmd.Parameters.Add(new SqlParameter("@Values", fd.Values));
            cmd.Parameters.Add(new SqlParameter("@SearchCondition", fd.SearchCondition));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", fd.FilterGroupID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterDetails_Delete_ByFilterID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
