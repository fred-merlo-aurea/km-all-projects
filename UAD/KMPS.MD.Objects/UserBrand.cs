using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class UserBrand
    {
        public UserBrand() { }

        #region Properties
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int BrandID { get; set; }
        #endregion

        #region Data
        public static List<UserBrand> getAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<UserBrand> retList = new List<UserBrand>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select ub.* from UserBrand ub join brand b on ub.BrandID = b.BrandID where b.Isdeleted = 0", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<UserBrand> builder = DynamicBuilder<UserBrand>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    UserBrand x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<UserBrand> getByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<UserBrand> ub = UserBrand.getAll(clientconnection).FindAll(x => x.UserID == userID);
            return ub;
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, UserBrand u)
        {
            SqlCommand cmd = new SqlCommand("e_UserBrand_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", u.UserID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", u.BrandID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            SqlCommand cmd = new SqlCommand("e_UserBrand_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}
