using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class CustomerNote
    {
        public static ECN_Framework_Entities.Accounts.CustomerNote GetByNoteID(int noteID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerNote_Select_NoteID";
            cmd.Parameters.Add(new SqlParameter("@noteID", noteID));
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerNote> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerNote_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerNote> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerNote> retList = new List<ECN_Framework_Entities.Accounts.CustomerNote>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerNote retItem = new ECN_Framework_Entities.Accounts.CustomerNote();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerNote>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Accounts.CustomerNote Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.CustomerNote retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.CustomerNote();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerNote>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Accounts.CustomerNote customerNote)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerNote_Save";
            cmd.Parameters.Add(new SqlParameter("@NoteID", customerNote.NoteID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerNote.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Notes", customerNote.Notes));
            cmd.Parameters.Add(new SqlParameter("@IsBillingNotes", customerNote.IsBillingNotes));
            if (customerNote.NoteID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerNote.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerNote.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

    }
}
