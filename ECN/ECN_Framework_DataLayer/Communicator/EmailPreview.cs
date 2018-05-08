using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class EmailPreview
    {
        public static ECN_Framework_Entities.Communicator.EmailPreview GetByEmailTestID(int emailTestID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailPreview_Select_EmailTestID";
            cmd.Parameters.AddWithValue("@EmailTestID", emailTestID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailPreview> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailPreview_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailPreview> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailPreview_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.EmailPreview Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.EmailPreview retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.EmailPreview();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailPreview>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.EmailPreview> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.EmailPreview> retList = new List<ECN_Framework_Entities.Communicator.EmailPreview>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.EmailPreview retItem = new ECN_Framework_Entities.Communicator.EmailPreview();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailPreview>.CreateBuilder(rdr);
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

        public static bool Insert(ECN_Framework_Entities.Communicator.EmailPreview x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailPreview_Insert";
            cmd.Parameters.AddWithValue("@EmailTestID", x.EmailTestID);
            cmd.Parameters.AddWithValue("@BlastID", x.BlastID);
            cmd.Parameters.AddWithValue("@CustomerID", x.CustomerID);
            cmd.Parameters.AddWithValue("@ZipFile", x.ZipFile);
            cmd.Parameters.AddWithValue("@CreatedByID", x.CreatedByID);
            cmd.Parameters.AddWithValue("@LinkTestID", x.LinkTestID);
            cmd.Parameters.AddWithValue("@BaseChannelGUID", x.BaseChannelGUID);

            return DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetUsage(int customerID, int month, int year)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailPreview_GetUsage";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetUsageByBaseChannelID(int baseChannelID, int month, int year)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailPreview_GetUsage_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetUsageDetails(int customerID, int month, int year)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailPreview_GetUsage_Details";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
