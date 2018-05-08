using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace DQM.Helpers.DataLoader
{
    class BulkInsertData
    {
        public bool BulkInsert(FileInfo fileInfo, DataTable myData, Dictionary<int, string> myColumns, string sqlConnection)
        {
            bool noErrors = true;



            return noErrors;
        }
        public static bool BulkInsertDataTable(string fileName, DataTable dataTable, string[] columns)
        {
            
            return false;
        }

        private void tableColmuns()
        {
            //select mapToColumn from staging_columns where columnName = this.columns

        }

    }
}
