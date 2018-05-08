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
    public class BlastActivityClicks
    {
        public static ECN_Framework_Entities.Activity.BlastActivityClicks GetByClickID(int clickID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityClicks with (nolock) where ClickID = @ClickID";
            cmd.Parameters.AddWithValue("@ClickID", clickID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityClicks with (nolock) where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityClicks with (nolock) where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityClicks with (nolock) where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastLinkID(int blastLinkID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityClicks with (nolock) where BlastLinkID = @BlastLinkID";
            cmd.Parameters.AddWithValue("@BlastLinkID", blastLinkID);
            return GetList(cmd);
        }

        public static int GetUniqueByURL(string url, int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivityClicks_GetUniqueByURL";
            cmd.Parameters.AddWithValue("@URL", url);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()).ToString());
        }


        private static ECN_Framework_Entities.Activity.BlastActivityClicks Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityClicks retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityClicks();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityClicks>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityClicks> retList = new List<ECN_Framework_Entities.Activity.BlastActivityClicks>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityClicks retItem = new ECN_Framework_Entities.Activity.BlastActivityClicks();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityClicks>.CreateBuilder(rdr);
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

        public static DataTable GetByBlastID(int customerID, int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_BlastActivityClicks_GetByBlastID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int CountByBlastID(int blastID)
        {
            int count = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT COUNT(DISTINCT(EmailID)) FROM BlastActivityClicks WHERE BlastID=@BlastID";
                cmd.Parameters.AddWithValue("@BlastID", blastID);
                count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()));
            }
            catch (Exception)
            {
            }

            return count;
        }
    }
}
