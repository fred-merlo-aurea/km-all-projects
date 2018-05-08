using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using CommBlastSingle = ECN_Framework_Entities.Communicator.BlastSingle;
using CommBlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using PlatformUser = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastSingle
    {
        private const string NoOpen = "noopen";
        private const string TriggerPlanInvalid = "TriggerPlanID is invalid";
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastSingle;

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
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Regular_Blast.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        public static bool ExistsByBlastID(int blastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastSingle.ExistsByBlastID(blastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByBlastSingleID(int blastSingleID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastSingle.ExistsByBlastSingleID(blastSingleID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByBlastEmailLayoutPlan(int blastID, int emailID, int layoutPlanID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastSingle.ExistsByBlastEmailLayoutPlan(blastID, emailID, layoutPlanID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static int GetRefBlastID(int blastID, int emailID, int customerID, string BlastType)
        {
            int refBlastID = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                refBlastID = ECN_Framework_DataLayer.Communicator.BlastSingle.GetRefBlastID(blastID, emailID, customerID, BlastType);
                scope.Complete();
            }
            return refBlastID;
        }

        public static int GetRefBlastID_ByBlastID(int blastID)
        {
            int refBlastID = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                refBlastID = ECN_Framework_DataLayer.Communicator.BlastSingle.GetRefBlastID_ByBlastID(blastID);
                scope.Complete();
            }
            return refBlastID;
        }
        public static DataTable DownloadEmailLayoutPlanID_Processed(int LayoutPlanID,string Processed, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            DataTable dtBlastSingleEmails = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtBlastSingleEmails = ECN_Framework_DataLayer.Communicator.BlastSingle.DownloadEmailLayoutPlanID_Processed(LayoutPlanID, Processed);
                scope.Complete();
            }

            return dtBlastSingleEmails;
        }


        public static DateTime GetCancelDate_ByLayoutID(int LayoutPlanID)
        {
            DateTime cancelDate =DateTime.MinValue;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                cancelDate = ECN_Framework_DataLayer.Communicator.BlastSingle.GetCancelDate_ByLayoutID(LayoutPlanID);
                scope.Complete();
            }
            return cancelDate;
        }
        public static DateTime GetCancelDate_ByTriggerPlan(int TriggerPlanID, int blastID)
        {
            DateTime cancelDate = DateTime.MinValue;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                cancelDate = ECN_Framework_DataLayer.Communicator.BlastSingle.GetCancelDate_ByTriggerPlan(TriggerPlanID,blastID);
                scope.Complete();
            }
            return cancelDate;
        }
        public static int Insert(ECN_Framework_Entities.Communicator.BlastSingle blastSingle, KMPlatform.Entity.User user)
        {
            Validate(blastSingle, user);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(blastSingle,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                blastSingle.BlastSingleID = ECN_Framework_DataLayer.Communicator.BlastSingle.Insert(blastSingle);
                scope.Complete();
            }

            return blastSingle.BlastSingleID;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static int Insert_NoAccessCheck(ECN_Framework_Entities.Communicator.BlastSingle blastSingle)
        {
            Validate_NoAccessCheck(blastSingle);

            using (TransactionScope scope = new TransactionScope())
            {
                blastSingle.BlastSingleID = ECN_Framework_DataLayer.Communicator.BlastSingle.Insert(blastSingle);
                scope.Complete();
            }

            return blastSingle.BlastSingleID;
        }

        public static void Validate(CommBlastSingle blastSingle, PlatformUser user)
        {
            ValidateBlastSingle(blastSingle, user);
        }

        private static void ValidateBlastSingle(CommBlastSingle blastSingle, PlatformUser user = null)
        {
            Enums.Method Method = Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            CommBlastAbstract blastAbstract;

            if (user == null)
            {
                blastAbstract = Blast.GetByBlastID_NoAccessCheck(blastSingle.BlastID.Value, false);
            }
            else
            {
                blastAbstract = Blast.GetByBlastID(blastSingle.BlastID.Value, user, false);
            }

            if (blastSingle.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.CustomerID)} is invalid"));
            }
            else
            {
                if (blastSingle.BlastID == null || (!Blast.Exists(blastSingle.BlastID.Value, blastSingle.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.BlastID)} is invalid"));
                }
                if (blastSingle.EmailID == null)
                {
                    errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.CustomerID)} is invalid"));
                }
                else if (!Email.Exists(blastSingle.EmailID.Value, blastSingle.CustomerID.Value))
                {
                    //check if email has been merged
                    int mergedEmailID = EmailHistory.FindMergedEmailID(blastSingle.EmailID.Value);
                    if (mergedEmailID > 0)
                    {
                        blastSingle.EmailID = mergedEmailID;
                    }
                    else
                    {

                        errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.EmailID)} no longer exists"));
                    }
                }
                if (blastAbstract != null && !blastAbstract.BlastType.ToLower().Equals(NoOpen))
                {
                    if (blastSingle.LayoutPlanID == null || (!LayoutPlans.Exists(blastSingle.LayoutPlanID.Value, blastSingle.CustomerID.Value)))
                    {
                        errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.LayoutPlanID)} is invalid"));
                    }
                }
                else if (blastAbstract != null && blastAbstract.BlastType.ToLower().Equals(NoOpen))
                {
                    if (blastSingle.LayoutPlanID == null || (!TriggerPlans.Exists(blastSingle.LayoutPlanID.Value, blastSingle.CustomerID.Value)))
                    {
                        errorList.Add(new ECNError(Entity, Method, TriggerPlanInvalid));
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.BlastID)} is invalid"));
                }

                if (blastSingle.RefBlastID == null || (!Blast.Exists(blastSingle.RefBlastID.Value, blastSingle.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.RefBlastID)} is invalid"));
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!Accounts.Customer.Exists(blastSingle.CustomerID.Value))
                    {
                        errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.CustomerID)} is invalid"));
                    }

                    if (user != null)
                    {
                        if (blastSingle.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(blastSingle.CreatedUserID.Value, blastSingle.CustomerID.Value))
                        {
                            errorList.Add(new ECNError(Entity, Method, $"{nameof(blastSingle.CreatedUserID)} is invalid"));
                        }
                    }
                    scope.Complete();
                }
            }
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static void Validate_NoAccessCheck(CommBlastSingle blastSingle)
        {
            ValidateBlastSingle(blastSingle);
        }

        public static void DeleteForLayoutPlanID(int LayoutPlanID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.DeleteForLayoutPlan(LayoutPlanID,user.UserID);
                scope.Complete();
            }
        }
        public static void DeleteByEmailID(int EmailID, int LayoutPlanID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.DeleteByEmailID(EmailID, LayoutPlanID, user.UserID);
                scope.Complete();
            }
        }
        public static void DeleteByEmailID_NoAccessCheck(int EmailID, int LayoutPlanID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.DeleteByEmailID(EmailID, LayoutPlanID, user.UserID);
                scope.Complete();
            }
        }

        public static void DeleteNoOpenFromAbandon_NoAccessCheck(int EmailID, int LayoutPlanID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.DeleteNoOpenFromAbandon_EmailID(EmailID, LayoutPlanID, user.UserID);
                scope.Complete();
            }
        }
        public static void Pause_UnPause_ForLayoutPlanID(int LayoutPlanID,bool isPause, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.Pause_UnPause_ForLayoutPlan(LayoutPlanID, isPause);
                scope.Complete();
            }
        }

        public static void Pause_Unpause_ForTriggerPlanID(int TriggerPlanID, bool isPause, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.Pause_UnPause_ForTriggerPlan(TriggerPlanID, isPause);
                scope.Complete();
            }
        }

        public static void DeleteForTriggerPlan(int TriggerPlanID, int blastID, KMPlatform.Entity.User user)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.BlastSingle.DeleteForTriggerPlan(TriggerPlanID, blastID,user.UserID);
                scope.Complete();
            }
        }
    }
}
