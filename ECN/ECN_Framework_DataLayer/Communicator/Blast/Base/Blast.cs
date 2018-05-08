using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;
using CommCampaignItemBlastFilter = ECN_Framework_Entities.Communicator.CampaignItemBlastFilter;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Blast
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Blast;

        private static string _CacheRegion = "Blast";
        private const string ProcedureBlastExistsByBlastId = "e_Blast_Exists_ByBlastID";
        private const string ProcedureBlastActiveOrSent = "e_Blast_ActiveOrSent";

        private const string CommaSeparator = ",";
        private const string CloseTag = "\">";
        private const string OpenTag = "</";
        private const string FilterIdsTag = "<FilterIDs>";
        private const string FilterIdsCloseTag = "</FilterIDs>";
        private const string SsIdTag = "<ssID id=\"";
        private const string RefBlastIdOpenTag = "<refBlastIDs>";
        private const string RefBlastIdCloseTag = "</refBlastIDs></ssID>";
        private const string SuppFiltersCloseTag = "</SuppFilters>";
        private const string SuppresionGroupTag = "<SuppressionGroup id=\"";
        private const string SuppressionGroupCloseTag = "</SuppressionGroup>";

        public static bool CreatedUserExists(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Blast_IsCreater";
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataTable dtResult = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            string result = dtResult.DefaultView[0][0].ToString();
            return result.ToUpper() == "TRUE";
        }

        #region GET
        private static ECN_Framework_Entities.Communicator.BlastAbstract GetInstance(ECN_Framework_Common.Objects.Communicator.Enums.BlastType BlastType)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract b = null;

            switch (BlastType)
            {
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.HTML: //regular blast
                    b = new ECN_Framework_Entities.Communicator.BlastRegular();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.TEXT: //regular blast
                    b = new ECN_Framework_Entities.Communicator.BlastRegular();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Sample: //ab blast
                    b = new ECN_Framework_Entities.Communicator.BlastAB();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion: //champion blast
                    b = new ECN_Framework_Entities.Communicator.BlastChampion();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.SMS: //sms blast
                    b = new ECN_Framework_Entities.Communicator.BlastSMS();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Social: //social blast
                    b = new ECN_Framework_Entities.Communicator.BlastSocial();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Layout: //layout blast
                    b = new ECN_Framework_Entities.Communicator.BlastLayout();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.NoOpen: //layout blast
                    b = new ECN_Framework_Entities.Communicator.BlastNoOpen();
                    break;
                case ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization: //layout blast
                    b = new ECN_Framework_Entities.Communicator.BlastPersonalization();
                    break;
            }
            return b;
        }

        private static ECN_Framework_Entities.Communicator.BlastAbstract GetBlast(DataRow row)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract b = null;

            b = GetInstance(GetBlastType(row["BlastType"].ToString()));
            b.BlastType = row["BlastType"].ToString();

            b.BlastID = Convert.ToInt32(row["BlastID"].ToString());
            if (!DBNull.Value.Equals(row["CustomerID"]))
                b.CustomerID = Convert.ToInt32(row["CustomerID"].ToString());
            b.EmailSubject = row["EmailSubject"].ToString();
            b.EmailFrom = row["EmailFrom"].ToString();
            b.EmailFromName = row["EmailFromName"].ToString();
            if (!DBNull.Value.Equals(row["SendTime"]))
                b.SendTime = Convert.ToDateTime(row["SendTime"].ToString());
            if (!DBNull.Value.Equals(row["AttemptTotal"]))
                b.AttemptTotal = Convert.ToInt32(row["AttemptTotal"].ToString());
            if (!DBNull.Value.Equals(row["SendTotal"]))
                b.SendTotal = Convert.ToInt32(row["SendTotal"].ToString());
            if (!DBNull.Value.Equals(row["SendBytes"]))
                b.SendBytes = Convert.ToInt32(row["SendBytes"].ToString());
            b.StatusCode = row["StatusCode"].ToString();
            //b.StatusCodeID = GetBlastStatusCode(b.StatusCode);
            if (!DBNull.Value.Equals(row["CodeID"]))
                b.CodeID = Convert.ToInt32(row["CodeID"].ToString());
            if (!DBNull.Value.Equals(row["LayoutID"]))
                b.LayoutID = Convert.ToInt32(row["LayoutID"].ToString());
            if (!DBNull.Value.Equals(row["GroupID"]))
                b.GroupID = Convert.ToInt32(row["GroupID"].ToString());
            if (!DBNull.Value.Equals(row["FinishTime"]))
                b.FinishTime = Convert.ToDateTime(row["FinishTime"].ToString());
            if (!DBNull.Value.Equals(row["SuccessTotal"]))
                b.SuccessTotal = Convert.ToInt32(row["SuccessTotal"].ToString());
            b.BlastLog = row["BlastLog"].ToString();
            if (!DBNull.Value.Equals(row["CreatedUserID"]))
                b.CreatedUserID = Convert.ToInt32(row["CreatedUserID"].ToString());
            if (!DBNull.Value.Equals(row["CreatedDate"]))
                b.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
            if (!DBNull.Value.Equals(row["UpdatedUserID"]))
                b.UpdatedUserID = Convert.ToInt32(row["UpdatedUserID"].ToString());
            if (!DBNull.Value.Equals(row["UpdatedDate"]))
                b.UpdatedDate = Convert.ToDateTime(row["UpdatedDate"].ToString());
            b.Spinlock = row["Spinlock"].ToString();
            b.ReplyTo = row["ReplyTo"].ToString();
            b.TestBlast = row["TestBlast"].ToString();
            b.BlastFrequency = row["BlastFrequency"].ToString();
            b.RefBlastID = row["RefBlastID"].ToString();
            b.BlastSuppression = row["BlastSuppression"].ToString();
            if (!DBNull.Value.Equals(row["AddOptOuts_to_MS"]))
                b.AddOptOuts_to_MS = Convert.ToBoolean(row["AddOptOuts_to_MS"].ToString());
            b.DynamicFromName = row["DynamicFromName"].ToString();
            b.DynamicFromEmail = row["DynamicFromEmail"].ToString();
            b.DynamicReplyToEmail = row["DynamicReplyToEmail"].ToString();
            if (!DBNull.Value.Equals(row["BlastEngineID"]))
                b.BlastEngineID = Convert.ToInt32(row["BlastEngineID"].ToString());
            if (!DBNull.Value.Equals(row["HasEmailPreview"]))
                b.HasEmailPreview = Convert.ToBoolean(row["HasEmailPreview"].ToString());
            if (!DBNull.Value.Equals(row["BlastScheduleID"]))
                b.BlastScheduleID = Convert.ToInt32(row["BlastScheduleID"].ToString());
            if (!DBNull.Value.Equals(row["OverrideAmount"]))
                b.OverrideAmount = Convert.ToInt32(row["OverrideAmount"].ToString());
            if (!DBNull.Value.Equals(row["OverrideIsAmount"]))
                b.OverrideIsAmount = Convert.ToBoolean(row["OverrideIsAmount"].ToString());
            if (!DBNull.Value.Equals(row["StartTime"]))
                b.StartTime = Convert.ToDateTime(row["StartTime"].ToString());
            if (!DBNull.Value.Equals(row["SMSOptInTotal"]))
                b.SMSOptInTotal = Convert.ToInt32(row["SMSOptInTotal"].ToString());
            b.NodeID = row["NodeID"].ToString();
            if (!DBNull.Value.Equals(row["SampleID"]))
                b.SampleID = Convert.ToInt32(row["SampleID"].ToString());
            if (!DBNull.Value.Equals(row["IgnoreSuppression"]))
            {
                b.IgnoreSuppression = Convert.ToBoolean(row["IgnoreSuppression"].ToString());
            }  
            return b;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetMasterRefBlast(int blastID, int EmailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Select_MasterRefBlast";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);

            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return GetBlast(dt.Rows[0]);
            }
            else
                return null;

        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_BlastID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            ECN_Framework_Entities.Communicator.BlastAbstract retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.BlastAbstract)KM.Common.CacheUtil.GetFromCache(blastID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    DataTable dt = null;
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                        scope.Complete();
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        retItem = GetBlast(dt.Rows[0]);
                        if (retItem != null && retItem.LayoutID.HasValue && retItem.LayoutID.Value > 0)//Adding layout ID check for champion caching issue - JWelter 10/19/2016
                            KM.Common.CacheUtil.AddToCache(blastID.ToString(), retItem, _CacheRegion);
                    }
                }
            }
            else
            {
                DataTable dt = null;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                    scope.Complete();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    retItem = GetBlast(dt.Rows[0]);
                }
            }
            return retItem;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetTopOneByLayoutID(int layoutID)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract retItem = null;

            SqlCommand cmd = new SqlCommand("e_Blast_Select_Top1ByLayoutID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);

            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                retItem = GetBlast(dt.Rows[0]);
            }

            return retItem;
        }

        private static DataTable GetList(SqlCommand cmd)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetByCampaignID(int campaignID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_CampaignID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return GetList(cmd);
        }

        public static DataTable GetBySearch(int customerID, string emailSubject, int? userID, int? groupID, bool? isTest, string statusCode, DateTime? modifiedFrom, DateTime? modifiedTo, int? campaignID, string campaignName, string campaignItemName)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_Search");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            if (emailSubject.Trim().Length > 0)
                cmd.Parameters.AddWithValue("@EmailSubject", emailSubject);
            if (statusCode.Trim().Length > 0)
                cmd.Parameters.AddWithValue("@StatusCode", statusCode);
            cmd.Parameters.Add(new SqlParameter("@UserID", (object)userID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)groupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsTest", (object)isTest ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ModifiedFrom", (object)modifiedFrom ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ModifiedTo", (object)modifiedTo ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", (object)campaignID ?? DBNull.Value));
            if(campaignName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@CampaignName", campaignName));
            if (campaignItemName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@CampaignItemName", campaignItemName));

            return GetList(cmd);
        }

        public static DataTable GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_CampaignItemID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return GetList(cmd);
        }

        public static DataTable GetByCampaignItemBlastID(int campaignItemBlastID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_CampaignItemBlastID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            return GetList(cmd);
        }

        public static DataTable GetByCampaignItemTestBlastID(int campaignItemTestBlastID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_CampaignItemTestBlastID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", campaignItemTestBlastID);
            return GetList(cmd);
        }

        public static bool ResendTestBlast(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Resend";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString())) > 0 ? true : false;
        }

        public static DataTable GetBySampleID(int sampleID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_SampleID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            return GetList(cmd);
        }

        public static DataTable GetByGroupID(int GroupID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_GroupID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            return GetList(cmd);
        }

        public static DataTable GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand("e_Blast_Select_CustomerID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }



        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastEngineIDFinishTime(int BlastEngineID, DateTime FinishTime)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract retItem = null;

            SqlCommand cmd = new SqlCommand("e_Blast_Select_BlastEngineIDFinishTime");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastEngineID", BlastEngineID);
            cmd.Parameters.AddWithValue("@FinishTime", FinishTime);

            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                retItem = GetBlast(dt.Rows[0]);
            }

            return retItem;
        }
        #endregion

        #region EXISTS
        public static bool Exists(int blastID, int customerID)
        {
            var readAndFillParams = new FillCommunicatorArgs {BlastId = blastID, CustomerId = customerID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureBlastExistsByBlastId) > 0;
        }
        #endregion

        #region EXISTS BY STATUS
        public static bool ActiveOrSent(int blastID, int customerID)
        {
            var readAndFillParams = new FillCommunicatorArgs {BlastId = blastID, CustomerId = customerID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureBlastActiveOrSent) > 0;
        }

        

        public static bool ActivePendingOrSentByBlast(int blastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_ActivePendingOrSent_BlastID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ActivePendingOrSentByLayout(int layoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_ActivePendingOrSent_LayoutID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ActivePendingOrSentByGroup(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_ActivePendingOrSent_GroupID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ActivePendingOrSentByFilter(int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_ActivePendingOrSent_FilterID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ActivePendingOrSentBySample(int sampleID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_ActivePendingOrSent_SampleID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        #endregion

        public static int Save(ECN_Framework_Entities.Communicator.BlastAbstract blast)
        {
            if (blast.BlastID > 0)
                DeleteCache(blast.BlastID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Save";
            cmd.Parameters.Add(new SqlParameter("@BlastID", blast.BlastID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blast.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailSubject", blast.EmailSubject));
            cmd.Parameters.Add(new SqlParameter("@EmailFrom", blast.EmailFrom));
            cmd.Parameters.Add(new SqlParameter("@EmailFromName", blast.EmailFromName));
            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blast.SendTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AttemptTotal", (object)blast.AttemptTotal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SendTotal", (object)blast.SendTotal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SendBytes", (object)blast.SendBytes ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StatusCode", blast.StatusCode));
            cmd.Parameters.Add(new SqlParameter("@BlastType", blast.BlastType));
            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)blast.CodeID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)blast.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blast.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FinishTime", (object)blast.FinishTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SuccessTotal", (object)blast.SuccessTotal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastLog", blast.BlastLog));
            if (blast.BlastID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Spinlock", blast.Spinlock));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", blast.ReplyTo));
            cmd.Parameters.Add(new SqlParameter("@TestBlast", blast.TestBlast));
            cmd.Parameters.Add(new SqlParameter("@BlastFrequency", blast.BlastFrequency));
            cmd.Parameters.Add(new SqlParameter("@RefBlastID", blast.RefBlastID));
            cmd.Parameters.Add(new SqlParameter("@BlastSuppression", blast.BlastSuppression));
            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)blast.AddOptOuts_to_MS ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", blast.DynamicFromName));
            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", blast.DynamicFromEmail));
            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", blast.DynamicReplyToEmail));
            cmd.Parameters.Add(new SqlParameter("@BlastEngineID", (object)blast.BlastEngineID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", (object)blast.HasEmailPreview ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)blast.BlastScheduleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)blast.OverrideAmount ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)blast.OverrideIsAmount ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)blast.StartTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SMSOptInTotal", (object)blast.SMSOptInTotal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@NodeID", blast.NodeID));
            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)blast.SampleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EnableCacheBuster", (object)blast.EnableCacheBuster ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IgnoreSuppression", (object)blast.IgnoreSuppression ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static void DeleteCache(int blastID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                if (KM.Common.CacheUtil.GetFromCache(blastID.ToString(), _CacheRegion) != null)
                    KM.Common.CacheUtil.RemoveFromCache(blastID.ToString(), _CacheRegion);
            }
        }

        public static void SetHasEmailPreview(int blastID, bool hasEmailPreview)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Set_HasEmailPreview";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@hasEmailPreview", hasEmailPreview);

            ECN_Framework_DataLayer.DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static bool RefBlastsExists(string blastIDs, int customerID, DateTime sendTime)
        {
            char[] delimiter = { ',' };
            string[] strBlasts = blastIDs.Split(delimiter);
            int blastCount = 0;
            for (int j = 0; j < strBlasts.Length; j++)
            {
                try
                {
                    blastCount++;
                }
                catch
                {
                }
            }
            if (blastCount > 0)
            {
                Int32 countActual = Convert.ToInt32(DataFunctions.ExecuteScalar("SELECT COUNT(BlastID) FROM Blast with (nolock) WHERE BlastID in (" + blastIDs + ") AND StatusCode <> 'Deleted' AND CustomerID = " + customerID + " AND SendTime < '" + sendTime + "'", DataFunctions.ConnectionString.Communicator.ToString()));
                if (countActual == blastCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool DynamicCotentExists(int blastID)
        {
            int exists = Convert.ToInt32(DataFunctions.ExecuteScalar("if exists (select top 1 blastID  from BlastDynamicContents where blastID = " + blastID + ")  select 1 else select 0", DataFunctions.ConnectionString.Communicator.ToString()));
            if (exists == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static ECN_Framework_Common.Objects.Communicator.Enums.BlastType GetBlastType(string type)
        {
            ECN_Framework_Common.Objects.Communicator.Enums.BlastType returnType;
            switch (type.Trim().ToLower())
            {
                case "html":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.HTML;
                    break;
                case "text": //regular blast
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.TEXT;
                    break;
                case "sample":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Sample;
                    break;
                case "champion":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion;
                    break;
                case "sms":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.SMS;
                    break;
                case "social":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Social;
                    break;
                case "layout":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Layout;
                    break;
                case "noopen":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.NoOpen;
                    break;
                case "personalization": //Personalized content for each email address
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization;
                    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Unknown;
                    break;
            }
            return returnType;
        }

        private static ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode GetBlastStatusCode(string code)
        {
            ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode returnType;
            switch (code.Trim().ToLower())
            {
                case "active":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active;
                    break;
                //case "cancelled":
                //    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Cancelled;
                //    break;
                case "deleted":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Deleted;
                    break;
                //case "deployed":
                //    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed;
                //    break;
                case "pending":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending;
                    break;
                case "sent":
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Sent;
                    break;
                //case "system":
                //    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.System;
                //    break;
                default:
                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Unknown;
                    break;
            }
            return returnType;
        }

        //public static System.Collections.Generic.List<string> ValidateBlastContent(ECN_Framework_Entities.Communicator.BlastAbstract blast)
        //{
        //    System.Collections.Generic.List<string> listLY = new System.Collections.Generic.List<string>();
        //    StringBuilder sbLY = new StringBuilder();
        //    sbLY.Append(" select ContentSource as Content from Layout ");
        //    sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9  ");
        //    sbLY.Append(" where layout.layoutID in (" + blast.LayoutID.Value + ") union all ");
        //    sbLY.Append(" select ContentText as Content from Layout ");
        //    sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 ");
        //    sbLY.Append(" where layout.layoutID in (" + blast.LayoutID.Value + ") union all ");
        //    sbLY.Append(" select ContentMobile as Content from Layout ");
        //    sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
        //    sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 ");
        //    sbLY.Append(" where layout.layoutID in (" + blast.LayoutID.Value + ")");

        //    DataTable dtLY = new DataTable();
        //    dtLY = DataFunctions.GetDataTable(sbLY.ToString(), DataFunctions.ConnectionString.Communicator.ToString());

        //    foreach (DataRow dr in dtLY.Rows)
        //    {
        //        listLY.Add(dr["Content"].ToString().ToLower());
        //    }
        //    listLY.Add(blast.EmailSubject.Trim().ToLower());
        //    if (blast.DynamicReplyToEmail.Trim().Length > 0)
        //        listLY.Add(blast.DynamicReplyToEmail.Trim().ToLower());
        //    if (blast.DynamicFromEmail.Trim().Length > 0)
        //        listLY.Add(blast.DynamicFromEmail.Trim().ToLower());
        //    if (blast.DynamicFromName.Trim().Length > 0)
        //        listLY.Add(blast.DynamicFromName.Trim().ToLower());

        //    return listLY;
        //}

        public static int? GetNextBlastForEngine(int blastEngineID, string status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_GetNextBlastForEngine";
            cmd.Parameters.AddWithValue("@BlastEngineID", blastEngineID);
            cmd.Parameters.AddWithValue("@Status", status);

            int blastID = 0;
            int.TryParse(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString(), out blastID);
            if (blastID > 0)
                return blastID;
            else
                return null;

            //return blastID;
        }

        public static int GetSampleCount(int sampleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_GetSampleCount";
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int blastID, int customerID, int userID)
        {
            DeleteCache(blastID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Pause_UnPauseBlast(int blastID,bool isPause, int customerID, int userID)
        {
            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_Pause";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@IsPause", isPause);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetBlastCalendarDetails(int customerID, int isSummary, DateTime startDate, DateTime endDate, int campaignID, string blastType, string subject, string group, int sentUserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastCalendarDetails";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@IsSummary", isSummary);
            cmd.Parameters.AddWithValue("@FromDate", startDate);
            cmd.Parameters.AddWithValue("@ToDate", endDate);
            cmd.Parameters.AddWithValue("@campaignID", campaignID);
            cmd.Parameters.AddWithValue("@BlastType", blastType);
            cmd.Parameters.AddWithValue("@SubjectSearch", subject);
            cmd.Parameters.AddWithValue("@GroupSearch", group);
            cmd.Parameters.AddWithValue("@SentUserID", sentUserID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetBlastCalendarDaily(int customerID, DateTime startDate, DateTime endDate, int campaignID, string blastType, string subject, string group, int sentUserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastCalendarDaily";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FromDate", startDate);
            cmd.Parameters.AddWithValue("@ToDate", endDate);
            cmd.Parameters.AddWithValue("@campaignID", campaignID);
            cmd.Parameters.AddWithValue("@BlastType", blastType);
            cmd.Parameters.AddWithValue("@SubjectSearch", subject);
            cmd.Parameters.AddWithValue("@GroupSearch", group);
            cmd.Parameters.AddWithValue("@SentUserID", sentUserID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetAutoRespondersForGrid(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetAutoResponders";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetSampleInfo(int customerID, int sampleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetSampleInfo";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetCustomerSamples(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_CustomerSamples";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static void UpdateStartTime(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_UpdateStartTime";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateFilterForAPITestBlasts(int blastID, int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update blast set filterid = @filterid where blastid = @blastid and testblast = 'Y'";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateStatus(int blastID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_UpdateStatus";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@Status", status.ToString());
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateStatusBlastEngineID(int blastID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_UpdateStatusBlastEngineID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@Status", status.ToString());
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetBlastEmailListForDynamicContent(int customerID, int blastID, int groupID, int filterID, string blastIDBounceDomain, string actionType, string refBlastID, string suppressionList, bool onlyCounts)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastEmailsListForDynamicContent";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastIDBounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actionType);
            cmd.Parameters.AddWithValue("@refBlastID", refBlastID);
            cmd.Parameters.AddWithValue("@SupressionList", suppressionList);
            cmd.Parameters.AddWithValue("@OnlyCounts", onlyCounts);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetBlastEmailListForDynamicContent(int customerID, int blastID, int groupID, int filterID, string blastIDBounceDomain, string actionType, string refBlastID, string suppressionList, bool onlyCounts, bool LogSuppressed)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastEmailsListForDynamicContent";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastIDBounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actionType);
            cmd.Parameters.AddWithValue("@refBlastID", refBlastID);
            cmd.Parameters.AddWithValue("@SupressionList", suppressionList);
            cmd.Parameters.AddWithValue("@OnlyCounts", onlyCounts);
            cmd.Parameters.AddWithValue("@LogSuppressed", LogSuppressed);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetBlastEmailListForDynamicContent_Filters(int customerID, int blastID, int groupID, List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters, string blastIDBounceDomain, bool onlyCounts, bool LogSuppressed)
        {
            #region Building XML for filters and ss for blast group and suppression groups
            StringBuilder actiontypes = new StringBuilder();
            actiontypes.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            actiontypes.Append("<SmartSegments>");
            StringBuilder SuppFilters = new StringBuilder();
            SuppFilters.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            SuppFilters.Append("<SuppFilters>");
            string filterIDs = string.Empty;

            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemBlastID != null))
            {
                if (cibf.FilterID != null)
                {
                    filterIDs += cibf.FilterID.ToString() + ",";
                }
                else if (cibf.SmartSegmentID != null)
                {
                    actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                    actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                }
            }
            actiontypes.Append("</SmartSegments>");
            filterIDs = filterIDs.TrimEnd(',');
            SuppFilters = FillSuppresionFilters(filters, SuppFilters);
            #endregion
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastEmailsListForDynamicContent";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterIDs);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastIDBounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actiontypes.ToString());

            cmd.Parameters.AddWithValue("@SupressionList", SuppFilters.ToString());
            cmd.Parameters.AddWithValue("@OnlyCounts", onlyCounts);
            cmd.Parameters.AddWithValue("@LogSuppressed", LogSuppressed);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetDefaultContentForSlotandDynamicTags(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetDefaultContent_For_SlotandDynamicTags";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //    scope.Complete();
            //}
            return dt;
        }

        public static int GetBlastEmailsListCount(int customerID, int blastID, int groupID, int filterID, string blastIDAndBounceDomain, string actionType, string refBlastIDs, string suppressionList)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastEmailsListForDynamicContent";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastIDAndBounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actionType);
            cmd.Parameters.AddWithValue("@refBlastID", refBlastIDs);
            cmd.Parameters.AddWithValue("@SupressionList", suppressionList);
            cmd.Parameters.AddWithValue("@OnlyCounts", true);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetBlastEmailsListCount(int customerID, int blastID, int groupID, List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters, string blastIDAndBounceDomain, string suppressionList,bool testblast)
        {

            #region Building XML for filters and ss for blast group and suppression groups
            StringBuilder actiontypes = new StringBuilder();
            actiontypes.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            actiontypes.Append("<SmartSegments>");
            StringBuilder SuppFilters = new StringBuilder();
            SuppFilters.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            SuppFilters.Append("<SuppFilters>");
            string filterIDs = string.Empty;

            //adding this for resending test blast with filter
            if (testblast)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemTestBlastID != null))
                {
                    if (cibf.FilterID != null)
                    {
                        filterIDs += cibf.FilterID.ToString() + ",";
                    }
                }
            }
            else
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemBlastID != null))
                {
                    if (cibf.FilterID != null)
                    {
                        filterIDs += cibf.FilterID.ToString() + ",";
                    }
                    else if (cibf.SmartSegmentID != null)
                    {
                        actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                        actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                    }
                }
            }
            actiontypes.Append("</SmartSegments>");
            filterIDs = filterIDs.TrimEnd(',');
            SuppFilters = FillSuppresionFilters(filters, SuppFilters);
            #endregion

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastEmailsListForDynamicContent";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FilterID", filterIDs);
            cmd.Parameters.AddWithValue("@BlastID_and_BounceDomain", blastIDAndBounceDomain);
            cmd.Parameters.AddWithValue("@ActionType", actiontypes.ToString());
            cmd.Parameters.AddWithValue("@SupressionList", SuppFilters.ToString());
            cmd.Parameters.AddWithValue("@OnlyCounts", true);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static StringBuilder FillSuppresionFilters(
            IList<CommCampaignItemBlastFilter> blastFilters, 
            StringBuilder suppFilters)
        {
            Guard.NotNull(blastFilters, nameof(blastFilters));
            Guard.NotNull(suppFilters, nameof(suppFilters));

            var groups = blastFilters.Where(x => x.SuppressionGroupID != null)
                .Select(e => e.SuppressionGroupID ?? 0)
                .Distinct()
                .ToArray();

            foreach (var currentGroupId in groups)
            {
                var suppFilter = blastFilters.Where(x => x.SuppressionGroupID == currentGroupId).ToList();
                suppFilters.Append($"{SuppresionGroupTag}{currentGroupId}{CloseTag}");
                suppFilters.Append(FilterIdsTag);

                foreach (var filter in suppFilter.Where(x => x.FilterID != null).ToList())
                {
                    suppFilters.Append(filter.FilterID + CommaSeparator);
                }

                suppFilters.Append(FilterIdsCloseTag);
                foreach (var filter in suppFilter.Where(x => x.SmartSegmentID != null).ToList())
                {
                    suppFilters.Append($"{SsIdTag}{filter.SmartSegmentID}{CloseTag}");
                    suppFilters.Append($"{RefBlastIdOpenTag}{filter.RefBlastIDs}{RefBlastIdCloseTag}");
                }

                suppFilters.Append(SuppressionGroupCloseTag);
            }

            suppFilters.Append(SuppFiltersCloseTag);
            suppFilters = suppFilters.Replace(CommaSeparator + OpenTag, OpenTag);
            return suppFilters;
        }

        public static int GetBlastUserByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CreatedUserID FROM Blast WITH (NOLOCK) WHERE BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetSampleBlastUserBySampleID(int sampleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Top 1 CreatedUserID FROM Blast WITH (NOLOCK) WHERE SampleID = @SampleID and BlastType = 'Sample'";
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        //reports
        public static DataTable GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status, int? baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_Blast_ByStatus";
            cmd.Parameters.AddWithValue("@Status", status.ToString());
            if (baseChannelID != null)
                cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID.Value);
            DataTable dt = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            //scope.Complete();
            //}
            return dt;
        }

        public static DataTable GetGroupNamesByBlasts(int campaignItemID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_Blast_GetGroupNamesByBlasts";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        public static DataSet GetBlastGroupClicksData(int? campaignItemID, int? blastID, string howMuch, string isp, string reportType, string topClickURL, int pageNo, int pageSize, string udfName, string udfData, string startDate = "", string endDate = "", bool unique = false)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_Blast_GetBlastGroupClicksData";
            if (campaignItemID != null)
                cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID.Value);
            else if (blastID != null)
                cmd.Parameters.AddWithValue("@BlastID", blastID.Value);
            cmd.Parameters.AddWithValue("@HowMuch", howMuch);
            cmd.Parameters.AddWithValue("@ISP", isp);
            cmd.Parameters.AddWithValue("@ReportType", reportType);
            cmd.Parameters.AddWithValue("@TopClickURL", topClickURL);
            cmd.Parameters.AddWithValue("@PageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@UDFname", udfName);
            cmd.Parameters.AddWithValue("@UDFdata", udfData);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@Unique", unique);

            DataSet ds = null;
            ds = DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return ds;
        }

        public static DataTable ClickActivityDetailedReport(int blastID, int customerID, string linkURL, string ProfileFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_Blast_ClickActivityDetailedReport";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@LinkURL", linkURL);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ProfileFilter", ProfileFilter);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        public static DataTable GetBlastStatusByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand("rpt_Blast_StatusByBlastID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }
        public static DataTable GetBlastStatusByBlastEngineIDFinishTime(int BlastEngineID, DateTime FinishTime)
        {
            SqlCommand cmd = new SqlCommand("rpt_Blast_StatusByBlastEngineIDFinishTime");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastEngineID", BlastEngineID);
            cmd.Parameters.AddWithValue("@FinishTime", FinishTime);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }
        public static DataTable GetBlastComparison(int customerID, DateTime startTime, DateTime endTime, int? userID, int? groupID, int? campaignID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_Comparison";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@StartTime", startTime);
            cmd.Parameters.AddWithValue("@EndTime", endTime);
            if (userID != null)
                cmd.Parameters.AddWithValue("@UserID", userID.Value);
            if (groupID != null)
                cmd.Parameters.AddWithValue("@GroupID", groupID.Value);
            if (campaignID != null)
                cmd.Parameters.AddWithValue("@CampaignID", campaignID.Value);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        public static DataTable GetEstimatedSendsCount(string XMLFormat, int customerID, bool ignoreSuppression)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetEstimatedSendsCount";
            cmd.Parameters.AddWithValue("@XMLFormat", XMLFormat);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ignoreSuppression", ignoreSuppression);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        //for link tracking params
        public static string GetLinkTrackingParam(int blastID, string param)
        {
            string ret = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            switch (param.ToUpper())
            {
                case "FOLDERNAME":
                    cmd.CommandText = "select case when g.FolderID = 0 then 'ROOT' else ISNULL(f.FolderName, '') end from Blast b with (nolock) join Groups g with (nolock) on b.GroupID = g.GroupID left outer join Folder f with (nolock) on g.FolderID = f.FolderID where b.BlastID=@BlastID";
                    break;
                case "GROUPNAME":
                    cmd.CommandText = "select g.GroupName from blast b with (nolock) join Groups g with (nolock) on b.GroupID=g.GroupID where b.BlastID=@BlastID";
                    break;
                case "LAYOUTNAME":
                    cmd.CommandText = "select l.LayoutName from blast b with (nolock) join Layout l with (nolock) on b.LayoutID=l.LayoutID where b.BlastID=@BlastID";
                    break;
                case "EMAILSUBJECT":
                    cmd.CommandText = "select b.EmailSubject from blast b with (nolock) where b.BlastID=@BlastID";
                    break;
                default:
                    break;
            }

            return DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
        }

        //Get info for abuse in activity
        public static DataTable GetBlastInfoForAbuseReporting(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetBlastInfoForAbuse";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        //for public preview
        public static DataTable GetHTMLPreview(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Blast_GetHTMLPreview";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }

        //get list of sent blast ids by group id
        public static string GetSentByGroupID(int groupID)
        {
            string ret = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_blast_GetSentByGroup";
            cmd.Parameters.AddWithValue("@GroupID", groupID);

            return DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
        }
    }
}
