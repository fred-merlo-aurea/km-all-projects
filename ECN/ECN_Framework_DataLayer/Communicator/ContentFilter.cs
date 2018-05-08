using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class ContentFilter
    {
        private const string ProcedureContentFilterDeleteSingle = "e_ContentFilter_Delete_Single";
        private const string ProcedureContentFilterDeleteAll = "e_ContentFilter_Delete_All";

        public static bool Exists(int contentID, int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "e_ContentFilter_Exists_ByFilterID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int contentID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilter_Exists_ByContentID";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int filterID, string filterName, int LayoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilter_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterName", filterName);
            cmd.Parameters.AddWithValue("@LayoutID", LayoutID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool HasDynamicContent(int layoutID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(FilterID) from ContentFilter where LayoutID = @LayoutID and IsDeleted = 0";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.ContentFilter GetByFilterID(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select cf.*, c.ContentTitle, c.CustomerID from ContentFilter cf with (nolock) join Content c with (nolock) on cf.ContentID = c.ContentID where cf.FilterID = @FilterID and cf.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByContentID(int contentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select cf.*, c.ContentTitle, c.CustomerID from ContentFilter cf with (nolock) join Content c with (nolock) on cf.ContentID = c.ContentID where cf.ContentID = @ContentID and cf.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ContentFilter> GetByLayoutIDSlotNumber(int layoutID, int slotNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select cf.*, c.ContentTitle, c.CustomerID from ContentFilter cf with (nolock) join Content c with (nolock) on cf.ContentID = c.ContentID where LayoutID = @LayoutID and SlotNumber = @SlotNumber and cf.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@SlotNumber", slotNumber);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.ContentFilter Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ContentFilter retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ContentFilter();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ContentFilter>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.ContentFilter> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> retList = new List<ECN_Framework_Entities.Communicator.ContentFilter>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ContentFilter retItem = new ECN_Framework_Entities.Communicator.ContentFilter();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ContentFilter>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Delete(int contentID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryContent(
                contentID, null, customerID, userID, ProcedureContentFilterDeleteAll);
        }

        public static void Delete(int contentID, int filterID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryContent(
                contentID, filterID, customerID, userID, ProcedureContentFilterDeleteSingle);
        }

        public static int Save(ECN_Framework_Entities.Communicator.ContentFilter filter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilter_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)filter.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)filter.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SlotNumber", (object)filter.SlotNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentID", (object)filter.ContentID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)filter.GroupID ?? DBNull.Value));

            cmd.Parameters.Add(new SqlParameter("@FilterName", filter.FilterName));
            cmd.Parameters.Add(new SqlParameter("@WhereClause", filter.WhereClause)); ;
            if (filter.FilterID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filter.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filter.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
