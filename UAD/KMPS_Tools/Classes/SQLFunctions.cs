using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KMPS_Tools
{
    public static class SQLFunctions
    {
        static string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["KMPSWQT"].ConnectionString;

        public static int getMagazineID(string MagazineCode)
        {
            int MagazineID;

            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("select MagazineID from Magazines where Magazine_code=@MagazineCode", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@MagazineCode", MagazineCode);

            cmd.Connection.Open();
            MagazineID = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            return MagazineID;
        }

        public static  Dictionary<string, int> getQSource()
        {
            Dictionary<string, int> Qsource = new Dictionary<string, int>(); ;

            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("select QSource_ID, QSource_Value from QSource", conn);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                Qsource.Add(rdr["QSource_Value"].ToString(),  Convert.ToInt32(rdr["QSource_ID"]));
            }

            conn.Close();

            return Qsource;
        }

        public static void UpdateMagazineIssueDate(int MagazineID, DateTime? IssueDate)
        {
                SqlConnection conn = new SqlConnection(connectionstring);
                SqlCommand cmdUpdate = new SqlCommand("update magazines set Issuedate = @IssueDate where magazineID= @MagazineID", conn);

                cmdUpdate.CommandTimeout = 0;
                cmdUpdate.CommandType = CommandType.Text;

                cmdUpdate.Parameters.AddWithValue("@MagazineID", MagazineID);
                cmdUpdate.Parameters.AddWithValue("@Issuedate", String.Format("{0:MM/dd/yyyy}", IssueDate));

                conn.Open();
                cmdUpdate.ExecuteNonQuery();
                conn.Close();
        }

        public static void ImportSubscribers(int MagazineID, string xmlImport)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("sp_ImportSubscriber", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MagazineID", MagazineID);
            cmd.Parameters.AddWithValue("@XML", xmlImport);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                conn.Close();
            }
        }

        public static void DeleteSubscribers(int MagazineID)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            //SqlCommand cmd = new SqlCommand("delete from subscriptiondetails From subscriptiondetails sd join subscriptions s on sd.SubscriptionID = s.SubscriptionID where MagazineID = @magazineID; delete from subscriptions where MagazineID = @magazineID", conn);

            SqlCommand cmd = new SqlCommand("sp_DeleteSubscribers", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MagazineID", MagazineID);

            try
            {
                cmd.Connection.Open();
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

        public static void ProcessSubscriberData(int MagazineID)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("sp_ImportSubscriberDetails", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MagazineID", MagazineID);

            try
            {
                cmd.Connection.Open();
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

        

        public static List<string> getMagazineFields(int MagazineID)
        {
            List<string> lMagazineFields = new List<string>();

            lMagazineFields.Add("SEQUENCE"); 
            lMagazineFields.Add("EMAIL"); 
            lMagazineFields.Add("FNAME"); 
            lMagazineFields.Add("LNAME"); 
            lMagazineFields.Add("COMPANY"); 
            lMagazineFields.Add("TITLE"); 
            lMagazineFields.Add("ADDRESS"); 
            lMagazineFields.Add("MAILSTOP"); 
            lMagazineFields.Add("CITY"); 
            lMagazineFields.Add("STATE"); 
            lMagazineFields.Add("ZIP"); 
            lMagazineFields.Add("PLUS4"); 
            lMagazineFields.Add("COUNTRY"); 
            lMagazineFields.Add("CTRY"); 
            lMagazineFields.Add("PHONE");
            lMagazineFields.Add("FAX");
            lMagazineFields.Add("FORZIP");   
            lMagazineFields.Add("DEMO7"); 
            lMagazineFields.Add("DEMO31"); 
            lMagazineFields.Add("DEMO32"); 
            lMagazineFields.Add("DEMO33"); 
            lMagazineFields.Add("DEMO34"); 
            lMagazineFields.Add("DEMO35"); 
            lMagazineFields.Add("DEMO36"); 
            lMagazineFields.Add("CAT"); 
            lMagazineFields.Add("XACT"); 
            lMagazineFields.Add("XACTDATE");  
            lMagazineFields.Add("QSOURCE"); 
            lMagazineFields.Add("QDATE"); 
            lMagazineFields.Add("SUBSRC"); 
            lMagazineFields.Add("PAR3C"); 
            lMagazineFields.Add("EXPIRE");
            lMagazineFields.Add("COPIES");
            
            //added 4/22/2014
            lMagazineFields.Add("MOBILE"); 
            lMagazineFields.Add("COUNTY"); 

            //For MHC
            lMagazineFields.Add("INCOME1");
            lMagazineFields.Add("AGE1");
            lMagazineFields.Add("HOME_VALUE");

            //Added 08/23/2014

            lMagazineFields.Add("MBR_ID");

            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("select upper(responsegroupname) as responsegroupname from ResponseGroups where MagazineID = @MagazineID", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@MagazineID", MagazineID);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                if (!lMagazineFields.Contains(rdr["responsegroupname"].ToString()))
                    lMagazineFields.Add(rdr["responsegroupname"].ToString());
            }

            conn.Close();

            return lMagazineFields;
        }
    }
}
