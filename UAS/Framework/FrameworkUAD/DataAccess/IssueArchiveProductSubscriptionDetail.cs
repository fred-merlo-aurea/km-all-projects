using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueArchiveProductSubscriptionDetail
    {
        private static Entity.IssueArchiveProductSubscriptionDetail Get(SqlCommand cmd)
        {
            Entity.IssueArchiveProductSubscriptionDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueArchiveProductSubscriptionDetail();
                        DynamicBuilder<Entity.IssueArchiveProductSubscriptionDetail> builder = DynamicBuilder<Entity.IssueArchiveProductSubscriptionDetail>.CreateBuilder(rdr);
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
        private static List<Entity.IssueArchiveProductSubscriptionDetail> GetList(SqlCommand cmd)
        {
            List<Entity.IssueArchiveProductSubscriptionDetail> retList = new List<Entity.IssueArchiveProductSubscriptionDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueArchiveProductSubscriptionDetail retItem = new Entity.IssueArchiveProductSubscriptionDetail();
                        DynamicBuilder<Entity.IssueArchiveProductSubscriptionDetail> builder = DynamicBuilder<Entity.IssueArchiveProductSubscriptionDetail>.CreateBuilder(rdr);
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

        public static bool Save(Entity.IssueArchiveProductSubscriptionDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscriptionDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@IAProductSubscriptionDetailID", x.IAProductSubscriptionDetailID));
            cmd.Parameters.Add(new SqlParameter("@IssueArchiveSubscriptionId", x.IssueArchiveSubscriptionId));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", x.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@ResponseID", x.CodeSheetID));
            //cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool SaveBulkSqlInsert(List<Entity.IssueArchiveProductSubscriptionDetail> list, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.IssueArchiveProductSubscriptionDetail>.ToDataTable(list);
            bool done = true;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            //SqlConnection conn = DataFunctions.GetSqlConnection(client);

            try
            {
                cmd.Connection.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(cmd.Connection, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", cmd.Connection.Database.ToString(), "IssueArchiveProductSubscriptionDetail");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("IAProductSubscriptionDetailID", "IAProductSubscriptionDetailID");
                bc.ColumnMappings.Add("IssueArchiveSubscriptionId", "IssueArchiveSubscriptionId");
                bc.ColumnMappings.Add("PubSubscriptionID", "PubSubscriptionID");
                bc.ColumnMappings.Add("SubscriptionID", "SubscriptionID");
                bc.ColumnMappings.Add("CodeSheetID", "CodeSheetID");
                bc.ColumnMappings.Add("ResponseOther", "ResponseOther");
                bc.ColumnMappings.Add("IssueID", "IssueID");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch
            {
                done = false;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }

            return done;
        }

        public static List<Entity.IssueArchiveProductSubscriptionDetail> SelectForUpdate(int productID, int issueid, string pubsubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscriptionDetail_SelectForUpdate";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@IssueID", issueid);
            cmd.Parameters.AddWithValue("@PubSubs", pubsubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        public static List<Entity.IssueArchiveProductSubscriptionDetail> IssueArchiveProductSubscriptionDetailUpdateBulkSql(KMPlatform.Object.ClientConnections client, List<Entity.IssueArchiveProductSubscriptionDetail> list)
        {
            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<XML>");
            foreach (Entity.IssueArchiveProductSubscriptionDetail x in list)
            {
                sbXML.AppendLine("<IssueArchiveProductSubscriptionDetail>");

                sbXML.AppendLine("<IssueArchiveSubscriptionId>" + x.IssueArchiveSubscriptionId.ToString() + "</IssueArchiveSubscriptionId>");
                sbXML.AppendLine("<PubSubscriptionID>" + x.PubSubscriptionID.ToString() + "</PubSubscriptionID>");
                sbXML.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                sbXML.AppendLine("<CodeSheetID>" + x.CodeSheetID.ToString() + "</CodeSheetID>");
                sbXML.AppendLine("<DateCreated>" + x.DateCreated.ToString() + "</DateCreated>");
                sbXML.AppendLine("<DateUpdated>" + x.DateUpdated.ToString() + "</DateUpdated>");
                sbXML.AppendLine("<CreatedByUserID>" + x.CreatedByUserID.ToString() + "</CreatedByUserID>");
                sbXML.AppendLine("<UpdatedByUserID>" + x.UpdatedByUserID.ToString() + "</UpdatedByUserID>");
                if (x.ResponseOther != null)
                    sbXML.AppendLine("<ResponseOther>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(x.ResponseOther.ToString()) + "</ResponseOther>");
                else
                    sbXML.AppendLine("<ResponseOther></ResponseOther>");

                sbXML.AppendLine("</IssueArchiveProductSubscriptionDetail>");
            }
            sbXML.AppendLine("</XML>");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscriptionDetail_BulkUpdate";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@xml", sbXML.ToString()));
            //cmd.Connection = conn;

            //int rowCount = Convert.ToInt32(DataFunctions.ExecuteNonQuery(cmd));
            //if (rowCount > 0)
            //    done = true;
            //else
            //    done = false;

            return GetList(cmd);
        }
    }
}
