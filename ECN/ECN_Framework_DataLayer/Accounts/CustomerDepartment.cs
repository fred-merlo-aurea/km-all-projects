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
    public class CustomerDepartment
    {
        public static bool Exists(int departmentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(DepartmentID) from CustomerDepartments where DepartmentID = @DepartmentID";
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerDepartment> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Select_CustomerDepartment_CustomerID";
            cmd.Parameters.AddWithValue("@customerID", customerID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerDepartment> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerDepartment> retList = new List<ECN_Framework_Entities.Accounts.CustomerDepartment>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerDepartment retItem = new ECN_Framework_Entities.Accounts.CustomerDepartment();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerDepartment>.CreateBuilder(rdr);
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

    }
}
