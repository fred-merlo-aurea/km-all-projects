using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastEnvelope
    {
        public static bool Exists(int blastEnvelopeID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 BlastEnvelopeID FROM BlastEnvelopes WHERE BlastEnvelopeID = @BlastEnvelopeID and CustomerID = @CustomerID and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@BlastEnvelopeID", blastEnvelopeID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastEnvelope> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastEnvelope_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.BlastEnvelope GetByBlastEnvelopeID(int blastEnvelopeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastEnvelopes_Select_BlastEnvelopeID";
            cmd.Parameters.AddWithValue("@BlastEnvelopeID", blastEnvelopeID);
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.BlastEnvelope> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastEnvelope> retList = new List<ECN_Framework_Entities.Communicator.BlastEnvelope>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastEnvelope retItem = new ECN_Framework_Entities.Communicator.BlastEnvelope();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastEnvelope>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.BlastEnvelope Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastEnvelope retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastEnvelope();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastEnvelope>.CreateBuilder(rdr);
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

        public static void Delete(int blastEnvelopeID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastEnvelopes_Delete";
            cmd.Parameters.AddWithValue("@BlastEnvelopeID", blastEnvelopeID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.BlastEnvelope blastEnvelope)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastEnvelopes_Save";
            cmd.Parameters.Add(new SqlParameter("@BlastEnvelopeID", (object)blastEnvelope.BlastEnvelopeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blastEnvelope.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromName", blastEnvelope.FromName));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", blastEnvelope.FromEmail));
            if (blastEnvelope.BlastEnvelopeID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blastEnvelope.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blastEnvelope.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }

}
