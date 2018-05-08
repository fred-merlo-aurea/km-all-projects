using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class UADFilter
    {
      

        #region Data
        public static List<Object.UADFilter> GetFilterByUserIDType(KMPlatform.Object.ClientConnections clientconnection, int userID, FrameworkUAD.BusinessLogic.Enums.ViewType ViewType, int pubID, int brandID, bool IsAdmin, bool IsViewMode)
        {
            List<Object.UADFilter> retList = new List<Object.UADFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            string sqlquery = "select f.*, b.BrandID, BrandName from Filters f left outer join brand b on f.brandID = b.BrandID left outer join filtersegmentation fs on f.filterID = fs.FilterID ";

            if (IsViewMode)
            {
                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                    sqlquery += " where filterType = 'ProductView' and pubID = @PubID ";
                else if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                    sqlquery += " where filterType = 'CrossProductView' ";
                else
                    sqlquery += " where (Isnull(filterType, '') = 'ConsensusView' or filterType = 'RecencyView') ";

                if (brandID > 0)
                    sqlquery += " and b.brandID = @BrandID ";

                if (userID > 0)
                {
                    sqlquery += " and f.CreatedUserID = @UserID ";
                }
                else
                {
                    if (!IsAdmin)
                        sqlquery += " and islocked = 0 or (islocked = 1 and f.CreatedUserID = @UserID) ";
                }
            }
            else
            {
                if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                    sqlquery += " where filterType = 'ProductView' and pubID = @PubID ";
                else if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.RecencyView)
                    sqlquery += " where filterType = 'RecencyView' ";
                else if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.CrossProductView)
                    sqlquery += " where filterType = 'CrossProductView' ";
                else if (ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView)
                    sqlquery += " where Isnull(filterType, '') = 'ConsensusView' and Isnull(pubID,0)=0 ";
                if (!IsAdmin)
                    sqlquery += " and islocked = 0 or (islocked = 1 and f.CreatedUserID = @UserID) ";
                if (brandID > 0)
                    sqlquery += " and b.brandID = @BrandID ";
                else
                    sqlquery += " and (b.brandID = 0 or b.brandID is null) ";
            }
            sqlquery += " and f.IsDeleted=0 and isnull(b.IsDeleted,0) = 0 and fs.filtersegmentationID is null and f.FilterID in (select FilterID from FilterGroup with (nolock) Group by FilterID having COUNT(FilterID) = 1) order by Name ASC";
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.UADFilter> builder = DynamicBuilder<Object.UADFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Object.UADFilter x = builder.Build(rdr);
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

        public static Object.UADFilter GetByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            Object.UADFilter x = new Object.UADFilter();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from Filters where FilterID = @FilterID", conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.UADFilter> builder = DynamicBuilder<Object.UADFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    x = builder.Build(rdr);

                    if (x.FilterType == null || x.FilterType == string.Empty)
                        x.FilterType = "ConsensusView";

                    x.ViewType = GetViewType(x.FilterType);

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
            return x;
        }

        public static List<Object.UADFilter> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Object.UADFilter> retList = new List<Object.UADFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Filters_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.UADFilter> builder = DynamicBuilder<Object.UADFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Object.UADFilter x = builder.Build(rdr);

                    if (x.FilterType == null || x.FilterType == string.Empty)
                        x.FilterType = "ConsensusView";

                    x.ViewType = GetViewType(x.FilterType);

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

        public static List<Object.UADFilter> GetNotInBrand_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Object.UADFilter> retList = GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);

            var query = retList.Select(r => new Object.UADFilter()
            {
                FilterId = r.FilterId,
                Name =
                (
                    r.FilterType == "ProductView" ? "[Product] " + r.Name :
                    r.FilterType == "RecencyView" ? "[Recency] " + r.Name :
                    r.FilterType == "CrossProductView" ? "[CrossProduct] " + r.Name : "[Consensus] " + r.Name
                )
            }).ToList();

            return query;
        }

        public static List<Object.UADFilter> GetByBrandID_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<Object.UADFilter> retList = GetAll(clientconnection).FindAll(x => x.BrandID == brandID);

            var query = retList.Select(r => new Object.UADFilter()
            {
                FilterId = r.FilterId,
                Name =
                (
                    r.FilterType == "ProductView" ? "[Product] " + r.Name :
                    r.FilterType == "RecencyView" ? "[Recency] " + r.Name :
                    r.FilterType == "CrossProductView" ? "[CrossProduct] " + r.Name : "[Consensus] " + r.Name
                )
            }).ToList();

            return query;
        }

        public static List<Object.UADFilter> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<Object.UADFilter> GetInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID > 0);
        }

        public static List<Object.UADFilter> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static List<Object.UADFilter> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<Object.UADFilter> retList = new List<Object.UADFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Filters_Select_UserID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.UADFilter> builder = DynamicBuilder<Object.UADFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Object.UADFilter x = builder.Build(rdr);
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

        public static List<Object.UADFilter> GetInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID > 0);
        }

        public static List<Object.UADFilter> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int brandID, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID == brandID);
        }

        public static List<Object.UADFilter> GetNotInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static Object.UADFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            Object.UADFilter retItem = new Object.UADFilter();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from Filters where FilterID = @FilterID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.UADFilter> builder = DynamicBuilder<Object.UADFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);

                    if (retItem.FilterType == null || retItem.FilterType == string.Empty)
                        retItem.FilterType = "ConsensusView";

                    retItem.ViewType = GetViewType(retItem.FilterType);
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
            return retItem;
        }

        private static FrameworkUAD.BusinessLogic.Enums.ViewType GetViewType(string ViewType)
        {
            return (FrameworkUAD.BusinessLogic.Enums.ViewType) Enum.Parse(typeof(FrameworkUAD.BusinessLogic.Enums.ViewType), ViewType, true);
        }

        public static bool ExistsByFilterName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string filterName)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByFilterName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterName", filterName));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByFilterCategoryID(KMPlatform.Object.ClientConnections clientconnection, int filterCategoryID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByFilterCategoryID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@filterCategoryID", filterCategoryID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsByQuestionCategoryID(KMPlatform.Object.ClientConnections clientconnection, int questionCategoryID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByQuestionCategoryID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionCategoryID", questionCategoryID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsQuestionName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string questionName)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_QuestionName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionName", questionName));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        #endregion

        #region CRUD
        public static int insert(KMPlatform.Object.ClientConnections clientconnection, Object.UADFilter mdf)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", mdf.FilterId));
            cmd.Parameters.Add(new SqlParameter("@Name", mdf.Name));
            cmd.Parameters.Add(new SqlParameter("@Notes", mdf.Notes));
            cmd.Parameters.Add(new SqlParameter("@FilterXML", (object) mdf.FilterXML ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterType", (object) mdf.FilterType.ToString() ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubID", (object) mdf.PubID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BrandID", (object) mdf.BrandID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterCategoryID", (object) mdf.FilterCategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddtoSalesView", (object) mdf.AddtoSalesView ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QuestionName", (object) mdf.QuestionName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QuestionCategoryID", (object) mdf.QuestionCategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", (object) mdf.IsDeleted ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", (object) mdf.UpdatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", (object) mdf.UpdatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object) mdf.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", (object) mdf.CreatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsLocked", (object) mdf.IsLocked ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterID, int userID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion

    }
}
