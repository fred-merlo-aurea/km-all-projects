using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace ecn.common.classes
{
    public class DataLists
    {
        public DataLists()
        {
        }
        public static string connStr = ConfigurationManager.AppSettings["connstring"];
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];
        public static string creatordb = ConfigurationManager.AppSettings["creatordb"];

        //returns users login based on CustomerID & usermatch string
        public static SqlDataReader GetUsersDR(string UserMatch, string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery =
                " SELECT UserID, UserName " +
                " FROM " + accountsdb + ".dbo.[Users] " +
                " WHERE UserID LIKE '" + UserMatch + "' " +
                " AND CustomerID='" + CustomerID + "' " +
                " AND ActiveFlag='Y' and IsDeleted = 0 ";
            //if (es.HasProductFeature("ecn.communicator", "Users Departments"))
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(es.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.UsersDepartments))
            {
                sqlQuery += "AND UserID IN " +
                                    "	( SELECT UserID FROM " + accountsdb + ".dbo.[Users] " +
                                    "		WHERE UserID IN " +
                                    "		(SELECT DISTINCT UserID FROM " + accountsdb + ".dbo.UserDepartments WHERE DepartmentID IN " +
                                    "			(SELECT DISTINCT DepartmentID FROM " + accountsdb + ".dbo.UserDepartments WHERE UserID = " + sc.UserID().ToString() + ") " +
                                    "		) " +
                                    "	) ";
            }

            sqlQuery += " ORDER BY UserName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns users login based on CustomerID & usermatch string
        public static SqlDataReader GetDepartmentsDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT DepartmentID, DepartmentName " +
                " FROM " + accountsdb + ".dbo.[CustomerDepartments] " +
                " WHERE CustomerID = " + CustomerID +
                " ORDER BY DepartmentName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns users login based on CustomerID & usermatch string


        public static SqlDataReader GetDepartmentsDR(string CustomerID, string UserID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT cd.DepartmentID, cd.DepartmentName " +
                " FROM " + accountsdb + ".dbo.CustomerDepartments cd JOIN " + accountsdb + ".dbo.UserDepartments ud ON cd.DepartmentID = ud.DepartmentID " +
                " WHERE cd.CustomerID = " + CustomerID + " AND ud.UserID = " + UserID +
                " ORDER BY DepartmentName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Folders DR
        /*public static SqlDataReader GetFoldersDR(String CustomerID) {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery=
                " SELECT FolderID, (' -- '+FolderName) as FolderName "+
                " FROM Folders "+
                " WHERE CustomerID="+CustomerID;
            SqlCommand sqlCmd = new SqlCommand(sqlQuery,sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }*/

        //returns Templates based on ChannelID 
        public static SqlDataReader GetTemplatesDR(string ChannelID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT TemplateID, TemplateName " +
                " FROM " + communicatordb + ".dbo.Template " +
                " WHERE ChannelID='" + ChannelID + "' " +
                " AND IsActive=1 and IsDeleted = 0 " +
                " ORDER BY TemplateName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Contents based on CustomerID 
        public static SqlDataReader GetContentDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery =
                    " SELECT ContentID, ContentTitle " +
                    " FROM " + communicatordb + ".dbo.Content " +
                    " WHERE CustomerID='" + CustomerID + "' and IsDeleted = 0 " +
                    " ORDER BY ContentTitle, UpdatedDate ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        public static SqlDataReader GetContentDR(string CustomerID, string FolderID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery =
                    " SELECT ContentID, ContentTitle " +
                    " FROM " + communicatordb + ".dbo.Content " +
                    " WHERE CustomerID='" + CustomerID + "' AND FolderID = '" + FolderID + "' and IsDeleted = 0 " +
                    " ORDER BY ContentTitle";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        public static SqlDataReader GetCustomerRolesDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT RoleID, RoleName " +
                " FROM " + accountsdb + ".dbo.[Role] " +
                " WHERE CustomerID='" + CustomerID + "' and isdeleted = 0";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns a SqlDataReader of  Layouts based on CustomerID 
        public static SqlDataReader GetLayoutsDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 ORDER BY LayoutName";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns a SqlDataReader of  Layouts based on CustomerID & FolderID
        public static SqlDataReader GetLayoutsDR(string CustomerID, string folderID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);

            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 AND FolderID = " + folderID + " ORDER BY LayoutName";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns a SqlDataReader of Layouts based on CustomerID & FolderID
        public static SqlDataReader GetLayoutsDR(string CustomerID, string folderID, string sortField)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);

            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 AND FolderID = " + folderID;

            if (sortField.Length == 0)
            {
                sqlQuery += " ORDER BY RTRIM(LTRIM(LayoutName)) ";
            }
            else
            {
                sqlQuery += " ORDER BY " + sortField;
            }

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns a DataTable of Layouts based on CustomerID 
        public static DataTable GetLayoutsDT(string CustomerID)
        {

            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 ORDER BY LayoutName";

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connStr);
            //adapter.SelectCommand.CommandTimeout = 0;			
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        //returns a DataTable of Layouts based on CustomerID & FolderID
        public static DataTable GetLayoutsDT(string CustomerID, string folderID)
        {

            SqlConnection sqlConn = new SqlConnection(connStr);

            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 AND FolderID = " + folderID + " ORDER BY LayoutName";

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connStr);
            //adapter.SelectCommand.CommandTimeout = 0;			
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        public static DataTable GetLayoutsDTWithDate(string CustomerID, string folderID, string searchMessage, bool SearchAllFolder)
        {
            //SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);

            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery = " SELECT LayoutID, LayoutName, UpdatedDate as 'Date' FROM [Layout] WHERE CustomerID = " + CustomerID + " AND IsDeleted = 0 AND LayoutName like '%" + searchMessage + "%'"; ;

            if (!SearchAllFolder)
            {
                sqlQuery += " AND FolderID = " + folderID;
            }


            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connStr);
            //adapter.SelectCommand.CommandTimeout = 0;			
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        //returns a DataTable of Layouts based on CustomerID & FolderID
        public static DataTable GetLayoutsDT(string CustomerID, string folderID, string sortField)
        {
            //SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);

            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string userID = sc.UserID();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string sqlQuery = "SELECT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(UpdatedDate, IsNull(CreatedDate, '')), 101)) AS 'LayoutName' " +
                                "FROM Layout " +
                                "WHERE CustomerID= 1 and IsDeleted = 0 AND FolderID = " + folderID;

            if (sortField.Length == 0)
            {
                sqlQuery += " ORDER BY RTRIM(LTRIM(LayoutName)) ";
            }
            else
            {
                sqlQuery += " ORDER BY " + sortField;
            }

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connStr);
            //adapter.SelectCommand.CommandTimeout = 0;			
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        //returns Layouts based on CustomerID 
        public static SqlDataReader GetLinkAliasDR(string CustomerID, string LayoutID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT la.Link, la.Alias " +
                " FROM LinkAlias la JOIN Layout lt ON ( la.ContentID =lt.ContentSlot1 OR la.ContentID = lt.ContentSlot2 OR la.ContentID = lt.ContentSlot3 OR la.ContentID = lt.ContentSlot4 OR la.ContentID = lt.ContentSlot5 OR la.ContentID = lt.ContentSlot6) " +
                " WHERE lt.LayoutID=" + LayoutID + " and lt.IsDeleted = 0 and la.IsDeleted = 0" +
                " AND la.ContentID > 0" +
                " ORDER BY LayoutName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Groups based on CustomerID 
        public static SqlDataReader GetGroupsDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            string userID = sc.UserID().ToString();

            string sqlQuery =
                    " SELECT g.GroupName, g.GroupID" +
                    " FROM " + communicatordb + ".dbo.[Groups] g" +
                    " WHERE CustomerID=" + CustomerID +
                    " AND g.GroupID in (select GroupID from " + communicatordb + ".dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                    " ORDER BY GroupName ";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        public static DataTable GetGroupsDT(string CustomerID)
        {
            string sqlQuery = string.Empty;
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string userID = sc.UserID().ToString();
            sqlQuery =
                    " SELECT g.GroupName, g.GroupID" +
                    " FROM " + communicatordb + ".dbo.Groups g" +
                    " WHERE CustomerID=" + CustomerID +
                    " AND g.GroupID in (select GroupID from " + communicatordb + ".dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                    " ORDER BY GroupName ";

            return DataFunctions.GetDataTable(sqlQuery, connStr);
        }

        public static DataTable GetGroupsDTWithSubscriber(string CustomerID, string FolderID, string GroupSearchStr, bool SearchAllFolder)
        {
            string sqlQuery = string.Empty;
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string userID = sc.UserID().ToString();
            sqlQuery =
                    " SELECT g.GroupName, g.GroupID, COUNT(eg.EmailGroupID) AS Subscribers " +
                    " FROM " + communicatordb + ".dbo.Groups g left outer join EmailGroups eg on g.groupID = eg.groupID " +
                    " WHERE eg.subscribetypecode='S' and g.CustomerID = " + CustomerID;

            if (!SearchAllFolder)
            {
                sqlQuery += " AND g.FolderID=" + FolderID;
            }

            sqlQuery +=
                " AND g.GroupID in (select GroupID from " + communicatordb + ".dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                " AND g.GroupName like '%" + GroupSearchStr + "%'" +
                " GROUP BY g.GroupID, g.GroupName" +
                " ORDER BY g.GroupName, g.GroupID";

            return DataFunctions.GetDataTable(sqlQuery, connStr);
        }

        public static DataTable GetGroupsDT(string CustomerID, string FolderID, string GroupSearchStr)
        {
            string sqlQuery = string.Empty;
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string userID = sc.UserID().ToString();
            sqlQuery =
                    " SELECT g.GroupName, g.GroupID" +
                    " FROM " + communicatordb + ".dbo.Groups g" +
                    " WHERE CustomerID=" + CustomerID +
                    " AND FolderID=" + FolderID +
                    " AND g.GroupID in (select GroupID from " + communicatordb + ".dbo.fn_getGroupsforUser(" + CustomerID + "," + userID + ")) " +
                    " AND GroupName like '%" + GroupSearchStr + "%'" +
                    " ORDER BY GroupName ";

            return DataFunctions.GetDataTable(sqlQuery, connStr);
        }

        //returns Groups based on CustomerID, UserID
        public static SqlDataReader GetGroupsDR(string CustomerID, string UserID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery = "";
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            sqlQuery =
                    " SELECT g.GroupName, g.GroupID" +
                    " FROM " + communicatordb + ".dbo.Groups g" +
                    " WHERE CustomerID=" + CustomerID +
                    " AND g.GroupID in (select GroupID from " + communicatordb + ".dbo.fn_getGroupsforUser(" + CustomerID + "," + UserID + ")) " +
                    " ORDER BY GroupName ";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Groups based on CustomerID 
        public static SqlDataReader GetGroupidDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT GroupID, GroupName " +
                " FROM " + communicatordb + ".dbo.Groups " +
                " WHERE CustomerID=" + CustomerID +
                " ORDER BY GroupName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Filters based on GroupID & CustomerID 
        public static SqlDataReader GetFiltersDR(string GroupID, string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT FilterID, FilterName " +
                " FROM Filter " +
                " WHERE CustomerID=" + CustomerID + " and IsDeleted = 0 and GroupID = " + GroupID +
                " ORDER BY FilterName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Channel List
        public static SqlDataReader GetChannelsDR()
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT ChannelID, ChannelName+' ('+ChannelTypeCode+')' as ChannelName " +
                " FROM Channel " +
                " ORDER BY ChannelName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns CustomerList based onChannelID
        public static SqlDataReader GetCustomersDR(string theChannel)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT CustomerID, CustomerName " +
                " FROM Customer " +
                " WHERE BaseChannelID=" + theChannel +
                " AND IsActive=1 and IsDeleted = 0" +
                " ORDER BY CustomerName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns BaseChannel List
        public static SqlDataReader GetBaseChannelsDR()
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT BaseChannelID, BaseChannelName " +
                " FROM BaseChannel WHERE IsDeleted = 0" +
                " ORDER BY BaseChannelName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Channels List based on BaseChannelID
        public static SqlDataReader GetChannelsDR(string theBaseChannel)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT ChannelID, ChannelName+' ('+ChannelTypeCode+')' as ChannelName " +
                " FROM Channel " +
                " WHERE BaseChannelID=" + theBaseChannel +
                " AND ChannelTypeCode <> 'accounts' AND IsDeleted = 0 " +
                " AND Active = 'Y' " +
                " ORDER BY ChannelName ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns Folders DR
        public static SqlDataReader GetFoldersDR(string CustomerID, string Type)
        {
            SqlDataReader dr;
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery;

            if (Type.ToUpper().Equals("GRP"))
                sqlQuery =
                    /*" if (select count(groupID)  from "+communicatordb+".dbo.usergroups where userID = " + sc.UserID() + ") > 0 " +
                    " SELECT distinct f.FolderID, (' -- '+f.FolderName) as FolderName " +
                    " FROM "+communicatordb+".dbo.Folders f " +
                    " WHERE f.CustomerID=" + CustomerID + " AND f.FolderType = 'GRP' and " +
                    "  folderID in (select g.folderID from "+communicatordb+".dbo.groups g join "+communicatordb+".dbo.usergroups ug on g.groupID = ug.groupID where userID = " + sc.UserID() + ") " +
                    " ORDER BY FolderName " +
                    " else " +*/
                    //Commented 'cos the above code checks for permissions on the folder. We don't need that. - Sunil 06/25/07
                    " SELECT FolderID, (' -- '+FolderName) as FolderName " +
                    " FROM " + communicatordb + ".dbo.Folder " +
                    " WHERE CustomerID= " + CustomerID + " AND FolderType = 'GRP' AND IsDeleted = 0 " +
                    " ORDER BY FolderName";
            else
                sqlQuery =
                    " SELECT FolderID, (' -- '+FolderName) as FolderName " +
                    " FROM " + communicatordb + ".dbo.Folder " +
                    " WHERE CustomerID=" + CustomerID +
                    " AND FolderType = '" + Type + "' AND IsDeleted = 0 " +
                    " ORDER BY FolderName ";

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns HeaderFooter DR
        public static SqlDataReader GetHeaderFooterDR(string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT * " +
                " FROM " + creatordb + ".dbo.HeaderFooters " +
                " WHERE CustomerID=" + CustomerID;
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns creator Codes DR
        public static SqlDataReader GetCodesDR(string CodeType)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT * " +
                " FROM " + creatordb + ".dbo.[Code] " +
                " WHERE CodeType = '" + CodeType + "' AND SystemFlag = 'Y' ORDER BY SortCode ";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns accounts Codes DR
        public static SqlDataReader GetAccountCodesDR(string CodeType)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT * " +
                " FROM " + accountsdb + ".dbo.[Code] " +
                " WHERE CodeType = '" + CodeType + "' AND IsDeleted = 0";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns accounts Codes DR
        public static SqlDataReader GetCommCodesDR(string CodeType, string CustomerID)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT * " +
                " FROM " + communicatordb + ".dbo.[Code] " +
                " WHERE CodeType = '" + CodeType + "' AND CustomerID = " + CustomerID + " AND IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }

        //returns accounts Codes DR
        public static DataTable GetCommCodesDT(string CodeType, string CustomerID)
        {
            return DataFunctions.GetDataTable(" SELECT * FROM " + communicatordb + ".dbo.[Code] WHERE CodeType = '" + CodeType + "' AND CustomerID = " + CustomerID + " AND IsDeleted = 0 ORDER BY CodeDisplay, SortOrder ASC");
        }

        //returns Media DR
        public static SqlDataReader GetMediaDR(string CustomerID, string MediaType)
        {
            SqlDataReader dr;
            SqlConnection sqlConn = new SqlConnection(connStr);
            string sqlQuery =
                " SELECT * " +
                " FROM " + creatordb + ".dbo.Code " +
                " WHERE CustomerID=" + CustomerID + " and MediaType = '" + MediaType + "'";
            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
            sqlCmd.Connection.Open();
            dr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }
    }
}
