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
    public class BlastActivityBounces
    {
        public static DataTable GetByDateRangeForCustomers(string startDate, string endDate, string customerIDs, string stringToFind)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_BlastActivityBounces_GetByDateRangeForCustomers";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@CustomerIDs", customerIDs);
            cmd.Parameters.AddWithValue("@StringToFind", stringToFind);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static ECN_Framework_Entities.Activity.BlastActivityBounces GetByBounceID(int bounceID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bab.*, bc.BounceCode from BlastActivityBounces bab with (nolock) join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID where BounceID = @BounceID";
            cmd.Parameters.AddWithValue("@BounceID", bounceID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bab.*, bc.BounceCode from BlastActivityBounces bab with (nolock) join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bab.*, bc.BounceCode from BlastActivityBounces bab with (nolock) join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bab.*, bc.BounceCode from BlastActivityBounces bab with (nolock) join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivityBounces Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityBounces retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityBounces();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityBounces>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityBounces> retList = new List<ECN_Framework_Entities.Activity.BlastActivityBounces>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityBounces retItem = new ECN_Framework_Entities.Activity.BlastActivityBounces();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityBounces>.CreateBuilder(rdr);
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

        public static DataTable BlastReportWithUDF(int blastID, string udfName, string udfData)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText =   "SELECT UPPER(bc.BounceCode) AS 'ACTIONVALUE', COUNT(DISTINCT babo.EmailID) AS 'UNIQUEBounces', COUNT(babo.EmailID) AS 'TOTALBounces' " +
	                            "FROM " + 
		                            "BlastActivityBounces babo with(nolock) " +
		                            "join BounceCodes bc with (nolock) on bc.BounceCodeID = babo.BounceCodeID " + 
		                            "join [fn_Blast_Report_Filter_By_UDF](@BlastID,@UDFName,@UDFData) t on babo.EmailID = t.emailID " +
	                            "WHERE " +
		                            "BlastID = @BlastID " +
	                            "GROUP BY " +
		                            "bc.BounceCode " +
	                            "ORDER BY " +
		                            "bc.BounceCode";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@UDFName", udfName);
            cmd.Parameters.AddWithValue("@UDFData", udfData);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
            return dt;
        }

        public static DataTable BlastReport(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT UPPER(bc.BounceCode) AS 'ACTIONVALUE', COUNT(DISTINCT babo.EmailID) AS 'UNIQUEBounces', COUNT(babo.EmailID) AS 'TOTALBounces' " +
                                "FROM " +
                                    "BlastActivityBounces babo with(nolock) " +
                                    "join BounceCodes bc with (nolock) on bc.BounceCodeID = babo.BounceCodeID " +
                                "WHERE " +
                                    "BlastID = @BlastID " +
                                "GROUP BY " +
                                    "bc.BounceCode " +
                                "ORDER BY " +
                                    "bc.BounceCode";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
            return dt;
        }

        public static DataTable BlastReportByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT UPPER(bc.BounceCode) AS 'ACTIONVALUE', COUNT(DISTINCT babo.EmailID) AS 'UNIQUEBounces', COUNT(babo.EmailID) AS 'TOTALBounces' " +
                                "FROM " +
                                    "BlastActivityBounces babo with(nolock) " +
                                    "join BounceCodes bc with (nolock) on bc.BounceCodeID = babo.BounceCodeID " +
                                    "join ecn5_communicator..CampaignItemBlast cib with (nolock) on babo.BlastID = cib.BlastID " +
                                "WHERE " +
                                    "cib.CampaignItemID = @CampaignItemID " +
                                "GROUP BY " +
                                    "bc.BounceCode " +
                                "ORDER BY " +
                                    "bc.BounceCode";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
            return dt;
        }
    }
}
