using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemTestBlast
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTestBlast;

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
            else if(AccessCode == KMPlatform.Enums.Access.Delete)
            {
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                    return true;
            }
            return false;
        }

        public static bool Exists(int campaignItemID, int CampaignItemTestBlastID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.Exists(campaignItemID, CampaignItemTestBlastID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast, KMPlatform.Entity.User user, bool isQTB = false)
        {
            Validate(ciBlast, user, isQTB);

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //Commenting out transactionscope to see if it fixes my issue
            //using (TransactionScope scope = new TransactionScope())
            //{
                ciBlast.CampaignItemTestBlastID = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.Save(ciBlast);
                if(ciBlast.Filters != null && ciBlast.Filters.Count > 0)
                {
                    foreach(ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters)
                    {
                        cibf.CampaignItemBlastFilterID = -1;
                        cibf.CampaignItemTestBlastID = ciBlast.CampaignItemTestBlastID;
                        cibf.CampaignItemBlastID = null;
                        cibf.CampaignItemSuppresionID = null;
                        CampaignItemBlastFilter.Save(cibf);
                    }
                }
                //create test blast
                if (isQTB)
                    Blast.CreateBlastsFromQuickTestBlast(ciBlast.CampaignItemTestBlastID, user);
                else
                    Blast.CreateBlastsFromCampaignItemTestBlast(ciBlast.CampaignItemTestBlastID, user);

            //    scope.Complete();
            //}

            return ciBlast.CampaignItemTestBlastID;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTestBlast, KMPlatform.Entity.User user, bool isQTB = false)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (ciTestBlast.CampaignItemTestBlastID > 0)
                errorList.Add(new ECNError(Entity, Method, "Cannot update a test blast"));

            if (ciTestBlast.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                if (ciTestBlast.CampaignItemID == null)
                    errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                else
                {
                    ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(ciTestBlast.CampaignItemID.Value, user, false);
                    if (item == null || !ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, ciTestBlast.CampaignItemID.Value, ciTestBlast.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemID is invalid"));
                    else
                    {
                        if (ciTestBlast.BlastID != null)
                            errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItemTestBlast"));
                        else
                        {
                            if (ciTestBlast.GroupID != null)
                            {
                                if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(ciTestBlast.GroupID.Value, ciTestBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                                else
                                {
                                    System.Collections.Generic.List<string> listLY = null;
                                    if (!isQTB)
                                    {
                                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, false);

                                        if (ciBlastList[0].LayoutID != null)
                                        {
                                            if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(ciBlastList[0].LayoutID.Value, ciBlastList[0].CustomerID.Value))
                                                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                                            else
                                            {
                                                listLY = Layout.ValidateLayoutContent(ciBlastList[0].LayoutID.Value);
                                                Group.ValidateDynamicTags(ciTestBlast.GroupID.Value, ciBlastList[0].LayoutID.Value, user);
                                            }
                                        }
                                        if (listLY == null)
                                            listLY = new System.Collections.Generic.List<string>();
                                        if (ciBlastList[0].DynamicFromName.Trim().Length > 0)
                                        {
                                            listLY.Add(ciBlastList[0].DynamicFromName.Trim().ToLower());
                                        }
                                        if (ciBlastList[0].DynamicFromEmail.Trim().Length > 0)
                                        {
                                            listLY.Add(ciBlastList[0].DynamicFromEmail.Trim().ToLower());
                                        }
                                        if (ciBlastList[0].DynamicReplyTo.Trim().Length > 0)
                                        {
                                            listLY.Add(ciBlastList[0].DynamicReplyTo.Trim().ToLower());
                                        }
                                        if (ciBlastList[0].EmailSubject.Trim().Length > 0)
                                        {
                                            listLY.Add(ciBlastList[0].EmailSubject.Trim().ToLower());
                                        }
                                        Group.ValidateDynamicStrings(listLY, ciTestBlast.GroupID.Value, user);
                                    }
                                    else
                                    {
                                        if (ciTestBlast.LayoutID != null)
                                        {
                                            if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(ciTestBlast.LayoutID.Value, ciTestBlast.CustomerID.Value))
                                                errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                                            else
                                            {
                                                listLY = Layout.ValidateLayoutContent(ciTestBlast.LayoutID.Value);
                                                Group.ValidateDynamicTags(ciTestBlast.GroupID.Value, ciTestBlast.LayoutID.Value, user);
                                            }
                                        }
                                        if (listLY == null)
                                            listLY = new System.Collections.Generic.List<string>();
                                        
                                        Group.ValidateDynamicStrings(listLY, ciTestBlast.GroupID.Value, user);
                                    }
                                }
                            }
                            else
                            {
                                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
                            }

                            if (ciTestBlast.HasEmailPreview == null)
                                errorList.Add(new ECNError(Entity, Method, "HasEmailPreview is invalid"));

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(ciTestBlast.CustomerID.Value))
                                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                                if (ciTestBlast.CreatedUserID == null || (ciTestBlast.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(user)))
                                {
                                    if (ciTestBlast.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(ciTestBlast.CreatedUserID.Value, ciTestBlast.CustomerID.Value))
                                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                                }
                                scope.Complete();
                            }
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }        

        public static ECN_Framework_Entities.Communicator.CampaignItemTestBlast GetByCampaignItemTestBlastID(int CampaignItemTestBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemTestBlastID(CampaignItemTestBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);
            }

            return ciBlast;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemTestBlast GetByBlastID(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlast, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);
                ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemTestBlastID(ciBlast.CampaignItemTestBlastID);
            }

            return ciBlast;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemTestBlast GetByBlastID_NoAccessCheck(int blastID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlast = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByBlastID(blastID);
                scope.Complete();
            }

            if (ciBlast != null && getChildren)
            {
                ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck(ciBlast.BlastID.Value,  getChildren);
                ciBlast.Filters = CampaignItemBlastFilter.GetByCampaignItemTestBlastID(ciBlast.CampaignItemTestBlastID);
            }

            return ciBlast;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID(ciBlast.BlastID.Value, user, getChildren);                        
                    }
                }
            }

            return ciBlastList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetByCampaignItemID_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(ciBlastList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_UseAmbientTransaction(ciBlast.BlastID.Value, user, getChildren);
                    }
                }
            }

            return ciBlastList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetByCampaignItemID_NoAccessCheck(int campaignItemID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck(ciBlast.BlastID.Value,getChildren);
                    }
                }
            }

            return ciBlastList;
        }
        public static List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(int campaignItemID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast>();
            using (TransactionScope scope = new TransactionScope())
            {
                ciBlastList = ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (ciBlastList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast in ciBlastList)
                {
                    if (ciBlast.BlastID != null)
                    {
                        ciBlast.Blast = Blast.GetByBlastID_NoAccessCheck_UseAmbientTransaction(ciBlast.BlastID.Value, getChildren);
                    }
                }
            }

            return ciBlastList;
        }

        public static void Delete(int campaignItemID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> ciBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(campaignItemID, user, false);

            var blastExists = ciBlastList.Where(x => x.BlastID != null);
            if (blastExists.Any())
            {
                List<ECNError> errorList = new List<ECNError>();
                errorList.Add(new ECNError(Entity, Method, "Cannot delete CampaignItemTestBlast as Blasts have been created"));
                throw new ECNException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.Delete(campaignItemID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int campaignItemID, int CampaignItemTestBlastID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (!HasPermission(KMPlatform.Enums.Access.Delete, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemTestBlastID(CampaignItemTestBlastID, user, false);

            if (ciBlast != null)
            {
                if (ciBlast.BlastID != null)
                {
                    errorList.Add(new ECNError(Entity, Method, "Cannot delete CampaignItemTestBlast as Blasts have been created"));
                    throw new ECNException(errorList);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.Delete(campaignItemID, CampaignItemTestBlastID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
            }
            else
            {                
                errorList.Add(new ECNError(Entity, Method, "Item does not exist"));
                throw new ECNException(errorList);
            }
        }

        internal static void UpdateBlastID(int CampaignItemTestBlastID, int blastID, int userID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTestBlast.UpdateBlastID(CampaignItemTestBlastID, blastID, userID);
                scope.Complete();
            }
        }

    }
}

