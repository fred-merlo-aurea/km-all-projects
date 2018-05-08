using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using CommLayoutPlans = ECN_Framework_DataLayer.Communicator.LayoutPlans;
using KmCacheUtil = KM.Common.CacheUtil;
using LayoutPlansEntity = ECN_Framework_Entities.Communicator.LayoutPlans;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LayoutPlans
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LayoutPlans;
        private static readonly string LayoutPlan_Group_CacheName = "CACHE_LAYOUTPLAN_";
        private static readonly string LayoutPlan_RegionName = "LAYOUTPLAN";
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Trigger;

        private const string CacheTypeSf = "SF_";
        private const string CacheTypeForm = "Form_";

        //public static bool HasPermission(ECN_Framework_Common.Objects.Communicator.Enums.EntityRights rights, ECN_Framework_Entities.Accounts.User user)
        //{
        //    if (user.IsSysAdmin || user.IsChannelAdmin || user.IsAdmin)
        //        return true;
        //}


        public static ECN_Framework_Entities.Communicator.LayoutPlans GetByLayoutPlanID(int layoutPlanID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.LayoutPlans layoutPlans = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layoutPlans = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return layoutPlans;
        }

        public static ECN_Framework_Entities.Communicator.LayoutPlans GetByLayoutPlanID_UseAmbientTransaction(int layoutPlanID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.LayoutPlans layoutPlans = null;
            using (TransactionScope scope = new TransactionScope())
            {
                layoutPlans = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return layoutPlans;
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByGroupID(int groupID, int CustomerID, KMPlatform.Entity.User user)
        {
            string cacheKey = LayoutPlan_Group_CacheName + "GROUP_" + groupID.ToString();


            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.LayoutPlans> layoutPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();
            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByGroupID(groupID, CustomerID);
                    scope.Complete();
                }
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByGroupID(groupID, CustomerID);
                    scope.Complete();
                }
                KM.Common.CacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName);
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlansList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return layoutPlansList;
        }
        
        public static List<LayoutPlansEntity> GetByFormTokenUID_NoAccessCheck(Guid tokenUid)
        {
            return CreateLayoutPlansList(null, null, CacheTypeForm, tokenUid) as List<LayoutPlansEntity>;
        }
        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByGroupID_NoAccessCheck(int groupID, int CustomerID)
        {
            string cacheKey = LayoutPlan_Group_CacheName + "GROUP_" + groupID.ToString();

            List<ECN_Framework_Entities.Communicator.LayoutPlans> layoutPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();
            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByGroupID(groupID, CustomerID);
                    scope.Complete();
                }
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByGroupID(groupID, CustomerID);
                    scope.Complete();
                }
                KM.Common.CacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName);
            }

            return layoutPlansList;
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByLayoutID(int LayoutID, KMPlatform.Entity.User user, bool useCache = true)
        {
            string cacheKey = LayoutPlan_Group_CacheName + "LAYOUT_" + LayoutID.ToString();

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.LayoutPlans> layoutPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();
            if (!KM.Common.CacheUtil.IsCacheEnabled() || !useCache)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByLayoutID(LayoutID, user.CustomerID);
                    scope.Complete();
                }
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByLayoutID(LayoutID, user.CustomerID);
                    scope.Complete();
                }
                KM.Common.CacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName);
            }

            return layoutPlansList;
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByType(int layoutID, int customerID, string eventType, KMPlatform.Entity.User user)
        {
            string cacheKey = LayoutPlan_Group_CacheName + "TYPE_" + layoutID.ToString() + "_" + eventType;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.LayoutPlans> layoutPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();

            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByType(layoutID, customerID, eventType);
                    scope.Complete();
                }
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByType(layoutID, customerID, eventType);
                    scope.Complete();
                }
                KM.Common.CacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName);
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlansList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return layoutPlansList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByType_NoAccessCheck(int layoutID, int customerID, string eventType)
        {
            string cacheKey = LayoutPlan_Group_CacheName + "TYPE_" + layoutID.ToString() + "_" + eventType;

            List<ECN_Framework_Entities.Communicator.LayoutPlans> layoutPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();

            if (!KM.Common.CacheUtil.IsCacheEnabled())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByType(layoutID, customerID, eventType);
                    scope.Complete();
                }
            }
            else if (KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetByType(layoutID, customerID, eventType);
                    scope.Complete();
                }
                KM.Common.CacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = (List<ECN_Framework_Entities.Communicator.LayoutPlans>)KM.Common.CacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName);
            }

            return layoutPlansList;
        }

        public static List<LayoutPlansEntity> GetBySmartFormID(int smartFormId, int customerId, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException();
            }

            var layoutPlansList = CreateLayoutPlansList(smartFormId, customerId, CacheTypeSf, null);
            if (!AccessCheck.CanAccessByCustomer(layoutPlansList, user))
            {
                throw new SecurityException();
            }

            return layoutPlansList as List<LayoutPlansEntity>;
        }

        public static List<LayoutPlansEntity> GetBySmartFormID_NoAccessCheck(int smartFormId, int customerId)
        {
            return CreateLayoutPlansList(smartFormId, customerId, CacheTypeSf, null) as List<LayoutPlansEntity>;
        }

        public static IList<LayoutPlansEntity> CreateLayoutPlansList(
            int? smartFormId, 
            int? customerId, 
            string cacheType, 
            Guid? token)
        {
            var cacheKey = cacheType.Equals(CacheTypeSf) 
                ? $"{LayoutPlan_Group_CacheName}{CacheTypeSf}{smartFormId}" 
                : $"{LayoutPlan_Group_CacheName}{CacheTypeForm}{token}";
            
            var layoutPlansList = new List<LayoutPlansEntity>();

            if (!KmCacheUtil.IsCacheEnabled())
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = token == null
                        ? CommLayoutPlans.GetBySmartFormID(smartFormId.Value, customerId.Value)
                        : CommLayoutPlans.GetByFormTokenUID(token.Value);
                    scope.Complete();
                }
            }
            else if (KmCacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) == null)
            {
                using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    layoutPlansList = token == null
                        ? CommLayoutPlans.GetBySmartFormID(smartFormId.Value, customerId.Value)
                        : CommLayoutPlans.GetByFormTokenUID(token.Value);
                }
                KmCacheUtil.AddToCache(cacheKey, layoutPlansList, LayoutPlan_RegionName);
            }
            else
            {
                layoutPlansList = KmCacheUtil.GetFromCache(cacheKey, LayoutPlan_RegionName) as List<LayoutPlansEntity>;
            }

            return layoutPlansList;
        }

        public static bool Exists(int layoutPlanID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LayoutPlans.Exists(layoutPlanID, customerID);
                scope.Complete();
            }
            return exists;
        }
        public static bool Exists(int groupID, string criteria)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LayoutPlans.Exists(groupID, criteria);
                scope.Complete();
            }
            return exists;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan)
        {
            var layoutPlansValidate = new LayoutPlansValidate();
            layoutPlansValidate.Validate(layoutPlan);
        }

        public static void Validate_UseAmbientTransaction(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan)
        {
            var layoutPlansValidate = new LayoutPlansValidateUseAmbient();
            layoutPlansValidate.Validate(layoutPlan);
        }

        public static int Save(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlans, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            Validate(layoutPlans);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                layoutPlans.LayoutPlanID = ECN_Framework_DataLayer.Communicator.LayoutPlans.Save(layoutPlans);
                scope.Complete();
            }
            ClearCacheForLayoutPlan(layoutPlans);

            return layoutPlans.LayoutPlanID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlans, KMPlatform.Entity.User user)
        {

            Validate_UseAmbientTransaction(layoutPlans);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();


            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layoutPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                layoutPlans.LayoutPlanID = ECN_Framework_DataLayer.Communicator.LayoutPlans.Save(layoutPlans);
                scope.Complete();
            }
            ClearCacheForLayoutPlan(layoutPlans);

            return layoutPlans.LayoutPlanID;
        }

        public static void Delete(int layoutPlanID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();
            
            ECN_Framework_Entities.Communicator.LayoutPlans lpToDelete = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID, user);
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();

            if (lpToDelete.Criteria.ToUpper().Equals("S"))
            {
                checkList.AddRange(ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(layoutPlanID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe));
            }
            else if (lpToDelete.Criteria.ToUpper().Equals("U"))
            {
                checkList.AddRange(ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(layoutPlanID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe));
            }

            if (checkList.Count > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Group Trigger is used in Marketing Automation(s). Deleting is not allowed."));
                throw new ECNException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LayoutPlans.Delete(layoutPlanID, user.CustomerID, user.UserID);
                scope.Complete();
            }
            ClearCacheForLayoutPlan(lpToDelete);
        }

        public static void Delete(int layoutID, int layoutPlanID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.LayoutPlans lpToDelete = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID, user);

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();
            if (lpToDelete.EventType.ToLower().Equals("click"))
            {
                checkList.AddRange(ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(layoutPlanID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click));
            }
            else if (lpToDelete.EventType.ToLower().Equals("open"))
            {
                checkList.AddRange(ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(layoutPlanID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open));
            }            

            if (checkList.Count > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Trigger is used in Marketing Automation(s). Deleting is not allowed."));
                throw new ECNException(errorList);
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LayoutPlans.Delete(layoutID, layoutPlanID, user.CustomerID, user.UserID);
                scope.Complete();
            }
            ClearCacheForLayoutPlan(lpToDelete);
        }

        public static void DeleteByLayoutPlanID(int layoutPlanID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.LayoutPlans lpToDelete = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByLayoutPlanID(layoutPlanID, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.LayoutPlans.Delete(layoutPlanID, user.UserID);
                scope.Complete();
            }
            ClearCacheForLayoutPlan(lpToDelete);
        }


        public static List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> GetGroupLayoutPlanSummary(int customerID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> planSummaryList = new List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary>();
            ECN_Framework_Entities.Communicator.View.LayoutPlanSummary planSummary = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetGroupLayoutPlanSummary(customerID);
                foreach (DataRow row in dt.Rows)
                {
                    planSummary = new ECN_Framework_Entities.Communicator.View.LayoutPlanSummary(Convert.ToInt32(row["GroupID"]), Convert.ToString(row["GroupName"]), Convert.ToInt32(row["TriggerCount"]));
                    planSummaryList.Add(planSummary);
                }
                scope.Complete();
            }

            return planSummaryList;
        }

        public static List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> GetCampaignLayoutPlanSummary(int customerID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;
            List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary> planSummaryList = new List<ECN_Framework_Entities.Communicator.View.LayoutPlanSummary>();
            ECN_Framework_Entities.Communicator.View.LayoutPlanSummary planSummary = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.LayoutPlans.GetCampaignLayoutPlanSummary(customerID);
                foreach (DataRow row in dt.Rows)
                {
                    planSummary = new ECN_Framework_Entities.Communicator.View.LayoutPlanSummary(Convert.ToInt32(row["LayoutID"]), Convert.ToString(row["LayoutName"]), Convert.ToInt32(row["TriggerCount"]));
                    planSummaryList.Add(planSummary);
                }
                scope.Complete();
            }

            return planSummaryList;
        }

        public static void ClearCacheForLayoutPlan(ECN_Framework_Entities.Communicator.LayoutPlans lp)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string cacheKey = "";
                if (lp.SmartFormID.HasValue && lp.SmartFormID.Value > 0)
                {
                    cacheKey = LayoutPlan_Group_CacheName + "SF_" + lp.SmartFormID.Value.ToString();
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                }
                if (lp.GroupID.HasValue && lp.GroupID > 0)
                {
                    cacheKey = LayoutPlan_Group_CacheName + "GROUP_" + lp.GroupID.Value.ToString();
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                }
                else if (lp.LayoutID.HasValue && lp.LayoutID.Value > 0)
                {
                    cacheKey = LayoutPlan_Group_CacheName + "TYPE_" + lp.LayoutID.Value.ToString() + "_click";
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                    cacheKey = LayoutPlan_Group_CacheName + "TYPE_" + lp.LayoutID.Value.ToString() + "_open";
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                    cacheKey = LayoutPlan_Group_CacheName + "TYPE_" + lp.LayoutID.Value.ToString() + "_subscribe";
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                    cacheKey = LayoutPlan_Group_CacheName + "LAYOUT_" + lp.LayoutID.Value.ToString();
                    KM.Common.CacheUtil.RemoveFromCache(cacheKey, LayoutPlan_RegionName);
                }

                scope.Complete();
            }
        }
    }
}
