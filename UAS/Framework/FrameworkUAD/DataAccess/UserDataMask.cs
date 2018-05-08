using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
namespace FrameworkUAD.DataAccess
{
    public class UserDataMask
    {
        #region Data

        public static List<Entity.UserDataMask> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Entity.UserDataMask> udm = (List<Entity.UserDataMask>) CacheUtil.GetFromCache("USERDATAMASK", DatabaseName);

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
        private static List<Entity.UserDataMask> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.UserDataMask> retList = new List<Entity.UserDataMask>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.UserDataMask> builder = DynamicBuilder<Entity.UserDataMask>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.UserDataMask x = builder.Build(rdr);
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

        public static List<Entity.UserDataMask> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<Entity.UserDataMask> udm = UserDataMask.GetAll(clientconnection).FindAll(x => x.UserID == userID);
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.UserDataMask udm)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@MaskUserID", udm.UserID));
            cmd.Parameters.Add(new SqlParameter("@MaskField", udm.MaskField));
            cmd.Parameters.Add(new SqlParameter("@UserID", udm.CreatedUserID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserDataMask_Delete");
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
