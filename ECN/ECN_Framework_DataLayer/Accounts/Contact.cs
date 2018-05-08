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
    public class Contact
    {
        public static ECN_Framework_Entities.Accounts.Contact GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BillingContact_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.Contact Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.Contact retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.Contact();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Contact>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Accounts.Contact contact)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BillingContact_Save";
            cmd.Parameters.Add(new SqlParameter("@BillingContactID", contact.BillingContactID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", contact.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@StreetAddress", contact.StreetAddress));
            cmd.Parameters.Add(new SqlParameter("@City", contact.City));
            cmd.Parameters.Add(new SqlParameter("@State", contact.State));
            cmd.Parameters.Add(new SqlParameter("@Country", contact.Country));
            cmd.Parameters.Add(new SqlParameter("@Zip", contact.Zip));
            cmd.Parameters.Add(new SqlParameter("@Salutation", contact.Salutation.ToString()));
            cmd.Parameters.Add(new SqlParameter("@ContactName", contact.ContactName));
            cmd.Parameters.Add(new SqlParameter("@FirstName", contact.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", contact.LastName));
            cmd.Parameters.Add(new SqlParameter("@ContactTitle", contact.ContactTitle));
            cmd.Parameters.Add(new SqlParameter("@Email", contact.Email));
            cmd.Parameters.Add(new SqlParameter("@Phone", contact.Phone));
            cmd.Parameters.Add(new SqlParameter("@Fax", contact.Fax));
            if (contact.BillingContactID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)contact.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)contact.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
       } 
    }
}
