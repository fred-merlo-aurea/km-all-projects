using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace KMPS_Tools
{
    public class Blast
    {
        public int BlastID { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public DateTime sendTime { get; set; }

        public int IssueID { get; set; }

        static string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString;

        public static Blast getBlastDetails(int BlastID)
        {
            int index;
            string name;

            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("select BlastID, EmailFrom, EmailSubject, SendTime from Blast where BlastID = @BlastID", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@BlastID", BlastID);

            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            Blast b = new Blast();
            b.BlastID = BlastID;

            while (dr.Read())
            {
                name = "EmailFrom";
                index = dr.GetOrdinal(name);
                if (index >= 0 && !dr.IsDBNull(index))
                    b.EmailFrom = dr[index].ToString();

                name = "EmailSubject";
                index = dr.GetOrdinal(name);
                if (index >= 0 && !dr.IsDBNull(index))
                    b.Subject = dr[index].ToString();

                name = "SendTime";
                index = dr.GetOrdinal(name);
                if (index >= 0 && !dr.IsDBNull(index))
                    b.sendTime = Convert.ToDateTime(dr[index]);
            }

            conn.Close();

            return b;
        }

        public Dictionary<string, string> getBounces(int BlastID)
        {
            Dictionary<string, string> dBounces = new Dictionary<string, string>();

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select distinct e.EmailID, e.Emailaddress, bc.BounceMessage from ecn_Activity..BlastActivityBounces bc with (NOLOCK) join emails e with (NOLOCK)  on bc.emailID = e.emailID where bc.blastID =  @blastID ", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@blastID", BlastID);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                if (!dBounces.ContainsKey(rdr["Emailaddress"].ToString().ToLower()))
                    dBounces.Add(rdr["Emailaddress"].ToString().ToLower(), rdr["BounceMessage"].ToString());
            }

            conn.Close();

            return dBounces;
        }

        public Dictionary<string, int> getSends(int BlastID)
        {
            Dictionary<string, int> dSends = new Dictionary<string, int>();

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ECNCommunicator"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select distinct e.Emailaddress, abs(bs.sendid) as sendid from ecn_Activity..BlastActivitySends bs with (NOLOCK) join emails e with (NOLOCK)  on bs.emailID = e.emailID where bs.blastID = @blastID", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@blastID", BlastID);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                if (!dSends.ContainsKey(rdr["Emailaddress"].ToString().ToLower()))
                    dSends.Add(rdr["Emailaddress"].ToString().ToLower(), Convert.ToInt32(rdr["SendID"]));
            }

            conn.Close();

            return dSends;
        }


    }



}
