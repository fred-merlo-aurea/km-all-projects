using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastLinks
    {
        public static int GetMaxLinkColumnLength()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Convert(int,CHARACTER_MAXIMUM_LENGTH) from information_schema.columns where table_name = 'BlastLinks' and DATA_TYPE = 'varchar' and COLUMN_NAME = 'LinkURL'";
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
