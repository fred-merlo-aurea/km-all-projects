using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher
{
    public class Link
    {
        public static bool Exists(int linkID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Exists_ByID";
            cmd.Parameters.Add(new SqlParameter("@LinkID", linkID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Publisher.Link> GetByPageID(int pageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Select_PageID";
            cmd.Parameters.Add(new SqlParameter("@PageID", pageID));
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Publisher.Link GetByLinkID(int linkID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Select_LinkID";
            cmd.Parameters.Add(new SqlParameter("@LinkID", linkID));
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Link> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Link> retList = new List<ECN_Framework_Entities.Publisher.Link>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Link retItem = new ECN_Framework_Entities.Publisher.Link();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Link>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Publisher.Link Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Publisher.Link retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Publisher.Link();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Link>.CreateBuilder(rdr);
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

        public static void Delete(int linkID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Delete";
            cmd.Parameters.Add(new SqlParameter("@LinkID", linkID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }

        public static void DeleteByEditionID(int editionID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Delete_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }

        public static int Save(ECN_Framework_Entities.Publisher.Link link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Link_Save";
            cmd.Parameters.Add(new SqlParameter("@LinkID", link.LinkID));
            cmd.Parameters.Add(new SqlParameter("@PageID", link.PageID));
            cmd.Parameters.Add(new SqlParameter("@LinkType", link.LinkType));
            cmd.Parameters.Add(new SqlParameter("@LinkURL", link.LinkURL));
            cmd.Parameters.Add(new SqlParameter("@x1", link.x1));
            cmd.Parameters.Add(new SqlParameter("@y1", link.y1));
            cmd.Parameters.Add(new SqlParameter("@x2", link.x2));
            cmd.Parameters.Add(new SqlParameter("@y2", link.y2));
            cmd.Parameters.Add(new SqlParameter("@Alias", link.Alias));
            if (link.LinkID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)link.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)link.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }
    }
}
