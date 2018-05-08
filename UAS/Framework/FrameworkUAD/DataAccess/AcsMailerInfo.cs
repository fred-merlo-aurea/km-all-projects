using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class AcsMailerInfo
    {
        public static Entity.AcsMailerInfo SelectByID(int acsMailerInfoID, KMPlatform.Object.ClientConnections client)
        {
            Entity.AcsMailerInfo retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@AcsMailerInfoId", acsMailerInfoID));
            cmd.CommandText = "e_AcsMailerInfo_Select_ID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = Get(cmd);
            return retItem;
        }

        public static List<Entity.AcsMailerInfo> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.AcsMailerInfo> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsMailerInfo_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.AcsMailerInfo Get(SqlCommand cmd)
        {
            Entity.AcsMailerInfo retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.AcsMailerInfo();
                        DynamicBuilder<Entity.AcsMailerInfo> builder = DynamicBuilder<Entity.AcsMailerInfo>.CreateBuilder(rdr);
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
        private static List<Entity.AcsMailerInfo> GetList(SqlCommand cmd)
        {
            List<Entity.AcsMailerInfo> retList = new List<Entity.AcsMailerInfo>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.AcsMailerInfo retItem = new Entity.AcsMailerInfo();
                        DynamicBuilder<Entity.AcsMailerInfo> builder = DynamicBuilder<Entity.AcsMailerInfo>.CreateBuilder(rdr);
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

        public static int Save(Entity.AcsMailerInfo x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsMailerInfo_Save";
            cmd.Parameters.Add(new SqlParameter("@AcsMailerInfoId", x.AcsMailerInfoId));
            cmd.Parameters.Add(new SqlParameter("@AcsCode", x.AcsCode));
            cmd.Parameters.Add(new SqlParameter("@MailerID", x.MailerID));
            cmd.Parameters.Add(new SqlParameter("@ImbSeqCounter", x.ImbSeqCounter));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
