using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class LinkAlias
    {
        private const string ProcedureLinkAliasSelectOwnerId = "e_LinkAlias_Select_OwnerID";
        private const string ProcedureLinkAliasSelectLink = "e_LinkAlias_Select_Link";
        private const string ProcedureLinkAliasDeleteAll = "e_LinkAlias_Delete_All";
        private const string ProcedureLinkAliasExistsByAlias = "e_LinkAlias_Exists_ByAlias";
        private const string ProcedureCodeExistsLinkTypeId = "e_Code_Exists_LinkTypeID";

        private static List<ECN_Framework_Entities.Communicator.LinkAlias> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkAlias> retList = new List<ECN_Framework_Entities.Communicator.LinkAlias>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkAlias retItem = new ECN_Framework_Entities.Communicator.LinkAlias();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkAlias>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.LinkAlias Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkAlias retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkAlias();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkAlias>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.LinkAlias GetByAliasID(int aliasID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Select_AliasID";
            cmd.Parameters.AddWithValue("@AliasID", aliasID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.LinkAlias GetByBlastLink(int blastID, string link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_GetByBlastLink";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@Link", link);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByContentID(int contentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Select_ContentID";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByOwnerID(int linkOwnerID)
        {
            var cmd = CommunicatorMethodsHelper.GetLinkAlias(null, linkOwnerID, null, ProcedureLinkAliasSelectOwnerId);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkAlias> GetByLink(int contentID, string link)
        {
            var cmd = CommunicatorMethodsHelper.GetLinkAlias(contentID, null, link, ProcedureLinkAliasSelectLink);
            return GetList(cmd);
        }

        public static bool Exists(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exists_ByCustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByOwnerID(int ownerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exist_LinkOwnerID";
            cmd.Parameters.AddWithValue("@LinkOwnerID", ownerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int contentID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exists_ByContentID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int layoutID, string link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exists_ByLink_LayoutID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);            
            cmd.Parameters.AddWithValue("@Link", link);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int contentID, int aliasID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exists_ByID";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@AliasID", aliasID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int contentID, string link, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Exists_ByLink";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Link", link);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int aliasID, string alias, int contentID, int customerID)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                CustomerId = customerID,
                Alias = alias,
                AliasId = aliasID,
                ContentId = contentID
            };
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureLinkAliasExistsByAlias) > 0;
        }

        public static bool CodeUsedInLinkAlias(int codeID)
        {
            var readAndFillParams = new FillCommunicatorArgs {LinkTypeId = codeID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureCodeExistsLinkTypeId) > 0;
        }

        public static void Delete(int contentID, int aliasID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@AliasID", aliasID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int contentID, string link, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Delete_ByLink";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Link", link);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int contentID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryContent(
                contentID, null, customerID, userID, ProcedureLinkAliasDeleteAll);
        }

        public static int Save(ECN_Framework_Entities.Communicator.LinkAlias alias)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkAlias_Save";
            cmd.Parameters.Add(new SqlParameter("@AliasID", (object)alias.AliasID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentID", (object)alias.ContentID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LinkOwnerID", (object)alias.LinkOwnerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Link", alias.Link));
            cmd.Parameters.Add(new SqlParameter("@Alias", alias.Alias));
            cmd.Parameters.Add(new SqlParameter("@LinkTypeID", (object)alias.LinkTypeID ?? DBNull.Value));
            if (alias.AliasID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)alias.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)alias.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static DataTable GetLinkAliasDR(int customerID, int layoutID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LinkAlias_GetLinkAliasDR";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
