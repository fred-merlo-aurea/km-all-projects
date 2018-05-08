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
    public class EditionHistory
    {
        public static bool Exists(int editionHistoryID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionHistory_Exists_ByID";
            cmd.Parameters.Add(new SqlParameter("@EditionHistory", editionHistoryID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByEditionID(int editionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionHistory_Exists_ByEditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Publisher.EditionHistory GetByEditionID(int editionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionHistory_Select_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Publisher.EditionHistory Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Publisher.EditionHistory retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Publisher.EditionHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.EditionHistory>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Publisher.EditionHistory editionHistory)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionHistory_Save";
            cmd.Parameters.Add(new SqlParameter("@EditionHistoryID", editionHistory.EditionHistoryID));
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionHistory.EditionID));
            cmd.Parameters.Add(new SqlParameter("@ActivatedDate", (object)editionHistory.ActivatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ArchievedDate", (object)editionHistory.ArchievedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DeActivatedDate", (object)editionHistory.DeActivatedDate ?? DBNull.Value));

            if (editionHistory.EditionHistoryID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)editionHistory.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)editionHistory.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }
    }
}
