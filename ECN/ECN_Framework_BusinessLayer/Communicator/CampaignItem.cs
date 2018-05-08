using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItem
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItem;

        public static int Count(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            int count = 0;

            string sSuppressionList = string.Empty;

            foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in item.SuppressionList)
            {
                sSuppressionList += (sSuppressionList == string.Empty ? cis.GroupID.Value.ToString() : "," + cis.GroupID.Value.ToString());
            }

            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in item.BlastList)
            {

            }

            return count;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (item.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {

                if (item.CampaignID == null || !ECN_Framework_BusinessLayer.Communicator.Campaign.Exists(item.CampaignID.Value, item.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "CampaignID is invalid"));
                }
                else
                {
                    if (item.CampaignItemID > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItem as there are Sent or Active Blasts"));
                            break;
                        }
                    }

                    if (item.CampaignItemIDOriginal != null && (!ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, item.CampaignItemIDOriginal.Value, item.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemIDOriginal is invalid"));

                    if (item.CampaignItemName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemName cannot be empty"));
                    else
                        if (item.CampaignItemID <= 0 && Exists(item.CampaignItemName, item.CampaignID.Value, item.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CampaignItemName already exists"));

                    //wgh - was failing with scheduled blasts when creating new
                    //if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(item.CampaignItemName))
                    //    errorList.Add(new ECNError(Entity, Method, "CampaignItemName has invalid characters"));
                }
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(item.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    if (item.CampaignItemID <= 0 && (item.CreatedUserID == null || (item.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(item.CreatedUserID.Value, false)))))
                    {
                        if (item.CampaignItemID <= 0 && (item.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(item.CreatedUserID.Value, item.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (item.CampaignItemID > 0 && (item.UpdatedUserID == null || (item.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(item.UpdatedUserID.Value, false)))))
                    {
                        if (item.CampaignItemID > 0 && (item.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(item.UpdatedUserID.Value, item.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }

                if (item.SampleID != null && (!ECN_Framework_BusinessLayer.Communicator.Sample.Exists(item.SampleID.Value, item.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));
            }

            if (item.BlastScheduleID != null && (!ECN_Framework_BusinessLayer.Communicator.BlastSchedule.Exists(item.BlastScheduleID.Value)))
                errorList.Add(new ECNError(Entity, Method, "BlastScheduleID is invalid"));

            if (item.FromEmail != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(item.FromEmail)))
            {
                errorList.Add(new ECNError(Entity, Method, "FromEmail is invalid"));
            }

            if (item.ReplyTo != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(item.ReplyTo)))
            {
                errorList.Add(new ECNError(Entity, Method, "ReplyTo is invalid"));
            }

            if (item.OverrideIsAmount != null)
                if (item.OverrideAmount == null || item.OverrideAmount <= 0)
                    errorList.Add(new ECNError(Entity, Method, "BlastAmount is invalid"));

            if (item.IsHidden == null)
                errorList.Add(new ECNError(Entity, Method, "IsHidden is invalid"));

            if (item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.SMS.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Social.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Salesforce.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Personalization.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemType is invalid"));

            if (item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.TEXT.ToString() &&
                item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemFormatType is invalid"));

            if (item.CampaignItemNameOriginal.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "CampaignItemNameOriginal cannot be empty"));

            if (!ValidateSchedule(item, user))
            {
                if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. AB campaign item cannot be scheduled to go out after the related Champion campaign item"));
                }
                else if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. Champion campaign item cannot be scheduled to go out before the related AB campaign item"));
                }
            }


            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (item.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {

                if (item.CampaignID == null || !ECN_Framework_BusinessLayer.Communicator.Campaign.Exists(item.CampaignID.Value, item.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "CampaignID is invalid"));
                }
                else
                {
                    if (item.CampaignItemID > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItem as there are Sent or Active Blasts"));
                            break;
                        }
                    }

                    if (item.CampaignItemIDOriginal != null && (!ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists(item.CampaignID.Value, item.CampaignItemIDOriginal.Value, item.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemIDOriginal is invalid"));

                    if (item.CampaignItemName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemName cannot be empty"));
                    else
                        if (item.CampaignItemID <= 0 && Exists(item.CampaignItemName, item.CampaignID.Value, item.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CampaignItemName already exists"));

                    //wgh - was failing with scheduled blasts when creating new
                    //if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(item.CampaignItemName))
                    //    errorList.Add(new ECNError(Entity, Method, "CampaignItemName has invalid characters"));
                }
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(item.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    
                    scope.Complete();
                }

                if (item.SampleID != null && (!ECN_Framework_BusinessLayer.Communicator.Sample.Exists(item.SampleID.Value, item.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));
            }

            if (item.BlastScheduleID != null && (!ECN_Framework_BusinessLayer.Communicator.BlastSchedule.Exists(item.BlastScheduleID.Value)))
                errorList.Add(new ECNError(Entity, Method, "BlastScheduleID is invalid"));

            if (item.FromEmail != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(item.FromEmail)))
            {
                errorList.Add(new ECNError(Entity, Method, "FromEmail is invalid"));
            }

            if (item.ReplyTo != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(item.ReplyTo)))
            {
                errorList.Add(new ECNError(Entity, Method, "ReplyTo is invalid"));
            }

            if (item.OverrideIsAmount != null)
                if (item.OverrideAmount == null || item.OverrideAmount <= 0)
                    errorList.Add(new ECNError(Entity, Method, "BlastAmount is invalid"));

            if (item.IsHidden == null)
                errorList.Add(new ECNError(Entity, Method, "IsHidden is invalid"));

            if (item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.SMS.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Social.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Salesforce.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemType is invalid"));

            if (item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.TEXT.ToString() &&
                item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemFormatType is invalid"));

            if (item.CampaignItemNameOriginal.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "CampaignItemNameOriginal cannot be empty"));

            if (!ValidateSchedule_NoAccessCheck(item, user))
            {
                if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. AB campaign item cannot be scheduled to go out after the related Champion campaign item"));
                }
                else if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. Champion campaign item cannot be scheduled to go out before the related AB campaign item"));
                }
            }


            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Validate_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (item.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {

                if (item.CampaignID == null || !ECN_Framework_BusinessLayer.Communicator.Campaign.Exists_UseAmbientTransaction(item.CampaignID.Value, item.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, Method, "CampaignID is invalid"));
                }
                else
                {
                    if (item.CampaignItemID > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, false);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            if (ciBlast.BlastID != null && ECN_Framework_BusinessLayer.Communicator.Blast.ActiveOrSent_UseAmbientTransaction(ciBlast.BlastID.Value, ciBlast.CustomerID.Value))
                                errorList.Add(new ECNError(Entity, Method, "Cannot update CampaignItem as there are Sent or Active Blasts"));
                            break;
                        }
                    }

                    if (item.CampaignItemIDOriginal != null && (!ECN_Framework_BusinessLayer.Communicator.CampaignItem.Exists_UseAmbientTransaction(item.CampaignID.Value, item.CampaignItemIDOriginal.Value, item.CustomerID.Value)))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemIDOriginal is invalid"));

                    if (item.CampaignItemName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemName cannot be empty"));
                    else
                        if (item.CampaignItemID <= 0 && Exists_UseAmbientTransaction(item.CampaignItemName, item.CampaignID.Value, item.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CampaignItemName already exists"));

                    //wgh - was failing with scheduled blasts when creating new
                    //if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(item.CampaignItemName))
                    //    errorList.Add(new ECNError(Entity, Method, "CampaignItemName has invalid characters"));
                }
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(item.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    if (item.CreatedUserID == null || (item.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(item.CreatedUserID.Value, false))))
                    {
                        if (item.CreatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(item.CreatedUserID.Value, item.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }
                    if (item.CampaignItemID > 0 && (item.UpdatedUserID == null || (item.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(item.UpdatedUserID.Value, false)))))
                    {
                        if (item.CampaignItemID > 0 && (item.UpdatedUserID == null || (!KMPlatform.BusinessLogic.User.Exists(item.UpdatedUserID.Value, item.CustomerID.Value))))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }

                if (item.SampleID != null && (!ECN_Framework_BusinessLayer.Communicator.Sample.Exists_UseAmbientTransaction(item.SampleID.Value, item.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "SampleID is invalid"));
            }

            if (item.BlastScheduleID != null && (!ECN_Framework_BusinessLayer.Communicator.BlastSchedule.Exists_UseAmbientTransaction(item.BlastScheduleID.Value)))
                errorList.Add(new ECNError(Entity, Method, "BlastScheduleID is invalid"));

            if (item.FromEmail != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress_UseAmbientTransaction(item.FromEmail)))
            {
                errorList.Add(new ECNError(Entity, Method, "FromEmail is invalid"));
            }

            if (item.ReplyTo != string.Empty && (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress_UseAmbientTransaction(item.ReplyTo)))
            {
                errorList.Add(new ECNError(Entity, Method, "ReplyTo is invalid"));
            }

            if (item.OverrideIsAmount != null)
                if (item.OverrideAmount == null || item.OverrideAmount <= 0)
                    errorList.Add(new ECNError(Entity, Method, "BlastAmount is invalid"));

            if (item.IsHidden == null)
                errorList.Add(new ECNError(Entity, Method, "IsHidden is invalid"));

            if (item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Layout.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.NoOpen.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.SMS.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Social.ToString() &&
                item.CampaignItemType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Salesforce.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemType is invalid"));

            if (item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.TEXT.ToString() &&
                item.CampaignItemFormatType != ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString())
                errorList.Add(new ECNError(Entity, Method, "CampaignItemFormatType is invalid"));

            if (item.CampaignItemNameOriginal.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "CampaignItemNameOriginal cannot be empty"));

            if (!ValidateSchedule(item, user))
            {
                if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. AB campaign item cannot be scheduled to go out after the related Champion campaign item"));
                }
                else if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString()))
                {
                    errorList.Add(new ECNError(Entity, Method, "Invalid Schedule. Champion campaign item cannot be scheduled to go out before the related AB campaign item"));
                }
            }


            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool ValidateSchedule(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString()) && item.SendTime != null)
            {
                ECN_Framework_Entities.Communicator.CampaignItem itemRelated = GetBySampleID_UseAmbientTransaction(item.SampleID.Value, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion, user, false);
                if (itemRelated != null && itemRelated.SendTime != null)
                {
                    if (itemRelated.SendTime > item.SendTime)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            else if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString()) && item.SendTime != null)
            {
                ECN_Framework_Entities.Communicator.CampaignItem itemRelated = GetBySampleID_UseAmbientTransaction(item.SampleID.Value, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB, user, false);
                if (itemRelated.SendTime < item.SendTime)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ValidateSchedule_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString()) && item.SendTime != null)
            {
                ECN_Framework_Entities.Communicator.CampaignItem itemRelated = GetBySampleID_NoAccessCheck(item.SampleID.Value, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion, user, false);
                if (itemRelated != null && itemRelated.SendTime != null)
                {
                    if (itemRelated.SendTime > item.SendTime)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            else if (item.CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion.ToString()) && item.SendTime != null)
            {
                ECN_Framework_Entities.Communicator.CampaignItem itemRelated = GetBySampleID_NoAccessCheck(item.SampleID.Value, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB, user, false);
                if (itemRelated.SendTime < item.SendTime)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            Validate(item, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item,   user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                item.CampaignItemID = ECN_Framework_DataLayer.Communicator.CampaignItem.Save(item);
                scope.Complete();
            }

            return item.CampaignItemID;
        }

        public static int Save_NoAccessCheck(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            Validate_NoAccessCheck(item, user);            

            using (TransactionScope scope = new TransactionScope())
            {
                item.CampaignItemID = ECN_Framework_DataLayer.Communicator.CampaignItem.Save(item);
                scope.Complete();
            }

            return item.CampaignItemID;
        }

        public static int Save_UseAmbientTransaction(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            Validate_UseAmbientTransaction(item, user);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                item.CampaignItemID = ECN_Framework_DataLayer.Communicator.CampaignItem.Save(item);
                scope.Complete();
            }

            return item.CampaignItemID;
        }


        public static void Move(ECN_Framework_Entities.Communicator.CampaignItem item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();


            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item,  ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit , user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (Exists(item.CampaignItemName, item.CampaignID.Value, user.CustomerID))
            {
                errorList.Add(new ECNError() { ErrorMessage = "Campaign item name already exists", Entity = Enums.Entity.CampaignItem, Method = Method });
                throw new ECNException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItem.Save(item);
                scope.Complete();
            }
        }

        public static bool Exists(int campaignID, int campaignItemID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItem.Exists(campaignID, campaignItemID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int campaignID, int campaignItemID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItem.Exists(campaignID, campaignItemID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int campaignID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItem.Exists(campaignID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(string campaignItemName, int campaignID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItem.Exists(campaignItemName, campaignID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(string campaignItemName, int campaignID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItem.Exists(campaignItemName, campaignID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemTestBlastID(int campaignItemTestBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemTestBlastID(campaignItemTestBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemTestBlastID_NoAccessCheck(int campaignItemTestBlastID,  bool getChildren)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemTestBlastID(campaignItemTestBlastID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID,  getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemBlastID(int campaignItemBlastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemBlastID(campaignItemBlastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByBlastID(int blastID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByBlastID(blastID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
            }

            return item;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.CampaignItem GetByBlastID_NoAccessCheck(int blastID, bool getChildren)
        {

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByBlastID(blastID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByBlastID_NoAccessCheck_UseAmbientTransaction(int blastID, bool getChildren)
        {

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope())
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByBlastID(blastID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user,getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemID_UseAmbientTransaction(int campaignItemID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope())
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemID_NoAccessCheck(int campaignItemID,  bool getChildren)
        {

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID,getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(int campaignItemID, bool getChildren)
        {

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope())
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID, getChildren);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(item.CampaignItemID);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByNodeID(string NodeID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByNodeID(NodeID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (item != null && getChildren)
            {
                item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user);
                item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetBySampleID(int SampleID, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType CampaignItemType, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            if (CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB) || CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion))
            {
                if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetBySampleID(SampleID, CampaignItemType.ToString());
                    scope.Complete();
                }

                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (item != null && getChildren)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
                }
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetBySampleID_UseAmbientTransaction(int SampleID, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType CampaignItemType, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            if (CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB) || CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion))
            {
                if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                using (TransactionScope scope = new TransactionScope())
                {
                    item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetBySampleID(SampleID, CampaignItemType.ToString());
                    scope.Complete();
                }
                if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(item, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                if (item != null && getChildren)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user);
                }
            }

            return item;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetBySampleID_NoAccessCheck(int SampleID, ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType CampaignItemType, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.CampaignItem item = null;
            if (CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB) || CampaignItemType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Champion))
            {                
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    item = ECN_Framework_DataLayer.Communicator.CampaignItem.GetBySampleID(SampleID, CampaignItemType.ToString());
                    scope.Complete();
                }

                if (item != null && getChildren)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID,  getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
                }
            }

            return item;
        }

                

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetByCampaignID(int campaignID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignID(campaignID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (itemList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItem item in itemList)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(item.CampaignItemID, user, getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID(item.CampaignItemID, user);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(item.CampaignItemID, user);
                }
            }

            return itemList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetByCampaignID_UseAmbientTransaction(int campaignID, KMPlatform.Entity.User user, bool getChildren)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope())
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignID(campaignID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(itemList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (itemList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItem item in itemList)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user, getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_UseAmbientTransaction(item.CampaignItemID, user);
                }
            }

            return itemList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetByCampaignID_NoAccessCheck(int campaignID, bool getChildren)
        {

            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByCampaignID(campaignID);
                scope.Complete();
            }

            if (itemList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItem item in itemList)
                {
                    item.BlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                    item.TestBlastList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID, getChildren);
                    item.SuppressionList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
                    item.OptOutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID_NoAccessCheck(item.CampaignItemID);
                }
            }

            return itemList;
        }

        public static void Delete(int campaignID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = CampaignItem.GetByCampaignID(campaignID, user, false);
            if (itemList.Count > 0)
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    //for all of the child table deletes we are overriding so they won't call Blast.CreateBlastsFromCampaignItem() as we call it here
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Delete(itemList[0].CampaignItemID, user, true);
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Delete(itemList[0].CampaignItemID, user, true);
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.Delete(itemList[0].CampaignItemID, user);
                    ECN_Framework_DataLayer.Communicator.CampaignItem.Delete(campaignID, user.CustomerID, user.UserID);
                    scope.Complete();
                }

            }
        }

        public static void Delete(int campaignID, int campaignItemID, KMPlatform.Entity.User user, bool overrideUpdate = false)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new ECN_Framework_Common.Objects.SecurityException();

            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            ECN_Framework_Entities.Communicator.CampaignItem item = GetByCampaignItemID(campaignItemID, user, true);
            if (item != null)
            {
                bool justHide = false;
                bool justDelete = false;
                bool deleteANDhide = false;//hide the ci if there is a test blast and no production blast
                if (item.BlastList.Count > 0 && item.TestBlastList.Count > 0)
                {
                    var blastExists = item.BlastList.Where(x => x.BlastID != null);
                    var blastTestExists = item.TestBlastList.Where(x => x.BlastID != null);
                    if (blastExists.Any() == false && blastTestExists.Any() == true)
                    {
                        justHide = true;
                    }
                    else if (blastExists.Any() == true && blastTestExists.Any() == true)
                    {
                        deleteANDhide = true;
                    }
                    else if (blastExists.Any() == true && blastTestExists.Any() == false)
                    {
                        justDelete = true;
                    }
                }
                else
                {
                    justDelete = true;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    if (justDelete)
                    {
                        //for all of the child table deletes we are overriding so they won't call Blast.CreateBlastsFromCampaignItem() as we call it here
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Delete(campaignItemID, user, true);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Delete(campaignItemID, user, true);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.Delete(campaignItemID, user);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemLinkTracking.DeleteByCampaignItemID(campaignItemID, user);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.Delete(campaignItemID, user.CustomerID, user);
                        ECN_Framework_DataLayer.Communicator.CampaignItem.Delete(campaignID, campaignItemID, user.CustomerID, user.UserID);
                    }
                    else if (justHide)
                    {
                        SetIsHidden(campaignItemID, true, user);
                    }
                    else if (deleteANDhide)
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Delete(campaignItemID, user, true);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Delete(campaignItemID, user, true);
                        SetIsHidden(campaignItemID, true, user);
                    }

                    scope.Complete();
                }

            }
        }

        public static void Cancel(int campaignItemID, KMPlatform.Entity.User user, string action)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            List<ECN_Framework_Entities.Communicator.BlastAbstract> listB = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(campaignItemID, user, false);
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID, user, true);

            //Check for active, pending, or sent blasts 
            if (listB.Where(x => x.TestBlast.ToLower().Equals("n") && x.StatusCode == ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active.ToString()).Count() > 0)
            {
                errorList.Add(new ECNError(Enums.Entity.CampaignItem, Method, "Blasts have already started going out"));
            }
            else if (listB.Where(x => x.TestBlast.ToLower().Equals("n") && x.StatusCode == ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString() && x.SendTime.Value < DateTime.Now).Count() > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Blasts have already started going out"));

            }
            else if (listB.Where(x => x.TestBlast.ToLower().Equals("n") && x.StatusCode == ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Sent.ToString()).Count() > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Blasts have already gone out"));
            }

            if (ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(campaignItemID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem).Count > 0)
            {
                errorList.Add(new ECNError(Entity, Method, "Campaign Item is used in Marketing Automation. Cancelling is not allowed."));
            }
            if (errorList.Count == 0)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (ECN_Framework_Entities.Communicator.BlastAbstract ba in listB)
                    {
                        //Only try to cancel live blasts, don't worry about test blasts.
                        if (ba.TestBlast.ToLower().Equals("n"))
                        {
                            //ECN_Framework_BusinessLayer.Communicator.BlastAbstract.UpdateStatus(ba.BlastID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Cancelled);
                            ECN_Framework_BusinessLayer.Communicator.BlastAbstract.Delete(ba.BlastID, user);
                        }
                    }
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ci.BlastList)
                    {
                        ciBlast.BlastID = null;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(ciBlast, user);
                    }

                   
                    scope.Complete();
                }
            }
            else
            {
                throw new ECNException(errorList);
            }
        }

        internal static void SetIsHidden(int campaignItemID, bool isHidden, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItem.SetIsHidden(campaignItemID, isHidden, user.UserID);
                scope.Complete();
            }
        }

        public static DataTable GetByStatus(int customerID, string status, KMPlatform.Entity.User user)
        {
            DataTable dtCampaignItems = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCampaignItems = ECN_Framework_DataLayer.Communicator.CampaignItem.GetByStatus(user.CustomerID, status);
                scope.Complete();
            }
            return dtCampaignItems;
        }

        public static DataTable GetSentCampaignItems(string campaignName, string campaignItemName, string emailSubject, string layoutName, string groupName, int blastID, DateTime searchFrom, DateTime searchTo,
                                                     int? userID, bool? testBlast, int customerID, KMPlatform.Entity.User user)
        {
            DataTable dtCampaignItems = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCampaignItems = ECN_Framework_DataLayer.Communicator.CampaignItem.GetSentCampaignItems(campaignName, campaignItemName, emailSubject, layoutName, groupName, blastID,
                                                                                                        searchFrom, searchTo, userID,
                                                                                                        testBlast, customerID);
                scope.Complete();
            }
            return dtCampaignItems;
        }

        public static int CopyCampaignItem(int campaignItemId, User user)
        {
            // Get Existing CampaignItem and its details
            var campaignItem = GetByCampaignItemID(campaignItemId, user, false);
            var ciSuppressionList = CampaignItemSuppression.GetByCampaignItemID(campaignItemId, user, true);
            var ciBlastList = CampaignItemBlast.GetByCampaignItemID(campaignItemId, user, true);
            var ciOptOutGroupList = CampaignItemOptOutGroup.GetByCampaignItemID(campaignItemId, user);
            var ciTrackingList = CampaignItemLinkTracking.GetByCampaignItemID(campaignItemId, user);
            var ciSocialMedia = CampaignItemSocialMedia.GetByCampaignItemID(campaignItemId);

            // Copy CampaignItem
            var copyCampaignItem = CopyCampaignItemWithoutDetails(user, campaignItem);

            using (var scope = new TransactionScope())
            {
                if (copyCampaignItem.CampaignItemType == ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.AB.ToString())
                {
                    CopySample(user, campaignItem, copyCampaignItem);
                }

                // Save Copied CampaignItem
                Save(copyCampaignItem, user);

                // Copy and Save CampaignItemSuppression List
                CopySuppressions(user, ciSuppressionList, copyCampaignItem);

                // Copy and Save CampaignItemBlast List
                CopyBlasts(user, ciBlastList, copyCampaignItem);

                // Copy and Save OptOutGroups List
                CopyOptOutGroups(user, ciOptOutGroupList, copyCampaignItem);

                // Copy and Save CampaignItemLinkTracking List
                CopyLinkTracking(user, ciTrackingList, copyCampaignItem);

                // Copy and Save CampaignItemSocialMedia list
                CopySocialMedia(user, ciSocialMedia, copyCampaignItem);

                // Copy and Save CampaignItemMeta Tag info for subscriber share
                CopyMetaTags(campaignItemId, user, copyCampaignItem);

                scope.Complete();
            }

            return copyCampaignItem.CampaignItemID;
        }

        private static ECN_Framework_Entities.Communicator.CampaignItem CopyCampaignItemWithoutDetails(User user, ECN_Framework_Entities.Communicator.CampaignItem campaignItem)
        {
            var copyCampaignItem = ObjectFunctions.DeepCopy(campaignItem);
            copyCampaignItem.CampaignItemID = -1;
            copyCampaignItem.CampaignItemName = copyCampaignItem.CampaignItemName + " - COPY " + DateTime.Now;
            copyCampaignItem.SendTime = null;
            copyCampaignItem.OverrideAmount = null;
            copyCampaignItem.OverrideIsAmount = null;
            copyCampaignItem.CampaignItemNameOriginal = copyCampaignItem.CampaignItemNameOriginal + " - COPY " + DateTime.Now;
            copyCampaignItem.CampaignItemIDOriginal = null;
            copyCampaignItem.BlastScheduleID = null;
            copyCampaignItem.CompletedStep = null;
            copyCampaignItem.CreatedUserID = user.UserID;
            copyCampaignItem.EnableCacheBuster = null;
            return copyCampaignItem;
        }

        private static void CopySample(User user, ECN_Framework_Entities.Communicator.CampaignItem campaignItem, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            if (campaignItem.SampleID.HasValue)
            {
                var campaignItemSample = Sample.GetBySampleID(campaignItem.SampleID.Value, user);

                var sample = new ECN_Framework_Entities.Communicator.Sample
                {
                    SampleName = copyCampaignItem.CampaignItemName,
                    CreatedUserID = user.UserID,
                    CustomerID = user.CustomerID,
                    ABWinnerType = campaignItemSample.ABWinnerType
                };

                Sample.Save(sample, user);
                copyCampaignItem.SampleID = sample.SampleID;
            }
        }

        private static void CopySuppressions(User user, List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> ciSuppressionList, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            var copyCiSuppressionList = ObjectFunctions.DeepCopy(ciSuppressionList);
            foreach (var ciSuppression in copyCiSuppressionList)
            {
                ciSuppression.CampaignItemID = copyCampaignItem.CampaignItemID;
                ciSuppression.CampaignItemSuppressionID = -1;
                ciSuppression.CreatedUserID = user.UserID;

                CampaignItemSuppression.Save(ciSuppression, user);
            }
        }

        private static void CopyBlasts(User user, List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            foreach (var ciBlast in ciBlastList)
            {
                var copyCiBlast = ObjectFunctions.DeepCopy(ciBlast);
                copyCiBlast.CampaignItemID = copyCampaignItem.CampaignItemID;
                copyCiBlast.BlastID = null;
                copyCiBlast.CampaignItemBlastID = -1;
                copyCiBlast.CreatedUserID = user.UserID;
                copyCiBlast.Blast = null;
                CampaignItemBlast.Save(copyCiBlast, user, true);

                //Get Existing CampaignItemBlastRefBlast List
                CampaignItemBlastRefBlast.GetByCampaignItemBlastID(ciBlast.CampaignItemBlastID, user);
            }
        }

        private static void CopyOptOutGroups(User user, List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> ciOptOutGroupList, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            var copyciOptOutGroupList = ObjectFunctions.DeepCopy(ciOptOutGroupList);
            foreach (var ciOptOut in copyciOptOutGroupList)
            {
                ciOptOut.CampaignItemID = copyCampaignItem.CampaignItemID;
                ciOptOut.CampaignItemOptOutID = -1;
                ciOptOut.CreatedUserID = user.UserID;
                CampaignItemOptOutGroup.Save(ciOptOut, user);
            }
        }

        private static void CopyLinkTracking(User user, List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> ciTrackingList, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            var copyciTrackingList = ObjectFunctions.DeepCopy(ciTrackingList);
            foreach (var linkTracking in copyciTrackingList)
            {
                linkTracking.CampaignItemID = copyCampaignItem.CampaignItemID;
                linkTracking.CILTID = -1;
                CampaignItemLinkTracking.Save(linkTracking, user);
            }
        }

        private static void CopySocialMedia(User user, List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> ciSocialMedia, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            var copyciSocialMedia = ObjectFunctions.DeepCopy(ciSocialMedia);
            foreach (var socialMedia in copyciSocialMedia)
            {
                socialMedia.CampaignItemID = copyCampaignItem.CampaignItemID;
                socialMedia.CampaignItemSocialMediaID = -1;
                if (socialMedia.SimpleShareDetailID.HasValue)
                {
                    var ssdOriginal = SimpleShareDetail.GetBySimpleShareDetailID(socialMedia.SimpleShareDetailID.Value);
                    var ssdNew = new ECN_Framework_Entities.Communicator.SimpleShareDetail
                    {
                        SimpleShareDetailID = -1,
                        CampaignItemID = copyCampaignItem.CampaignItemID,
                        Content = ssdOriginal.Content,
                        ImagePath = ssdOriginal.ImagePath,
                        IsDeleted = false,
                        PageAccessToken = ssdOriginal.PageAccessToken,
                        PageID = ssdOriginal.PageID,
                        SocialMediaAuthID = ssdOriginal.SocialMediaAuthID,
                        SocialMediaID = ssdOriginal.SocialMediaID,
                        SubTitle = ssdOriginal.SubTitle,
                        Title = ssdOriginal.Title,
                        UseThumbnail = ssdOriginal.UseThumbnail,
                        CreatedUserID = user.UserID
                    };
                    socialMedia.SimpleShareDetailID = SimpleShareDetail.Save(ssdNew);
                }

                CampaignItemSocialMedia.Save(socialMedia);
            }
        }

        private static void CopyMetaTags(int campaignItemId, User user, ECN_Framework_Entities.Communicator.CampaignItem copyCampaignItem)
        {
            var ciMetaTags = CampaignItemMetaTag.GetByCampaignItemID(campaignItemId);
            foreach (var metaTag in ciMetaTags)
            {
                metaTag.CampaignItemID = copyCampaignItem.CampaignItemID;
                metaTag.CampaignItemMetaTagID = -1;
                metaTag.CreatedUserID = user.UserID;

                CampaignItemMetaTag.Save(metaTag);
            }
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetSentSaleforceCampaignItems(KMPlatform.Entity.User user, int Days)
        {
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItem.GetSentSaleforceCampaignItems(Days);
                scope.Complete();
            }


            return itemList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetSentSaleforceCampaignItems_NoAccessCheck( int Days)
        {

            List<ECN_Framework_Entities.Communicator.CampaignItem> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItem.GetSentSaleforceCampaignItems(Days);
                scope.Complete();
            }


            return itemList;
        }

        public static bool HasEmailActvity(int CampaignItemID, int days)
        {
            bool HasEmailActvity = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                HasEmailActvity = ECN_Framework_DataLayer.Communicator.CampaignItem.HasEmailActvity(CampaignItemID, days);
                scope.Complete();
            }
            return HasEmailActvity;
        }

        public static DataTable GetCampaignItemsForCampaignSearch(int campaignID, string status, string name, int pageSize, int pageIndex)
        {
            DataTable dtReturn = new DataTable();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.CampaignItem.GetCampaignItemsForCampaignSearch(campaignID, status, name, pageSize, pageIndex);
                scope.Complete();
            }

            return dtReturn;
        }

        public static DataTable GetPendingCampaignItems_NONRecurring(int customerID )
        {
            DataTable dtReturn = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtReturn = ECN_Framework_DataLayer.Communicator.CampaignItem.GetPendingCampaignItems_NONRecurring(customerID);
                scope.Complete();
            }
            return dtReturn;
        }

        public static void UpdateCampaignItemName(ECN_Framework_Entities.Communicator.CampaignItem ci)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(ci.CampaignItemName, ci.CampaignID.Value, ci.CustomerID.Value))
                errorList.Add(new ECNError(Entity, Method, "CampaignItemName already exists"));

            if (errorList.Count > 0)
                throw new ECNException(errorList, Enums.ExceptionLayer.Business);
            else
            {
                using(TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_DataLayer.Communicator.CampaignItem.Save(ci);
                    scope.Complete();
                }
            }
        }

        public static void UpdateSendTime(int campaignItemID, DateTime newSendTime)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItem.UpdateSendTime(campaignItemID, newSendTime);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> UsedAsSmartSegment(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItem> isUsed = new List<ECN_Framework_Entities.Communicator.CampaignItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                isUsed = ECN_Framework_DataLayer.Communicator.CampaignItem.UsedAsSmartSegment(blastID);
                scope.Complete();
            }
            return isUsed;
        }
    }
}

