using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ecn.activityengines.engines
{
    public partial class LoadTest : System.Web.UI.Page
    {
        public delegate void LongTimeTask_Delegate(string s);

        public int Counter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //LongTimeTask_Delegate d = null;
            //d = new LongTimeTask_Delegate(LongTimeTask);

            //for (int i = 0; i < 10; i++)
            //{
            //    IAsyncResult R = null;
            //    R = d.BeginInvoke("TestString", null, null); //invoking the method 
            //}
        }

        public void LongTimeTask(string s)
        {
            int blastid = 8;
            int emailid = 8;
            Counter++;
            // Write your time consuming task here
            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            //cmd.CommandType = System.Data.CommandType.Text;
            //cmd.CommandText = "if not exists (select top 1 EAID from EmailActivityLog where EmailID = " + emailid + " and BlastID = " + blastid + " and ActionTypeCode = 'click' and datediff(ss,ActionDate,getdate()) <= 5) INSERT INTO EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate) VALUES (@EmailID, @BlastID, @ActionTypeCode, @ActionValue, GetDate());";
            //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmailID", emailid));
            //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BlastID", blastid));
            //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionTypeCode", "click"));
            //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionValue", Counter.ToString() + ": Old Code"));
            //ExecuteScalar(cmd, "Communicator");
            string SQLQuery = string.Empty;
            SQLQuery = " if not exists (select top 1 EAID from EmailActivityLog where EmailID = @EmailID and BlastID = @BlastID and ActionTypeCode = @ActionTypeCode and datediff(ss,ActionDate,getdate()) <= 5) ";
            SQLQuery += "INSERT INTO EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate) VALUES (@EmailID, @BlastID, @ActionTypeCode, @ActionValue, GetDate());";

            //-- for tracking UNIQUE open & click records per profile - commented on 06/25/2010 - as per Iris. 
            //if (event_type.ToLower() == "read" || event_type.ToLower() == "open" ) 
            //    SQLQuery = " IF NOT EXISTS (SELECT TOP 1 EAID FROM EmailActivityLog WHERE EmailID = " + EmailID + " AND BlastID = " + BlastID + " AND ActionTypeCode = '" + event_type + "') ";
            //else if (event_type.ToLower() == "click")
            //    SQLQuery = " IF NOT EXISTS (SELECT TOP 1 EAID FROM EmailActivityLog WHERE EmailID = " + EmailID + " AND BlastID = " + BlastID + " AND ActionTypeCode = '" + event_type + "' AND ActionValue='" + info + "') ";

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = SQLQuery;
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmailID", emailid));
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BlastID", blastid));
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionTypeCode", "click"));
            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionValue", Counter.ToString() + ": Old Code"));
            ExecuteScalar(cmd, "Communicator");
        }

        //public void LongTimeTask(string s)
        //{
        //    int blastid = 6;
        //    int emailid = 6;
        //    Counter++;
        //    // Write your time consuming task here
        //    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        //    cmd.CommandType = System.Data.CommandType.Text;
        //    cmd.CommandText = " INSERT INTO EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate) SELECT @EmailID, @BlastID, @ActionTypeCode, @ActionValue, GetDate() where not exists (select top 1 EAID from EmailActivityLog where EmailID = " + emailid + " and BlastID = " + blastid + " and ActionTypeCode = 'click' and datediff(ss,ActionDate,getdate()) <= 5);";
        //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmailID", emailid));
        //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BlastID", blastid));
        //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionTypeCode", "click"));
        //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionValue", Counter.ToString() + ": New Code"));
        //    ExecuteScalar(cmd, "Communicator");
        //}

        public static object ExecuteScalar(SqlCommand cmd, string connectionStringName)
        {
            object obj = null;
            using (SqlConnection conn = GetSqlConnection(connectionStringName))
            {
                cmd = MinDateCheck(cmd);
                cmd = MinTimeCheck(cmd);
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                try
                {
                cmd.Connection.Open();
                obj = cmd.ExecuteScalar();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (cmd != null)
                    {
                cmd.Connection.Close();
                        cmd.Dispose();
                    }
                }
            }
            return obj;
        }

        public static SqlConnection GetSqlConnection(string connectionStringName)
        {
            string connectionString;
            bool isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

            if (isDemo == true)
                connectionString = ConfigurationManager.ConnectionStrings[connectionStringName.ToString()].ToString();
            else
                connectionString = ConfigurationManager.ConnectionStrings[connectionStringName.ToString()].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        private static SqlCommand MinDateCheck(SqlCommand cmd)
        {
            foreach (SqlParameter p in cmd.Parameters)
            {
                if (p != null && p.Value != null)
                {
                    if (p.Value.ToString().Equals(DateTime.MinValue.ToString()))
                    {
                        p.Value = GetMinDate().ToString();
                    }
                }
            }
            return cmd;
        }

        private static SqlCommand MinTimeCheck(SqlCommand cmd)
        {
            foreach (SqlParameter p in cmd.Parameters)
            {
                if (p != null && p.Value != null)
                {
                    if (p.Value.ToString().Equals(TimeSpan.MinValue.ToString()))
                    {
                        p.Value = GetMinTime().ToString();
                    }
                }
            }
            return cmd;
        }

        public static DateTime GetMinDate()
        {
            DateTime minDate = Convert.ToDateTime("1/1/1900");
            return minDate;
        }

        public static TimeSpan GetMinTime()
        {
            TimeSpan minTime;
            TimeSpan.TryParse("00:00:00.0000000", out minTime);
            return minTime;
        }
    }
}