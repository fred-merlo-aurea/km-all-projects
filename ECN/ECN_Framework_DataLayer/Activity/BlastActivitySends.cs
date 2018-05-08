using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class BlastActivitySends
    {
        public static ECN_Framework_Entities.Activity.BlastActivitySends GetBySendID(int sendID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, e.EmailAddress from BlastActivitySends bas with (nolock) join ecn5_communicator..Emails e on bas.EmailID = e.EmailID where bas.SendID = @SendID and e.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@SendID", sendID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, e.EmailAddress from BlastActivitySends bas with (nolock) join ecn5_communicator..Emails e on bas.EmailID = e.EmailID where bas.BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, e.EmailAddress from BlastActivitySends bas with (nolock) join ecn5_communicator..Emails e on bas.EmailID = e.EmailID where bas.EmailID = @EmailID and e.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, e.EmailAddress from BlastActivitySends bas with (nolock) join ecn5_communicator..Emails e on bas.EmailID = e.EmailID where bas.BlastID = @BlastID and bas.EmailID = @EmailID and e.IsDeleted = 0";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivitySends Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivitySends retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivitySends();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivitySends>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivitySends> retList = new List<ECN_Framework_Entities.Activity.BlastActivitySends>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivitySends retItem = new ECN_Framework_Entities.Activity.BlastActivitySends();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivitySends>.CreateBuilder(rdr);
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

        public static void update_SMTPMessage(string xmldata)
        {
            SqlConnection conn = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.Activity.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_UpdateSMTPMessage";
            cmd.Parameters.AddWithValue("@logdata", xmldata);
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void doBulkUpdate_SMTPMessage()
        {
            SqlConnection conn = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.Activity.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_UpdateSMTPMessagetoLive";
            
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public static void update_BounceData(string xmldata)
        {
            SqlConnection conn = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.Activity.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_UpdateBounceData";
            cmd.Parameters.AddWithValue("@logdata", xmldata);
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool ActivityByBlastIDsEmailID(string blastIDs, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_BlastActivitySend_EmailID_GroupID";
            cmd.Parameters.AddWithValue("@BlastIDs", blastIDs);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString())) > 0 ? true : false;
        }
    }
}
