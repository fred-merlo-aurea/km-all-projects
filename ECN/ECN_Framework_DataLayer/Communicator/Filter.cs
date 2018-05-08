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
    public class Filter
    {
        private const string ProcedureFilterDeleteFilterId = "e_Filter_Delete_FilterID";

        public static ECN_Framework_Entities.Communicator.Filter GetByFilterID(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select_FilterID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Get(cmd);
        }
        
        public static void ArchiveFilter(int FilterID, bool Archived, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Archive";
            cmd.Parameters.AddWithValue("@FilterID", FilterID);
            cmd.Parameters.AddWithValue("@Archived", Archived);
            cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.Filter> GetByGroupID(int groupID, bool validWhereOnly,string archiveFilter = "all")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ValidWhereOnly", validWhereOnly);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            return GetList(cmd);
        }

        public static bool Exists(int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int filterID, string filterName, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterName", filterName);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        private static ECN_Framework_Entities.Communicator.Filter Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Filter retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Filter();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Filter>.CreateBuilder(rdr);
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


        public static List<ECN_Framework_Entities.Communicator.Filter> GetByFilterSearch( int? groupID,int CustomerID, bool? archived = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select_Search";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            if(groupID.HasValue)
                cmd.Parameters.AddWithValue("@GroupID", groupID);
            if (archived != null)
            {
                cmd.Parameters.AddWithValue("@Archived", archived.Value);
            }
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Filter> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Filter> retList = new List<ECN_Framework_Entities.Communicator.Filter>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Filter retItem = new ECN_Framework_Entities.Communicator.Filter();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Filter>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Delete(int filterID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilter(
                filterID, null, customerID, userID , ProcedureFilterDeleteFilterId);
        }

        public static void UpdateWhereClause(int filterID, string whereClause, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_UpdateWhere_FilterID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@WhereClause", whereClause);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateDynamicWhere(int filterID, string dynamicWhere, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_UpdateDynamicWhere_FilterID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@DynamicWhere", dynamicWhere);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.Filter filter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)filter.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)filter.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)filter.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupCompareType", filter.GroupCompareType));
            cmd.Parameters.Add(new SqlParameter("@FilterName", filter.FilterName));
            if (filter.FilterID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filter.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filter.CreatedUserID ?? DBNull.Value));

            cmd.Parameters.AddWithValue("@Archived", filter.Archived.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
