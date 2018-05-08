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
    public class Edition
    {
        public static bool Exists(int editionID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Exists_ByID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByName(int publicationID, int editionID, string editionName, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Exists_ByName";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            cmd.Parameters.Add(new SqlParameter("@editionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@editionName", editionName));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Publisher.Edition> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }


        public static ECN_Framework_Entities.Publisher.Edition GetByPublicationCode(string publicationcode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Select_PublicationCode";
            cmd.Parameters.Add(new SqlParameter("@publicationcode", publicationcode));
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Publisher.Edition GetByEditionID(int editionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Select_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Publisher.Edition> GetByPublicationID(int publicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Select_PublicationID";
            cmd.Parameters.Add(new SqlParameter("@publicationID", publicationID));
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Publisher.Edition Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Publisher.Edition retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Publisher.Edition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Edition>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        if (retItem != null)
                        {
                            retItem.StatusID = GetStatus(retItem.Status);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static ECN_Framework_Common.Objects.Publisher.Enums.Status GetStatus(string Status)
        {
            return (ECN_Framework_Common.Objects.Publisher.Enums.Status)Enum.Parse(typeof(ECN_Framework_Common.Objects.Publisher.Enums.Status), Status);
        }

        private static List<ECN_Framework_Entities.Publisher.Edition> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Edition> retList = new List<ECN_Framework_Entities.Publisher.Edition>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Edition retItem = new ECN_Framework_Entities.Publisher.Edition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Edition>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retItem.StatusID = GetStatus(retItem.Status);

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

        public static int Save(ECN_Framework_Entities.Publisher.Edition edition)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Save";
            cmd.Parameters.Add(new SqlParameter("@EditionID", edition.EditionID));
            cmd.Parameters.Add(new SqlParameter("@EditionName", edition.EditionName));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", edition.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@Status", edition.Status));
            cmd.Parameters.Add(new SqlParameter("@FileName", edition.FileName));
            cmd.Parameters.Add(new SqlParameter("@Pages", edition.Pages));
            cmd.Parameters.Add(new SqlParameter("@EnableDate", (object)edition.EnableDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DisableDate", (object)edition.DisableDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsSearchable", edition.IsSearchable));
            cmd.Parameters.Add(new SqlParameter("@IsLoginRequired", edition.IsLoginRequired));
            cmd.Parameters.Add(new SqlParameter("@xmlTOC", edition.xmlTOC));

            if (edition.EditionID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)edition.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)edition.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }

        public static void Delete(int editionID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Edition_Delete";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }
    }
}
