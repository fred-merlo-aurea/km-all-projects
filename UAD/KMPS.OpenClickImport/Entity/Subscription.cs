using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace KMPS.ActivityImport.Entity
{
    public class Subscription
    {
        public string FName { get; set; }
        public string LName { get; set; }
       
        public static List<Subscription> Get(string email, string sql)
        {
            //CREATE PROCEDURE e_Subscriptions_Select_Email
            //@Email varchar(100)
            //AS
            //SELECT * 
            //FROM Subscriptions With(NoLock)
            //WHERE EMAIL = @Email

            List<Subscription> retList = new List<Subscription>();
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriptions_Select_Email";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DynamicBuilder<Subscription> builder = DynamicBuilder<Subscription>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                Subscription x = new Subscription();
                x = builder.Build(rdr);
                retList.Add(x);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();

            return retList;
        }

        public static int Insert(string firstName, string lastName, string emailAddress, string address, string city, string state, string zip, string sql)
        {
            //CREATE PROCEDURE e_Subscription_Insert_Profile
            //@FirstName varchar(100),
            //@LastName varchar(100),
            //@EmailAddress varchar(100),
            //@Address varchar(255),
            //@City varchar(50),
            //@State varchar(50),
            //@Zip varchar(10)
            //AS

            //INSERT INTO Subscriptions (SEQUENCE,FNAME,LNAME,EMAIL,EmailExists,ADDRESS,CITY,STATE,ZIP)
            //VALUES(-1,@FirstName,@LastName,@EmailAddress,'true',@Address,@City,@State,@Zip);SELECT @@IDENTITY;

            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Insert_Profile";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@Zip", zip);
            cmd.Connection.Open();
            int ID = 0;
            int.TryParse(cmd.ExecuteScalar().ToString(), out ID);
            cmd.Connection.Close();
            return ID;
        }
    }
}
