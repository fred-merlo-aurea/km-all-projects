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
    public class CustomerTemplate
    {
        public static bool Exists(int CTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Exists_ByID";
            cmd.Parameters.AddWithValue("@CTID", CTID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Accounts.CustomerTemplate GetByTypeID(int customerID, string templateType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Select_TypeCode";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@TemplateTypeCode", templateType);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Accounts.CustomerTemplate GetByCTID(int CTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Select_CTID";
            cmd.Parameters.AddWithValue("@CTID", CTID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerTemplate> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerTemplate> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerTemplate> retList = new List<ECN_Framework_Entities.Accounts.CustomerTemplate>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerTemplate retItem = new ECN_Framework_Entities.Accounts.CustomerTemplate();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerTemplate>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Accounts.CustomerTemplate Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.CustomerTemplate();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerTemplate>.CreateBuilder(rdr);
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


        public static int Save(ECN_Framework_Entities.Accounts.CustomerTemplate customerTemplate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Save";
            cmd.Parameters.Add(new SqlParameter("@CTID", customerTemplate.CTID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerTemplate.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@TemplateTypeCode", customerTemplate.TemplateTypeCode));
            cmd.Parameters.Add(new SqlParameter("@HeaderSource", customerTemplate.HeaderSource));
            cmd.Parameters.Add(new SqlParameter("@FooterSource", customerTemplate.FooterSource));
            cmd.Parameters.Add(new SqlParameter("@IsActive", customerTemplate.IsActive));
            if (customerTemplate.CTID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerTemplate.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerTemplate.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }


        public static void Delete(int CTID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerTemplate_Delete";
            cmd.Parameters.Add(new SqlParameter("@CTID", CTID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
