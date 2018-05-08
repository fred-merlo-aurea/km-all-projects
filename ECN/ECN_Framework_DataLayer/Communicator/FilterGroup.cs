using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class FilterGroup
    {
        private const string ProcedureFilterGroupDeleteFilterId = "e_FilterGroup_Delete_FilterID";
        private const string ProceudreFilterGroupDeleteFilterGroupId = "e_FilterGroup_Delete_FilterGroupID";

        public static bool Exists(int filterGroupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int filterGroupID, string name, int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        private static ECN_Framework_Entities.Communicator.FilterGroup Get(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.FilterGroup>.Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.FilterGroup> GetList(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.FilterGroup>.GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.FilterGroup GetByFilterGroupID(int filterGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Select_FilterGroupID";
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.FilterGroup> GetByFilterID(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Select_FilterID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return GetList(cmd);
        }

        public static void Delete(int filterID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilter(filterID, customerID, userID, ProcedureFilterGroupDeleteFilterId);
        }

        public static void Delete(int filterID, int filterGroupID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilterGroup(filterID, filterGroupID, customerID, userID, ProceudreFilterGroupDeleteFilterGroupId);
        }

        public static int Save(ECN_Framework_Entities.Communicator.FilterGroup filterGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", (object)filterGroup.FilterGroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)filterGroup.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)filterGroup.SortOrder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Name", filterGroup.Name));
            cmd.Parameters.Add(new SqlParameter("@ConditionCompareType", filterGroup.ConditionCompareType));
            if (filterGroup.FilterGroupID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filterGroup.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filterGroup.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetSortOrder(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_GetSortOrder";
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void UpdateSortOrder(int filterGroupID, int sortOrder, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_UpdateSortOrder_FilterGroupID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
