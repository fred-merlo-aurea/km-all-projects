using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework.Common;
using KM.Common;

namespace ECN_Framework.Accounts.Entity
{
    [Flags]
    public enum StaffRoleEnum
    {
        CustomerService = 1,
        AccountExecutive = 2,
        AccountManager = 3,
        DemoManager = 4
    }

    public class Staff
    {
        public Staff()
        {
 
        }
        public Staff(string firstName, string lastName, string email, StaffRoleEnum role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Roles = (Int16)role;
        }

        public Staff(int staffid)
        {
            StaffID = staffid;
        }

        public int StaffID { get; set; }
        public int BaseChannelID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserID { get; set; }
        public int Roles { get; set; }
        public bool LicenseUpdateFlag { get; set; }
        public bool FeatureUpdateFlag { get; set; }

        #region Database Methods

        /// At this point, insert only!

        public void Save()
        {
            SqlConnection conn = ECN_Framework_DataLayer.DataFunctions.GetSqlConnection(ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()); 
            SqlCommand insert = new SqlCommand("INSERT INTO Staff (FirstName, LastName, Email, Roles) VALUES (@firstName, @lastName, @email, @roles);", conn);
            insert.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = FirstName;
            insert.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = LastName;
            insert.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = Email;
            insert.Parameters.Add("@roles", SqlDbType.SmallInt, 2).Value = (Int16)Roles;

            try
            {
                conn.Open();
                insert.Prepare();
                insert.ExecuteScalar();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                insert.Dispose();
            }
        }
        #endregion

        #region Static Database Methods

        public static List<Staff> GetAll()
        {

            return new List<Staff>();

            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                return GetData();
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache("Staff_Cache") == null)
            {
                List<Staff> lStaff = new List<Staff>();
                lStaff = GetData();
                ECN_Framework_Common.Functions.CacheHelper.AddToCache("Staff_Cache", lStaff);
                return lStaff;
            }
            else
            {
                return (List<Staff>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache("Staff_Cache");
            }
        }

         private static List<Staff> GetData()
        {
            List<Staff> retItemList = new List<Staff>();

            SqlCommand cmd = new SqlCommand("SELECT * from Staff");
            cmd.CommandType = CommandType.Text;

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                var builder = DynamicBuilder<Staff>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Staff retItem = builder.Build(rdr);
                    retItemList.Add(retItem);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItemList;
        }

         public static Staff GetStaffByID(int staffID)
         {
             return GetAll().Find(x => x.StaffID == staffID);
         }

         public static Staff GetStaffByUserID(int userID)
         {
             return GetAll().Find(x => x.UserID == userID);
         }


        public static List<Staff> GetStaffByRole(StaffRoleEnum role)
        {
            return (List<Staff>) GetAll().Where(x => x.Roles == (Int16)role);
        }

        public static Staff CurrentStaff
        {
            get
            {
                SecurityCheck sc = new SecurityCheck();
                return GetStaffByUserID(Convert.ToInt32(sc.UserID()));
            }
        }

        
        private static Staff GetStaff(DataRow row)
        {
            Staff staff = new Staff(Convert.ToString(row["FirstName"]), Convert.ToString(row["LastName"]), Convert.ToString(row["Email"]), (StaffRoleEnum)Convert.ToInt16(row["Roles"]));
            staff.StaffID = Convert.ToInt32(row["StaffID"]);
            return staff;
        }


        #endregion
    }
}
