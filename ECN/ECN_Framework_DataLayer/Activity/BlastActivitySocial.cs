using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class BlastActivitySocial
    {
        private const string InsertBlastActivitySocialStoredProcedure = "spInsertBlastActivitySocial";

        public static ECN_Framework_Entities.Activity.BlastActivitySocial GetByBlastSocialID(int socialID)
        {
            ECN_Framework_Entities.Activity.BlastActivitySocial retItem = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, sac.SocialActivityCode, sm.DisplayName " +
                               " from BlastActivitySocial bas WITH (NOLOCK) " +
                               " JOIN SocialActivityCodes sac WITH (NOLOCK) on bas.SocialActivityCodeID = sac.SocialActivityCodeID " +
                               " JOIN ecn5_communicator..SocialMedia sm WITH (NOLOCK) on bas.SocialMediaID = sm.SocialMediaID " +
                               " where bas.SocialID = @SocialID";
            cmd.Parameters.AddWithValue("@SocialID", socialID);

            DataTable dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                retItem = new ECN_Framework_Entities.Activity.BlastActivitySocial();
                retItem.SocialID = Convert.ToInt32(dt.Rows[0]["SocialID"].ToString());
                if (dt.Rows[0]["BlastID"] != DBNull.Value)
                {
                    retItem.BlastID = Convert.ToInt32(dt.Rows[0]["BlastID"].ToString());
                }
                if (dt.Rows[0]["EmailID"] != DBNull.Value)
                {
                    retItem.EmailID = Convert.ToInt32(dt.Rows[0]["EmailID"].ToString());
                }
                if (dt.Rows[0]["RefEmailID"] != DBNull.Value)
                {
                    retItem.RefEmailID = Convert.ToInt32(dt.Rows[0]["RefEmailID"].ToString());
                }
                if (dt.Rows[0]["SocialActivityCodeID"] != DBNull.Value)
                {
                    retItem.SocialActivityCodeID = Convert.ToInt32(dt.Rows[0]["SocialActivityCodeID"].ToString());
                }
                if (dt.Rows[0]["SocialActivityCode"] != DBNull.Value)
                {
                    retItem.SocialActivityCode = dt.Rows[0]["SocialActivityCode"].ToString();
                }
                if (dt.Rows[0]["ActionDate"] != DBNull.Value)
                {
                    retItem.ActionDate = Convert.ToDateTime(dt.Rows[0]["ActionDate"].ToString());
                }
                if (dt.Rows[0]["URL"] != DBNull.Value)
                {
                    retItem.URL = dt.Rows[0]["URL"].ToString();
                }
                if (dt.Rows[0]["SocialMediaID"] != DBNull.Value)
                {
                    retItem.SocialMediaID = Convert.ToInt32(dt.Rows[0]["SocialMediaID"].ToString());
                }
                if (dt.Rows[0]["SocialMedia"] != DBNull.Value)
                {
                    retItem.SocialMedia = dt.Rows[0]["SocialMedia"].ToString();
                }
            }

            return retItem;
        }

        public static int Insert(ECN_Framework_Entities.Activity.BlastActivitySocial social)
        {
            int socialID = 0;
            //need to validate objects first
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DataFunctions.ConnectionString.Activity.ToString()].ToString()))
            {
                connection.Open();
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {

                    var cmd = DataFunctions.BuildCommand
                    (
                        connection,
                        InsertBlastActivitySocialStoredProcedure,
                        new Dictionary<string, object>()
                        {
                            { "@BlastID", social.BlastID },
                            { "@EmailID", social.EmailID },
                            { "@RefEmailID", social.RefEmailID },
                            { "@SocialActivityCodeID", social.SocialActivityCodeID },
                            { "@URL", social.URL },
                            { "@SocialMediaID", social.SocialMediaID }
                        }
                    );

                    var exec = DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString());
                    socialID = Convert.ToInt32(exec.ToString());
                    scope.Complete();
                }
            }
            return socialID;
        }

        public static bool FBHasBeenSharedAlready(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastActivitySocial_FBHasBeenShared";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString()).ToString()) > 0 ? true : false;
        }
    }
}
