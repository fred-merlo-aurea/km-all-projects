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
    public class CustomerLicense
    {
        public static List<ECN_Framework_Entities.Accounts.CustomerLicense> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLicense_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.License GetCurrentLicensesByCustomerID(int customerID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeCode  licensetypecode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLicense_Select_CustomerID_LicenseTypeCode";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@licensetypecode", licensetypecode.ToString()));

            ECN_Framework_Entities.Accounts.License retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.License();

                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.License>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        if (retItem.Allowed == -1)
                        {
                            retItem.LicenseOption = ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.unlimited;
                        }
                        else if (retItem.Allowed > 0)
                        {
                            retItem.LicenseOption = ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.limited;
                        }
                        else
                        {
                            retItem.LicenseOption = ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.notavailable;
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static ECN_Framework_Entities.Accounts.CustomerLicense GetByCLID(int CLID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLicense_Select_CLID";
            cmd.Parameters.Add(new SqlParameter("@CLID", CLID));
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.CustomerLicense Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.CustomerLicense retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.CustomerLicense();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerLicense>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.CustomerLicense> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerLicense> retList = new List<ECN_Framework_Entities.Accounts.CustomerLicense>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerLicense retItem = new ECN_Framework_Entities.Accounts.CustomerLicense();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerLicense>.CreateBuilder(rdr);
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

        public static void Delete(int CLID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLicense_Delete";
            cmd.Parameters.Add(new SqlParameter("@CLID", CLID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static int Save(ECN_Framework_Entities.Accounts.CustomerLicense customerLicense)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLicense_Save";
            cmd.Parameters.Add(new SqlParameter("@CLID", customerLicense.CLID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerLicense.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@QuoteItemID", customerLicense.QuoteItemID));
            cmd.Parameters.Add(new SqlParameter("@LicenseTypeCode", customerLicense.LicenseTypeCode));
            cmd.Parameters.Add(new SqlParameter("@LicenseLevel", customerLicense.LicenseLevel));
            cmd.Parameters.Add(new SqlParameter("@Quantity", customerLicense.Quantity));
            cmd.Parameters.Add(new SqlParameter("@Used", customerLicense.Used));
            cmd.Parameters.Add(new SqlParameter("@ExpirationDate", customerLicense.ExpirationDate));
            cmd.Parameters.Add(new SqlParameter("@AddDate", customerLicense.AddDate));
            cmd.Parameters.Add(new SqlParameter("@IsActive", customerLicense.IsActive));
            if (customerLicense.CLID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerLicense.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerLicense.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }
    }
}
