using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Runtime.Serialization;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class UserDataMask
    {
        public UserDataMask() { }
        #region Properties
        [DataMember]
        public int UserDataMaskID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string MaskField { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        #endregion

        #region Data

        public static List<UserDataMask> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<UserDataMask> udm = (List<UserDataMask>)CacheUtil.GetFromCache("USERDATAMASK", DatabaseName);

                if (udm == null)
                {
                    udm = GetData(clientconnection);

                    CacheUtil.AddToCache("USERDATAMASK", udm, DatabaseName);
                }

                return udm;
            }
            else
            {
                return GetData(clientconnection);
            }
        }
        public static List<UserDataMask> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<UserDataMask> retList = new List<UserDataMask>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<UserDataMask> builder = DynamicBuilder<UserDataMask>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    UserDataMask x = builder.Build(rdr);
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

        public static List<UserDataMask> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<UserDataMask> udm = UserDataMask.GetAll(clientconnection).FindAll(x => x.UserID == userID);
            return udm;
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("USERDATAMASK", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("USERDATAMASK", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, UserDataMask udm)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaskUserID", udm.UserID));
            cmd.Parameters.Add(new SqlParameter("@MaskField", udm.MaskField));
            cmd.Parameters.Add(new SqlParameter("@UserID", udm.CreatedUserID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
