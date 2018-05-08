using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class EmailActivityLog
    {
        private static ECN_Framework_Entities.Communicator.EmailActivityLog Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.EmailActivityLog retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.EmailActivityLog();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailActivityLog>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.EmailActivityLog GetByBlastEAID(int eaid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select eal.*, b.CustomerID from EmailActivityLog eal with (nolock) join Blast b with (nolock) on eal.BlastID = b.BlastID where EAID = @EAID";
            cmd.Parameters.AddWithValue("@EAID", eaid);
            return Get(cmd);
        }

        public static int GetOpenCount(int EmailID, int BlastID)
        {
            int resultCount = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " SELECT COUNT(EmailID) " +
                                                " FROM EmailActivityLog  with (nolock) " +
                                                " WHERE EmailID = @EmailID AND BlastID = @BlastID AND ActionTypeCode = 'open' ";
            cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
            cmd.Parameters["@EmailID"].Value = EmailID;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = BlastID;

            try
            {
                resultCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception)
            {
            }

            return resultCount;
        }

        public static int GetConversionRevenueCount(int EmailID, int BlastID)
        {
            int resultCount = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " SELECT COUNT(EmailID) " +
                                                " FROM EmailActivityLog  with (nolock) " +
                                                " WHERE EmailID = @EmailID AND BlastID = @BlastID AND ActionTypeCode = 'conversionRevenue' AND (ActionDate BETWEEN GETDATE()-1 AND GETDATE())";
            cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
            cmd.Parameters["@EmailID"].Value = EmailID;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = BlastID;

            try
            {
                resultCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception)
            {
            }

            return resultCount;
        }

        public static int GetSendCount(int EmailID, int BlastID)
        {
            int resultCount = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " SELECT COUNT(EmailID) " +
                                                " FROM EmailActivityLog  with (nolock) " +
                                                " WHERE EmailID = @EmailID AND BlastID = @BlastID AND ActionTypeCode = 'send' ";
            cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
            cmd.Parameters["@EmailID"].Value = EmailID;
            cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmd.Parameters["@BlastID"].Value = BlastID;

            try
            {
                resultCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception)
            {
            }

            return resultCount;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.EmailActivityLog log)
        {
            //need to validate objects first

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spInsertEmailActivityLog";
            cmd.Parameters.AddWithValue("@EmailID", log.EmailID);
            cmd.Parameters.AddWithValue("@BlastID", log.BlastID);
            cmd.Parameters.AddWithValue("@ActionTypeCode", log.ActionTypeCode);
            cmd.Parameters.AddWithValue("@ActionValue", log.ActionValue);
            cmd.Parameters.AddWithValue("@ActionNotes", log.ActionNotes);
            cmd.Parameters.AddWithValue("@Processed", log.Processed);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int InsertSeedClick(ECN_Framework_Entities.Communicator.EmailActivityLog log)
        {
            //need to validate objects first

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivityClicksInternal_Insert";
            cmd.Parameters.AddWithValue("@EmailID", log.EmailID);
            cmd.Parameters.AddWithValue("@BlastID", log.BlastID);
            cmd.Parameters.AddWithValue("@URL", log.ActionValue);
            cmd.Parameters.AddWithValue("@UniqueLinkID", log.ActionNotes);
            

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()).ToString());
        }

        public static int InsertSeedOpen(ECN_Framework_Entities.Communicator.EmailActivityLog log)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivityOpensInternal_Insert";
            cmd.Parameters.AddWithValue("@EmailID", log.EmailID);
            cmd.Parameters.AddWithValue("@BlastID", log.BlastID);
            cmd.Parameters.AddWithValue("@BrowserInfo", log.ActionValue);
            


            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()).ToString());
        }

        public static void InsertBounce(string xmlDoc, int defaultThreshold)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailActivityLog_InsertBounce";
            cmd.Parameters.AddWithValue("@xmlDocument", xmlDoc);
            cmd.Parameters.AddWithValue("@defaultThreshold", defaultThreshold);

            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void InsertSpamFeedbackXML(string xmlDoc, string actionTypeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailActivityLog_InsertSpamFeedbackXML";
            cmd.Parameters.AddWithValue("@xmlDocument", xmlDoc);
            cmd.Parameters.AddWithValue("@ActionTypeCode", actionTypeCode);

            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void InsertSpamFeedback(int BlastID, int EmailID, string Reason, string subscribeTypeCode, string actionTypeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailActivityLog_InsertSpamFeedback";
            cmd.Parameters.AddWithValue("@ActionNotes", "UNSUBSCRIBED THRU BLAST: " + BlastID.ToString() + Reason);
            cmd.Parameters.AddWithValue("@ActionTypeCode", actionTypeCode);
            cmd.Parameters.AddWithValue("@ebIDs", EmailID.ToString() + "|" + BlastID.ToString());
            cmd.Parameters.AddWithValue("@SubscribeTypeCode", subscribeTypeCode);

            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void InsertOptOutFeedback(int BlastID, string Groups, int EmailID, string Reason)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailActivityLog_InsertOptOutFeedback";
            cmd.Parameters.AddWithValue("@BlastID", BlastID);
            cmd.Parameters.AddWithValue("@Groups", Groups);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@Reason", Reason);

            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
