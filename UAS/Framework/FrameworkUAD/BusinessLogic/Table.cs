using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class Table
    {
        public List<Object.Table> Select(string dbName)
        {
            List<Object.Table> x = null;
            x = DataAccess.Table.Select(dbName).ToList();
            return x;
        }

        public List<Object.Table> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.Table> x = null;
            x = DataAccess.Table.Select(client).ToList();
            return x;
        }

        public DataTable Select(KMPlatform.Object.ClientConnections client, string dbName, string table, string pubCode)
        {
            DataTable dt = null;
            dt = DataAccess.Table.Select(client, dbName, table, pubCode);

            return dt;
        } 
    
    }
}
