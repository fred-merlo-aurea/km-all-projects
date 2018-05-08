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
    [Serializable]
    public class Page
    {
        public static bool Exists(int pageID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Page_Exists_ByID";
            cmd.Parameters.Add(new SqlParameter("@PageID", pageID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetLinks(int editionID, string pageNo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetLinks";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@Pageno", pageNo));
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetByEditionIDPageNo(int editionID, string pageNo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Page_Select_EditionID_PageNo";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@Pageno", pageNo));
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Publisher.Page> GetByEditionID(int editionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Page_Select_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Page> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Page> retList = new List<ECN_Framework_Entities.Publisher.Page>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Page retItem = new ECN_Framework_Entities.Publisher.Page();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Page>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Publisher.Page Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Publisher.Page retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Publisher.Page();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Page>.CreateBuilder(rdr);
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

        public static void DeleteByEditionID(int editionID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Page_Delete_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }

        public static int Save(ECN_Framework_Entities.Publisher.Page page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Page_Save";
            cmd.Parameters.Add(new SqlParameter("@PageID", page.PageID));
            cmd.Parameters.Add(new SqlParameter("@EditionID", page.EditionID));
            cmd.Parameters.Add(new SqlParameter("@PageNumber", page.PageNumber));
            cmd.Parameters.Add(new SqlParameter("@DisplayNumber", page.DisplayNumber));
            cmd.Parameters.Add(new SqlParameter("@Width", page.Width));
            cmd.Parameters.Add(new SqlParameter("@Height", page.Height));
            cmd.Parameters.Add(new SqlParameter("@TextContent", page.TextContent));
            if (page.PageID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)page.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)page.CreatedUserID ?? DBNull.Value));

            page.PageID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));

            return page.PageID;
        }
    }
}
