using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class ImportDimension
    {
        public static List<Entity.ImportDimension> Select(int accountId, bool isMergedToUAD)
        {
            List<Entity.ImportDimension> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimension_Select_AccountId_IsMergedToUAD";
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@IsMergedToUAD", isMergedToUAD);

            retItem = GetImportDimensionList(cmd);

            //lets get all our Detail independently then merge together
            List<Entity.ImportDimensionDetail> iddList = SelectImportDimensionDetail(accountId, isMergedToUAD);
            if (iddList != null)
            {
                foreach (Entity.ImportDimension id in retItem)
                    id.Dimensions = iddList.Where(x => x.ImportDimensionId == id.ImportDimensionId).ToList();
            }

            return retItem;
        }
        public static List<Entity.ImportDimension> Select(int accountId, int publicationID, bool isMergedToUAD)
        {
            List<Entity.ImportDimension> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimension_Select_AccountId_PublicationID_IsMergedToUAD";
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@IsMergedToUAD", isMergedToUAD);

            retItem = GetImportDimensionList(cmd);

            //lets get all our Detail independently then merge together
            List<Entity.ImportDimensionDetail> iddList = SelectImportDimensionDetail(accountId, publicationID, isMergedToUAD);
            if (iddList != null)
            {
                foreach (Entity.ImportDimension id in retItem)
                    id.Dimensions = iddList.Where(x => x.ImportDimensionId == id.ImportDimensionId).ToList();
            }
            //foreach(Entity.ImportDimension id in retItem)
            //    id.Dimensions = Select(id.ImportDimensionId);//this would be tons of calls
            return retItem;
        }
        public static bool SaveBulkXml(string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimension_SaveBulkXml";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        private static Entity.ImportDimension GetImportDimension(SqlCommand cmd)
        {
            Entity.ImportDimension retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ImportDimension();
                        DynamicBuilder<Entity.ImportDimension> builder = DynamicBuilder<Entity.ImportDimension>.CreateBuilder(rdr);
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
        private static List<Entity.ImportDimension> GetImportDimensionList(SqlCommand cmd)
        {
            List<Entity.ImportDimension> retList = new List<Entity.ImportDimension>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ImportDimension retItem = new Entity.ImportDimension();
                        DynamicBuilder<Entity.ImportDimension> builder = DynamicBuilder<Entity.ImportDimension>.CreateBuilder(rdr);
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

        #region ImportDimensionDetail
        private static List<Entity.ImportDimensionDetail> SelectImportDimensionDetail(int importDimensionId)
        {
            List<Entity.ImportDimensionDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimensionDetail_ImportDimensionId";
            cmd.Parameters.AddWithValue("@ImportDimensionId", importDimensionId);

            retItem = GetImportDimensionDetailList(cmd);
            return retItem;
        }
        private static List<Entity.ImportDimensionDetail> SelectImportDimensionDetail(int accountId, int publicationID, bool isMergedToUAD)
        {
            List<Entity.ImportDimensionDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimensionDetail_AccountId_PublicationID_IsMergedToUAD";
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Parameters.AddWithValue("@IsMergedToUAD", isMergedToUAD);

            retItem = GetImportDimensionDetailList(cmd);
            return retItem;
        }
        private static List<Entity.ImportDimensionDetail> SelectImportDimensionDetail(int accountId, bool isMergedToUAD)
        {
            List<Entity.ImportDimensionDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportDimensionDetail_AccountId_IsMergedToUAD";
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@IsMergedToUAD", isMergedToUAD);

            retItem = GetImportDimensionDetailList(cmd);
            return retItem;
        }
        private static List<Entity.ImportDimensionDetail> GetImportDimensionDetailList(SqlCommand cmd)
        {
            List<Entity.ImportDimensionDetail> retList = new List<Entity.ImportDimensionDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ImportDimensionDetail retItem = new Entity.ImportDimensionDetail();
                        DynamicBuilder<Entity.ImportDimensionDetail> builder = DynamicBuilder<Entity.ImportDimensionDetail>.CreateBuilder(rdr);
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
