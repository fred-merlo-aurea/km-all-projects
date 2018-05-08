using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformSplitTrans
    {
        public static List<Entity.TransformSplitTrans> Select()
        {
            List<Entity.TransformSplitTrans> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SplitTransform_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformSplitTrans> Select(int TransformationID)
        {
            List<Entity.TransformSplitTrans> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SplitTransform_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformSplitTrans> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformSplitTrans> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SplitTransform_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", sourceFileID));

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.TransformSplitTrans Get(SqlCommand cmd)
        {
            Entity.TransformSplitTrans retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformSplitTrans();
                        DynamicBuilder<Entity.TransformSplitTrans> builder = DynamicBuilder<Entity.TransformSplitTrans>.CreateBuilder(rdr);
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

        private static List<Entity.TransformSplitTrans> GetList(SqlCommand cmd)
        {
            List<Entity.TransformSplitTrans> retList = new List<Entity.TransformSplitTrans>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformSplitTrans retItem = new Entity.TransformSplitTrans();
                        DynamicBuilder<Entity.TransformSplitTrans> builder = DynamicBuilder<Entity.TransformSplitTrans>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformSplitTrans x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SplitTransform_Save";
            cmd.Parameters.Add(new SqlParameter("@SplitTransformID", x.SplitTransformID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@SplitBeforeID", x.SplitBeforeID));
            cmd.Parameters.Add(new SqlParameter("@DataMapID", x.DataMapID));
            cmd.Parameters.Add(new SqlParameter("@SplitAfterID", x.SplitAfterID));
            cmd.Parameters.Add(new SqlParameter("@Column", x.Column));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
