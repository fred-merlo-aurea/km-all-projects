using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class UserDepartment
    {
        public static List<ECN_Framework_Entities.Accounts.UserDepartment> GetByDepartmentID(int departmentID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Select_Users_DepartmentID";
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
            return GetList(cmd, customerID);
        }

        public static List<ECN_Framework_Entities.Accounts.UserDepartment> GetByUserID(int userID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Select_UserDepartment_UserID";
            cmd.Parameters.AddWithValue("@userID", userID);
            return GetList(cmd, customerID);
        }

        private static List<ECN_Framework_Entities.Accounts.UserDepartment> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.UserDepartment> retList = new List<ECN_Framework_Entities.Accounts.UserDepartment>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.UserDepartment retItem = new ECN_Framework_Entities.Accounts.UserDepartment();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.UserDepartment>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            if (retItem.CustomerID == customerID)
                            {
                                retList.Add(retItem);
                            }
                            else
                            {
                                retList = null;
                                throw new SecurityException("SECURITY VIOLATION!");
                            }
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

        public static void Insert(ref ECN_Framework_Entities.Accounts.UserDepartment userDept)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserDepartment_Insert";
            cmd.Parameters.AddWithValue("@userID", userDept.UserID);
            cmd.Parameters.AddWithValue("@DepartmentID", userDept.DepartmentID.Value);
            cmd.Parameters.AddWithValue("@IsDefaultDept", userDept.IsDefaultDept ? "Y" : "N");
            userDept.UserDepartmentID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        public static void Update(ref ECN_Framework_Entities.Accounts.UserDepartment userDept)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserDepartment_Update";
            cmd.Parameters.AddWithValue("@UserDepartmentID", userDept.UserDepartmentID);
            cmd.Parameters.AddWithValue("@userID", userDept.UserID);
            cmd.Parameters.AddWithValue("@DepartmentID", userDept.DepartmentID.Value);
            cmd.Parameters.AddWithValue("@IsDefaultDept", userDept.IsDefaultDept ? "Y" : "N");
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static bool Delete(int userID)
        {
            SqlCommand cmd = new SqlCommand("e_UserDepartment_Delete_UserID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserDepartment_Delete_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
            return true;
        }

        public static bool Exists(int UserDepartmentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(UserDepartmentID) from UserDepartments where UserDepartmentID = @UserDepartmentID";
            cmd.Parameters.AddWithValue("@UserDepartmentID", UserDepartmentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static void DefaultAll(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update UserDepartments Set IsDefaultDept = 'Y' WHERE UserDepartments.UserID = 4496";
            cmd.Parameters.AddWithValue("@userID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
