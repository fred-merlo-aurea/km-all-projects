using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class Batch
    {
        public static Entity.Batch StartNewBatch(int userID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Entity.Batch newBatch = new Entity.Batch();
            newBatch.BatchCount = 0;
            newBatch.DateCreated = DateTime.Now;
            newBatch.DateFinalized = null;
            newBatch.IsActive = true;
            newBatch.UserID = userID;
            newBatch.PublicationID = publicationID;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Batch_Create";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            newBatch = Get(cmd);

            return newBatch;
        }
        public static List<Entity.Batch> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Batch_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Batch> Select(int userID, bool isActive, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Batch_Select_UserID_IsActive";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", isActive));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        private static Entity.Batch Get(SqlCommand cmd)
        {
            Entity.Batch retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Batch();
                        DynamicBuilder<Entity.Batch> builder = DynamicBuilder<Entity.Batch>.CreateBuilder(rdr);
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
        private static List<Entity.Batch> GetList(SqlCommand cmd)
        {
            List<Entity.Batch> retList = new List<Entity.Batch>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Batch retItem = new Entity.Batch();
                        DynamicBuilder<Entity.Batch> builder = DynamicBuilder<Entity.Batch>.CreateBuilder(rdr);
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
        private static bool Validate(Entity.Batch x)
        {
            bool isValid = true;

            if (x.BatchCount < 1)
                x.BatchCount = 1;
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            //should thrown an error if UserID not > 0
            return isValid;
        }
        public static int Save(Entity.Batch x, KMPlatform.Object.ClientConnections client)
        {
            if (Validate(x))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Batch_Save";
                cmd.Parameters.Add(new SqlParameter("@BatchID", x.BatchID));
                cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
                cmd.Parameters.Add(new SqlParameter("@BatchCount", x.BatchCount));
                cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
                cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
                cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
                cmd.Parameters.Add(new SqlParameter("@DateFinalized", (object)x.DateFinalized ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@BatchNumber", x.BatchNumber));
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);

                return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            }
            else
                return -1;
        }
        public static bool CloseBatches(int userID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Batch_CloseBatches";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static int FinalizeBatchID(int batchID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Batch_FinalizeBatchID";
            cmd.Parameters.Add(new SqlParameter("@BatchID", batchID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
