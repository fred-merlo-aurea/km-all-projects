using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Communicator.BlastCreator;
using ECN_Framework_Common.Objects;
using KM.Common;
using CommBlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using CommBlastRegular = ECN_Framework_Entities.Communicator.BlastRegular;
using CampaignItemEntity = ECN_Framework_Entities.Communicator.CampaignItem;
using CommTestBlast = ECN_Framework_Entities.Communicator.CampaignItemTestBlast;
using CommEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using Enums = ECN_Framework_Common.Objects.Enums;
using KmUser = KMPlatform.Entity.User;
using StringFunctions = KM.Common.StringFunctions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Blast
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        private const string ReplaceHtmlRegex = @"
<\s*(a(?:rea)?|link)           # link or anchor tag or area
\s                 # followed by whitespace (a single newline or space)
([^>]*)            # optionally followed by some chars that don't close the tag
(href\s*=\s*)      # followed by href=, with spaces allowed on either side of the equal sign
([""'])            # followed by open single or double quote
([^>]*?&l=([^>]*?))  # followed by some text that doesn't close the tag and does contain &l=<stuff>
\4                 # followed by a close quote matching the prior open quote type
([^>]*>)           # followed by some more stuff and then a close tag
";
        private const string MatchIdRegex = @"ecn_id=([""']?)(.*?)\1";
        private const string ReplacePattern = @"<$1 $2 $3""$5"" $7";
        private const string BlastIdKey = "BlastID";
        private const string YesCode = "Y";
        private const int DefaultSendTime = 15;
        public static readonly Regex LinkRegex = new Regex(ReplaceHtmlRegex, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        public static readonly Regex EcnIdRegex = new Regex(MatchIdRegex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Blast;

        public static bool CreatedUserExists(int userID)
        {
            List<ECN_Framework_Entities.Communicator.Blast> contentList = new List<ECN_Framework_Entities.Communicator.Blast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                bool result = ECN_Framework_DataLayer.Communicator.Blast.CreatedUserExists(userID);
                scope.Complete();
                return result;
            }
        }

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        #region GET
        public static List<ECN_Framework_Entities.Communicator.BlastAbstract> GetByCampaignID(int campaignID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignID(campaignID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    blastList.Add(blast);
                }
            }
            return blastList;
        }

        public static List<CommBlastAbstract> GetByCampaignItemID(int campaignItemId, KmUser user, bool getChildren)
        {
            return GetBlastList(campaignItemId, null, null, user, getChildren) as List<CommBlastAbstract>;
        }

        public static List<CommBlastAbstract> GetByCampaignItemID_NoAccessCheck(int campaignItemId, bool getChildren)
        {
            return GetBlastList(campaignItemId, null, null, null, getChildren) as List<CommBlastAbstract>;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByCampaignItemBlastID(int campaignItemBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    break;
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByCampaignItemBlastID_NoAccessCheck(int campaignItemBlastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID_NoAccessCheck(Convert.ToInt32(row["BlastID"].ToString()), getChildren);
                    break;
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByCampaignItemBlastID_UseAmbientTransaction(int campaignItemBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope())
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID_UseAmbientTransaction(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    break;
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByCampaignItemTestBlastID(int campaignItemTestBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignItemTestBlastID(campaignItemTestBlastID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    break;
                }
            }
            return blast;
        }

        public static int GetBlastUserID(int blastID)
        {
            return ECN_Framework_DataLayer.Communicator.Blast.GetByBlastID(blastID).CreatedUserID.Value;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetMasterRefBlast(int blastID, int EmailID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetMasterRefBlast(blastID, EmailID);
                scope.Complete();

            }
            return blast;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastID_NoAccessCheck(int blastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (blast != null)
            {

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID_NoAccessCheck(blastID);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, getChildren);
                    //if (blast.CreatedUserID != null)
                    //  blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    //if (blast.UpdatedUserID != null)
                    //  blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID(blast.BlastID, true);
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastID_NoAccessCheck_UseAmbientTransaction(int blastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope())
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (blast != null)
            {

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID_NoAccessCheck_UseAmbientTransaction(blastID);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck_UseAmbientTransaction(blast.GroupID.Value);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck_UseAmbientTransaction(blast.LayoutID.Value, getChildren);
                    //if (blast.CreatedUserID != null)
                    //  blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    //if (blast.UpdatedUserID != null)
                    //  blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID_UseAmbientTransaction(blast.BlastID, true);
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastID(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (blast != null)
            {
                if (!HasPermission(KMPlatform.Enums.Access.View, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID(blastID, user);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(blast.LayoutID.Value, user, getChildren);
                    //if (blast.CreatedUserID != null)
                    //blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    //if (blast.UpdatedUserID != null)
                    //blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID(blast.BlastID, true);
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetByBlastID_UseAmbientTransaction(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope())
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (blast != null)
            {
                if (!HasPermission(KMPlatform.Enums.Access.View, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID_UseAmbientTransaction(blastID, user);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck_UseAmbientTransaction(blast.GroupID.Value);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_UseAmbientTransaction(blast.LayoutID.Value, user, getChildren);
                    //if (blast.CreatedUserID != null)
                    //blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    //if (blast.UpdatedUserID != null)
                    //blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID_UseAmbientTransaction(blast.BlastID, true);
                }
            }
            return blast;
        }


        public static ECN_Framework_Entities.Communicator.BlastAbstract GetTopOneByLayoutID(int layoutID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetTopOneByLayoutID(layoutID);
                scope.Complete();
            }

            if (blast != null)
            {
                if (!HasPermission(KMPlatform.Enums.Access.View, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID(blast.BlastID, user);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(blast.GroupID.Value, user);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(blast.LayoutID.Value, user, getChildren);
                    if (blast.CreatedUserID != null)
                        blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.UpdatedUserID != null)
                        blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID(blast.BlastID, true);
                }
            }
            return blast;
        }

        public static ECN_Framework_Entities.Communicator.BlastAbstract GetTopOneByLayoutID_NoAccessCheck(int layoutID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blast = ECN_Framework_DataLayer.Communicator.Blast.GetTopOneByLayoutID(layoutID);
                scope.Complete();
            }

            if (blast != null)
            {

                ECN_Framework_Entities.Communicator.BlastFields fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID_NoAccessCheck(blast.BlastID);
                if (fields != null)
                {
                    blast.Fields = fields;
                }
                if (getChildren)
                {
                    if (blast.GroupID != null)
                        blast.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
                    if (blast.LayoutID != null)
                        blast.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, getChildren);
                    if (blast.CreatedUserID != null)
                        blast.CreatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.UpdatedUserID != null)
                        blast.UpdatedUser = KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, blast.CustomerID.Value, getChildren);
                    if (blast.BlastScheduleID != null)
                        blast.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID(blast.BlastID, true);
                }
            }
            return blast;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastAbstract> GetBySearch(int customerID, string emailSubject, int? userID, int? groupID, bool? isTest, string statusCode, DateTime? modifiedFrom, DateTime? modifiedTo, int? campaignID, string campaignName, string campaignItemName, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBySearch(customerID, emailSubject, userID, groupID, isTest, statusCode, modifiedFrom, modifiedTo, campaignID, campaignName, campaignItemName);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    if (blast != null)
                        blastList.Add(blast);
                }
            }
            return blastList;
        }

        public static List<CommBlastAbstract> GetBySampleID(int sampleId, KmUser user, bool getChildren)
        {
            return GetBlastList(null, null, sampleId, user, getChildren) as List<CommBlastAbstract>;
        }

        public static List<CommBlastAbstract> GetBySampleID_NoAccessCheck(int sampleId, bool getChildren)
        {
            return GetBlastList(null, null, sampleId, null, getChildren) as List<CommBlastAbstract>;
        }

        public static void ResendTestBlast(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = GetByBlastID(blastID, user, false);

            if (blast.TestBlast.ToLower().Equals("y"))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    ECN_Framework_DataLayer.Communicator.Blast.ResendTestBlast(blastID);
                    scope.Complete();
                }
            }
        }

        public static List<ECN_Framework_Entities.Communicator.BlastAbstract> GetByGroupID(int groupID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByGroupID(groupID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID(Convert.ToInt32(row["BlastID"].ToString()), user, getChildren);
                    blastList.Add(blast);
                }
            }
            return blastList;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastAbstract> GetByGroupID_NoAccessCheck(int groupID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.BlastAbstract blast = null;
            List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetByGroupID(groupID);
                scope.Complete();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    blast = GetByBlastID_NoAccessCheck(Convert.ToInt32(row["BlastID"].ToString()), getChildren);
                    blastList.Add(blast);
                }
            }
            return blastList;
        }

        public static List<CommBlastAbstract> GetByCustomerID(int customerId, KmUser user, bool getChildren)
        {
            return GetBlastList(null, customerId, null, user, getChildren) as List<CommBlastAbstract>;
        }

        private static IList<CommBlastAbstract> GetBlastList(int? campaignItemId, int? customerId, int? sampleId, KmUser user, bool getChildren)
        {
            int blastId;
            DataTable dtBlast = null;
            var blastList = new List<CommBlastAbstract>();

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (campaignItemId > 0)
                {
                    dtBlast = ECN_Framework_DataLayer.Communicator.Blast.GetByCampaignItemID(campaignItemId.Value);
                }
                if (customerId > 0)
                {
                    dtBlast = ECN_Framework_DataLayer.Communicator.Blast.GetByCustomerID(customerId.Value);
                }
                if (sampleId > 0)
                {
                    dtBlast = ECN_Framework_DataLayer.Communicator.Blast.GetBySampleID(sampleId.Value);
                }
                scope.Complete();
            }
            if (dtBlast?.Rows.Count > 0)
            {
                foreach (DataRow row in dtBlast.Rows)
                {
                    if (int.TryParse(row[BlastIdKey].ToString(), out blastId))
                    {
                        var blast = user != null
                            ? GetByBlastID(Convert.ToInt32(row[BlastIdKey].ToString()), user, getChildren)
                            : GetByBlastID_NoAccessCheck(Convert.ToInt32(row[BlastIdKey].ToString()), getChildren);
                        blastList.Add(blast);
                    }
                }
            }
            return blastList;
        }

        #endregion

        #region EXISTS
        public static bool Exists(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.Exists(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.Exists(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }
        #endregion

        #region EXISTS BY STATUS
        public static bool ActiveOrSent(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActiveOrSent(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }



        public static bool ActiveOrSent_UseAmbientTransaction(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActiveOrSent(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActivePendingOrSentByBlast(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActivePendingOrSentByBlast(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActivePendingOrSentByLayout(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActivePendingOrSentByLayout(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActivePendingOrSentByGroup(int groupID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActivePendingOrSentByGroup(groupID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActivePendingOrSentByFilter(int filterID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActivePendingOrSentByFilter(filterID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ActivePendingOrSentBySample(int sampleID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.ActivePendingOrSentBySample(sampleID, customerID);
                scope.Complete();
            }
            return exists;
        }
        #endregion

        internal static int Save(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            blast.BlastID = ECN_Framework_DataLayer.Communicator.Blast.Save(blast);
            return blast.BlastID;
        }

        public static void SetHasEmailPreview(int blastID, bool hasEmailPreview)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Blast.SetHasEmailPreview(blastID, hasEmailPreview);
                scope.Complete();
            }
        }

        public static bool RefBlastsExists(string blastIDs, int customerID, DateTime sendTime)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.RefBlastsExists(blastIDs, customerID, sendTime);
                scope.Complete();
            }
            return exists;
        }

        public static bool DynamicCotentExists(int blastID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Blast.DynamicCotentExists(blastID);
                scope.Complete();
            }
            return exists;
        }

        public static void ValidateBlastContent(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        {
            var layoutList = Layout.ValidateLayoutContent(blast.LayoutID.Value);
            layoutList.Add(blast.EmailSubject.Trim().ToLower());
            if (blast.DynamicReplyToEmail.Trim().Length > 0)
            {
                layoutList.Add(blast.DynamicReplyToEmail.Trim().ToLower());
            }
            if (blast.DynamicFromEmail.Trim().Length > 0)
            {
                layoutList.Add(blast.DynamicFromEmail.Trim().ToLower());
            }
            if (blast.DynamicFromName.Trim().Length > 0)
            {
                layoutList.Add(blast.DynamicFromName.Trim().ToLower());
            }
            Group.ValidateDynamicStrings(layoutList.ToList(), blast.GroupID.Value, user);
        }

        public static int? GetNextBlastForEngine(int blastEngineID, string status)
        {
            int? blastID = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastID = ECN_Framework_DataLayer.Communicator.Blast.GetNextBlastForEngine(blastEngineID, status);
                scope.Complete();
            }
            return blastID;
        }

        public static int GetSampleCount(int sampleID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.Blast.GetSampleCount(sampleID);
                scope.Complete();
            }
            return count;
        }

        public static void UpdateStartTime(int blastID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Blast.UpdateStartTime(blastID);
                scope.Complete();
            }
        }

        public static void UpdateStatus(int blastID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Blast.UpdateStatus(blastID, status);
                scope.Complete();
            }
        }

        public static void UpdateStatusBlastEngineID(int blastID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Blast.UpdateStatusBlastEngineID(blastID, status);
                scope.Complete();
            }
        }

        public static void UpdateFilterForAPITestBlasts(int blastID, int filterID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Blast.UpdateFilterForAPITestBlasts(blastID, filterID);
                scope.Complete();
            }
        }

        public static void Delete(int blastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            ECN_Framework_Entities.Communicator.BlastAbstract blast = GetByBlastID(blastID, user, false);
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(blastID, user.CustomerID))
            {
                errorList.Add(new ECNError(Entity, Method, "Cannot delete Blast as it is active or sent"));
                throw new ECNException(errorList);
            }
            else
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(blastID, user, false);
                List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
                checkList = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(ci.CampaignItemID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem);
                if (checkList.Count > 0)
                {
                    errorList.Add(new ECNError(Entity, Method, "Campaign Item is used in Marketing Automation(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }

                List<ECN_Framework_Entities.Communicator.CampaignItem> ssBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItem.UsedAsSmartSegment(blastID);
                if (ssBlastList.Count > 0)
                {
                    StringBuilder sbBlast = new StringBuilder();
                    foreach(ECN_Framework_Entities.Communicator.CampaignItem b in ssBlastList)
                    {
                        sbBlast.Append(b.CampaignItemName.ToString() + ", ");
                    }

                    errorList.Add(new ECNError(Enums.Entity.Blast, Method, "Cannot delete Blast as it is tied to smart segment for Campaign Item(s):" + sbBlast.ToString().Trim().TrimEnd(',')));
                    throw new ECNException(errorList);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_BusinessLayer.Communicator.BlastFields.Delete(blastID, user);
                        ECN_Framework_DataLayer.Communicator.Blast.Delete(blastID, user.CustomerID, user.UserID);
                        ECN_Framework_BusinessLayer.Communicator.ReportSchedule.DeleteByBlastId(blastID, user);
                        scope.Complete();
                    }
                }
            }
        }

        public static void Pause_UnPauseBlast(int blastID, bool isPause, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            List<ECNError> errorList = new List<ECNError>();

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.BlastAbstract blast = GetByBlastID(blastID, user, false);

            using (TransactionScope scope = new TransactionScope())
            {

                ECN_Framework_DataLayer.Communicator.Blast.Pause_UnPauseBlast(blastID, isPause, blast.CustomerID.Value, user.UserID);

                scope.Complete();
            }

        }

        public static DataTable GetBlastCalendarDetails(int isSummary, DateTime startDate, DateTime endDate, int campaignID, string blastType, string subject, string group, int sentUserID, int customerID)
        {
            DataTable dtCalendar = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCalendar = ECN_Framework_DataLayer.Communicator.Blast.GetBlastCalendarDetails(customerID, isSummary, startDate, endDate, campaignID, blastType, subject, group, sentUserID);
                scope.Complete();
            }

            return dtCalendar;
        }

        public static DataTable GetBlastCalendarDaily(DateTime startDate, DateTime endDate, int campaignID, string blastType, string subject, string group, int sentUserID, int customerID)
        {
            DataTable dtCalendar = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCalendar = ECN_Framework_DataLayer.Communicator.Blast.GetBlastCalendarDaily(customerID, startDate, endDate, campaignID, blastType, subject, group, sentUserID);
                scope.Complete();
            }

            return dtCalendar;
        }


        public static DataTable GetAutoRespondersForGrid(int customerID, KMPlatform.Entity.User user)
        {
            DataTable dtAutoResponders = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtAutoResponders = ECN_Framework_DataLayer.Communicator.Blast.GetAutoRespondersForGrid(customerID);
                scope.Complete();
            }

            return dtAutoResponders;
        }

        public static void PreValidate(ECN_Framework_Entities.Communicator.BlastAbstract blast)
        {
            PreValidateBlast(blast, true);
        }

        private static void ValidateCreatedAndUpdatedUser(ECN_Framework_Entities.Communicator.BlastAbstract blast, Enums.Method methodValidate, List<ECNError> errorList)
        {
            if (blast.CreatedUserID == null
                || (blast.CreatedUserID.HasValue
                    && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(blast.CreatedUserID.Value, false))))
            {
                if (blast.CreatedUserID == null
                    || (!KMPlatform.BusinessLogic.User.Exists(blast.CreatedUserID.Value, blast.CustomerID.Value)))
                {
                    var invalidation = Base.UserValidation.Invalidate(blast);
                    if (!string.IsNullOrWhiteSpace(invalidation))
                    {
                        errorList.Add(new ECNError(Entity, methodValidate, invalidation));
                    }
                }
            }
            if (blast.BlastID > 0
                && (blast.UpdatedUserID == null
                    || (blast.UpdatedUserID.HasValue
                        && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(blast.UpdatedUserID.Value, false)))))
            {
                if (blast.BlastID > 0
                    && (blast.UpdatedUserID == null
                        || (!KMPlatform.BusinessLogic.User.Exists(blast.UpdatedUserID.Value, blast.CustomerID.Value))))
                {
                    errorList.Add(new ECNError(Entity, methodValidate, "UserID is invalid"));
                }
            }
        }

        public static void PreValidate_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastAbstract blast)
        {
            PreValidateBlast(blast);
        }

        private static void PreValidateBlast(ECN_Framework_Entities.Communicator.BlastAbstract blast, bool shouldHandleCreateAndUpdateUser = false)
        {
            var methodValidate = Enums.Method.Validate;
            var errorList = new List<ECNError>();

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (blast.CustomerID == null || (!Customer.Exists(blast.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, methodValidate, "CustomerID is invalid"));
                }
                else if(shouldHandleCreateAndUpdateUser)
                {
                    ValidateCreatedAndUpdatedUser(blast, methodValidate, errorList);
                }
                scope.Complete();
            }

            if (blast.SendTime == null || blast.SendTime.Value.AddSeconds(5) <= DateTime.Now)
            {
                errorList.Add(new ECNError(Entity, methodValidate, "SendTime must be in the future"));
            }
            if (blast.CodeID != null
                && (blast.CustomerID == null
                    || !Code.Exists(blast.CodeID.Value, blast.CustomerID.Value)))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "CodeID is invalid"));
            }
            if (blast.BlastScheduleID != null && !BlastSchedule.Exists(blast.BlastScheduleID.Value))
            {
                errorList.Add(new ECNError(Entity, methodValidate, "BlastScheduleID is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        internal static void CreateBlastsFromCampaignItemTestBlast(int campaignItemTestBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemTestBlastID(campaignItemTestBlastID, user, true);
            if (item != null && item.TestBlastList.Count > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTestBlast = item.TestBlastList.Single(x => x.CampaignItemTestBlastID == campaignItemTestBlastID);
                var blast = SetBlastRegularProperties(user, item, ciTestBlast, true);

                if (item.BlastField1.Trim().Length > 0 || item.BlastField2.Trim().Length > 0 || item.BlastField3.Trim().Length > 0 || item.BlastField4.Trim().Length > 0 || item.BlastField5.Trim().Length > 0)
                {
                    ECN_Framework_Entities.Communicator.BlastFields fields = new ECN_Framework_Entities.Communicator.BlastFields();
                    fields.Field1 = item.BlastField1;
                    fields.Field2 = item.BlastField2;
                    fields.Field3 = item.BlastField3;
                    fields.Field4 = item.BlastField4;
                    fields.Field5 = item.BlastField5;
                    blast.Fields = fields;

                }

                BlastRegular reg = new BlastRegular();
                int newBlastID = reg.Save(blast, user);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.UpdateBlastID(campaignItemTestBlastID, newBlastID, user.UserID);
            }
        }

        internal static void CreateBlastsFromQuickTestBlast(int campaignItemTestBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemTestBlastID(campaignItemTestBlastID, user, true);
            if (item != null && item.TestBlastList.Count > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTestBlast = item.TestBlastList.Single(x => x.CampaignItemTestBlastID == campaignItemTestBlastID);
                var blast = SetBlastRegularProperties(user, item, ciTestBlast, false);

                if (item.BlastField1.Trim().Length > 0 || item.BlastField2.Trim().Length > 0 || item.BlastField3.Trim().Length > 0 || item.BlastField4.Trim().Length > 0 || item.BlastField5.Trim().Length > 0)
                {
                    ECN_Framework_Entities.Communicator.BlastFields fields = new ECN_Framework_Entities.Communicator.BlastFields();
                    fields.Field1 = item.BlastField1;
                    fields.Field2 = item.BlastField2;
                    fields.Field3 = item.BlastField3;
                    fields.Field4 = item.BlastField4;
                    fields.Field5 = item.BlastField5;
                    blast.Fields = fields;

                }

                BlastRegular reg = new BlastRegular();
                int newBlastID = reg.Save(blast, user);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.UpdateBlastID(campaignItemTestBlastID, newBlastID, user.UserID);
            }
        }

        private static CommBlastAbstract SetBlastRegularProperties(KmUser user, CampaignItemEntity item, CommTestBlast ciTestBlast, bool useCampaignItem)
        {
            Guard.NotNull(user, nameof(user));
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(ciTestBlast, nameof(ciTestBlast));

            var blast = new CommBlastRegular
            {
                CreatedUserID = user.UserID,
                CustomerID = user.CustomerID,
                TestBlast = YesCode,
                StatusCode = CommEnums.BlastStatusCode.Pending.ToString(),
                SendTime = DateTime.Now.AddSeconds(DefaultSendTime),
                BlastScheduleID = item.BlastScheduleID,
                OverrideAmount = item.OverrideAmount,
                OverrideIsAmount = item.OverrideIsAmount,
                HasEmailPreview = ciTestBlast.HasEmailPreview,
                GroupID = ciTestBlast.GroupID,
                EnableCacheBuster = item.EnableCacheBuster
            };

            // BlastList may come with values, but it is not guaranteed that they should be used
            if (useCampaignItem)
            {
                blast.EmailSubject = item.BlastList[0].EmailSubject;
                blast.EmailFrom = item.FromEmail;
                blast.EmailFromName = item.FromName;
                blast.ReplyTo = item.ReplyTo;
                blast.LayoutID = item.BlastList[0].LayoutID;
                blast.AddOptOuts_to_MS = item.BlastList[0].AddOptOuts_to_MS;
                blast.DynamicFromEmail = item.BlastList[0].DynamicFromEmail;
                blast.DynamicFromName = item.BlastList[0].DynamicFromName;
                blast.DynamicReplyToEmail = item.BlastList[0].DynamicReplyTo;
            }
            else
            {
                blast.EmailSubject = ciTestBlast.EmailSubject;
                blast.EmailFrom = ciTestBlast.FromEmail;
                blast.EmailFromName = ciTestBlast.FromName;
                blast.ReplyTo = ciTestBlast.ReplyTo;
                blast.LayoutID = ciTestBlast.LayoutID;
                blast.AddOptOuts_to_MS = false;
                blast.DynamicFromEmail = string.Empty;
                blast.DynamicFromName = string.Empty;
                blast.DynamicReplyToEmail = string.Empty;
            }

            SetBlastTypeProperty(item, ciTestBlast, blast);
            return blast;
        }

        private static void SetBlastTypeProperty(CampaignItemEntity item, CommTestBlast ciTestBlast, CommBlastRegular blast)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(ciTestBlast, nameof(ciTestBlast));
            Guard.NotNull(blast, nameof(blast));

            if (item.CampaignItemType == CommEnums.CampaignItemType.Regular.ToString() || item.CampaignItemType == CommEnums.CampaignItemType.Salesforce.ToString())
            {
                if (ciTestBlast.CampaignItemTestBlastType == CommEnums.CampaignItemFormatType.HTML.ToString())
                {
                    blast.BlastType = CommEnums.BlastType.HTML.ToString();
                }
                else if (ciTestBlast.CampaignItemTestBlastType == CommEnums.CampaignItemFormatType.TEXT.ToString())
                {
                    blast.BlastType = CommEnums.BlastType.TEXT.ToString();
                }
            }
            else if (item.CampaignItemType == CommEnums.CampaignItemType.AB.ToString())
            {
                blast.BlastType = CommEnums.BlastType.Sample.ToString();
            }
            else if (item.CampaignItemType == CommEnums.CampaignItemType.Champion.ToString())
            {
                blast.BlastType = CommEnums.BlastType.Champion.ToString();
            }
            else if (item.CampaignItemType == CommEnums.CampaignItemType.SMS.ToString())
            {
                blast.BlastType = CommEnums.BlastType.SMS.ToString();
            }
        }

        public static void CreateBlastsFromCampaignItem(int campaignItemID, KMPlatform.Entity.User user, bool checkFirst = false)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID, user, false);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(campaignItemID, user, false);
            if (item != null)
            {
                if (checkFirst)
                {
                    bool updateBlastTable = false;
                    //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                    var blastExists = ciBlastList.Where(x => x.BlastID != null);
                    if (blastExists.Any())
                        updateBlastTable = true;

                    if (updateBlastTable)
                        CreateBlastsFromCampaignItem(campaignItemID, user);
                }
                else
                {
                    CreateBlastsFromCampaignItem(campaignItemID, user);
                }
            }
        }

        public static void CreateBlastsFromCampaignItem_NoAccessCheck(int campaignItemID, KMPlatform.Entity.User user, bool checkFirst = false)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemID, false);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(campaignItemID, false);
            if (item != null)
            {
                if (checkFirst)
                {
                    bool updateBlastTable = false;
                    //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                    var blastExists = ciBlastList.Where(x => x.BlastID != null);
                    if (blastExists.Any())
                        updateBlastTable = true;

                    if (updateBlastTable)
                        CreateBlastsFromCampaignItem_NoAccessCheck(campaignItemID, user);
                }
                else
                {
                    CreateBlastsFromCampaignItem_NoAccessCheck(campaignItemID, user);
                }
            }
        }

        public static void CreateBlastsFromCampaignItem_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool checkFirst = false, bool keepPaused = false)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_UseAmbientTransaction(campaignItemID, user, false);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(campaignItemID, user, false);
            if (item != null)
            {
                if (checkFirst)
                {
                    bool updateBlastTable = false;
                    //if we have ever created a Blast for this CampaignItem we will also update Blast table at the end
                    var blastExists = ciBlastList.Where(x => x.BlastID != null);
                    if (blastExists.Any())
                        updateBlastTable = true;

                    if (updateBlastTable)
                        CreateBlastsFromCampaignItem_UseAmbientTransaction(campaignItemID, user, keepPaused);
                }
                else
                {
                    CreateBlastsFromCampaignItem_UseAmbientTransaction(campaignItemID, user, keepPaused);
                }
            }
        }

        private static void CreateBlastsFromCampaignItem(int campaignItemID, KMPlatform.Entity.User user)
        {
            BlastFromCampaignCreator.CreateBlastsFromCampaignItem(campaignItemID, user);
        }
        private static void CreateBlastsFromCampaignItem_NoAccessCheck(int campaignItemID, KMPlatform.Entity.User user)
        {
            BlastFromCampaignCreator.CreateBlastsFromCampaignItem_NoAccessCheck(campaignItemID, user);
        }
        private static void CreateBlastsFromCampaignItem_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool keepPaused)
        {
            BlastFromCampaignCreator.CreateBlastsFromCampaignItem_UseAmbientTransaction(campaignItemID, user,keepPaused);
        }
     
        public static DataTable GetSampleInfo(int customerID, int sampleID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetSampleInfo(customerID, sampleID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetCustomerSamples(int customerID, int userID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetCustomerSamples(customerID, userID);
                scope.Complete();
            }

            return dt;
        }

        //public static bool CanBlastUsingLicense(ECN_Framework_Entities.Communicator.BlastAbstract blast, KMPlatform.Entity.User user)
        //{
        //    ECN_Framework_Entities.Accounts.License license = ECN_Framework_BusinessLayer.Accounts.License.GetCurrentLicensesByCustomerID(blast.CustomerID.Value, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeCode.emailblock10k);

        //    if (license.LicenseOption == ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.unlimited)
        //        return true;
        //    else if (license.LicenseOption == ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.notavailable)
        //        return false;
        //    else
        //    {
        //        int filterID = 0;
        //        if (blast.FilterID != null)
        //            filterID = blast.FilterID.Value;
        //        string smartSegmentName = string.Empty;
        //        if (blast.SmartSegmentID != null)
        //            smartSegmentName = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(blast.SmartSegmentID.Value).SmartSegmentName;
        //        int countToSend = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailsListCount(blast.CustomerID.Value, blast.BlastID, blast.GroupID.Value, filterID, "", smartSegmentName, blast.RefBlastID, blast.BlastSuppression, user);
        //        if (countToSend.ToString() == "0")
        //            return true;
        //        else
        //        {
        //            if ((license.Available < 1) || (countToSend > license.Available))
        //                return false;
        //            else
        //                return true;
        //        }
        //    }
        //}

        public static DataTable GetBlastEmailListForDynamicContent(int customerID, int blastID, int groupID, List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters, string blastIDBounceDomain, string actionType, string suppressionList, bool onlyCounts, bool logSuppressed)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastEmailListForDynamicContent_Filters(customerID, blastID, groupID, filters, blastIDBounceDomain, onlyCounts, logSuppressed);
                scope.Complete();
            }

            return dt;
        }
        public static DataTable GetBlastEmailListForDynamicContent(int customerID, int blastID, int groupID, int filterID, string blastIDBounceDomain, string actionType, string refBlastID, string suppressionList, bool onlyCounts)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastEmailListForDynamicContent(customerID, blastID, groupID, filterID, blastIDBounceDomain, actionType, refBlastID, suppressionList, onlyCounts);
                scope.Complete();
            }

            return dt;
        }
        public static DataTable GetBlastEmailListForDynamicContent(int customerID, int blastID, int groupID, int filterID, string blastIDBounceDomain, string actionType, string refBlastID, string suppressionList, bool onlyCounts, bool LogSuppressed)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastEmailListForDynamicContent(customerID, blastID, groupID, filterID, blastIDBounceDomain, actionType, refBlastID, suppressionList, onlyCounts, LogSuppressed);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetDefaultContentForSlotandDynamicTags(int blastID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetDefaultContentForSlotandDynamicTags(blastID);
                scope.Complete();
            }

            return dt;
        }

        public static int GetBlastEmailsListCount(int customerID, int blastID, int groupID, List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters, string blastIDAndBounceDomain, string suppressionList, bool testblast, KMPlatform.Entity.User user)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {

                count = ECN_Framework_DataLayer.Communicator.Blast.GetBlastEmailsListCount(customerID, blastID, groupID, filters, blastIDAndBounceDomain, suppressionList, testblast);
                scope.Complete();
            }

            return count;
        }

        public static int GetBlastEmailsListCount(int customerID, int blastID, int groupID, int filterId, string blastIDAndBounceDomain, string actiontype, string refBlastIDs, string suppressionList, KMPlatform.Entity.User user)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.Blast.GetBlastEmailsListCount(customerID, blastID, groupID, filterId, blastIDAndBounceDomain, actiontype, refBlastIDs, suppressionList);
                scope.Complete();
            }

            return count;
        }

        public static int GetBlastUserByBlastID(int blastID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.Blast.GetBlastUserByBlastID(blastID);
                scope.Complete();
            }

            return count;
        }

        public static int GetSampleBlastUserBySampleID(int sampleID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.Blast.GetSampleBlastUserBySampleID(sampleID);
                scope.Complete();
            }

            return count;
        }

        //reports
        public static DataTable GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status, int? baseChannelID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastGridByStatus(status, baseChannelID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetGroupNamesByBlasts(int campaignItemID, int customerID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetGroupNamesByBlasts(campaignItemID, customerID);
                scope.Complete();
            }

            return dt;
        }

        public static DataSet GetBlastGroupClicksData(int? campaignItemID, int? blastID, string howMuch, string isp, string reportType, string topClickURL, int pageNo, int pageSize, string udfName, string udfData, string startDate = "", string endDate = "", bool unique = false)
        {
            DataSet ds = null;
            //getting the objects so that they can perform a security check
            if (campaignItemID != null && campaignItemID > 0)
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemID.Value, false);
            else
                ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID.Value, false);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ds = ECN_Framework_DataLayer.Communicator.Blast.GetBlastGroupClicksData(campaignItemID, blastID, howMuch, isp, reportType, topClickURL, pageNo, pageSize, udfName, udfData, startDate, endDate, unique);
                scope.Complete();
            }

            return ds;
        }

        public static DataTable ClickActivityDetailedReport(int blastID, int customerID, string linkURL, string profileFilter = "ProfilePlusStandalone")
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.ClickActivityDetailedReport(blastID, customerID, linkURL, profileFilter);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetBlastStatusByBlastID(int blastID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastStatusByBlastID(blastID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetBlastStatusByBlastEngineIDFinishTime(int BlastEngineID, DateTime FinishTime)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastStatusByBlastEngineIDFinishTime(BlastEngineID, FinishTime);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetBlastComparison(int customerID, DateTime startTime, DateTime endTime, int? userID, int? groupID, int? campaignID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastComparison(customerID, startTime, endTime, userID, groupID, campaignID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetEstimatedSendsCount(string XMLFormat, int customerID, bool ignoreSuppression = false)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetEstimatedSendsCount(XMLFormat, customerID, ignoreSuppression);
                scope.Complete();
            }

            return dt;
        }

        //for link tracking params
        public static string GetLinkTrackingParam(int blastID, string param)
        {
            string ret = string.Empty;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ret = ECN_Framework_DataLayer.Communicator.Blast.GetLinkTrackingParam(blastID, param).Replace(" ", "%20");
                scope.Complete();
            }

            return ret;
        }

        //Get info for abuse in activity
        public static DataTable GetBlastInfoForAbuseReporting(int blastID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetBlastInfoForAbuseReporting(blastID);
                scope.Complete();
            }

            return dt;
        }

        //Activity & Blast Engines code from ECN object
        //Link Rewriter
        public static string LinkReWriter(string text, ECN_Framework_Entities.Communicator.BlastAbstract blast, int customerId, string virtualPath, string hostName, string forwardFriend, KMPlatform.Entity.User user)
        {
            var body = text;
            var blastId = blast.BlastID;
            var emailFromAddress = blast.EmailFrom;
            var groupId = blast.GroupID.ToString();
            var customer = Accounts.Customer.GetByCustomerID(customerId, false);

            var groupName = string.Empty;
            try
            {
                groupName = Group.GetByGroupID(blast.GroupID.Value, user).GroupName;
            }
            catch
            {
                // TODO: Handle this exception properly
            }

            body = ReplaceCustomerInfo(body, customer, groupName);
            body = ReplaceHostname(hostName, body);
            body = ReplaceUnsubscribeLinksFromLinkTrack(body);
            body = ReplaceRedirectLinks(customerId, body, blastId);
            body = ReplaceUnsubscribeLink(customerId, body, blastId);
            body = ReplaceProfilePreferences(customerId, body, groupId, customer);
            body = ReplaceSubscriptionManagement(body, customer);
            body = ReplacePublicViewLink(body, blastId);
            body = ReplaceReportAbuseLink(customerId, body, blastId, groupId);
            body = ReplaceForwardFriend(blast, customerId, forwardFriend, user, body, blastId);
            body = ReplaceEmailToFriend(blast, hostName, user, body, blastId, emailFromAddress);

            return body;
        }

        private static string ReplaceCustomerInfo(string body, ECN_Framework_Entities.Accounts.Customer customer, string groupName)
        {
            var customerName = string.Empty;
            var customerWebAddress = string.Empty;
            var customerAddress = string.Empty;
            var customerUdf1 = string.Empty;
            var customerUdf2 = string.Empty;
            var customerUdf3 = string.Empty;
            var customerUdf4 = string.Empty;
            var customerUdf5 = string.Empty;

            if (customer != null)
            {
                customerName = customer.CustomerName;
                customerWebAddress = customer.WebAddress;
                customerAddress = $"{customer.Address.Trim()}, {customer.City.Trim()}, {customer.State.Trim()} - {customer.Zip.Trim()}";
                customerUdf1 = customer.customer_udf1;
                customerUdf2 = customer.customer_udf2;
                customerUdf3 = customer.customer_udf3;
                customerUdf4 = customer.customer_udf4;
                customerUdf5 = customer.customer_udf5;
            }

            body = StringFunctions.Replace(body, "%%groupname%%", groupName);
            body = StringFunctions.Replace(body, "%%customer_name%%", customerName);
            body = StringFunctions.Replace(body, "%%customer_address%%", customerAddress);
            body = StringFunctions.Replace(body, "%%customer_webaddress%%", customerWebAddress);
            body = StringFunctions.Replace(body, "%%customer_udf1%%", customerUdf1);
            body = StringFunctions.Replace(body, "%%customer_udf2%%", customerUdf2);
            body = StringFunctions.Replace(body, "%%customer_udf3%%", customerUdf3);
            body = StringFunctions.Replace(body, "%%customer_udf4%%", customerUdf4);
            body = StringFunctions.Replace(body, "%%customer_udf5%%", customerUdf5);
            return body;
        }

        private static string ReplaceHostname(string hostName, string body)
        {
            // set the hostname
            body = StringFunctions.Replace(body, "%%hostname%%", hostName);
            return body;
        }

        private static string ReplaceUnsubscribeLinksFromLinkTrack(string body)
        {
            //catch to remove unsub from link track 
            body = StringFunctions.Replace(body, "http://%%unsubscribelink%%/", "%%unsubscribelink%%");
            body = StringFunctions.Replace(body, "http://%%emailtofriend%%/", "%%emailtofriend%%");
            body = StringFunctions.Replace(body, "http://%%publicview%%/", "%%publicview%%");
            body = StringFunctions.Replace(body, "http://%%reportabuselink%%/", "%%reportabuselink%%");
            body = StringFunctions.Replace(body, "%%publicview%%/", "%%publicview%%");
            body = StringFunctions.Replace(body, "http://%%unsubscribelink%%", "%%unsubscribelink%%");
            body = StringFunctions.Replace(body, "http://%%emailtofriend%%", "%%emailtofriend%%");
            body = StringFunctions.Replace(body, "http://%%publicview%%", "%%publicview%%");
            body = StringFunctions.Replace(body, "http://%%reportabuselink%%", "%%reportabuselink%%");
            return body;
        }

        private static string ReplaceRedirectLinks(int customerId, string body, int blastId)
        {
            var redirectPage = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/linkfrom.aspx";
            if ((ConfigurationManager.AppSettings["OpenClick_UseOldSite"] ?? string.Empty).Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                redirectPage = $"{ConfigurationManager.AppSettings["MVCActivity_DomainPath"]}/Clicks/";
            }

            body = StringFunctions.Replace(body, "href=http://", $"href={redirectPage}?b={blastId}&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href=https://", $"href={redirectPage}?b={blastId}&e=%%EmailID%%&l=https://");
            body = StringFunctions.Replace(body, "href=\"http://", $"href=\"{redirectPage}?b={blastId}&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href=\"https://", $"href=\"{redirectPage}?b={blastId}&e=%%EmailID%%&l=https://");
            body = StringFunctions.Replace(body, "href='http://", $"href='{redirectPage}?b={blastId}&e=%%EmailID%%&l=http://");
            body = StringFunctions.Replace(body, "href='https://", $"href='{redirectPage}?b={blastId}&e=%%EmailID%%&l=https://");

            //rewrite mailto links If they have the Featre Turned ON
            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(customerId, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.TrackmailtoClickThru))
            {
                body = StringFunctions.Replace(body, "href=\"mailto:", $"href=\"{redirectPage}?b={blastId}&e=%%EmailID%%&l=mailto:");
                body = StringFunctions.Replace(body, "href='mailto:", $"href=\"{redirectPage}?b={blastId}&e=%%EmailID%%&l=mailto:");
                body = StringFunctions.Replace(body, "href=mailto:", $"href=\"{redirectPage}?b={blastId}&e=%%EmailID%%&l=mailto:");
            }

            return body;
        }

        private static string ReplaceUnsubscribeLink(int customerId, string body, int blastId)
        {
            //unsubscribe Link 
            var unSubscribePage = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/Unsubscribe.aspx?e=%%EmailAddress%%&g=%%GroupID%%&b={blastId}&c={customerId}&s=U&f=html";
            body = StringFunctions.Replace(body, "%%unsubscribelink%%", unSubscribePage);
            return body;
        }

        private static string ReplaceProfilePreferences(int customerId, string body, string groupId, ECN_Framework_Entities.Accounts.Customer customer)
        {
            var imgPath = $"{ConfigurationManager.AppSettings["Image_DomainPath"]}/channels/{customer.BaseChannelID.ToString()}/images/";

            //Email List Preferences Link [GROUPs]
            if (Accounts.Customer.HasProductFeature(customerId, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailListPreferences))
            {
                var profilepreferencespage = $"<a href='{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%&prefrence=list'>Manage my Subscriptions</a>&nbsp;";
                body = StringFunctions.Replace(body, "%%profilepreferences%%", profilepreferencespage);
            }
            else
            {
                body = StringFunctions.Replace(body, "%%profilepreferences%%", string.Empty);
            }

            //Email Profile Preferences Link [UDFs]
            if (Accounts.Customer.HasProductFeature(customerId, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailProfilePreferences))
            {
                var userprofilepreferencespage = $"<a href='{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%,{groupId}&prefrence=email'><img border=\"0\" src=\"{imgPath}/email_pref.gif\"></a>&nbsp;";
                body = StringFunctions.Replace(body, "%%userprofilepreferences%%", userprofilepreferencespage);
            }
            else
            {
                body = StringFunctions.Replace(body, "%%userprofilepreferences%%", string.Empty);
            }

            //Email List & Profile Preferences Link [GROUPs & UDFs]
            if (Accounts.Customer.HasProductFeature(customerId, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailListandProfilePreferences))
            {
                var listprofilepreferencespage = $"<a href='{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%&prefrence=both'><img border=\"0\" src=\"{imgPath}/list_email_pref.gif\"></a>&nbsp;";
                body = StringFunctions.Replace(body, "%%listprofilepreferences%%", listprofilepreferencespage);
            }
            else
            {
                body = StringFunctions.Replace(body, "%%listprofilepreferences%%", string.Empty);
            }

            return body;
        }

        private static string ReplaceSubscriptionManagement(string body, ECN_Framework_Entities.Accounts.Customer customer)
        {
            if (body.IndexOf("ECN.SUBSCRIPTIONMGMT.", StringComparison.Ordinal) > 0)
            {
                var regex = new Regex("ECN.SUBSCRIPTIONMGMT");
                var emailbody = regex.Split(body);
                for (var i = 0; i < emailbody.Length; i++)
                {
                    try
                    {
                        var lineData = emailbody.GetValue(i).ToString();
                        if (i % 2 != 0)
                        {
                            var tempSubMLink = lineData.Remove(lineData.Length - 1, 1).Remove(0, 1);
                            var subscriptionManagement = Accounts.SubscriptionManagement.GetByBaseChannelID(customer.BaseChannelID.Value)
                                .Find(x => x.Name == tempSubMLink);
                            var subManagementPage = $"<a href='{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/subscriptionmanagement.aspx?smid={subscriptionManagement.SubscriptionManagementID}&e=%%EmailAddress%%'>Subscription Management</a>";
                            body = body.Replace($"ECN.SUBSCRIPTIONMGMT.{tempSubMLink}.ECN.SUBSCRIPTIONMGMT", subManagementPage);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Handle this exception properly
                    }
                }
            }

            return body;
        }

        private static string ReplacePublicViewLink(string body, int blastId)
        {
            // Public View Link 
            var publicViewPage = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/publicPreview.aspx?blastID={blastId}&emailID=%%EmailID%%";
            body = StringFunctions.Replace(body, "%%publicview%%", publicViewPage);
            return body;
        }

        private static string ReplaceReportAbuseLink(int customerId, string body, int blastId, string groupId)
        {
            //Report Abuse Link
            var reportAbusePage = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/reportspam.aspx?p=%%EmailAddress%%,%%EmailID%%,{groupId},{customerId},{blastId}";
            body = StringFunctions.Replace(body, "%%reportabuselink%%", reportAbusePage);
            return body;
        }

        private static string ReplaceForwardFriend(ECN_Framework_Entities.Communicator.BlastAbstract blast, int customerId, string forwardFriend, KMPlatform.Entity.User user, string body, int blastId)
        {
            if (!string.IsNullOrWhiteSpace(forwardFriend))
            {
                var header = string.Empty;
                var f2fNotes = string.Empty;
                try
                {
                    var template = Accounts.CustomerTemplate.GetByTypeID(blast.CustomerID.Value, "F2FIntroEmailHdr", user);
                    header = template.HeaderSource;
                }
                catch
                {
                    var template = Accounts.CustomerTemplate.GetByTypeID(1, "F2FIntroEmailHdr", user);
                    header = template.HeaderSource;
                }

                //Subscribe to Group Link 
                var subscribeLink = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/subscribe.aspx?e=%%EmailAddress%%&g=%%GroupID%%&b={blastId}&c={customerId}&s=S&f=html";
                header = header.Replace("%%sub_link%%", subscribeLink);
                header = header.Replace("%%email_friend%%", forwardFriend);

                //Notes entered by the Person who's refering. Its in a session Variable "F2FNotes"
                try
                {
                    f2fNotes = System.Web.HttpContext.Current.Session["F2FNotes"].ToString();
                }
                catch
                {
                    f2fNotes = string.Empty;
                }
                header = header.Replace("%%F2FNotes%%", f2fNotes);

                body = header + body;
            }

            return body;
        }

        private static string ReplaceEmailToFriend(ECN_Framework_Entities.Communicator.BlastAbstract blast, string hostName, KMPlatform.Entity.User user, string body, int blastId, string emailFromAddress)
        {
            var companyAddress = string.Empty;
            try
            {
                companyAddress = Layout.GetByLayoutID(blast.LayoutID.Value, user, false).DisplayAddress;
            }
            catch
            {
                // TODO: Handle this exception properly
            }

            var emailToFriendPage = $"{ConfigurationManager.AppSettings["Activity_DomainPath"]}/engines/emailtofriend.aspx?e=%%EmailID%%&b={blastId}";
            body = StringFunctions.Replace(body, "%%emailtofriend%%", emailToFriendPage);
            body = StringFunctions.Replace(body, "%%hostname%%", hostName);
            body = StringFunctions.Replace(body, "%%company_address%%", companyAddress);
            body = StringFunctions.Replace(body, "%%EmailFromAddress%%", emailFromAddress);
            body = StringFunctions.Replace(body, "%%blast_start_time%%", blast.SendTime.ToString());
            body = StringFunctions.Replace(body, "%%blast_end_time%%", blast.FinishTime.ToString());
            body = StringFunctions.Replace(body, "%%blast_id%%", blastId.ToString());
            body = HTMLReplaceWithLinkID(body, blastId);
            return body;
        }

        private static String HTMLReplaceWithLinkID(string text, int blastId)
        {
            var linkList = ExtractLinksWithGuid(ref text);

            for (var aLoop = 0; aLoop < linkList.Count; aLoop++)
            {
                var blastLinkId = 0;
                var linkFound = linkList[aLoop];

                if (linkFound.link.IndexOf("&l=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    var blastLink = GetOrCreateBlastLink(blastId, linkFound, out blastLinkId);

                    AppendLinkId(blastLink, linkFound, blastId, blastLinkId);

                    text = text.Replace(linkFound.aToReplace, linkFound.originalA);
                }
            }

            return text;
        }

        private static List<LinkWithGuid> ExtractLinksWithGuid(ref string text)
        {
            var linkList = new List<LinkWithGuid>();
            var aIndex = 0;
            foreach (Match m in LinkRegex.Matches(text))
            {
                var ecn_ID = GetEcnId(m);
                var oTag = m.Groups[0].Value;
                var oLink = m.Groups[5].Value;
                var eTag = LinkRegex.Replace(oTag, ReplacePattern);
                var lFound = m.Groups[6].Value;

                try
                {
                    var href = new LinkWithGuid();
                    href.link = oLink;
                    if (!string.IsNullOrEmpty(ecn_ID))
                    {
                        href.Guid = ecn_ID;
                        var toReplace = oTag.Insert(0, aIndex.ToString());
                        text = text.Replace(oTag, toReplace);
                        href.aToReplace = toReplace;
                        href.originalA = eTag;
                    }
                    else
                    {
                        href.aToReplace = oTag;
                        href.originalA = eTag;
                    }

                    href.lFound = lFound;

                    if (!linkList.Contains(href))
                    {
                        linkList.Add(href);
                    }
                }
                catch
                {
                    // TODO: Fix this empty catch by adding logging or handling code
                }
                aIndex++;
            }

            return linkList;
        }

        private static ECN_Framework_Entities.Communicator.BlastLink GetOrCreateBlastLink(int blastId, LinkWithGuid linkFound, out int blastLinkId)
        {
            var blastLink = BlastLink.GetByLinkURL(blastId, linkFound.lFound);
            if (blastLink == null || blastLink.BlastLinkID <= 0)
            {
                blastLink = new ECN_Framework_Entities.Communicator.BlastLink
                {
                    BlastID = blastId,
                    LinkURL = linkFound.lFound
                };

                blastLinkId = BlastLink.Insert(blastLink);
            }
            else
            {
                blastLinkId = blastLink.BlastLinkID;
            }

            return blastLink;
        }

        private static void AppendLinkId(ECN_Framework_Entities.Communicator.BlastLink blastLink, LinkWithGuid linkFound, int blastId, int blastLinkId)
        {
            var uniqueLinkFound = false;
            var uniqueLinkId = 0;
            //Blastlink object exists, check for it with the url and guid
            //if null create a new uniquelink object for this blast link
            if (!string.IsNullOrWhiteSpace(linkFound.Guid))
            {
                blastLink = BlastLink.GetByLinkURL_ECNID(blastId, linkFound.lFound, linkFound.Guid);
                if (blastLink == null || blastLink.BlastLinkID <= 0)
                {
                    var uniqueLink = new ECN_Framework_Entities.Communicator.UniqueLink
                    {
                        BlastLinkID = blastLinkId,
                        UniqueID = linkFound.Guid,
                        BlastID = blastId
                    };
                    uniqueLinkId = UniqueLink.Save(uniqueLink);
                }
                else
                {
                    var uniqueLink = UniqueLink.GetByBlastID_UniqueID(blastId, linkFound.Guid);
                    uniqueLinkId = uniqueLink.UniqueLinkID;
                }
                uniqueLinkFound = true;
            }

            //If we have a uniqueLink then append the UniqueLinkID to the parameters, else just do the normal blastlink id append
            if (uniqueLinkFound)
            {
                var findText = "&l=" + linkFound.lFound;
                var replaceText = "&lid=" + blastLinkId.ToString() + "&ulid=" + uniqueLinkId + "&l=" + linkFound.lFound;
                linkFound.originalA = linkFound.originalA.Replace(findText, replaceText);
            }
            else if (!linkFound.link.Contains("&lid="))
            {
                var findText = "&l=" + linkFound.lFound;
                var replaceText = "&lid=" + blastLinkId.ToString() + "&l=" + linkFound.lFound;
                linkFound.originalA = linkFound.originalA.Replace(findText, replaceText);
            }
        }

        private static string GetEcnId(Match linkMatch)
        {
            var toSearch = String.Join(" ", (linkMatch.Groups[2].Value ?? string.Empty), (linkMatch.Groups[7].Value ?? string.Empty));
            var ecnIdMatch = EcnIdRegex.Match(toSearch);
            return ecnIdMatch.Success ? ecnIdMatch.Groups[2].Value : null;
        }

        public static DataTable GetHTMLPreview(int blastID, int emailID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.Blast.GetHTMLPreview(blastID, emailID);
                scope.Complete();
            }

            return dt;
        }

        //get list of sent blast ids by group id
        public static string GetSentByGroupID(int groupID)
        {
            string blastIDs = string.Empty;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                blastIDs = ECN_Framework_DataLayer.Communicator.Blast.GetSentByGroupID(groupID);
                scope.Complete();
            }
            return blastIDs;
        }
    }
    public class LinkWithGuid
    {
        public LinkWithGuid() { link = string.Empty; Guid = string.Empty; aToReplace = string.Empty; }
        public string link { get; set; }

        public string Guid { get; set; }

        public string aToReplace { get; set; }

        public string originalA { get; set; }

        public string lFound { get; set; }
    }
}
