using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using CommTriggerPlans = ECN_Framework_Entities.Communicator.TriggerPlans;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class TriggerPlans
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Trigger;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.TriggerPlans;

        public static bool Exists(int triggerPlanID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.TriggerPlans.Exists(triggerPlanID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.TriggerPlans GetByTriggerPlanID(int triggerPlanID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.TriggerPlans triggerPlans = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                triggerPlans = ECN_Framework_DataLayer.Communicator.TriggerPlans.GetByTriggerPlanID(triggerPlanID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(triggerPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return triggerPlans;
        }

        public static ECN_Framework_Entities.Communicator.TriggerPlans GetByRefTriggerID(int refTriggerID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.TriggerPlans triggerPlans = new ECN_Framework_Entities.Communicator.TriggerPlans();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                triggerPlans = ECN_Framework_DataLayer.Communicator.TriggerPlans.GetByRefTriggerID(refTriggerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(triggerPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return triggerPlans;
        }

        public static List<ECN_Framework_Entities.Communicator.TriggerPlans> GetNoOpenByBlastID(int blastID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.TriggerPlans> triggerPlansList = new List<ECN_Framework_Entities.Communicator.TriggerPlans>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                triggerPlansList = ECN_Framework_DataLayer.Communicator.TriggerPlans.GetNoOpenByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(triggerPlansList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return triggerPlansList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.TriggerPlans> GetNoOpenByBlastID_NoAccessCheck(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.TriggerPlans> triggerPlansList = new List<ECN_Framework_Entities.Communicator.TriggerPlans>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                triggerPlansList = ECN_Framework_DataLayer.Communicator.TriggerPlans.GetNoOpenByBlastID(blastID);
                scope.Complete();
            }
            return triggerPlansList;
        }

        public static void Validate(CommTriggerPlans triggerPlan)
        {
            ValidateTriggerPlan(triggerPlan, false);
        }

        private static void ValidateTriggerPlan(CommTriggerPlans triggerPlan, bool useAmbient)
        {
            if (triggerPlan == null)
            {
                throw new ArgumentNullException(nameof(triggerPlan));
            }

            var method = Enums.Method.Validate;
            var errorList = new List<ECNError>();

            if (triggerPlan.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.CustomerID)} is invalid"));
            }
            else
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!Accounts.Customer.Exists(triggerPlan.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.CustomerID)} is invalid"));
                    }

                    if (triggerPlan.CreatedUserID == null || (triggerPlan.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(triggerPlan.CreatedUserID.Value, false))))
                    {
                        if (triggerPlan.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(triggerPlan.CreatedUserID.Value, triggerPlan.CustomerID.Value)))
                        {
                            errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.CreatedUserID)} is invalid"));
                        }
                    }

                    if (triggerPlan.TriggerPlanID > 0 && (triggerPlan.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(triggerPlan.UpdatedUserID.Value, false))))
                    {
                        if (triggerPlan.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(triggerPlan.UpdatedUserID.Value, triggerPlan.CustomerID.Value)))
                        {
                            errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.UpdatedUserID)} is invalid"));
                        }
                    }
                    scope.Complete();
                }

                if (triggerPlan.EventType.Trim() == string.Empty)
                {
                    errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.EventType)} cannot be empty"));
                }

                if (useAmbient)
                {
                    if (triggerPlan.BlastID == null || !Blast.Exists_UseAmbientTransaction(triggerPlan.BlastID.Value, triggerPlan.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.BlastID)} is invalid"));
                    }
                }
                else
                {
                    if (triggerPlan.BlastID == null || !Blast.Exists(triggerPlan.BlastID.Value, triggerPlan.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.BlastID)} is invalid"));
                    }
                }

                if (triggerPlan.refTriggerID == null)
                {
                    errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.refTriggerID)} is invalid"));
                }

                if (triggerPlan.Period == null)
                {
                    errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.Period)} is invalid"));
                }

                if (triggerPlan.ActionName.Trim().Length == 0)
                {
                    errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.ActionName)} is invalid"));
                }

                if (useAmbient)
                {
                    if (triggerPlan.GroupID != null && triggerPlan.GroupID != 0 && (!Group.Exists_UseAmbientTransaction(triggerPlan.GroupID.Value, triggerPlan.CustomerID.Value)))
                    {
                        errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.GroupID)} is invalid"));
                    }
                }
                else
                {
                    if (triggerPlan.GroupID != null && triggerPlan.GroupID != 0 && (!Group.Exists(triggerPlan.GroupID.Value, triggerPlan.CustomerID.Value)))
                    {
                        errorList.Add(new ECNError(Entity, method, $"{nameof(triggerPlan.GroupID)} is invalid"));
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_UseAmbientTransaction(CommTriggerPlans triggerPlan)
        {
            ValidateTriggerPlan(triggerPlan, true);
        }

        public static int Save(ECN_Framework_Entities.Communicator.TriggerPlans triggerPlans, KMPlatform.Entity.User user)
        {
            Validate(triggerPlans);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(triggerPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                triggerPlans.TriggerPlanID = ECN_Framework_DataLayer.Communicator.TriggerPlans.Save(triggerPlans);
                scope.Complete();
            }
            return triggerPlans.TriggerPlanID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.TriggerPlans triggerPlans, KMPlatform.Entity.User user)
        {
            Validate_UseAmbientTransaction(triggerPlans);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(triggerPlans, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                triggerPlans.TriggerPlanID = ECN_Framework_DataLayer.Communicator.TriggerPlans.Save(triggerPlans);
                scope.Complete();
            }
            return triggerPlans.TriggerPlanID;
        }

        public static void Delete(int triggerPlanID, KMPlatform.Entity.User user)
        {
            
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            List<ECN_Framework_Entities.Communicator.MarketingAutomation> checkList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();

            checkList.AddRange(ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(triggerPlanID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen));


            if (checkList.Count > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Trigger is used in Marketing Automation(s). Deleting is not allowed."));
                throw new ECNException(errorList);
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.TriggerPlans.Delete(triggerPlanID, user.CustomerID, user.UserID);
                scope.Complete();
            }

        }
    }
}
