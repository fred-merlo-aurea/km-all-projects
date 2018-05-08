using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common.Entity;
using KM.Common.Extensions;
using BlastSingleEntity = ECN_Framework_Entities.Communicator.BlastSingle;
using BlastEntity = ECN_Framework_Entities.Communicator.Blast;
using BlastAbstractEntity = ECN_Framework_Entities.Communicator.BlastAbstract;
using CacheUtil = KM.Common.CacheUtil;
using EntityApplicationLog = KM.Common.Entity.ApplicationLog;
using EmailGroupEntity = ECN_Framework_Entities.Communicator.EmailGroup;
using EmailActivityLogEntity = ECN_Framework_Entities.Communicator.EmailActivityLog;
using EnumsFramework = ECN_Framework_Common.Objects.Communicator.Enums;
using LayoutPlansCommunicator = ECN_Framework_Entities.Communicator.LayoutPlans;
using PlatformUser = KMPlatform.Entity.User;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EventOrganizer
    {
        private const string InsertFailedValidation = "BlastSingle.Insert failed validation";
        private const string EventOrganizerFireEvent = "EventOrganizer.FireEvent";
        private const string KmCommonApplicationSettingsKey = "KMCommon_Application";
        private static string FormAbandonCacheName = "FORM_ABANDON_";
        private static string FormSubmitCacheName = "FORM_SUBMIT_";
        private const string Layout = "LAYOUT";
        private const string NoOpen = "NOOPEN";
        private const string Yes = "Y";

        public static void Event(ECN_Framework_Entities.Communicator.EmailActivityLog log, KMPlatform.Entity.User user)
        {

            // Get the relavant data
            if (log.BlastID > 0)
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(log.BlastID, false);
                ECN_Framework_Entities.Communicator.CampaignItem ciOriginal = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(log.BlastID, false);
                if (blast != null && blast.LayoutID.HasValue && blast.CustomerID.HasValue && ciOriginal != null)
                {
                    List<ECN_Framework_Entities.Communicator.LayoutPlans> typePlansList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByType_NoAccessCheck(blast.LayoutID.Value, blast.CustomerID.Value, log.ActionTypeCode);
                    if (typePlansList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.LayoutPlans typePlans in typePlansList)
                        {
                            if (!typePlans.CampaignItemID.HasValue)
                                FireEvent(typePlans, log, user);
                            else if (typePlans.CampaignItemID.HasValue && typePlans.CampaignItemID == ciOriginal.CampaignItemID)
                                FireEvent(typePlans, log, user);
                        }
                    }
                    //checking for blastid == 0 because group triggers shouldn't have a blast id and we don't want to do them if the activity came from a blast
                    if (log.BlastID == 0 && blast.GroupID.HasValue)
                    {
                        List<ECN_Framework_Entities.Communicator.LayoutPlans> groupTriggerList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID_NoAccessCheck(blast.GroupID.Value, blast.CustomerID.Value);
                        if (groupTriggerList.Count > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.LayoutPlans typePlans in groupTriggerList)
                            {
                                FireEvent(typePlans, log, user);

                            }
                        }
                    }
                    //Form Abandon Fire Event
                    List<ECN_Framework_Entities.Communicator.LayoutPlans> formabPlansList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByType_NoAccessCheck(blast.LayoutID.Value, blast.CustomerID.Value, ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Abandon.ToString().ToLower());
                    if (formabPlansList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.LayoutPlans formabtypePlans in formabPlansList)
                        {
                            if (!formabtypePlans.CampaignItemID.HasValue)
                            {
                                FireEvent(formabtypePlans, log, user);
                                KM.Common.CacheUtil.AddToCache(FormAbandonCacheName + log.EmailID.ToString(), formabtypePlans.LayoutPlanID);
                            }
                            else if (formabtypePlans.CampaignItemID.HasValue && formabtypePlans.CampaignItemID == ciOriginal.CampaignItemID)
                            {
                                FireEvent(formabtypePlans, log, user);
                                KM.Common.CacheUtil.AddToCache(FormAbandonCacheName + log.EmailID.ToString(), formabtypePlans.LayoutPlanID);
                            }

                        }
                    }

                    List<ECN_Framework_Entities.Communicator.LayoutPlans> formSubmitPlansList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByType_NoAccessCheck(blast.LayoutID.Value, blast.CustomerID.Value, ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Submit.ToString().ToLower());
                    if (formSubmitPlansList.Count > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.LayoutPlans formSubmitPlans in formSubmitPlansList)
                        {
                            if(formSubmitPlans.CampaignItemID.HasValue && formSubmitPlans.CampaignItemID.Value == ciOriginal.CampaignItemID)
                            {
                                KM.Common.CacheUtil.AddToCache(FormSubmitCacheName + log.EmailID.ToString(), ciOriginal.CampaignItemID);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void Event(int CustomerID, int GroupID, int EmailID, KMPlatform.Entity.User user, int? sfID)
        {
            if (CustomerID > 0 && GroupID > 0 && EmailID > 0)
            {
                List<ECN_Framework_Entities.Communicator.LayoutPlans> groupPlansList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID_NoAccessCheck(GroupID, CustomerID);
                List<ECN_Framework_Entities.Communicator.LayoutPlans> SFPlansList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();
                if (sfID != null && sfID > 0)
                {
                    SFPlansList = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetBySmartFormID_NoAccessCheck(sfID.Value, CustomerID);
                }
                if (SFPlansList.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.LayoutPlans plan in SFPlansList)
                    {
                        groupPlansList.RemoveAll(x => x.GroupID == plan.GroupID);
                        FireEvent(plan, EmailID, user);
                    }
                }
                if (groupPlansList.Count > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.LayoutPlans plan in groupPlansList)
                    {
                        FireEvent(plan, EmailID, user);
                    }
                }
            }
        }

        public static void Event(ECN_Framework_Entities.Communicator.LayoutPlans plan, int EmailID, KMPlatform.Entity.User user)
        {
            FireEvent(plan, EmailID, user);
        }

        private static void FireEvent(ECN_Framework_Entities.Communicator.LayoutPlans my_plan, int EmailID, KMPlatform.Entity.User user)
        {
            if (my_plan != null && my_plan.LayoutPlanID > 0)
            {
                if (my_plan.Status.ToString().ToUpper().Equals("Y"))
                {
                    if (my_plan.BlastID != null)
                    {
                        ECN_Framework_Entities.Communicator.BlastAbstract to_blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(my_plan.BlastID.Value, false);

                        if (my_plan.EventType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Subscribe.ToString().ToLower()))
                        {
                            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailIDGroupID_NoAccessCheck(EmailID, my_plan.GroupID.Value);

                            if (emailGroup != null)
                            {
                                // Should have an object for this.. no time right now.
                                if (!my_plan.EventType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Subscribe.ToString()) || (my_plan.EventType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Subscribe.ToString().ToLower()) && emailGroup.SubscribeTypeCode.ToLower().Equals(my_plan.Criteria.ToLower())))
                                {

                                    if (!ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastEmailLayoutPlan(to_blast.BlastID, emailGroup.EmailID, my_plan.LayoutPlanID, to_blast.CustomerID.Value))
                                    {

                                        ECN_Framework_Entities.Communicator.BlastSingle blastSingle = new ECN_Framework_Entities.Communicator.BlastSingle();
                                        try
                                        {
                                            blastSingle.SendTime = DateTime.Now.AddDays(Convert.ToDouble(my_plan.Period));
                                            blastSingle.BlastID = to_blast.BlastID;
                                            blastSingle.EmailID = emailGroup.EmailID;
                                            blastSingle.LayoutPlanID = my_plan.LayoutPlanID;
                                            blastSingle.RefBlastID = my_plan.BlastID;
                                            blastSingle.CustomerID = my_plan.CustomerID.Value;
                                            blastSingle.CreatedUserID = to_blast.CreatedUserID.Value;
                                            ECN_Framework_BusinessLayer.Communicator.BlastSingle.Insert_NoAccessCheck(blastSingle);
                                        }
                                        catch (ECNException ecn)
                                        {
                                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastSingle.Insert failed validation", "EventOrganizer.FireEvent", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateECNNote(blastSingle));
                                        }

                                    }
                                }
                            }
                            else
                            {
                                //check if email has been merged
                                int mergedEmailID = ECN_Framework_BusinessLayer.Communicator.EmailHistory.FindMergedEmailID(EmailID);
                                if (mergedEmailID > 0)
                                {
                                    // Should have an object for this.. no time right now.
                                    ECN_Framework_Entities.Communicator.EmailGroup emailGroupMerged = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailIDGroupID_NoAccessCheck(mergedEmailID, my_plan.GroupID.Value);
                                    if (!my_plan.EventType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Subscribe.ToString()) || (my_plan.EventType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Subscribe.ToString().ToLower()) && emailGroupMerged.SubscribeTypeCode.ToLower().Equals(my_plan.Criteria.ToLower())))
                                    {
                                        if (!ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastEmailLayoutPlan(to_blast.BlastID, emailGroup.EmailID, my_plan.LayoutPlanID, to_blast.CustomerID.Value))
                                        {

                                            ECN_Framework_Entities.Communicator.BlastSingle blastSingle = new ECN_Framework_Entities.Communicator.BlastSingle();
                                            try
                                            {
                                                blastSingle.SendTime = DateTime.Now.AddDays(Convert.ToDouble(my_plan.Period));
                                                blastSingle.BlastID = to_blast.BlastID;
                                                blastSingle.EmailID = mergedEmailID;
                                                blastSingle.LayoutPlanID = my_plan.LayoutPlanID;
                                                blastSingle.RefBlastID = my_plan.BlastID;
                                                blastSingle.CustomerID = my_plan.CustomerID.Value;
                                                blastSingle.CreatedUserID = to_blast.CreatedUserID.Value;
                                                ECN_Framework_BusinessLayer.Communicator.BlastSingle.Insert_NoAccessCheck(blastSingle);
                                            }
                                            catch (ECNException ecn)
                                            {
                                                KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastSingle.Insert failed validation", "EventOrganizer.FireEvent", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateECNNote(blastSingle));
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        /// Form Abandon
                        else if (my_plan.EventType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Abandon.ToString().ToLower())
                           || my_plan.EventType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.ActionTypeCode.Submit.ToString().ToLower()))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.BlastSingle.ExistsByBlastEmailLayoutPlan(my_plan.BlastID.Value, EmailID, my_plan.LayoutPlanID, my_plan.CustomerID.Value))
                            {
                                ECN_Framework_Entities.Communicator.BlastSingle blastSingle = new ECN_Framework_Entities.Communicator.BlastSingle();
                                blastSingle.SendTime = DateTime.Now.AddDays(Convert.ToDouble(my_plan.Period));
                                blastSingle.BlastID = my_plan.BlastID;
                                blastSingle.EmailID = EmailID;
                                blastSingle.LayoutPlanID = my_plan.LayoutPlanID;
                                blastSingle.RefBlastID = my_plan.BlastID;
                                blastSingle.CustomerID = my_plan.CustomerID.Value;
                                blastSingle.CreatedUserID = my_plan.CreatedUserID.Value;
                                try
                                {
                                    ECN_Framework_BusinessLayer.Communicator.BlastSingle.Insert_NoAccessCheck(blastSingle);

                                    

                                    
                                }
                                catch (ECNException ecn)
                                {
                                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastSingle.Insert failed validation", "EventOrganizer.FireEvent", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateECNNote(blastSingle));
                                }
                                // Check to see if there's a NO-OPEN Campaign set for this blast in the BlastSingles. IF YES, get the properties of that Plan
                                // and Insert a record for the NO-OPEN trigger
                                //sql -> SELECT tp.TriggerPlanID, tp.RefTriggerID, tp.BlastID, tp.Period FROM Blasts b JOIN TriggerPlans tp ON b.BlastID = tp.RefTriggerID WHERE tp.RefTriggerID = 85233 AND tp.EventType = 'noOpen'

                                List<ECN_Framework_Entities.Communicator.TriggerPlans> triggerPlansList = ECN_Framework_BusinessLayer.Communicator.TriggerPlans.GetNoOpenByBlastID_NoAccessCheck(to_blast.BlastID);
                                if (triggerPlansList.Count > 0)
                                {
                                    foreach (ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan in triggerPlansList)
                                    {
                                        blastSingle = new ECN_Framework_Entities.Communicator.BlastSingle();
                                        blastSingle.SendTime = DateTime.Now.AddDays(Convert.ToDouble(triggerPlan.Period) + Convert.ToDouble(my_plan.Period));
                                        blastSingle.BlastID = triggerPlan.BlastID.Value;
                                        blastSingle.EmailID = EmailID;
                                        blastSingle.LayoutPlanID = triggerPlan.TriggerPlanID;
                                        blastSingle.RefBlastID = my_plan.BlastID;
                                        blastSingle.CustomerID = to_blast.CustomerID.Value;
                                        blastSingle.CreatedUserID = to_blast.CreatedUserID.Value;
                                        try
                                        {
                                            ECN_Framework_BusinessLayer.Communicator.BlastSingle.Insert_NoAccessCheck(blastSingle);
                                        }
                                        catch (ECNException ecn)
                                        {
                                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("BlastSingle.Insert failed validation", "EventOrganizer.FireEvent", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateECNNote(blastSingle));
                                        }
                                    }
                                }
                            }
                        } // End for Form Abandon

                    }
                }
            }
        }

        private static void FireEvent(LayoutPlansCommunicator layoutPlans, EmailActivityLogEntity logEvent, PlatformUser user)
        {
            if (IsLayoutPlansValid(layoutPlans))
            {
                var toBlast = Blast.GetByBlastID_NoAccessCheck(layoutPlans.BlastID.Value, false);
                var fromBlast = Blast.GetByBlastID_NoAccessCheck(logEvent.BlastID, false);

                var check_against = layoutPlans.Criteria;

                if (!(string.IsNullOrWhiteSpace(check_against) || check_against == logEvent.ActionValue))
                {
                    return;
                }

                if (logEvent.EmailID > 0 && 
                    layoutPlans.GroupID != null && 
                    layoutPlans.GroupID != 0 && 
                    layoutPlans.EventType.Equals(EnumsFramework.ActionTypeCode.Subscribe.ToString()))
                {
                    var emailGroup = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(logEvent.EmailID, layoutPlans.GroupID.Value);

                    if (emailGroup != null && emailGroup.EmailID > 0)
                    {
                        BlastSingleValidateInsert(logEvent.EmailID, layoutPlans, toBlast, emailGroup, logEvent);
                    }
                    else
                    {
                        int mergedEmailID = EmailHistory.FindMergedEmailID(logEvent.EmailID);
                        if (mergedEmailID > 0)
                        {
                            var emailGroupMerged = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(mergedEmailID, layoutPlans.GroupID.Value);
                            BlastSingleValidateInsert(mergedEmailID, layoutPlans, toBlast, emailGroupMerged, logEvent);
                        }
                    }
                }
                else
                {
                    BlastSingleValidateInsert_NoGroup(layoutPlans, fromBlast, toBlast, logEvent);
                }
            }
        }

        private static void BlastSingleValidateInsert_NoGroup(LayoutPlansCommunicator layoutPlans, BlastAbstractEntity fromBlast, BlastAbstractEntity toBlast, EmailActivityLogEntity logEvent)
        {
            var email = Email.GetByEmailID_NoAccessCheck(logEvent.EmailID);
            var emailID = -1;
            if (email == null)
            {

                emailID = EmailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(fromBlast.GroupID.Value, toBlast.CustomerID.Value, email.EmailAddress);
                if (emailID <= 0)
                {
                    emailID = EmailHistory.FindMergedEmailID(logEvent.EmailID);
                }
            }
            else
            {
                emailID = email.EmailID;
            }

            if (emailID > 0)
            {
                BlastEntity refBlast = null;
                var blastIDToUse = fromBlast.BlastID;
                if (fromBlast.BlastType.EqualsIgnoreCase(Layout) || fromBlast.BlastType.EqualsIgnoreCase(NoOpen))
                {
                    var refBlastID = BlastSingle.GetRefBlastID(fromBlast.BlastID, emailID, fromBlast.CustomerID.Value, fromBlast.BlastType);
                    refBlast = Blast.GetByBlastID_NoAccessCheck(refBlastID, false);
                    blastIDToUse = refBlastID;
                }

                EmailGroupEntity emailGroup = null;
                if (blastIDToUse == fromBlast.BlastID)
                {
                    emailGroup = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(emailID, fromBlast.GroupID.Value);
                }
                else if (refBlast != null && blastIDToUse == refBlast.BlastID)
                {
                    emailGroup = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(emailID, refBlast.GroupID.Value);
                }

                if (IsBlastSingleValidateInsert_NoGroupValid(layoutPlans, emailGroup))
                {
                    if (!BlastSingle.ExistsByBlastEmailLayoutPlan(toBlast.BlastID, emailID, layoutPlans.LayoutPlanID, toBlast.CustomerID.Value))
                    {
                        var sendTime = DateTime.Now.AddDays(ParseDouble(layoutPlans.Period + ""));
                        BlastSingleInsert(sendTime, toBlast.BlastID, emailID, layoutPlans.LayoutPlanID, logEvent.BlastID,
                                            toBlast.CustomerID.Value, toBlast.CreatedUserID.Value);

                        InsertNoAccessChecks(layoutPlans, logEvent.EmailID, logEvent.BlastID, toBlast);
                    }
                }
            }
            else
            {
                int mergedEmailID = EmailHistory.FindMergedEmailID(logEvent.EmailID);
                if (mergedEmailID > 0)
                {
                    var emailGroupMerged = EmailGroup.GetByEmailIDGroupID_NoAccessCheck(mergedEmailID, fromBlast.GroupID.Value);
                    if (IsBlastSingleValidateInsert_NoGroupValid(layoutPlans, emailGroupMerged))
                    {
                        if (!BlastSingle.ExistsByBlastEmailLayoutPlan(toBlast.BlastID, mergedEmailID, layoutPlans.LayoutPlanID, toBlast.CustomerID.Value))
                        {
                            var sendTime = DateTime.Now.AddDays(ParseDouble(layoutPlans.Period + ""));
                            BlastSingleInsert(sendTime, toBlast.BlastID, logEvent.EmailID, layoutPlans.LayoutPlanID, logEvent.BlastID, 
                                                toBlast.CustomerID.Value, toBlast.CreatedUserID.Value);
                            InsertNoAccessChecks(layoutPlans, mergedEmailID, logEvent.BlastID, toBlast);
                        }
                    }
                }
            }
        }

        private static bool IsBlastSingleValidateInsert_NoGroupValid(LayoutPlansCommunicator layoutPlans, EmailGroupEntity emailGroup)
        {
            return !layoutPlans.EventType.Equals(EnumsFramework.ActionTypeCode.Subscribe.ToString()) ||
                        (layoutPlans.EventType.EqualsIgnoreCase(EnumsFramework.ActionTypeCode.Subscribe.ToString()) &&
                        emailGroup.SubscribeTypeCode.EqualsIgnoreCase(layoutPlans.Criteria));
        }

        private static bool IsLayoutPlansValid(LayoutPlansCommunicator layoutPlans)
        {
            return layoutPlans != null &&
                layoutPlans.LayoutPlanID > 0 &&
                layoutPlans.Status.ToString().EqualsIgnoreCase(Yes) &&
                layoutPlans.BlastID != null;
        }

        private static void BlastSingleValidateInsert(int emailId, LayoutPlansCommunicator layoutPlans, BlastAbstractEntity toBlast, EmailGroupEntity emailGroup, EmailActivityLogEntity logEvent)
        {
            if (IsBlastSingleValidateInsertValid(layoutPlans, emailGroup))
            {
                if (!BlastSingle.ExistsByBlastEmailLayoutPlan(toBlast.BlastID, logEvent.EmailID, layoutPlans.LayoutPlanID, toBlast.CustomerID.Value))
                {
                    var sendTime = DateTime.Now.AddDays(ParseDouble(layoutPlans.Period + ""));
                    BlastSingleInsert(sendTime, toBlast.BlastID, emailId, layoutPlans.LayoutPlanID, logEvent.BlastID, null, null);
                    if (layoutPlans.EventType.Equals(EnumsFramework.ActionTypeCode.Abandon.ToString()))
                    {
                        CacheUtil.AddToCache(FormAbandonCacheName + emailGroup.EmailID.ToString(), layoutPlans.LayoutPlanID);
                    }
                }
            }
        }

        private static bool IsBlastSingleValidateInsertValid(LayoutPlansCommunicator layoutPlans, EmailGroupEntity emailGroup)
        {
            return !layoutPlans.EventType.Equals(EnumsFramework.ActionTypeCode.Subscribe.ToString()) ||
                (layoutPlans.EventType.EqualsIgnoreCase(EnumsFramework.ActionTypeCode.Subscribe.ToString()) &&
                    emailGroup.SubscribeTypeCode.EqualsIgnoreCase(layoutPlans.Criteria));
        }

        private static void BlastSingleInsert(DateTime? sendTime, int? blastId, int? emailId, int? layoutPlanId,
                                              int? refBlastId, int? customerId, int? createdUserId)
        {
            var blastSingle = new BlastSingleEntity
            {
                SendTime = sendTime,
                BlastID = blastId,
                EmailID = emailId,
                LayoutPlanID = layoutPlanId,
                RefBlastID = refBlastId,
                CustomerID = customerId,
                CreatedUserID = createdUserId
            };

            try
            {
                BlastSingle.Insert_NoAccessCheck(blastSingle);
            }
            catch (ECNException ecn)
            {
                Trace.TraceError(ecn.Message);
                int applicationId;
                int.TryParse(ConfigurationManager.AppSettings[KmCommonApplicationSettingsKey], out applicationId);
                var sourceMethod = $"{nameof(EventOrganizer)}.{nameof(EventOrganizer.FireEvent)}";
                EntityApplicationLog.LogNonCriticalError(InsertFailedValidation, sourceMethod, applicationId, CreateECNNote(blastSingle));
            }
        }

        private static double ParseDouble(string data)
        {
            double doubleValue = 0;
            double.TryParse(data, out doubleValue);
            return doubleValue;
        }

        private static void InsertNoAccessChecks(LayoutPlansCommunicator layoutPlans, int emailId, int refBlastId, BlastEntity blastEntity)
        {
            var triggerPlansList = TriggerPlans.GetNoOpenByBlastID_NoAccessCheck(blastEntity.BlastID);
            if (triggerPlansList.Count <= 0)
            {
                return;
            }

            foreach (var triggerPlan in triggerPlansList)
            {
                var blastSingle = new BlastSingleEntity
                {
                    SendTime = DateTime.Now.AddDays(Convert.ToDouble(triggerPlan.Period) + Convert.ToDouble(layoutPlans.Period)),
                    BlastID = triggerPlan.BlastID.Value,
                    EmailID = emailId,
                    LayoutPlanID = triggerPlan.TriggerPlanID,
                    RefBlastID = refBlastId,
                    CustomerID = blastEntity.CustomerID.Value,
                    CreatedUserID = blastEntity.CreatedUserID.Value
                };

                try
                {
                    BlastSingle.Insert_NoAccessCheck(blastSingle);
                }
                catch (Exception exception) 
                    when (exception is InvalidOperationException 
                          || exception is ECNException)
                {
                    ApplicationLog.LogNonCriticalError(
                        InsertFailedValidation,
                        EventOrganizerFireEvent,
                        Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplicationSettingsKey]),
                        CreateECNNote(blastSingle));
                }
            }
        }

        private static string CreateECNNote(ECN_Framework_Entities.Communicator.BlastSingle bs)
        {
            StringBuilder sbNote = new StringBuilder();
            sbNote.AppendLine("EmailID:" + (bs.EmailID.HasValue ? bs.EmailID.Value.ToString() : ""));
            sbNote.AppendLine("BlastID:" + (bs.BlastID.HasValue ? bs.BlastID.Value.ToString() : ""));
            sbNote.AppendLine("LayoutPlanID:" + (bs.LayoutPlanID.HasValue ? bs.LayoutPlanID.Value.ToString() : ""));
            sbNote.AppendLine("RefBlastID:" + (bs.RefBlastID.HasValue ? bs.RefBlastID.Value.ToString() : ""));
            sbNote.AppendLine("CustomerID:" + (bs.CustomerID.HasValue ? bs.CustomerID.Value.ToString() : ""));


            return sbNote.ToString();
        }

        public static ECN_Framework_Entities.Communicator.LayoutPlans AddOpenPlan(int? layoutID, int blastID, int? groupID, string name, double period, string criteria, KMPlatform.Entity.User user)
        {
            return AddLayoutPlan(layoutID, blastID, groupID, name, period, ECN_Framework_Common.Objects.Communicator.Enums.EventType.Open, criteria, user);
        }

        private static ECN_Framework_Entities.Communicator.LayoutPlans AddLayoutPlan(int? layoutID, int blastID, int? groupID, string name, double period, ECN_Framework_Common.Objects.Communicator.Enums.EventType type, string criteria, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.LayoutPlans my_plan = new ECN_Framework_Entities.Communicator.LayoutPlans();
            my_plan.LayoutID = layoutID;
            my_plan.EventType = type.ToString();
            my_plan.BlastID = blastID;
            my_plan.Period = Convert.ToDecimal(period);
            my_plan.Criteria = criteria;
            my_plan.ActionName = name;
            my_plan.CustomerID = user.CustomerID;
            my_plan.GroupID = groupID;
            my_plan.Status = "Y";
            my_plan.CreatedUserID = user.UserID;
            ECN_Framework_BusinessLayer.Communicator.LayoutPlans.Save(my_plan, user);
            return my_plan;
        }

        public static ECN_Framework_Entities.Communicator.LayoutPlans AddClickPlan(int? layoutID, int blastID, int? groupID, string name, double period, string criteria, KMPlatform.Entity.User user)
        {
            return AddLayoutPlan(layoutID, blastID, groupID, name, period, ECN_Framework_Common.Objects.Communicator.Enums.EventType.Click, criteria, user);
        }

        public static ECN_Framework_Entities.Communicator.LayoutPlans AddSubscribePlan(int? layoutID, int blastID, int? groupID, string name, double period, string criteria, KMPlatform.Entity.User user)
        {
            return AddLayoutPlan(layoutID, blastID, groupID, name, period, ECN_Framework_Common.Objects.Communicator.Enums.EventType.Subscribe, criteria, user);
        }

        public static ECN_Framework_Entities.Communicator.LayoutPlans AddReferPlan(int? layoutID, int blastID, int? groupID, string name, double period, string criteria, KMPlatform.Entity.User user)
        {
            return AddLayoutPlan(layoutID, blastID, groupID, name, period, ECN_Framework_Common.Objects.Communicator.Enums.EventType.Subscribe, criteria, user);
        }

        public static ECN_Framework_Entities.Communicator.TriggerPlans AddNoOpenTriggerPlan(int refTriggerBlastID, int blastID, int? groupID, double period, string criteria, string actionName, KMPlatform.Entity.User user)
        {
            return AddTriggerPlan(refTriggerBlastID, ECN_Framework_Common.Objects.Communicator.Enums.EventType.NoOpen, blastID, groupID, period, criteria, actionName, user);
        }

        private static ECN_Framework_Entities.Communicator.TriggerPlans AddTriggerPlan(int refTriggerBlastID, ECN_Framework_Common.Objects.Communicator.Enums.EventType eventType, int blastID, int? groupID, double period, string criteria, string actionName, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.TriggerPlans my_plan = new ECN_Framework_Entities.Communicator.TriggerPlans();
            my_plan.EventType = eventType.ToString();
            my_plan.refTriggerID = refTriggerBlastID;
            my_plan.BlastID = blastID;
            my_plan.Period = Convert.ToDecimal(period);
            my_plan.Criteria = criteria;
            my_plan.ActionName = actionName;
            my_plan.CreatedUserID = user.UserID;
            my_plan.CustomerID = user.CustomerID;
            my_plan.GroupID = groupID;
            my_plan.Status = "Y";
            ECN_Framework_BusinessLayer.Communicator.TriggerPlans.Save(my_plan, user);
            return my_plan;
        }
    }
}
