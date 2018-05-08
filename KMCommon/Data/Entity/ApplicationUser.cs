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
    public class ApplicationUser
    {
        public ApplicationUser() 
        {
            ApplicationID = -1;
            UserID = -1;
        }
        #region Properties
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        #endregion

        public static bool Validate(ApplicationUser appUser)
        {
            if (appUser.ApplicationID <= 0)
                return false;
            if (appUser.UserID <= 0)
                return false;
            return true;
        }

        public static bool Delete(int? applicationID, int? userID)
        {
            if (applicationID != null || userID != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_ApplicationUser_Delete";
                cmd.Parameters.Add(new SqlParameter("@ApplicationID", (object)applicationID ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)userID ?? DBNull.Value));
                return DataFunctions.ExecuteNonQuery(cmd);
            }
            else
                return false;
        }

        public static bool Insert(ref ApplicationUser appUser)
        {
            if (!Validate(appUser))
                return false;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationUser_Insert";
            cmd.Parameters.Add(new SqlParameter("@UserID", appUser.UserID));
            cmd.Parameters.Add(new SqlParameter("@ApplicationID", appUser.ApplicationID));
            return DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
