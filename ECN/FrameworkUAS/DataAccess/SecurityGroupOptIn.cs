using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    [Serializable]
    public class SecurityGroupOptIn
    {
        public static int Save(KMPlatform.Entity.SecurityGroupOptIn sgoi)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_Save";
            cmd.Parameters.AddWithValue("@UserID", sgoi.UserID);
            cmd.Parameters.AddWithValue("@ClientID", sgoi.ClientID);
            cmd.Parameters.AddWithValue("@ClientGroupID", sgoi.ClientGroupID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", sgoi.SecurityGroupID);
            cmd.Parameters.AddWithValue("@SendTime", sgoi.SendTime);
            cmd.Parameters.AddWithValue("@SetID", sgoi.SetID);
            cmd.Parameters.AddWithValue("@HasAccepted", sgoi.HasAccepted);
            cmd.Parameters.AddWithValue("@UserClientSecurityGroupMapID", sgoi.UserClientSecurityGroupMapID);
            cmd.Parameters.AddWithValue("@CreatedByUserID", sgoi.CreatedByUserID);
            cmd.Parameters.AddWithValue("@IsDeleted", sgoi.IsDeleted);
            if (sgoi.SecurityGroupOptInID > 0)
                cmd.Parameters.AddWithValue("@SecurityGroupOptInID", sgoi.SecurityGroupOptInID);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString());
        }

        public static void MarkAsAccepted(int securityGroupOptInID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_MarkAsAccepted";
            cmd.Parameters.AddWithValue("@SecurityGroupOptInID", securityGroupOptInID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }

        public static void Delete(int securityGroupOptInID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_Delete";
            cmd.Parameters.AddWithValue("@SecurityGroupOptInID", securityGroupOptInID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }

        public static void Delete(int securityGroupID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_Delete_SGID_UserID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }

        public static List<KMPlatform.Entity.SecurityGroupOptIn> GetBySetID(Guid setID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_Select_SetID";
            cmd.Parameters.AddWithValue("@SetID", setID);

            return GetList(cmd);
        }

        public static List<Entity.SecurityGroupOptIn> SelectPendingForUser(int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_SelectPending_UserID";
            cmd.Parameters.AddWithValue("@UserID", UserID);

            return GetList(cmd);
        }

        public static List<Entity.SecurityGroupOptIn> SelectForSecurityGroup_UserID(int securityGroupID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupOptIn_Select_SGID_UserID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            return GetList(cmd);
        }

        public static List<Entity.SecurityGroupOptIn> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroupOptIn> retList = new List<Entity.SecurityGroupOptIn>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroupOptIn retItem = new Entity.SecurityGroupOptIn();
                        var builder = DynamicBuilder<Entity.SecurityGroupOptIn>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
