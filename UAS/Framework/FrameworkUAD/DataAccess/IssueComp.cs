using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueComp
    {
        private static Entity.IssueComp Get(SqlCommand cmd)
        {
            Entity.IssueComp retItem = null;
            try { 
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    retItem = new Entity.IssueComp();
                    DynamicBuilder<Entity.IssueComp> builder = DynamicBuilder<Entity.IssueComp>.CreateBuilder(rdr);
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
        private static List<Entity.IssueComp> GetList(SqlCommand cmd)
        {
            List<Entity.IssueComp> retList = new List<Entity.IssueComp>();
            try { 
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Entity.IssueComp retItem = new Entity.IssueComp();
                    DynamicBuilder<Entity.IssueComp> builder = DynamicBuilder<Entity.IssueComp>.CreateBuilder(rdr);
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

        public static List<Entity.IssueComp> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueComp> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueComp_Select_Issue";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static int Save(Entity.IssueComp x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueComp_Save";
            cmd.Parameters.Add(new SqlParameter("@IssueCompId", x.IssueCompId));
            cmd.Parameters.Add(new SqlParameter("@IssueId", x.IssueId));
            cmd.Parameters.Add(new SqlParameter("@ImportedDate", x.ImportedDate));
            cmd.Parameters.Add(new SqlParameter("@IssueCompCount", x.IssueCompCount));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool JobSaveComplimentary(KMPlatform.Object.ClientConnections client, string processCode, int publicationID, int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "job_IssueComp_IssueCompDetail_Add";
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", processCode));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileId", sourceFileID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
