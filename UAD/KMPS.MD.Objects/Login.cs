using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KMPS.MD.Objects
{
    public class Login
    {
        //public bool IsAuthenticated(string serverName, string userName, string password)
        //{
        //    string permission = string.Empty;
        //    int userId = 0;

        //    return IsAuthenticated(serverName, userName, password, out permission, out userId);
        //}

        //public bool IsAuthenticated(string serverName, string userName, string password, out string permission, out int userId)
        //{
        //    userId = 0;
        //    permission = string.Empty;
        //    bool isAuthenticated = false;

        //    //String sqlQuery =
        //    //    " SELECT * " +
        //    //    " FROM Users u, Customers c, ecn_misc.dbo.MasterDatabaseSecurity mds" +
        //    //    " WHERE u.UserName=@UserName AND u.Password=@Password " +
        //    //    " AND u.CustomerID = c.CustomerID AND mds.UserID = u.UserID and c.customerID in(" + ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_CustomerIDs"].ToString() + ")";

        //    String sqlQuery =
        //        " SELECT userID FROM Users u join [CUSTOMER] c on u.CustomerID = c.CustomerID  " +
        //        " WHERE u.UserName=@UserName AND u.Password=@Password and u.activeflag='y' AND c.customerID in(" + ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(serverName) + "_CustomerIDs"].ToString() + ")";

        //    SqlCommand cmd = new SqlCommand(sqlQuery);
        //    cmd.CommandTimeout = 0;
        //    cmd.CommandType = CommandType.Text;

        //    cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
        //    cmd.Parameters["@UserName"].Value = userName;

        //    cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 25));
        //    cmd.Parameters["@Password"].Value = password;

        //    userId = Convert.ToInt32(DataFunctions.executeScalar(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString)));

        //    //DataTable dt = DataFunctions.getDataTable(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString));

        //    if (userId != 0)
        //    {
        //        SqlCommand cmdUser = new SqlCommand("select * from users u where UserID  = " + userId);
        //        cmdUser.CommandType = CommandType.Text;
        //        DataTable dt = DataFunctions.getDataTable(cmdUser, DataFunctions.GetClientSqlConnection(client));

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            isAuthenticated = true;
        //            //UserID = dr["UserID"].ToString();
        //            //BaseChannelID = dr["BaseChannelID"].ToString();
        //            /*				UD=dr["CustomerID"].ToString()+","+
        //                                dr["BaseChannelID"].ToString()+","+
        //                                dr["CommunicatorChannelID"].ToString()+dr["CollectorChannelID"].ToString()+dr["CreatorChannelID"].ToString()+dr["PublisherChannelID"].ToString()+dr["CharityChannelID"].ToString()+","+
        //                                dr["AccountsOptions"].ToString()+dr["CommunicatorOptions"].ToString()+dr["CollectorOptions"].ToString()+dr["CreatorOptions"].ToString()+","+
        //                                dr["CommunicatorLevel"].ToString()+dr["CollectorLevel"].ToString()+dr["CreatorLevel"].ToString()+dr["AccountsLevel"].ToString()+dr["PublisherLevel"].ToString()+dr["CharityLevel"].ToString();
        //             */
        //            //UD = "tradedl|pubdl|viewtrade";
        //            //UD = "viewtrade";
        //            //UD = "reportonly";
        //            permission = dr["Permission"].ToString();
        //            //ActiveStatus = dr["ActiveFlag"].ToString();
        //        }
        //    }

        //    return isAuthenticated;
        //}
    }
}