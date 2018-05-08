using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class FilterSegmentationGroup
    {
        public static List<Entity.FilterSegmentationGroup> SelectByFilterSegmentationID(int filterSegmentationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterSegmentationGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentationGroup_Select_FilterSegmentationID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", filterSegmentationID));
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.FilterSegmentationGroup Get(SqlCommand cmd)
        {
            Entity.FilterSegmentationGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterSegmentationGroup();
                        DynamicBuilder<Entity.FilterSegmentationGroup> builder = DynamicBuilder<Entity.FilterSegmentationGroup>.CreateBuilder(rdr);
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

        public static List<Entity.FilterSegmentationGroup> GetList(SqlCommand cmd)
        {
            List<Entity.FilterSegmentationGroup> retList = new List<Entity.FilterSegmentationGroup>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FilterSegmentationGroup retItem = new Entity.FilterSegmentationGroup();
                        DynamicBuilder<Entity.FilterSegmentationGroup> builder = DynamicBuilder<Entity.FilterSegmentationGroup>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retItem.FilterGroupID_Selected = rdr["FilterGroupID_Selected"].ToString() == null || rdr["FilterGroupID_Selected"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Selected"].ToString());
                                retItem.FilterGroupID_Suppressed = rdr["FilterGroupID_Suppressed"].ToString() == null || rdr["FilterGroupID_Suppressed"].ToString() == "" ? new List<int>() : GetFilterGroupID(rdr["FilterGroupID_Suppressed"].ToString());
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

        private static List<int> GetFilterGroupID(string FilterGroupIDs)
        {
            return FilterGroupIDs.Split(',').Select(n => int.Parse(n)).ToList();
        }

        public static int Save(Entity.FilterSegmentationGroup x, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentationGroup_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterSegmentationGroupID", x.FilterSegmentationGroupID);
            cmd.Parameters.AddWithValue("@FilterSegmentationID", x.FilterSegmentationID);

            string fgID_Selected = string.Empty;
            string fgID_Suppressed = string.Empty;

            if (x.FilterGroupID_Selected != null)
            {
                foreach (int i in x.FilterGroupID_Selected)
                {
                    fgID_Selected += fgID_Selected == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            if (x.FilterGroupID_Suppressed != null)
            {
                foreach (int i in x.FilterGroupID_Suppressed)
                {
                    fgID_Suppressed += fgID_Suppressed == string.Empty ? i.ToString() : "," + i.ToString();
                }
            }

            cmd.Parameters.AddWithValue("@FilterGroupID_Selected", fgID_Selected);
            cmd.Parameters.AddWithValue("@FilterGroupID_Suppressed", fgID_Suppressed);
            cmd.Parameters.AddWithValue("@SelectedOperation", x.SelectedOperation);
            cmd.Parameters.AddWithValue("@SuppressedOperation", (object)x.SuppressedOperation ?? DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        private static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("FilterSegmentationGroup", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("FilterSegmentationGroup", DatabaseName);
                }
            }
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public static int DeleteByFilterSegmentationID(int filterSegmentationID, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentationGroup_Delete_FilterSegmentationID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", filterSegmentationID));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
        }
    }
}

