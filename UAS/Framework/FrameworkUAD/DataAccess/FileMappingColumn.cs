using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class FileMappingColumn
    {
        public static List<Object.FileMappingColumn> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileMappingColumn> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_FileMappingColumns";
            retItem = DataFunctions.GetList<Object.FileMappingColumn>(cmd, client);

            return retItem;
        }
       
        public static List<Object.FileMappingColumn> GetMappingColumns(int clientId, KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileMappingColumn> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_FileMappingColumns_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            retItem = DataFunctions.GetList<Object.FileMappingColumn>(cmd, client);

            return retItem;
        }
        public static List<Object.FileMappingColumn> GetMappingColumns(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileMappingColumn> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_FileMappingColumns_ClientId_PubId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@pubId", pubId);
            retItem = DataFunctions.GetList<Object.FileMappingColumn>(cmd, client);

            return retItem;
        }

        public static List<Object.FileMappingColumnValue> GetMappingValues(int clientId, KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileMappingColumnValue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_FileMappingColumnValues_ClientId";
            retItem = DataFunctions.GetList<Object.FileMappingColumnValue>(cmd, client);

            return retItem;
        }
        public static List<Object.FileMappingColumnValue> GetMappingValues(int clientId, int pubId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Get_FileMappingColumnValues_ClientId_PubId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@pubId", pubId);
            return DataFunctions.GetList<Object.FileMappingColumnValue>(cmd, client);
        }
    }
}
