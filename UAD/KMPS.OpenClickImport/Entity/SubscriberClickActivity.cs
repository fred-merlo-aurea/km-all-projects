using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.ActivityImport.Entity
{
    public class SubscriberClickActivity
    {
        public SubscriberClickActivity() { }
        #region Properties
        public int ClickActivityID { get; set; }
        public int PubSubscriptionID { get; set; }
        public int BlastID { get; set; }
        public string Link { get; set; }
        public string LinkAlias { get; set; }
        public string LinkSource { get; set; }
        public string LinkType { get; set; }
        public DateTime ActivityDate { get; set; }
        #endregion

        public static int Insert(SubscriberClickActivity x, string sql)
        {
            //CREATE PROCEDURE e_SubscriberClickActivity_Insert
            //@PubSubscriptionID int,
            //@BlastID int,
            //@Link varchar(255),
            //@LinkAlias varchar(100),
            //@LinkSource varchar(50),
            //@LinkType varchar(50),
            //@ActivityDate date
            //AS
            //INSERT INTO SubscriberClickActivity (PubSubscriptionID,BlastID,Link,LinkAlias,LinkSource,LinkType,ActivityDate)
            //VALUES(@PubSubscriptionID,@BlastID,@Link,@LinkAlias, @LinkSource,@LinkType,@ActivityDate);SELECT @@IDENTITY;

            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberClickActivity_Insert";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@PubSubscriptionID", x.PubSubscriptionID);
            cmd.Parameters.AddWithValue("@BlastID", x.BlastID);
            cmd.Parameters.AddWithValue("@Link", x.Link);
            cmd.Parameters.AddWithValue("@LinkAlias", x.LinkAlias);
            cmd.Parameters.AddWithValue("@LinkSource", x.LinkSource);
            cmd.Parameters.AddWithValue("@LinkType", x.LinkType);
            cmd.Parameters.AddWithValue("@ActivityDate", x.ActivityDate);
            cmd.Connection.Open();
            int ID = 0;
            int.TryParse(cmd.ExecuteScalar().ToString(), out ID);
            cmd.Connection.Close();
            return ID;
        }
        public static void Import(string xml, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ImportSubscriberClickActivity_XML";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Xml", xml);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
