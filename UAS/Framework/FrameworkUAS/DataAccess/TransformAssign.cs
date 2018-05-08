using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformAssign
    {
        public static List<Entity.TransformAssign> Select()
        {
            List<Entity.TransformAssign> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformAssign_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformAssign> Select(int TransformationID)
        {
            List<Entity.TransformAssign> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformAssign_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformAssign> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformAssign> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformAssign_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", sourceFileID));

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.TransformAssign Get(SqlCommand cmd)
        {
            Entity.TransformAssign retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformAssign();
                        DynamicBuilder<Entity.TransformAssign> builder = DynamicBuilder<Entity.TransformAssign>.CreateBuilder(rdr);
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

        private static List<Entity.TransformAssign> GetList(SqlCommand cmd)
        {
            List<Entity.TransformAssign> retList = new List<Entity.TransformAssign>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformAssign retItem = new Entity.TransformAssign();
                        DynamicBuilder<Entity.TransformAssign> builder = DynamicBuilder<Entity.TransformAssign>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformAssign x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformAssign_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformAssignID", x.TransformAssignID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@Value", x.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@HasPubID", x.HasPubID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(int TransformAssignID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformAssign_Delete";
            cmd.Parameters.Add(new SqlParameter("@TransformAssignID", TransformAssignID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
