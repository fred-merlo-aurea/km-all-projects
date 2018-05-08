using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class CustomerContact
    {
        public static bool Exists(int contactID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerContact_Exists_ByID";
            cmd.Parameters.AddWithValue("@ContactID", contactID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Accounts.CustomerContact GetByContactID(int contactID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerContact_Select_ContactID";
            cmd.Parameters.Add(new SqlParameter("@ContactID", contactID));
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerContact> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerContact_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerContact> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerContact> retList = new List<ECN_Framework_Entities.Accounts.CustomerContact>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerContact retItem = new ECN_Framework_Entities.Accounts.CustomerContact();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerContact>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Accounts.CustomerContact Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.CustomerContact retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.CustomerContact();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerContact>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Accounts.CustomerContact customerContact)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerContact_Save";
            cmd.Parameters.Add(new SqlParameter("@ContactID", customerContact.ContactID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerContact.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", customerContact.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", customerContact.LastName));
            cmd.Parameters.Add(new SqlParameter("@Address", customerContact.Address));
            cmd.Parameters.Add(new SqlParameter("@Address2", customerContact.Address2));
            cmd.Parameters.Add(new SqlParameter("@City", customerContact.City));
            cmd.Parameters.Add(new SqlParameter("@State", customerContact.State));
            cmd.Parameters.Add(new SqlParameter("@Zip", customerContact.Zip));
            cmd.Parameters.Add(new SqlParameter("@Phone", customerContact.Phone));
            cmd.Parameters.Add(new SqlParameter("@Mobile", customerContact.Mobile));
            cmd.Parameters.Add(new SqlParameter("@Email", customerContact.Email));
            if (customerContact.ContactID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerContact.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerContact.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

    }
}
