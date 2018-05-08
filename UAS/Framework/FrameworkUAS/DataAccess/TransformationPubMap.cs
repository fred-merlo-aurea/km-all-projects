using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformationPubMap
    {
        public static List<Entity.TransformationPubMap> Select()
        {
            List<Entity.TransformationPubMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationPubMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformationPubMap> Select(int transformationID)
        {
            List<Entity.TransformationPubMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationPubMap_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", transformationID));

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.TransformationPubMap Get(SqlCommand cmd)
        {
            Entity.TransformationPubMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformationPubMap();
                        DynamicBuilder<Entity.TransformationPubMap> builder = DynamicBuilder<Entity.TransformationPubMap>.CreateBuilder(rdr);
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
        private static List<Entity.TransformationPubMap> GetList(SqlCommand cmd)
        {
            List<Entity.TransformationPubMap> retList = new List<Entity.TransformationPubMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformationPubMap retItem = new Entity.TransformationPubMap();
                        DynamicBuilder<Entity.TransformationPubMap> builder = DynamicBuilder<Entity.TransformationPubMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformationPubMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationPubMap_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformationPubMapID", x.TransformationPubMapID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(int TransformationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationPubMap_Delete";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(int TransformationID, int PubID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationPubMap_Delete_TransformationIDandPubID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
