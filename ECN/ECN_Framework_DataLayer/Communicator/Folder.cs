using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Folder
    {
        public static bool SubfoldersExist(int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 FolderID from Folder where CustomerID = @CustomerID and ParentID = @ParentID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ParentID", folderID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 FolderID from Folder where CustomerID = @CustomerID and FolderID = @FolderID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int folderID, string folderName, int parentID, int customerID, string folderType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Folder_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FolderName", folderName);
            cmd.Parameters.AddWithValue("@ParentID", parentID);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@FolderType", folderType);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int folderID, int customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 FolderID from Folder where CustomerID = @CustomerID and FolderID = @FolderID and FolderType = @FolderType AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@FolderType", type.ToString());
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.Folder GetByFolderID(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Folder WHERE FolderID = @FolderID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Folder> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Folder WHERE CustomerID = @CustomerID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }


        public static List<ECN_Framework_Entities.Communicator.Folder> GetByType(int customerID, string type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " SELECT * " +
                                " FROM Folder " +
                                " WHERE CustomerID=@CustomerID" +
                                " AND FolderType = @Type  AND IsDeleted = 0" +
                                " ORDER BY FolderName ";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Type", type);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Folder> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Folder> retList = new List<ECN_Framework_Entities.Communicator.Folder>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Folder retItem = new ECN_Framework_Entities.Communicator.Folder();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Folder>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.Folder Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Folder retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Folder();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Folder>.CreateBuilder(rdr);
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

        public static bool Delete(int folderID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Folder_Delete";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return true;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Folder folder)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Folder_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", folder.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@FolderName", folder.FolderName));
            cmd.Parameters.Add(new SqlParameter("@FolderDescription", folder.FolderDescription));
            cmd.Parameters.Add(new SqlParameter("@FolderType", folder.FolderType));
            cmd.Parameters.Add(new SqlParameter("@IsSystem", folder.IsSystem));
            cmd.Parameters.Add(new SqlParameter("@ParentID", folder.ParentID));
            cmd.Parameters.Add(new SqlParameter("@FolderID", folder.FolderID));
            if (folder.FolderID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)folder.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)folder.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static DataTable GetFolderTree(int customerID, int userID, string folderType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Folder_GetFolderTree";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@FolderType", folderType);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetBlastCategoryFolders(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Folder_GetBlastCategoryFolders";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.Folder> GetByContentSearch(string folderName, int? folderID, int customerID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Folder_Select_Search";
            if (folderName.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@FolderName", folderName);
            }
            if (folderID != null)
            {
                cmd.Parameters.AddWithValue("@FolderID", folderID);
            }
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            if (updatedDateFrom != null)
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
            }
            if (updatedDateFrom != null)
            {
                cmd.Parameters.AddWithValue("@UpdatedDateFrom", updatedDateFrom);
            }
            if (updatedDateTo != null)
            {
                cmd.Parameters.AddWithValue("@UpdatedDateTo", updatedDateTo);
            }
            
            return GetList(cmd);
        }

        public static int GetFolderIDByName(int customerID, string folderName, string folderType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 1 FolderID FROM Folder WITH (NOLOCK) WHERE CustomerID = @CustomerID AND FolderName LIKE '%" + folderName + "%' AND IsDeleted = 0 AND FolderType = @FolderType";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FolderType", folderType);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }
    }
}
