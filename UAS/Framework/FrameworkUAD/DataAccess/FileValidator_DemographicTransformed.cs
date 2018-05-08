using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class FileValidator_DemographicTransformed
    {
        public static bool SaveBulkSqlInsert(List<Entity.FileValidator_Transformed> list, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FileValidator_DemographicTransformed> insertList = new List<Entity.FileValidator_DemographicTransformed>();
            foreach (Entity.FileValidator_Transformed so in list)
            {
                foreach (Entity.FileValidator_DemographicTransformed sdo in so.FV_DemographicTransformedList)
                {
                    insertList.Add(sdo);
                }
            }

            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.FileValidator_DemographicTransformed>.ToDataTable(insertList);

            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            conn.Open();

            try
            {
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "FileValidator_DemographicTransformed");
                bc.BatchSize = 0;  // Sunil - Changed 1000 to 0 (process all in one batch)
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("FV_DemographicTransformedID", "FV_DemographicTransformedID");
                bc.ColumnMappings.Add("PubID", "PubID");
                bc.ColumnMappings.Add("STRecordIdentifier", "STRecordIdentifier");
                bc.ColumnMappings.Add("MAFField", "MAFField");
                bc.ColumnMappings.Add("Value", "Value");
                bc.ColumnMappings.Add("NotExists", "NotExists");
                bc.ColumnMappings.Add("NotExistReason", "NotExistReason");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch { done = false; }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
    }
}
