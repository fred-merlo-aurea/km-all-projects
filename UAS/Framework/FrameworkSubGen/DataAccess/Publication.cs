using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Publication
    {
        private const string ClassName = "FrameworkSubGen.DataAccess.Publication";
        private const string CommandTextSaveBulkXml = "e_Publication_SaveBulkXml";

        public static List<Entity.Publication> Select()
        {
            List<Entity.Publication> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.Publication Select(string publicationName, int accountId)
        {
            Entity.Publication retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select_Name";
            cmd.Parameters.AddWithValue("@name", publicationName);
            cmd.Parameters.AddWithValue("@accountId", accountId);
            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Publication SelectKmPubId(int kmPubId, int kmClientId)
        {
            Entity.Publication retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select_KMPubId_KMClientId";
            cmd.Parameters.AddWithValue("@kmPubId", kmPubId);
            cmd.Parameters.AddWithValue("@kmClientId", kmClientId);
            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Publication SelectKmPubCode(string kmPubCode, int kmClientId)
        {
            Entity.Publication retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select_KMPubCode_KMClientId";
            cmd.Parameters.AddWithValue("@kmPubCode", kmPubCode);
            cmd.Parameters.AddWithValue("@kmClientId", kmClientId);
            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.Publication Get(SqlCommand cmd)
        {
            Entity.Publication retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Publication();
                        DynamicBuilder<Entity.Publication> builder = DynamicBuilder<Entity.Publication>.CreateBuilder(rdr);
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
        private static List<Entity.Publication> GetList(SqlCommand cmd)
        {
            List<Entity.Publication> retList = new List<Entity.Publication>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Publication retItem = new Entity.Publication();
                        DynamicBuilder<Entity.Publication> builder = DynamicBuilder<Entity.Publication>.CreateBuilder(rdr);
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
        public static bool SaveBulkXml(string xml)
        {
            return DataAccessBase.SaveBulkXml(xml, CommandTextSaveBulkXml, ClassName);
        }
    }
}
