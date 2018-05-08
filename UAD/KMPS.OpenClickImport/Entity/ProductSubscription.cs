using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.ActivityImport.Entity
{
    public class ProductSubscription
    {
        public ProductSubscription() { }
        #region Properties
        public int PubSubscriptionID { get; set; }
        public int SubscriptionID { get; set; }
        public int PubID { get; set; }
        public string demo7 { get; set; }
        public DateTime Qualificationdate { get; set; }
        public int PubQSourceID { get; set; }
        public int PubCategoryID { get; set; }
        public int PubTransactionID { get; set; }
        public int EmailStatusID { get; set; }
        #endregion

        #region Data
        public static List<ProductSubscription> Get(string pubCode, string emailAddress, string sql)
        {
            //CREATE PROCEDURE e_ProductSubscription_Select_PubCode_EmailAddress
            //@PubCode varchar(50),
            //@EmailAddress varchar(100)
            //AS
            //DECLARE @PubID int = (SELECT ISNULL(PubID,-1) FROM Pubs WHERE PubCode = @PubCode)

            //SELECT *
            //FROM PubSubscriptions With(NoLock)
            //WHERE SubscriptionID IN (SELECT SubscriptionID FROM Subscriptions WHERE EMAIL = @EmailAddress)
            //AND PubID = @PubID

            List<ProductSubscription> retList = new List<ProductSubscription>();
            if (!string.IsNullOrEmpty(emailAddress))
            {
                SqlConnection conn = new SqlConnection(sql);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_ProductSubscription_Select_PubCode_EmailAddress";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@PubCode", pubCode);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                cmd.Connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                try
                {
                    DynamicBuilder<ProductSubscription> builder = DynamicBuilder<ProductSubscription>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ProductSubscription x = new ProductSubscription();
                        x = builder.Build(rdr);
                        retList.Add(x);
                    }
                    rdr.Close();
                    rdr.Dispose();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    ex.StackTrace.ToString();
                }
            }
            return retList;
        }
        public static ProductSubscription Get(int pubID, int subscriptionID, string sql)
        {
            //CREATE PROCEDURE e_ProductSubscription_Select_PubID_SubscriptionID
            //@SubscriptionID int,
            //@PubID int
            //AS
            //SELECT *
            //FROM PubSubscriptions With(NoLock)
            //WHERE SubscriptionID = @SubscriptionID AND PubID = @PubID 


            ProductSubscription retItem = new ProductSubscription();
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_PubID_SubscriptionID";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DynamicBuilder<ProductSubscription> builder = DynamicBuilder<ProductSubscription>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                retItem = builder.Build(rdr);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();

            return retItem;
        }
        public static int Insert(ProductSubscription x, string sql)
        {
            //CREATE PROCEDURE e_ProductSubscription_Insert
            //@SubscriptionID int,
            //@PubID int,
            //@demo7 varchar(1),
            //@Qualificationdate date,
            //@PubQSourceID int,
            //@PubCategoryID int,
            //@PubTransactionID int,
            //@EmailStatusID int
            //AS
            //INSERT INTO PubSubscriptions (SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID)
            //VALUES(@SubscriptionID,@PubID,@demo7,@Qualificationdate,@PubQSourceID,@PubCategoryID,@PubTransactionID,@EmailStatusID)

            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Insert";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@SubscriptionID", x.SubscriptionID);
            cmd.Parameters.AddWithValue("@PubID", x.PubID);
            cmd.Parameters.AddWithValue("@demo7", x.demo7);
            cmd.Parameters.AddWithValue("@Qualificationdate", x.Qualificationdate);
            cmd.Parameters.AddWithValue("@PubQSourceID", x.PubQSourceID);
            cmd.Parameters.AddWithValue("@PubCategoryID", x.PubCategoryID);
            cmd.Parameters.AddWithValue("@PubTransactionID", x.PubTransactionID);
            cmd.Parameters.AddWithValue("@EmailStatusID", x.EmailStatusID);
            cmd.Connection.Open();
            int ID = 0;
            int.TryParse(cmd.ExecuteScalar().ToString(), out ID);
            cmd.Connection.Close();
            return ID;
        }
        #endregion
    }
}
