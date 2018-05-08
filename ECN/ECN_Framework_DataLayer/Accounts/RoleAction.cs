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
    public class RoleAction
    {
        public static List<ECN_Framework_Entities.Accounts.RoleAction> GetByRoleID(int roleID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RoleAction_Select_RoleID";
            cmd.Parameters.AddWithValue("@RoleID", roleID);
            return GetList(cmd, customerID);
        }

        private static List<ECN_Framework_Entities.Accounts.RoleAction> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Accounts.RoleAction> retList = new List<ECN_Framework_Entities.Accounts.RoleAction>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.RoleAction retItem = new ECN_Framework_Entities.Accounts.RoleAction();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.RoleAction>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Accounts.RoleAction roleAction)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RoleAction_Save";
            cmd.Parameters.Add(new SqlParameter("@RoleActionID", roleAction.RoleActionID));
            cmd.Parameters.Add(new SqlParameter("@RoleID", roleAction.RoleID));
            cmd.Parameters.Add(new SqlParameter("@ActionID", roleAction.ActionID));
            cmd.Parameters.Add(new SqlParameter("@Active", roleAction.Active));
            if (roleAction.RoleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)roleAction.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)roleAction.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }
    }
}
