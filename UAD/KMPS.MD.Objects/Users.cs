using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Runtime.Serialization;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Users
    {
        public Users() { }
        #region Properties
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Permission { get; set; }
        [DataMember]
        public string ExportPermissionIDs { get; set; }
        [DataMember]
        public bool ShowSalesView { get; set; }
        #endregion

        #region Data
        //public static List<Users> GetAllUsers()
        //{
        //    List<Users> retList = new List<Users>();
        //    string sqlQuery = "select * from Users";
        //    SqlCommand cmd = new SqlCommand(sqlQuery);
        //    cmd.CommandType = CommandType.Text;

        //    SqlDataReader rdr = DataFunctions.executeReader(cmd, DataFunctions.GetClientSqlConnection(client));
        //    DynamicBuilder<Users> builder = DynamicBuilder<Users>.CreateBuilder(rdr);
        //    while (rdr.Read())
        //    {
        //        Users x = builder.Build(rdr);
        //        retList.Add(x);
        //    }

        //    cmd.Connection.Close();
        //    return retList;
        //}

        //public static Users GetUserByID(int UserID)
        //{
        //    Users retItem = new Users();
        //    string sqlQuery = "select * from users where UserID  = @UserID";
            
        //    SqlCommand cmd = new SqlCommand(sqlQuery);
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));

        //    SqlDataReader rdr = DataFunctions.executeReader(cmd, DataFunctions.GetClientSqlConnection(client));
        //    DynamicBuilder<Users> builder = DynamicBuilder<Users>.CreateBuilder(rdr);
        //    while (rdr.Read())
        //    {
        //        retItem = builder.Build(rdr);
        //    }

        //    cmd.Connection.Close();
        //    return retItem;
        //}

        //public static List<Users> GetUsersNotInUserBrand()
        //{
        //    List<Users> retList = new List<Users>();
        //    string sqlQuery = "select u.* from users u where permission not in ('admin','clientadmin') and u.UserID not in (select UserID from UserBrand ub join brand b on b.brandid = ub.brandid where b.IsDeleted = 0)";
        //    SqlCommand cmd = new SqlCommand(sqlQuery);
        //    cmd.CommandType = CommandType.Text;

        //    SqlDataReader rdr = DataFunctions.executeReader(cmd, DataFunctions.GetClientSqlConnection(client));
        //    DynamicBuilder<Users> builder = DynamicBuilder<Users>.CreateBuilder(rdr);
        //    while (rdr.Read())
        //    {
        //        Users x = builder.Build(rdr);
        //        retList.Add(x);
        //    }

        //    cmd.Connection.Close();
        //    return retList;
        //}

        //public static DataTable GetBaseChannels()
        //{
        //    string query = "select BaseChannelID, BaseChannelName from basechannel order by BaseChannelName";

        //    SqlCommand cmd = new SqlCommand(query);
        //    cmd.CommandTimeout = 20000;
        //    return DataFunctions.getDataTable(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString));
        //}

        public static int GetSalesViewAccessCount(KMPlatform.Object.ClientConnections clientconnection)
        {
            SqlCommand cmd = new SqlCommand("Select count(UserID) from Users where ShowSalesView = 1");
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        #endregion

        //#region CRUD
        //public static int SaveUsers(Users u)
        //{
        //    SqlCommand cmd = new SqlCommand("sp_SaveUsers");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@ExportPermissionIDs", u.ExportPermissionIDs));
        //    cmd.Parameters.Add(new SqlParameter("@Permission", u.Permission));
        //    cmd.Parameters.Add(new SqlParameter("@UserID", u.UserID));
        //    cmd.Parameters.Add(new SqlParameter("@ShowSalesView", u.ShowSalesView));
        //    return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(client)));
        //}

        //public static void DeleteUser(int UserID)
        //{
        //    SqlCommand cmd = new SqlCommand("Delete from users where userID=@UserID");
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
        //    DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(client));
        //}
        //#endregion
    }
}