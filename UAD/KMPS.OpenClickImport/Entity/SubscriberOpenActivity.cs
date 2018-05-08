using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.ActivityImport.Entity
{
    public class SubscriberOpenActivity
    {
        public SubscriberOpenActivity() { }
        #region Properties
        public int OpenActivityID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int BlastID { get; set; }
        public DateTime ActivityDate { get; set; }
        #endregion

        public static void Insert(int pubSubscriptionID, int blastID, DateTime activityDate, string sql)
        {
            //CREATE PROCEDURE e_SubscriberOpenActivity_Insert
            //@PubSubscriptionID int, 
            //@BlastID int, 
            //@ActivityDate date
            //AS
            //IF NOT EXISTS(Select OpenActivityID From SubscriberOpenActivity WHERE PubSubscriptionID = @PubSubscriptionID AND ActivityDate = @ActivityDate AND @PubSubscriptionID > 0)
            //    BEGIN
            //        INSERT INTO SubscriberOpenActivity (PubSubscriptionID, BlastID, ActivityDate)
            //        VALUES(@PubSubscriptionID,@BlastID,@ActivityDate)
            //    END

            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOpenActivity_Insert";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@PubSubscriptionID", pubSubscriptionID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@ActivityDate", activityDate);
            cmd.Connection.Open();
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public static void Insert(SubscriberOpenActivity x, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberOpenActivity_Insert";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@PubSubscriptionID", x.PubSubscriptionID);
            cmd.Parameters.AddWithValue("@BlastID", x.BlastID);
            cmd.Parameters.AddWithValue("@ActivityDate", x.ActivityDate);
            
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public static void Import(string xml, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ImportSubscriberOpenActivity_XML";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Xml", xml);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
