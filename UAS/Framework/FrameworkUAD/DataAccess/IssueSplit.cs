using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class IssueSplit
    {
        public static List<Entity.IssueSplit> SelectIssueID(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueSplit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_Select_IssueID";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        
        public static bool ClearIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_Clear_Issue";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        

        private static Entity.IssueSplit Get(SqlCommand cmd)
        {
            Entity.IssueSplit retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueSplit();
                        DynamicBuilder<Entity.IssueSplit> builder = DynamicBuilder<Entity.IssueSplit>.CreateBuilder(rdr);
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
        private static List<Entity.IssueSplit> GetList(SqlCommand cmd)
        {
            List<Entity.IssueSplit> retList = new List<Entity.IssueSplit>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueSplit retItem = new Entity.IssueSplit();
                        DynamicBuilder<Entity.IssueSplit> builder = DynamicBuilder<Entity.IssueSplit>.CreateBuilder(rdr);
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

       
        public static int Save(Entity.IssueSplit x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_Save";
            cmd.Parameters.Add(new SqlParameter("@IssueSplitId", x.IssueSplitId));
            cmd.Parameters.Add(new SqlParameter("@IssueId", x.IssueId));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitCode", x.IssueSplitCode));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitName", x.IssueSplitName));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitCount", x.IssueSplitCount));
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@KeyCode", (object)x.KeyCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitRecords", x.IssueSplitRecords));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitDescription", (object)x.IssueSplitDescription ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int GetSubscriberCopiesCount(DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_Get_CopiesCount";
            cmd.Parameters.AddWithValue("@TVP_IssueSplitIds", dtIssueSplitPubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
            
        }

        public static int SaveNew(Entity.IssueSplit x,DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_SaveNew";
            cmd.Parameters.Add(new SqlParameter("@IssueSplitId", x.IssueSplitId));
            cmd.Parameters.Add(new SqlParameter("@IssueId", x.IssueId));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitCode", x.IssueSplitCode));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitName", x.IssueSplitName));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitCount", x.IssueSplitCount));
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object) x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object) x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@KeyCode", (object) x.KeyCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitRecords", x.IssueSplitRecords));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitDescription", (object) x.IssueSplitDescription ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TVP_IssueSplitArchivePubSubscriptionMap", dtIssueSplitPubs));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        internal static int UpdateIssueSplit(Entity.IssueSplit x,  ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_Update";
            cmd.Parameters.Add(new SqlParameter("@IssueSplitId", x.IssueSplitId));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID",(object) x.WaveMailingID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitName", x.IssueSplitName));
            cmd.Parameters.Add(new SqlParameter("@KeyCode", (object) x.KeyCode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", x.UpdatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@IssueSplitDescription", (object) x.IssueSplitDescription ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

    }
}
