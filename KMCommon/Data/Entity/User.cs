using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class User
    {
        public User() 
        {
            UserID = -1;
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
        }
        #region Properties
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        #endregion

        public static List<User> GetByApplicationID(int applicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ApplicationID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            return GetList(cmd);
        }

        public static User GetByUserID(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            return Get(cmd);
        }

        private static List<User> GetList(SqlCommand cmd)
        {
            List<User> retList = new List<User>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    User retItem = new User();
                    var builder = DynamicBuilder<User>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                }
            }

            return retList;
        }

        private static User Get(SqlCommand cmd)
        {
            User retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    retItem = new User();
                    var builder = DynamicBuilder<User>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                }
            }

            return retItem;
        }

        public static bool Validate(User user)
        {
            if (user.FirstName.Trim().Length == 0)
                return false;
            if (user.LastName.Trim().Length == 0)
                return false;
            if (user.EmailAddress.Trim().Length == 0)
                return false;
            return true;
        }

        public static bool Save(ref User user)
        {
            if (!Validate(user))
                return false;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Save";
            cmd.Parameters.Add(new SqlParameter("@UserID", user.UserID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", user.FirstName)); 
            cmd.Parameters.Add(new SqlParameter("@LastName", user.LastName));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", user.EmailAddress));

            int id = 0;
            int.TryParse(DataFunctions.ExecuteScalar(cmd).ToString(), out id);
            if (id > 0)
            {
                user.UserID = id;
                return true;
            }
            return false;
        }

        public static bool Delete(int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Delete";
            cmd.Parameters.AddWithValue("@UserID", userID);
            return DataFunctions.ExecuteNonQuery(cmd);
        }
    }    
}
