using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class BlastActivityConversion
    {
        public static ECN_Framework_Entities.Activity.BlastActivityConversion GetByConversionID(int conversionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityConversion with (nolock) where ConversionID = @ConversionID";
            cmd.Parameters.AddWithValue("@ConversionID", conversionID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityConversion> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityConversion with (nolock) where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityConversion> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityConversion with (nolock) where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityConversion> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityConversion with (nolock) where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivityConversion Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityConversion retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityConversion();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityConversion>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Activity.BlastActivityConversion> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityConversion> retList = new List<ECN_Framework_Entities.Activity.BlastActivityConversion>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityConversion retItem = new ECN_Framework_Entities.Activity.BlastActivityConversion();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityConversion>.CreateBuilder(rdr);
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
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static DataTable GetRevenueData(int customerID, int blastID, string type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivityConversion_GetRevenueData";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@Type", type);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static int GetCount(int blastID, int customerID, string url, int length, bool distinct)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivityConversion_GetCount";
            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@URL", url));
            cmd.Parameters.Add(new SqlParameter("@Length", length));
            cmd.Parameters.Add(new SqlParameter("@Distinct", distinct));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()).ToString());
        }
    }
}
