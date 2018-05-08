using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    [DataContract]
    public class LandingPageAssign
    {
        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return GetList(cmd);
        }
        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByBaseChannelID(int baseChannelID, int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_BaseChannelID_LPID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return Get(cmd);
        }


        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_customerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByCustomerID(int customerID, int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_customerID_LPID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetDefault()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_Default";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetByLPID(int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_LPID";
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetOneToUse(int customerID, int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_GetOneToUse";
            cmd.Parameters.AddWithValue("@LPID", LPID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssign GetByLPAID(int LPAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Select_LPAID";
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.LandingPageAssign Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssign retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.LandingPageAssign();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageAssign>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.LandingPageAssign> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssign> retList = new List<ECN_Framework_Entities.Accounts.LandingPageAssign>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.LandingPageAssign retItem = new ECN_Framework_Entities.Accounts.LandingPageAssign();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageAssign>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Accounts.LandingPageAssign lpa, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_Save";
            cmd.Parameters.Add(new SqlParameter("@LPAID", lpa.LPAID));
            cmd.Parameters.Add(new SqlParameter("@LPID", lpa.LPID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)lpa.BaseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerCanOverride", (object)lpa.CustomerCanOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerDoesOverride", (object)lpa.CustomerDoesOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelDoesOverride", (object)lpa.BaseChannelDoesOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)lpa.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Header", (object)lpa.Header ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Footer", (object)lpa.Footer ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Label", "Unsubscribe"));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString()); 
        }


        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssign_RemoveBaseChannelOverrideForCustomer";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static DataTable GetPreviewParameters(int LPAID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LandingPageAssign_GetPreviewParameters";
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static DataTable GetPreviewParameters_BaseChannel(int LPAID, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LandingPageAssign_GetParameters_BaseChannel";
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
