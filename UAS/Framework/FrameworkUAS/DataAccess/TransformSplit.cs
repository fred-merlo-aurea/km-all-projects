using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformSplit
    {
        public static List<Entity.TransformSplit> Select()
        {
            List<Entity.TransformSplit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformSplit_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformSplit> Select(int TransformationID)
        {
            List<Entity.TransformSplit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformSplit_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformSplit> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformSplit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformSplit_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", sourceFileID));

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.TransformSplit Get(SqlCommand cmd)
        {
            Entity.TransformSplit retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformSplit();
                        DynamicBuilder<Entity.TransformSplit> builder = DynamicBuilder<Entity.TransformSplit>.CreateBuilder(rdr);
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

        private static List<Entity.TransformSplit> GetList(SqlCommand cmd)
        {
            List<Entity.TransformSplit> retList = new List<Entity.TransformSplit>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformSplit retItem = new Entity.TransformSplit();
                        DynamicBuilder<Entity.TransformSplit> builder = DynamicBuilder<Entity.TransformSplit>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformSplit x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformSplit_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformSplitID", x.TransformSplitID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@Delimiter", x.Delimiter));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        #region Object.TransformSplitInfo
        public static List<Object.TransformSplitInfo> SelectObject(int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_TransformSplitInfo_Select";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            return GetObjectList(cmd);
        }
        public static List<Object.TransformSplitInfo> GetObjectList(SqlCommand cmd)
        {
            List<Object.TransformSplitInfo> retList = new List<Object.TransformSplitInfo>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.TransformSplitInfo retItem = new Object.TransformSplitInfo();
                        DynamicBuilder<Object.TransformSplitInfo> builder = DynamicBuilder<Object.TransformSplitInfo>.CreateBuilder(rdr);
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
        #endregion
    }
}
