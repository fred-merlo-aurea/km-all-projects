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
    public class FilterDetails
    {
        public FilterDetails() { }

        #region Properties
        [DataMember]
        public int FilterDetailsID { get; set; }
        [DataMember]
        public int? FilterID { get; set; }
        [DataMember]
        public Enums.FiltersType FilterType { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Values { get; set; }
        [DataMember]
        public string SearchCondition { get; set; }
        [DataMember]
        public int FilterGroupID { get; set; }
        #endregion

        #region Data
        public static List<FilterDetails> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int FilterID)
        {
            List<FilterDetails> retList = new List<FilterDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterDetails where FilterID = @FilterID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterID", FilterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterDetails> builder = DynamicBuilder<FilterDetails>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterDetails x = builder.Build(rdr);

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

        public static List<FilterDetails> getByFilterGroupID(KMPlatform.Object.ClientConnections clientconnection, int FilterGroupID)
        {
            List<FilterDetails> retList = new List<FilterDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterDetails where FilterGroupID = @FilterGroupID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", FilterGroupID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterDetails> builder = DynamicBuilder<FilterDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    FilterDetails x = builder.Build(rdr);

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

        private static Enums.FiltersType GetFilterType(string FilterType)
        {
            return (Enums.FiltersType)Enum.Parse(typeof(Enums.FiltersType), FilterType);
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterDetails fd)
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
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete_ByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            SqlCommand cmd = new SqlCommand("e_FilterDetails_Delete_ByFilterID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}