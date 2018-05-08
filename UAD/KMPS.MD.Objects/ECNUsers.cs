using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KMPS.MD.Objects
{
    public class ECNUsers
    {
        public ECNUsers() { }
        #region Properties
        public int UserID { get; set; }
        public string UserName { get; set; }
        #endregion

        #region Data
        //public static List<ECNUsers> GetECNUsers()
        //{
        //    string server = (string)HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

        //    List<ECNUsers> retList = new List<ECNUsers>();
        //    string sqlQuery = "select UserID, UserName from Users where IsDeleted = 0 and customerID in (" + ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_CustomerIDs"].ToString() + ") order by UserName";
        //    SqlCommand cmd = new SqlCommand(sqlQuery);
        //    cmd.CommandType = CommandType.Text;

        //    SqlDataReader rdr = DataFunctions.executeReader(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString));
        //    DynamicBuilder<ECNUsers> builder = DynamicBuilder<ECNUsers>.CreateBuilder(rdr);
        //    while (rdr.Read())
        //    {
        //        ECNUsers x = builder.Build(rdr);
        //        retList.Add(x);
        //    }

        //    cmd.Connection.Close();
        //    return retList;
        //}

        //public static int GetCustomerID(int userID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "SELECT c.customerID from Users u join Customer c on u.customerID = c.customerID WHERE u.UserID = @UserID";
        //    cmd.Parameters.AddWithValue("@UserID", userID);
        //    return Convert.ToInt32(DataFunctions.executeScalar(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString)));
        //}
        #endregion
    }
}