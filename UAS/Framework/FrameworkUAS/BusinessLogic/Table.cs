using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class Table
    {
        public List<Object.Table> Select()
        {
            List<Object.Table> x = null;
            x = DataAccess.Table.Select().ToList();

            return x;
        }      
          
        public DataTable Select(string table, int client, int file)
        {
            DataTable dt = null;
            dt = DataAccess.Table.Select(table, client, file);

            return dt;
        }    
    }
}
