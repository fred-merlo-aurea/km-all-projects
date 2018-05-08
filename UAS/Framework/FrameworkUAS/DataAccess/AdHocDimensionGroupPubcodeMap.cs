using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Core_AMS.Utilities;
using FrameworkUAS.DataAccess.Helpers;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class AdHocDimensionGroupPubcodeMap
    {
        private const string TableAdhocDimensionGroupPubcodeMap = "AdHocDimensionGroupPubcodeMap";
        private const string ColumnAdhocDimensionGroupId = "AdHocDimensionGroupId";
        private const string ColumnPubCode = "Pubcode";
        private const string ColumnIsActive = "IsActive";
        private const string ColumnDateCreated = "DateCreated";
        private const string ColumnDateUpdated = "DateUpdated";
        private const string ColumnCreatedByUserId = "CreatedByUserID";
        private const string ColumnUpdatedByUserId = "UpdatedByUserID";

        public static List<Entity.AdHocDimensionGroupPubcodeMap> Select(int adHocDimensionGroupId)
        {
            List<Entity.AdHocDimensionGroupPubcodeMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroupPubcodeMap_Select_AdHocDimensionGroupId";
            cmd.Parameters.AddWithValue("@AdHocDimensionGroupId", adHocDimensionGroupId);

            retItem = GetList(cmd);
            return retItem;
        }
        private static List<Entity.AdHocDimensionGroupPubcodeMap> GetList(SqlCommand cmd)
        {
            List<Entity.AdHocDimensionGroupPubcodeMap> retList = new List<Entity.AdHocDimensionGroupPubcodeMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.AdHocDimensionGroupPubcodeMap retItem = new Entity.AdHocDimensionGroupPubcodeMap();
                        DynamicBuilder<Entity.AdHocDimensionGroupPubcodeMap> builder = DynamicBuilder<Entity.AdHocDimensionGroupPubcodeMap>.CreateBuilder(rdr);
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
        public static bool Save(Entity.AdHocDimensionGroupPubcodeMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroupPubcodeMap_Save";
            cmd.Parameters.Add(new SqlParameter("@AdHocDimensionGroupId", x.AdHocDimensionGroupId));
            cmd.Parameters.Add(new SqlParameter("@Pubcode", x.Pubcode));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool SaveBulkSqlInsert(List<Entity.AdHocDimensionGroupPubcodeMap> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var dataTable = BulkDataReader<Entity.AdHocDimensionGroupPubcodeMap>.ToDataTable(list);
            var mappings = new Dictionary<string, string>
            {
                [ColumnAdhocDimensionGroupId] = ColumnAdhocDimensionGroupId,
                [ColumnPubCode] = ColumnPubCode,
                [ColumnIsActive] = ColumnIsActive,
                [ColumnDateCreated] = ColumnDateCreated,
                [ColumnDateUpdated] = ColumnDateUpdated,
                [ColumnCreatedByUserId] = ColumnCreatedByUserId,
                [ColumnUpdatedByUserId] = ColumnUpdatedByUserId
            };

            ClientMethodsHelper.BulkCopy(dataTable, TableAdhocDimensionGroupPubcodeMap, mappings, 0);
            return true;
        }
    }
}
