using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class BlastActivityOpens
    {
        public static ECN_Framework_Entities.Activity.BlastActivityOpens GetByOpenID(int openID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityOpens with (nolock) where OpenID = @OpenID";
            cmd.Parameters.AddWithValue("@OpenID", openID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityOpens with (nolock) where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByCampaignItemID(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityOpens> fullopenList = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetBlastID_fromCampaignItem";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                while (rdr.Read())
                {
                    fullopenList = MergeListCollections(fullopenList, GetByBlastID(Int32.Parse(rdr[0].ToString())));
                }
            }

            return fullopenList;
        }

        private static List<ECN_Framework_Entities.Activity.BlastActivityOpens> MergeListCollections(List<ECN_Framework_Entities.Activity.BlastActivityOpens> firstList, List<ECN_Framework_Entities.Activity.BlastActivityOpens> secondList)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityOpens> mergedList = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();
            mergedList.InsertRange(0, firstList);
            mergedList.InsertRange(mergedList.Count, secondList);
            return mergedList;
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityOpens with (nolock) where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityOpens with (nolock) where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivityOpens Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityOpens retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityOpens();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityOpens>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityOpens> retList = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityOpens retItem = new ECN_Framework_Entities.Activity.BlastActivityOpens();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityOpens>.CreateBuilder(rdr);
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
