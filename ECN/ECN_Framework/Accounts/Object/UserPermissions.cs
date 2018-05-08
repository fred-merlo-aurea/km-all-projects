using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Object
{
    [Serializable]
    public class UserPermissions
    {
        public UserPermissions() { }
        #region Properties
        public string ProductName { get; set; }
        public string ActionCode { get; set; }
        public bool Active { get; set; }
        #endregion

        #region Data
        public static List<UserPermissions> Get(int userID)
        {
            List<UserPermissions> retItemList = new List<UserPermissions>();
            string sqlQuery = "select p.ProductName, a.ActionCode, case when UA.active = 'y' then 1 else 0 end as Active  from useractions UA join Action a on ua.actionID = a.actionID join product p on p.productID = a.productID where UserID = @UserID";
            //string sqlQuery = "select p.ProductName, a.ActionCode, case when UA.active = 'y' then 1 else 0 end as Active  from useractions UA join Action a on ua.actionID = a.actionID join products p on p.productID = a.productID where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                var builder = DynamicBuilder<UserPermissions>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    UserPermissions retItem = builder.Build(rdr);
                    retItemList.Add(retItem);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItemList;
        }

        public static bool HasPermission(int userID, string product, string action)
        {
            string sqlQuery = "select count(a.ActionID) from useractions UA join Action a on ua.actionID = a.actionID join product p on p.productID = a.productID where UserID = @UserID and p.ProductName = @Product and a.ActionCode = @Action";
            //string sqlQuery = "select count(a.ActionID) from useractions UA join Action a on ua.actionID = a.actionID join products p on p.productID = a.productID where UserID = @UserID and p.ProductName = @Product and a.ActionCode = @Action";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Product", product);
            cmd.Parameters.AddWithValue("@Action", action);

            if (Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString())) >= 1)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
