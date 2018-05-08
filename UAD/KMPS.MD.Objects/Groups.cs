using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Groups
    {
        #region Properties
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string GroupName { get; set; }  
        [DataMember]
        public bool MasterSupression { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        #endregion

        public Groups()
        {

        }

        #region Data
        public static List<Groups> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Groups> groups = (List<Groups>)CacheUtil.GetFromCache("GROUPS", DatabaseName);

                if (groups == null)
                {
                    groups = GetData(clientconnection);

                    CacheUtil.AddToCache("GROUPS", groups, DatabaseName);
                }

                return groups;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<Groups> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            string sb = GetGroupIDs(clientconnection);
            Groups grps = null;
            List<Groups> grpList = new List<Groups>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand(" SELECT [GroupID], [GroupName] FROM [Groups] " +
                                            " where [GroupID] IN (" + sb + ")", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Groups> builder = DynamicBuilder<Groups>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    grps = new Groups();
                    grps = builder.Build(rdr);
                    grpList.Add(grps);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return grpList;
        }

        public static string GetGroupIDs(KMPlatform.Object.ClientConnections clientconnection)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("SELECT distinct GroupID  FROM [Pubgroups]", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append(dt.Rows[i][0].ToString());
                    if (i != dt.Rows.Count - 1)
                        sb.Append(",");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return sb.ToString();
        }

        public static List<Groups> GetGroupsByCustomerID(int CustomerID)
        {
            Groups grps = null;
            List<Groups> grpList = new List<Groups>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand(" SELECT [GroupID], [GroupName] + ' (' + Convert(varchar(50),[GroupID]) + ') '  as GroupName FROM [Groups] " +
                                            " where [CustomerID] = " + CustomerID + " and isnull(Mastersupression,0) <> 1 order by GroupName", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Groups> builder = DynamicBuilder<Groups>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    grps = new Groups();
                    grps = builder.Build(rdr);
                    grpList.Add(grps);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return grpList;
        }

        public static List<Groups> GetGroupsByCustomerIDFolderID(int CustomerID, int FolderID)
        {
            Groups grps = null;
            List<Groups> grpList = new List<Groups>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT GroupName, GroupID FROM Groups  WHERE customerID = @CustomerID and isnull(FolderID,0) = @FolderID order by GroupName asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Groups> builder = DynamicBuilder<Groups>.CreateBuilder(rdr);


                while (rdr.Read())
                {
                    grps = new Groups();
                    grps = builder.Build(rdr);
                    grpList.Add(grps);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return grpList;
        }

        public static List<Groups> GetGroupsByIDs(string GroupIDs)
        {
            List<Groups> retList = new List<Groups>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand(" SELECT [GroupID], [GroupName] + ' (' + Convert(varchar(50),[GroupID]) + ') ' as GroupName FROM [Groups] " +
                                " where [GroupID] in (" + GroupIDs + ") order by GroupName", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Groups> builder = DynamicBuilder<Groups>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Groups x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static Groups GetGroupByID(int GroupID)
        {
            Groups retItem = new Groups();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT [GroupID], [GroupName], MasterSupression, CustomerID FROM [Groups] where [GroupID] = @GroupID", conn);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Groups> builder = DynamicBuilder<Groups>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }

        public static bool ExistsByGroupNameByCustomerID(string GroupName, int CustomerID)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand("Select * from [Groups] WHERE GroupName = @GroupName AND CustomerID = @CustomerID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GroupName", GroupName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, conn)) > 0 ? true : false;
        }

        public static bool ExistsGroupNameByFolderID(string GroupName, int CustomerID, int folderID)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnCommunicator"].ConnectionString);
            SqlCommand cmd = new SqlCommand("Select * from [Groups] WHERE GroupName = @GroupName AND CustomerID = @CustomerID and isnull(FolderID,0) = @FolderID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GroupName", GroupName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, conn)) > 0 ? true : false;
        }
        #endregion
    }
}
