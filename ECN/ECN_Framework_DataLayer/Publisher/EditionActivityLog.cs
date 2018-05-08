using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher
{
    [Serializable]
    public class EditionActivityLog
    {
        public static void DeleteByEditionID(int editionID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionActivityLog_Delete_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }

        public static List<ECN_Framework_Entities.Publisher.EditionActivityLog> GetByEditionID(int editionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionActivityLog_Select_EditionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Publisher.EditionActivityLog> GetByEditionIDSessionID(int editionID, string sessionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionActivityLog_Select_EditionID_SessionID";
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionID));
            cmd.Parameters.Add(new SqlParameter("@SessionID", sessionID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.EditionActivityLog> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.EditionActivityLog> retList = new List<ECN_Framework_Entities.Publisher.EditionActivityLog>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.EditionActivityLog retItem = new ECN_Framework_Entities.Publisher.EditionActivityLog();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.EditionActivityLog>.CreateBuilder(rdr);

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
                }
            }

            return retItem;
        }

        public static int Save(ECN_Framework_Entities.Publisher.EditionActivityLog editionActivityLog)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EditionActivityLog_Save";
            cmd.Parameters.Add(new SqlParameter("@EAID", editionActivityLog.EAID));
            cmd.Parameters.Add(new SqlParameter("@EditionID", editionActivityLog.EditionID));
            cmd.Parameters.Add(new SqlParameter("@EmailID", editionActivityLog.EmailID));
            cmd.Parameters.Add(new SqlParameter("@BlastID", editionActivityLog.BlastID));
            cmd.Parameters.Add(new SqlParameter("@ActionTypeCode", editionActivityLog.ActionTypeCode));
            cmd.Parameters.Add(new SqlParameter("@ActionValue", editionActivityLog.ActionValue));
            cmd.Parameters.Add(new SqlParameter("@IPAddress", editionActivityLog.IPAddress));
            cmd.Parameters.Add(new SqlParameter("@IsAnonymous", editionActivityLog.IsAnonymous));
            cmd.Parameters.Add(new SqlParameter("@LinkID", (object)editionActivityLog.LinkID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PageID", (object)editionActivityLog.PageID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PageNo", editionActivityLog.PageNo));
            cmd.Parameters.Add(new SqlParameter("@SessionID", editionActivityLog.SessionID));
            cmd.Parameters.Add(new SqlParameter("@PageEnd", (object)editionActivityLog.PageEnd ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PageStart", (object)editionActivityLog.PageStart ?? DBNull.Value));

            if (editionActivityLog.EAID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)editionActivityLog.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)editionActivityLog.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }

    }
}
