using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class FilterCategory
    {
        private const string CacheKeyFilterCategory = "FILTERCATEGORY";
        private const string ExistsByCategoryNameCommandText = "e_FilterCategory_Exists_ByCategoryName";
        private const string ExistsByParentIdNameCommandText = "e_FilterCategory_Exists_ByParentID";
        private const string CategoryNameParameterName = "@CategoryName";
        private const string FilterCategoryIdParameterName = "@filterCategoryID"; 
        private const string ParentIdParameterName = "@ParentID"; 

        public FilterCategory() { }

        #region Properties
        [DataMember]
        public int FilterCategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        #endregion

        #region Data
        public static List<FilterCategory> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<FilterCategory> filterCategory = (List<FilterCategory>)CacheUtil.GetFromCache("FILTERCATEGORY", DatabaseName);

                if (filterCategory == null)
                {
                    filterCategory = GetData(clientconnection);

                    CacheUtil.AddToCache("FILTERCATEGORY", filterCategory, DatabaseName);
                }

                return filterCategory;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<FilterCategory> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<FilterCategory> retList = new List<FilterCategory>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterCategory_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterCategory> builder = DynamicBuilder<FilterCategory>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    FilterCategory x = builder.Build(rdr);
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

        public static FilterCategory GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterCategoryID)
        {
            FilterCategory fc = GetAll(clientconnection).Find(x => x.FilterCategoryID == filterCategoryID);
            return fc;
        }

        public static bool ExistsByCategoryName(ClientConnections clientConnections, int filterCategoryId, string categoryName)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter(CategoryNameParameterName, categoryName),
                new SqlParameter(FilterCategoryIdParameterName, filterCategoryId)
            };

            return DataFunctions.ExecuteScalar(clientConnections, ExistsByCategoryNameCommandText, sqlParameters) > 0;
        }

        public static bool ExistsByParentID(ClientConnections clientConnections, int parentId)
        {
            var sqlParameters = new List<SqlParameter> { new SqlParameter(ParentIdParameterName, parentId) };

            return DataFunctions.ExecuteScalar(clientConnections, ExistsByParentIdNameCommandText, sqlParameters) > 0;
        }

        public static void DeleteCache(ClientConnections clientConnections)
        {
            if (!CacheUtil.IsCacheEnabled())
            {
                return;
            }

            var databaseName = DataFunctions.GetDBName(clientConnections);

            CacheUtil.SafeRemoveFromCache(CacheKeyFilterCategory, databaseName);
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterCategory FilterCategory)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterCategory_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterCategoryID", FilterCategory.FilterCategoryID));
            cmd.Parameters.Add(new SqlParameter("@CategoryName", FilterCategory.CategoryName));
            cmd.Parameters.Add(new SqlParameter("@ParentID", FilterCategory.ParentID));
            if (FilterCategory.FilterCategoryID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", FilterCategory.UpdatedUserID));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", FilterCategory.CreatedUserID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int FilterCategoryID, int UserID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterCategory_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterCategoryID", FilterCategoryID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        #endregion
    }
}
