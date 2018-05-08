using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class ClientFTP
    {
        public static List<Entity.ClientFTP> Select()
        {
            List<Entity.ClientFTP> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientFTP_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.ClientFTP> SelectClient(int clientID)
        {
            List<Entity.ClientFTP> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientFTP_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.ClientFTP Get(SqlCommand cmd)
        {
            Entity.ClientFTP retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientFTP();
                        DynamicBuilder<Entity.ClientFTP> builder = DynamicBuilder<Entity.ClientFTP>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
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
            return retItem;
        }
        private static List<Entity.ClientFTP> GetList(SqlCommand cmd)
        {
            List<Entity.ClientFTP> retList = new List<Entity.ClientFTP>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientFTP retItem = new Entity.ClientFTP();
                        DynamicBuilder<Entity.ClientFTP> builder = DynamicBuilder<Entity.ClientFTP>.CreateBuilder(rdr);
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

        public static int Save(Entity.ClientFTP x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientFTP_Save";
            cmd.Parameters.Add(new SqlParameter("@FTPID", x.FTPID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@Server", x.Server));
            cmd.Parameters.Add(new SqlParameter("@UserName", x.UserName));
            cmd.Parameters.Add(new SqlParameter("@Password", x.Password));
            cmd.Parameters.Add(new SqlParameter("@Folder", x.Folder));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", x.IsDeleted));
            cmd.Parameters.Add(new SqlParameter("@IsExternal", x.IsExternal));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@FTPConnectionValidated", x.FTPConnectionValidated));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
