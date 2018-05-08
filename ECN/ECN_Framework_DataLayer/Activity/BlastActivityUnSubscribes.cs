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
    public class BlastActivityUnSubscribes
    {
        public static DataTable GetByDateRangeForCustomers(string startDate, string endDate, string customerIDs, string unsubscribeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_BlastActivityUnsubscribes_GetByDateRangeForCustomers";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@CustomerIDs", customerIDs);
            if(unsubscribeCode != string.Empty)
                cmd.Parameters.AddWithValue("@UnsubscribeCode", unsubscribeCode);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static ECN_Framework_Entities.Activity.BlastActivityUnSubscribes GetByUnsubscribeID(int unsubscribeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bau.*, uc.UnsubscribeCode from BlastActivityUnSubscribes bau with (nolock) join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where UnsubscribeID = @UnsubscribeID";
            cmd.Parameters.AddWithValue("@UnsubscribeID", unsubscribeID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bau.*, uc.UnsubscribeCode from BlastActivityUnSubscribes bau with (nolock) join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bau.*, uc.UnsubscribeCode from BlastActivityUnSubscribes bau with (nolock) join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bau.*, uc.UnsubscribeCode from BlastActivityUnSubscribes bau with (nolock) join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivityUnSubscribes Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityUnSubscribes retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityUnSubscribes();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> retList = new List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityUnSubscribes retItem = new ECN_Framework_Entities.Activity.BlastActivityUnSubscribes();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>.CreateBuilder(rdr);
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
    }
}
