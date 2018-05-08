using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class ClientCustomProcedure
    {
        public static List<Entity.ClientCustomProcedure> Select()
        {
            List<Entity.ClientCustomProcedure> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientCustomProcedure_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.ClientCustomProcedure> SelectClient(int clientID)
        {
            List<Entity.ClientCustomProcedure> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientCustomProcedure_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.ClientCustomProcedure Get(SqlCommand cmd)
        {
            Entity.ClientCustomProcedure retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientCustomProcedure();
                        DynamicBuilder<Entity.ClientCustomProcedure> builder = DynamicBuilder<Entity.ClientCustomProcedure>.CreateBuilder(rdr);
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
        private static List<Entity.ClientCustomProcedure> GetList(SqlCommand cmd)
        {
            List<Entity.ClientCustomProcedure> retList = new List<Entity.ClientCustomProcedure>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientCustomProcedure retItem = new Entity.ClientCustomProcedure();
                        DynamicBuilder<Entity.ClientCustomProcedure> builder = DynamicBuilder<Entity.ClientCustomProcedure>.CreateBuilder(rdr);
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

        public static int Save(Entity.ClientCustomProcedure x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientCustomProcedure_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientCustomProcedureID", x.ClientCustomProcedureID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@ProcedureType", x.ProcedureType));
            cmd.Parameters.Add(new SqlParameter("@ProcedureName", x.ProcedureName));
            cmd.Parameters.Add(new SqlParameter("@ExecutionOrder", x.ExecutionOrder));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", (object)x.ServiceFeatureID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExecutionPointID", x.ExecutionPointID));
            cmd.Parameters.Add(new SqlParameter("@IsForSpecialFile", x.IsForSpecialFile));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static bool ExecuteSproc(string sproc, int sfID, string fileName, KMPlatform.Entity.Client client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientCustomProcedure_ExecuteSproc";
            cmd.Parameters.Add(new SqlParameter("@sproc", sproc));
            cmd.Parameters.Add(new SqlParameter("@srcFile", sfID));
            cmd.Parameters.Add(new SqlParameter("@ClientId", client.ClientID));
            cmd.Parameters.Add(new SqlParameter("@FileName", fileName));
            if (!string.IsNullOrEmpty(processCode))
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            SqlConnection conn = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            cmd.Connection = conn;

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool ExecuteSproc(string sproc, int sfID, string fileName, KMPlatform.Entity.Client client, string xml, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientCustomProcedure_ExecuteSproc_Xml";
            cmd.Parameters.Add(new SqlParameter("@sproc", sproc));
            cmd.Parameters.Add(new SqlParameter("@srcFile", sfID));
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Parameters.Add(new SqlParameter("@FileName", fileName));
            if (!string.IsNullOrEmpty(processCode))
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            SqlConnection conn = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            cmd.Connection = conn;

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
