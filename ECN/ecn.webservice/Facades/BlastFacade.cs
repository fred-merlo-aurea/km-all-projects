using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ecn.common.classes;
using ecn.webservice.classes;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KM.Common.Extensions;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using User = KMPlatform.Entity.User;

namespace ecn.webservice.Facades
{
    public class BlastFacade: FacadeBase, IBlastFacade
    {
        private const string BlastUpdatedResponseMessage = "BLAST UPDATED";
        private const string BadBlastValueResponseMessage = "BAD BLAST VALUE: {0}";
        private const string UnknownBlastResponseMessage = "UNKNOWN BLAST: {0}";
        private const string TooManyEmailsResponseMessage = "ERROR: The Group list selected for test blast, contains more than the allowed {0} emails for testing. Use a Filter or choose a Group with {0} or less emails in it.";
        private const string NoLicensesAvailableResponseMessage = "NO LICENSES AVAILABLE";
        private const string ErrorCreatingTestBlastResponseMessage = "UNKNOWN ERROR CREATING TEST BLAST";
        private const string BlastCreatedResponseMessage = "BLAST CREATED";
        private const string CampaignItemDateTimeFormat = "MM/dd/yyyy hh:mm:ss.fff tt";
        private const string CampaignItemNameTemplate = "{0} {1}";
        private const int DefaultSendingDelaySeconds = 15;
        private const string MarketingCampaign = "Marketing Campaign";
        private const string LicenseUnlimited = "UNLIMITED";
        private const string LicenseNoLicense = "NO LICENSE";

        private IBlastManager _blastManager;
        private ICampaignItemBlastManager _campaignItemBlastManager;
        private ICampaignItemTestBlastManager _campaignItemTestBlastManager;
        private ICampaignItemBlastRefBlastManager _campaignItemBlastRefBlastManager;

        public IBlastManager BlastsManager
        {
            get
            {
                if (_blastManager == null)
                {
                    _blastManager = new ECN_Framework_BusinessLayer.Communicator.BlastManager();
                }
                return _blastManager;
            }
            set
            {
                _blastManager = value;
            }
        }

        public ICampaignItemBlastManager CampaignItemBlastsManager
        {
            get
            {
                if (_campaignItemBlastManager == null)
                {
                    _campaignItemBlastManager = new CampaignItemBlastManager();
                }
                return _campaignItemBlastManager;
            }
            set
            {
                _campaignItemBlastManager = value;
            }
        }

        public ICampaignItemTestBlastManager CampaignItemTestBlastManager
        {
            get
            {
                if (_campaignItemTestBlastManager == null)
                {
                    _campaignItemTestBlastManager = new CampaignItemTestBlastManager();
                }
                return _campaignItemTestBlastManager;
            }
            set
            {
                _campaignItemTestBlastManager = value;
            }
        }

        public ICampaignItemBlastRefBlastManager CampaignItemBlastRefBlastManager
        {
            get
            {
                if (_campaignItemBlastRefBlastManager == null)
                {
                    _campaignItemBlastRefBlastManager = new CampaignItemBlastRefBlastManager();
                }
                return _campaignItemBlastRefBlastManager;
            }
            set
            {
                _campaignItemBlastRefBlastManager = value;
            }
        }

        public string GetSubscriberCount(
            WebMethodExecutionContext context,
            Dictionary<string, object> parameters)
        {
            var groupId = (int)parameters[Consts.GroupIdParameter];
            var listFilters = new List<CommunicatorEntities.CampaignItemBlastFilter>();
            var blastCount = 0;
            var dataTable = BlastsManager.GetBlastEmailListForDynamicContent(
                context.User.CustomerID,
                0,
                groupId,
                listFilters,
                string.Empty,
                string.Empty,
                string.Empty,
                true,
                true);

            try
            {
                blastCount = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, blastCount.ToString());
        }

        public string AddBlast(WebMethodExecutionContext context, AddBlastParams parameters)
        {
            if (parameters.ReplyEmail.IsNullOrWhiteSpace())
            {
                parameters.ReplyEmail = parameters.FromEmail;
            }

            var isSmartSegment = false;
            var smartSegmentId = 0;
            var valueError = EnsureBlastValues(
                context.User.CustomerID,
                parameters.MessageId,
                parameters.ListId,
                parameters.FilterId,
                parameters.Subject,
                ref isSmartSegment,
                DateTime.Now,
                context.User,
                parameters.RefBlasts);

            if (isSmartSegment)
            {
                smartSegmentId = SmartSegment.GetNewIDFromOldID(parameters.FilterId);
                parameters.FilterId = 0;
            }

            if (valueError.Length == 0)
            {
                return PerformAddBlast(context, parameters, smartSegmentId, isSmartSegment);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, string.Format(BadBlastValueResponseMessage, valueError));
        }

        private string PerformAddBlast(
            WebMethodExecutionContext context,
            AddBlastParams parameters,
            int smartSegmentId,
            bool isSmartSegment)
        {
            if (!IsLicenceAvailable(context))
            {
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, NoLicensesAvailableResponseMessage);
            }

            if (parameters.IsTest)
            {
                var TestBlastCount = GetTestBlastCount(context.User.CustomerID);
                var emailsCount = Convert.ToInt32(BlastCheck(
                    context.User.CustomerID,
                    parameters.ListId,
                    parameters.FilterId,
                    smartSegmentId,
                    string.Empty,
                    string.Empty,
                    context.User));

                if (emailsCount > TestBlastCount)
                {
                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    var errorMessage = string.Format(TooManyEmailsResponseMessage, TestBlastCount);
                    return GetFailResponse(context, errorMessage.ToUpper());
                }
            }

            var campaign = GetOrCreateCampaign(context);
            var campaignItem = CreateCampaignItem(context, parameters, campaign);
            var itemBlast = CreateCampaignItemBlast(context, parameters, smartSegmentId, isSmartSegment, campaignItem);

            if (parameters.IsTest)
            {
                var testBlast = CreateCampaignItemTestBlast(context, parameters, campaignItem);

                var blast = Blast.GetByCampaignItemTestBlastID(testBlast.CampaignItemTestBlastID, context.User, false);
                if (blast == null || blast.BlastID <= 0)
                {
                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetFailResponse(context, ErrorCreatingTestBlastResponseMessage);
                }

                if (parameters.FilterId > 0)
                {
                    Blast.UpdateFilterForAPITestBlasts(blast.BlastID, parameters.FilterId);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BlastCreatedResponseMessage, blast.BlastID);
            }
            else
            {
                Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, context.User);
                var blast = Blast.GetByCampaignItemBlastID(itemBlast.CampaignItemBlastID, context.User, false);
                if (blast == null || blast.BlastID <= 0)
                {
                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetFailResponse(context, ErrorCreatingTestBlastResponseMessage);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BlastCreatedResponseMessage, blast.BlastID);
            }
        }

        private static CommunicatorEntities.CampaignItemTestBlast CreateCampaignItemTestBlast(
            WebMethodExecutionContext context,
            AddBlastParams parameters,
            CommunicatorEntities.CampaignItem campaignItem)
        {
            var testBlast = new CommunicatorEntities.CampaignItemTestBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                GroupID = parameters.ListId,
                CreatedUserID = context.User.UserID,
                CustomerID = context.User.CustomerID,
                HasEmailPreview = false
            };

            CampaignItemTestBlast.Insert(testBlast, context.User);

            if (parameters.FilterId > 0)
            {
                var blastFilter = new CommunicatorEntities.CampaignItemBlastFilter
                {
                    CampaignItemTestBlastID = testBlast.CampaignItemTestBlastID,
                    FilterID = parameters.FilterId,
                    IsDeleted = false
                };
                CampaignItemBlastFilter.Save(blastFilter);
            }

            return testBlast;
        }

        private CommunicatorEntities.CampaignItemBlast CreateCampaignItemBlast(
            WebMethodExecutionContext context,
            AddBlastParams parameters,
            int smartSegmentId,
            bool isSmartSegment,
            CommunicatorEntities.CampaignItem campaignItem)
        {
            var campaignItemBlast = new CommunicatorEntities.CampaignItemBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                CustomerID = context.User.CustomerID,
                GroupID = parameters.ListId,
                CreatedUserID = context.User.UserID,
                EmailSubject = parameters.Subject,
                LayoutID = parameters.MessageId
            };

            var blastId = CampaignItemBlast.Save(campaignItemBlast, context.User);

            if (parameters.FilterId > 0 || smartSegmentId > 0)
            {
                var blastFilter = new CommunicatorEntities.CampaignItemBlastFilter();
                blastFilter.CampaignItemBlastID = blastId;
                if (isSmartSegment && parameters.RefBlasts.Trim().Length > 0)
                {
                    blastFilter.SmartSegmentID = smartSegmentId;
                    blastFilter.RefBlastIDs = parameters.RefBlasts;
                }
                else
                {
                    blastFilter.FilterID = parameters.FilterId;
                }

                blastFilter.IsDeleted = false;
                CampaignItemBlastFilter.Save(blastFilter);
            }

            return campaignItemBlast;
        }

        private static CommunicatorEntities.CampaignItem CreateCampaignItem(
            WebMethodExecutionContext context,
            AddBlastParams parameters,
            CommunicatorEntities.Campaign campaign)
        {
            var campaignItem = new CommunicatorEntities.CampaignItem
            {
                CustomerID = context.User.CustomerID,
                CreatedUserID = context.User.UserID,
                CampaignItemName = string.Format(
                    CampaignItemNameTemplate,
                    parameters.Subject,
                    DateTime.Now.ToString(CampaignItemDateTimeFormat)),
                CampaignID = campaign.CampaignID,
                CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString(),
                CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString(),
                FromEmail = parameters.FromEmail,
                FromName = parameters.FromName,
                ReplyTo = parameters.ReplyEmail,
                IsHidden = false
            };
            campaignItem.CampaignItemNameOriginal = campaignItem.CampaignItemName;
            campaignItem.SendTime = DateTime.Now.AddSeconds(DefaultSendingDelaySeconds);
            campaignItem.CompletedStep = 4;

            if (parameters.IsTest)
            {
                campaignItem.IsHidden = true;
            }
            CampaignItem.Save(campaignItem, context.User);
            return campaignItem;
        }

        private CommunicatorEntities.Campaign GetOrCreateCampaign(WebMethodExecutionContext context)
        {
            var campaign = Campaign.GetByCampaignName(MarketingCampaign, context.User, false);

            if (campaign == null || campaign.CampaignID <= 0)
            {
                campaign = new CommunicatorEntities.Campaign
                {
                    CustomerID = context.User.CustomerID,
                    CreatedUserID = context.User.UserID,
                    CampaignName = MarketingCampaign
                };
                Campaign.Save(campaign, context.User);
            }

            return campaign;
        }

        private bool IsLicenceAvailable(WebMethodExecutionContext context)
        {
            var licenceCheck = new LicenseCheck();
            var blastLicensed = licenceCheck.Current(context.User.CustomerID.ToString());
            var blastAvailable = licenceCheck.Available(context.User.CustomerID.ToString());

            if (blastLicensed.Equals(LicenseUnlimited))
            {
                return true;
            }

            if (blastAvailable == LicenseNoLicense)
            {
                return false;
            }

            return true;
        }

        public string UpdateBlast(
            WebMethodExecutionContext context,
            UpdateBlastParams parameters)
        {
            var user = context.User;
            var campaignItemBlast = CampaignItemBlastsManager.GetByBlastId(parameters.BlastId, user, true);

            if (campaignItemBlast.Blast != null)
            {
                var isSmartSegment = false;
                var valueError = EnsureBlastValues(
                    user.CustomerID,
                    parameters.MessageId,
                    parameters.ListId,
                    parameters.FilterId,
                    parameters.Subject,
                    ref isSmartSegment,
                    DateTime.Now, user);

                if (isSmartSegment)
                {
                    parameters.FilterId = 0;
                }

                if (valueError.Length == 0)
                {
                    PerformUpdateBlast(campaignItemBlast, parameters, user);

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    GetSuccessResponse(context, BlastUpdatedResponseMessage, parameters.BlastId);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, string.Format(BadBlastValueResponseMessage,valueError));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, string.Format(UnknownBlastResponseMessage, parameters.BlastId));
        }

        private static void PerformUpdateBlast(CommunicatorEntities.CampaignItemBlast campaignItemBlast, UpdateBlastParams parameters, User user)
        {
            var campaignItem = CampaignItem.GetByCampaignItemID(
                campaignItemBlast.CampaignItemID.Value,
                user,
                false);

            campaignItem.UpdatedUserID = user.UserID;
            campaignItem.FromEmail = parameters.FromEmail;
            campaignItem.FromName = parameters.FromName;
            campaignItem.ReplyTo = parameters.ReplyEmail;
            CampaignItem.Save(campaignItem, user);

            campaignItemBlast.CampaignItemID = campaignItem.CampaignItemID;
            campaignItemBlast.GroupID = parameters.ListId;
            campaignItemBlast.UpdatedUserID = user.UserID;
            campaignItemBlast.EmailSubject = parameters.Subject;
            campaignItemBlast.LayoutID = parameters.MessageId;
            var campaginItemBlastId = CampaignItemBlast.Save(campaignItemBlast, user);

            if (parameters.FilterId > 0)
            {
                CampaignItemBlastFilter.DeleteByCampaignItemBlastID(campaginItemBlastId);
                var campaignItemBlastFilter =
                    new CommunicatorEntities.CampaignItemBlastFilter
                    {
                        CampaignItemBlastID = campaginItemBlastId,
                        FilterID = parameters.FilterId,
                        IsDeleted = false
                    };
                CampaignItemBlastFilter.Save(campaignItemBlastFilter);
            }
            else
            {
                CampaignItemBlastFilter.DeleteByCampaignItemBlastID(campaginItemBlastId);
            }

            Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, user);
        }

        public string AddScheduledBlast(
            WebMethodExecutionContext context,
            Dictionary<string, object> parameters)
        {
            var user = context.User;
            var messageId = (int)parameters[Consts.MessageIdParameter];
            var listId = (int)parameters[Consts.ListIdParameter];
            var filterId = (int)parameters[Consts.FilterIdParameter];
            var subject = parameters[Consts.SubjectParameter].ToString();
            var fromEmail = parameters[Consts.FromEmailParameter].ToString();
            var fromName = parameters[Consts.FromNameParameter].ToString();
            var replyEmail = parameters[Consts.ReplyEmailParameter].ToString();
            var refBlasts = parameters[Consts.RefBlastsParameter].ToString();
            var xmlSchedule = parameters[Consts.XmlScheduleParameter].ToString();

            bool isSmartSegment = false;
            int smartSegmentID = 0;
            string valueError = EnsureBlastValues(user.CustomerID, messageId, listId, filterId, subject, ref isSmartSegment, DateTime.Now, user,
                refBlasts);
            if (isSmartSegment)
            {
                smartSegmentID = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetNewIDFromOldID(filterId);
                filterId = 0;
            }
            if (valueError.Length == 0)
            {
                LicenseCheck lc = new LicenseCheck();
                string BlastLicensed = lc.Current(user.CustomerID.ToString());
                string BlastAvailable = lc.Available(user.CustomerID.ToString());

                if (BlastLicensed.Equals(LicenseUnlimited))
                {
                    BlastAvailable = "N/A";
                }

                if (BlastAvailable == LicenseNoLicense)
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                    return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, NoLicensesAvailableResponseMessage);
                }

                ECN_Framework_Entities.Communicator.BlastSchedule schedule =
                    ECN_Framework_BusinessLayer.Communicator.BlastSchedule.CreateScheduleFromXML(xmlSchedule, user.UserID);
                int blastScheduleID = 0;
                if (schedule != null)
                {
                    blastScheduleID = Convert.ToInt32(schedule.BlastScheduleID);
                    ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo =
                        ECN_Framework_BusinessLayer.Communicator.BlastSetupInfo.GetNextScheduledBlastSetupInfo(blastScheduleID, true);
                    if (setupInfo != null)
                    {
                        //create campaign
                        ECN_Framework_Entities.Communicator.Campaign c =
                            ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignName(MarketingCampaign, user, false);
                        if (c == null || c.CampaignID <= 0)
                        {
                            c = new ECN_Framework_Entities.Communicator.Campaign();
                            c.CustomerID = user.CustomerID;
                            c.CreatedUserID = user.UserID;
                            c.CampaignName = MarketingCampaign;
                            ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, user);
                        }

                        //create campaign item
                        ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                        ci.CustomerID = user.CustomerID;
                        ci.CreatedUserID = user.UserID;
                        ci.CampaignItemName = subject + " " + DateTime.Now.ToString(CampaignItemDateTimeFormat);
                        ci.CampaignID = c.CampaignID;
                        ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                        ci.IsHidden = false;
                        ci.CampaignItemNameOriginal = ci.CampaignItemName;
                        ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                        ci.FromEmail = fromEmail;
                        ci.FromName = fromName;
                        ci.ReplyTo = replyEmail;
                        ci.SendTime = setupInfo.SendTime;
                        ci.BlastScheduleID = setupInfo.BlastScheduleID;
                        ci.CompletedStep = 4;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, user);

                        //create campaign item blast
                        ECN_Framework_Entities.Communicator.CampaignItemBlast cib = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                        cib.CampaignItemID = ci.CampaignItemID;
                        cib.CustomerID = user.CustomerID;
                        cib.GroupID = listId;
                        cib.CreatedUserID = user.UserID;
                        cib.EmailSubject = subject;
                        cib.LayoutID = messageId;
                        int cibID = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, user);

                        if (filterId > 0 || smartSegmentID > 0)
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf =
                                new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                            cibf.CampaignItemBlastID = cibID;
                            if (isSmartSegment && refBlasts.Trim().Length > 0)
                            {
                                cibf.SmartSegmentID = smartSegmentID;
                                cibf.RefBlastIDs = refBlasts;
                            }
                            else
                            {
                                cibf.FilterID = filterId;
                            }
                            cibf.IsDeleted = false;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                        }

                        //create campaign item blast and actual blast
                        ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, user);
                        ECN_Framework_Entities.Communicator.BlastAbstract blast =
                            ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID(cib.CampaignItemBlastID, user, false);
                        if (blast == null || blast.BlastID <= 0)
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                            return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, "UNKNOWN ERROR CREATING BLAST");
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                            return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Success, blast.BlastID, BlastCreatedResponseMessage);
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                        return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, "ISSUE SETTING UP SCHEDULE");
                    }
                }
                else
                {
                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                    return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, "ISSUE SETTING UP SCHEDULE");
                }
            }
            else
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, "BAD BLAST VALUE: " + valueError);
            }
        }

        public string GetBlast(
            WebMethodExecutionContext context,
            Dictionary<string, object> parameters)
        {
            var blastId = (int)parameters[Consts.BlastIdParameter];
            var blast = BlastsManager.GetByBlastId(blastId, context.User, false);

            if (blast != null)
            {
                var blastList = new List<CommunicatorEntities.BlastAbstract>
                {
                    blast
                };
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                var output = BuildBlastReturnXML(blastList, context.User);
                return GetSuccessResponse(context, output);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            var errorOutput = string.Format(Consts.BlastNotFoundResponseOutput, blastId);
            return GetFailResponse(context, errorOutput);
        }

        public string SearchForBlasts(WebMethodExecutionContext context, string XMLSearch)
        {
            var user = context.User;

            var blastList = BuildBlastSearchParams(XMLSearch, user.CustomerID, user);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildBlastReturnXML(blastList, user));
        }

        public string DeleteBlast(WebMethodExecutionContext context, int blastId)
        {
            var user = context.User;

            var ciBlast = CampaignItemBlastsManager.GetByBlastId(blastId, user, false);
            var ciTestBlast = CampaignItemTestBlastManager.GetByBlastId(blastId, user, false);
            if (ciBlast != null)
            {
                CampaignItemBlastRefBlastManager.Delete(ciBlast.CampaignItemBlastID, user);
                CampaignItemBlastsManager.Delete(ciBlast.CampaignItemID.Value, ciBlast.CampaignItemBlastID, user);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, Consts.DeleteBlastResposneOutput, blastId);
            }

            if (ciTestBlast != null)
            {
                CampaignItemTestBlastManager.Delete(
                    ciTestBlast.CampaignItemID.Value,
                    ciTestBlast.CampaignItemTestBlastID,
                    user);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, Consts.DeleteBlastResposneOutput, blastId);

            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, Consts.BlastDoesntExistResponseOutput);
        }

        public string GetBlastReport(WebMethodExecutionContext context, int blastId)
        {
            var user = context.User;

            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReport(blastId, "", "", user);
            if (dt != null)
            {
                string BlastReport = "";
                int Unique;
                int Total;
                int UniquePercent;
                int TotalPercent;

                dt.TableName = "BlastReport";

                BlastReport = "<ROOT>";

                //get send totals
                int SendTotal = 0;
                int SendUnique = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ActionTypeCode"].ToString().ToLower() == "send")
                    {
                        if (row["DistinctCount"] != DBNull.Value)
                        {
                            SendUnique = Convert.ToInt32(row["DistinctCount"]);
                        }
                        if (row["total"] != DBNull.Value)
                        {
                            SendTotal = Convert.ToInt32(row["total"]);
                        }
                        BlastReport += "<Sends><Unique>" + SendUnique.ToString() + "</Unique><Total>" + SendTotal.ToString() + "</Total>";
                        BlastReport += "<UniquePercent>100</UniquePercent><TotalPercent>100</TotalPercent></Sends>";
                        break;
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    Unique = 0;
                    Total = 0;
                    UniquePercent = 0;
                    TotalPercent = 0;

                    if (row["DistinctCount"] != DBNull.Value)
                    {
                        Unique = Convert.ToInt32(row["DistinctCount"]);
                        if (SendUnique > 0)
                        {
                            UniquePercent = Unique * 100 / SendUnique;
                        }
                    }
                    if (row["total"] != DBNull.Value)
                    {
                        Total = Convert.ToInt32(row["total"]);
                        if (SendTotal > 0)
                        {
                            TotalPercent = Total * 100 / SendTotal;
                        }
                    }

                    switch (row["ActionTypeCode"].ToString().ToLower())
                    {
                        case "open":
                            BlastReport += "<Opens><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Opens>";
                            //unopened
                            Unique = SendUnique - Unique;
                            Total = SendTotal - Total;
                            UniquePercent = 0;
                            TotalPercent = 0;
                            if (SendUnique > 0)
                            {
                                UniquePercent = Unique * 100 / SendUnique;
                            }
                            if (SendTotal > 0)
                            {
                                TotalPercent = Total * 100 / SendTotal;
                            }
                            BlastReport += "<Unopened><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Unopened>";
                            break;
                        case "click":
                            BlastReport += "<Clicks><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Clicks>";
                            //unclicked
                            Unique = SendUnique - Unique;
                            Total = SendTotal - Total;
                            UniquePercent = 0;
                            TotalPercent = 0;
                            if (SendUnique > 0)
                            {
                                UniquePercent = Unique * 100 / SendUnique;
                            }
                            if (SendTotal > 0)
                            {
                                TotalPercent = Total * 100 / SendTotal;
                            }
                            BlastReport += "<NoClicks><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></NoClicks>";
                            break;
                        case "bounce":
                            BlastReport += "<Bounces><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Bounces>";
                            break;
                        case "resend":
                            BlastReport += "<Resends><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Resends>";
                            break;
                        case "refer": //forward
                            BlastReport += "<Forwards><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Forwards>";
                            break;
                        case "subscribe":
                            BlastReport += "<Unsubscribes><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></Unsubscribes>";
                            break;
                        case "MASTSUP_UNSUB":
                            BlastReport += "<MasterSupressed><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></MasterSupressed>";
                            break;
                        case "ABUSERPT_UNSUB":
                            BlastReport += "<AbuseComplaints><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></AbuseComplaints>";
                            break;
                        case "FEEDBACK_UNSUB":
                            BlastReport += "<ISPFeedbackLoops><Unique>" + Unique.ToString() + "</Unique><Total>" + Total.ToString() + "</Total>";
                            BlastReport += "<UniquePercent>" + UniquePercent.ToString() + "</UniquePercent><TotalPercent>" + TotalPercent.ToString() +
                                           "</TotalPercent></ISPFeedbackLoops>";
                            break;
                        default:
                            break;
                    }

                }
                BlastReport += "</ROOT>";
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Success, blastId, BlastReport);
            }
            else
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(context.ApiLogId, null);
                return SendResponse.response(context.MethodName, SendResponse.ResponseCode.Fail, 0, "BLAST DOESN'T EXIST");
            }
        }

        public string GetBlastReportByISP(WebMethodExecutionContext context, GetBlastReportByISPParams parameters)
        {
            var ISPs = BuildISPString(parameters.XmlSearch);
            var dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetISPReport(parameters.BlastId, ISPs);
            if (dt != null)
            {
                var stringWriter = new StringWriter();
                dt.TableName = "Report";
                dt.WriteXml(stringWriter);
                var resultsXml = stringWriter.ToString();
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, resultsXml, parameters.BlastId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "BLAST DOESN'T EXIST");
        }

        public string GetBlastOpensReport(WebMethodExecutionContext context, GetBlastReportParams parameters)
        {
            if (Blast.Exists(parameters.BlastId, context.User.CustomerID))
            {
                if (parameters.FilterType.Trim().ToUpper() != "UNIQUE")
                {
                    parameters.FilterType = "all";
                }

                var dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(
                    parameters.BlastId,
                    "open",
                    parameters.FilterType,
                    context.User);

                dt.TableName = "Report";
                var stringWriter = new StringWriter();
                dt.WriteXml(stringWriter);
                var fieldsToUse = new[] {"OpenTime", "EmailAddress", "Info"};
                var resultsXML = ModifyGeneratedXML(stringWriter, parameters.WithDetail, fieldsToUse);

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, resultsXML, parameters.BlastId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "BLAST DOESN'T EXIST", parameters.BlastId);
        }

        public string GetBlastClicksReport(WebMethodExecutionContext context, GetBlastReportParams parameters)
        {
            if (Blast.Exists(parameters.BlastId, context.User.CustomerID))
            {
                if (parameters.FilterType.Trim().ToUpper() != "UNIQUE")
                {
                    parameters.FilterType = "all";
                }

                var dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(
                    parameters.BlastId,
                    "click",
                    parameters.FilterType,
                    context.User);

                if (!parameters.WithDetail)
                {
                    var view = new DataView(dt);
                    dt = view.ToTable(true, "ClickTime", "EmailAddress", "Link");
                }

                dt.TableName = "Report";
                var stringWriter = new StringWriter();
                dt.WriteXml(stringWriter);
                var fieldsToUse = new [] {"ClickTime", "EmailAddress", "Link"};
                var resultsXML = ModifyGeneratedXML(stringWriter, parameters.WithDetail, fieldsToUse);

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, resultsXML, parameters.BlastId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "BLAST DOESN'T EXIST", parameters.BlastId);
        }

        public string GetBlastBounceReport(WebMethodExecutionContext context, GetBlastReportParams parameters)
        {
            if (Blast.Exists(parameters.BlastId, context.User.CustomerID))
            {
                var dataTable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(
                    parameters.BlastId,
                    "bounce",
                    string.Empty,
                    context.User);

                dataTable.TableName = "Report";
                var stringWriter = new StringWriter();
                dataTable.WriteXml(stringWriter);
                var fieldsToUse = new[] {"BounceTime", "EmailAddress", "BounceType", "BounceSignature"};
                var resultsXML = ModifyGeneratedXML(stringWriter, parameters.WithDetail, fieldsToUse);

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, resultsXML, parameters.BlastId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "BLAST DOESN'T EXIST", parameters.BlastId);
        }

        public string GetBlastUnsubscribeReport(WebMethodExecutionContext context, GetBlastReportParams parameters)
        {
            if (Blast.Exists(parameters.BlastId, context.User.CustomerID))
            {
                var dataTable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(
                    parameters.BlastId,
                    "unsubscribe",
                    "subscribe",
                    context.User);

                dataTable.TableName = "Report";
                var stringWriter = new StringWriter();
                dataTable.WriteXml(stringWriter);
                var fieldsToUse = new [] {"UnsubscribeTime", "EmailAddress", "Reason"};
                var resultsXML = ModifyGeneratedXML(stringWriter, parameters.WithDetail, fieldsToUse);

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, resultsXML, parameters.BlastId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "BLAST DOESN'T EXIST", parameters.BlastId);
        }

        public string GetBlastDeliveryReport(WebMethodExecutionContext context, GetBlastDeliveryReportParams parameters)
        {
            var dataTable = ECN_Framework_BusinessLayer.Activity.Report.BlastDelivery.Get(
                context.User.CustomerID.ToString(),
                parameters.DateFrom,
                parameters.DateTo);

            dataTable.TableName = "Report";
            var stringWriter = new StringWriter();
            dataTable.WriteXml(stringWriter);
            var resultsXML = stringWriter.ToString();

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, resultsXML);
        }

        public string BuildBlastReturnXML(IList<CommunicatorEntities.BlastAbstract> blastList, KMPlatform.Entity.User user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<DocumentElement xmlns=\"\">");
            foreach (ECN_Framework_Entities.Communicator.BlastAbstract blast in blastList)
            {
                var cib = CampaignItemBlastsManager.GetByBlastId(blast.BlastID, user, true);

                sb.Append("<Blast><BlastID>");
                sb.Append(blast.BlastID);
                sb.Append("</BlastID><LayoutID>");
                sb.Append(blast.LayoutID);
                sb.Append("</LayoutID><GroupID>");
                sb.Append(blast.GroupID);
                sb.Append("</GroupID><UserID>");
                sb.Append(blast.CreatedUserID);
                sb.Append("</UserID><FilterID>");
                if (cib.Filters != null && cib.Filters.Count(x => x.FilterID != null) > 0)
                {
                    string filterIDs = string.Empty;
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cib.Filters.Where(x => x.FilterID != null))
                    {
                        filterIDs += cibf.FilterID.ToString() + ",";
                    }
                    filterIDs = filterIDs.TrimEnd(',');
                    sb.Append(filterIDs);
                }
                sb.Append("</FilterID><SmartSegmentID>");
                if (cib.Filters != null && cib.Filters.Count(x => x.SmartSegmentID != null) > 0)
                {
                    string ssIDs = string.Empty;
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cib.Filters.Where(x => x.SmartSegmentID != null))
                    {
                        ssIDs += cibf.SmartSegmentID.ToString() + ",";
                    }
                    ssIDs = ssIDs.TrimEnd(',');
                    sb.Append(ssIDs);
                }

                sb.Append("</SmartSegmentID><EmailSubject>");
                sb.Append(cleanXMLString(blast.EmailSubject));
                sb.Append("</EmailSubject><EmailFrom>");
                sb.Append(blast.EmailFrom);
                sb.Append("</EmailFrom><EmailFromName>");
                sb.Append(cleanXMLString(blast.EmailFromName));
                sb.Append("</EmailFromName><ReplyTo>");
                sb.Append(blast.ReplyTo);
                sb.Append("</ReplyTo><BlastType>");
                sb.Append(blast.BlastType);
                sb.Append("</BlastType><SendTime>");
                sb.Append(blast.SendTime);
                sb.Append("</SendTime><TestBlast>");
                sb.Append(blast.TestBlast);
                sb.Append("</TestBlast></Blast>");
            }
            sb.Append("</DocumentElement>");
            return sb.ToString();
        }

        private string BuildISPString(string searchXML)
        {
            string ISPs = "";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(searchXML);

                XmlNode node = doc.SelectSingleNode("//ISPs");
                if (node != null)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "ISP":
                                ISPs += childNode.InnerText.Trim() + ",";
                                break;
                            default:
                                break;
                        }
                    }
                    ISPs = ISPs.Trim();
                    if (ISPs.Length > 0)
                    {
                        ISPs = ISPs.Remove(ISPs.Length - 1, 1);
                    }
                }
            }
            catch
            {
            }

            return ISPs;
        }

        private int GetTestBlastCount(int customerID)
        {
            int count = 0;
            ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);

            if (c.TestBlastLimit.HasValue)
            {
                count = c.TestBlastLimit.Value;
            }
            else
            {
                ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);
                if (bc.TestBlastLimit.HasValue)
                {
                    count = bc.TestBlastLimit.Value;
                }
                else
                {
                    count = 10;
                }
            }


            return count;
        }

        private string EnsureBlastValues(int customerID, int layoutID, int groupID, int filterID, string subject, ref bool isSmartSegment, DateTime sendTime, KMPlatform.Entity.User user, string refBlasts = "")
        {
            isSmartSegment = false;
            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(groupID, customerID))
            {
                return "List ID";
            }
            if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(layoutID, customerID))
            {
                return "Message ID";
            }
            if (filterID > 0)
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Filter.Exists(filterID, customerID))
                {
                    if (ECN_Framework_BusinessLayer.Communicator.SmartSegment.SmartSegmentOldExists(filterID) && refBlasts.Trim() != string.Empty)
                    {
                        isSmartSegment = true;
                        if (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(refBlasts, customerID, sendTime))
                        {
                            return "Filter ID";
                        }
                    }
                    else
                    {
                        return "Filter ID";
                    }
                }
            }
            try
            {
                System.Collections.Generic.List<string> listLY = ECN_Framework_BusinessLayer.Communicator.Layout.ValidateLayoutContent(layoutID);
                listLY.Add(subject.Trim().ToLower());
                ECN_Framework_BusinessLayer.Communicator.Group.ValidateDynamicStrings(listLY, groupID, user);
            }
            catch (ECN_Framework_Common.Objects.ECNException)
            {
                return "Invalid Codesnippet";
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                return "Invalid Codesnippet";
            }
            catch (Exception ex)
            {
                LogUnspecifiedException(ex, "ecn.webservice.BlastManager.AddBlast");
                return "Invalid Codesnippet";
            }

            return "";
        }

        private string BlastCheck(int CustomerID, int GroupID, int FilterID, int SmartSegmentID, string refBlastID, string Suppressionlist, KMPlatform.Entity.User user)
        {
            string actionType = "";
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listFilters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            if (FilterID > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                cibf.FilterID = FilterID;
                cibf.CampaignItemTestBlastID = 1;//setting this to 1 so it will work
                listFilters.Add(cibf);
            }
         
            return ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailsListCount(CustomerID, 0, GroupID, listFilters, "", "", true, user).ToString();
        }

        private IList<ECN_Framework_Entities.Communicator.BlastAbstract> BuildBlastSearchParams(string searchXML, int customerID, KMPlatform.Entity.User user)
        {
            string emailSubject = string.Empty;
            int? userID = null;
            int? groupID = null;
            bool? isTest = null;
            string statusCode = string.Empty;
            DateTime? modifiedFrom = null;
            DateTime? modifiedTo = null;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(searchXML);

            XmlNode node = doc.SelectSingleNode("//SearchField/Subject");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                emailSubject = node.InnerText.Trim();
            }
            node = doc.SelectSingleNode("//SearchField/User");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                KMPlatform.Entity.User searchUser = KMPlatform.BusinessLogic.User.GetByUserName(node.InnerText.Trim(), customerID, false);
                if (searchUser != null)
                    userID = searchUser.UserID;
            }
            node = doc.SelectSingleNode("//SearchField/Group");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                groupID = Convert.ToInt32(node.InnerText.Trim());
            }
            node = doc.SelectSingleNode("//SearchField/Test");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                if (node.InnerText.Trim().ToUpper() == "Y")
                    isTest = true;
                if (node.InnerText.Trim().ToUpper() == "N")
                    isTest = false;
            }
            node = doc.SelectSingleNode("//SearchField/Status");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                statusCode = node.InnerText.Trim();
            }
            node = doc.SelectSingleNode("//SearchField/ModifiedFrom");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                modifiedFrom = Convert.ToDateTime(node.InnerText.Trim());
            }
            node = doc.SelectSingleNode("//SearchField/ModifiedTo");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                modifiedTo = Convert.ToDateTime(node.InnerText.Trim());
            }
            return BlastsManager.GetBySearch(customerID, emailSubject, userID, groupID, isTest, statusCode, modifiedFrom, modifiedTo, null, "", "", user, false);
        }

        private string cleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        private string ModifyGeneratedXML(StringWriter inputXML, bool withDetail, string[] fieldsToUse)
        {
            XmlDocument newDoc = new XmlDocument();
            XmlElement newRoot = newDoc.CreateElement("Reports");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(inputXML.ToString());
            XmlNodeList nodeList;
            XmlNode root = doc.DocumentElement;
            nodeList = root.SelectNodes("/DocumentElement/Report");

            foreach (XmlNode childNode in nodeList)
            {
                XmlElement newElement = newDoc.CreateElement("Report");
                if (withDetail)
                {
                    foreach (XmlNode innerChildNode in childNode.ChildNodes)
                    {
                        if (innerChildNode.InnerText != null && innerChildNode.InnerText != string.Empty)
                        {
                            if (innerChildNode.Name.ToLower().Equals("link"))
                            {
                                //innerChildNode.InnerText = "<![CDATA[" + innerChildNode.InnerText + "]]>";
                                XmlCDataSection cData = doc.CreateCDataSection(innerChildNode.InnerText);
                                innerChildNode.InnerXml = cData.OuterXml;
                                newElement.AppendChild(newDoc.ImportNode(innerChildNode, true));
                            }
                            else
                            {
                                newElement.AppendChild(newDoc.ImportNode(innerChildNode, true));
                            }
                        }
                    }
                    newRoot.AppendChild(newElement);
                }
                else
                {
                    for (int i = 0; i < fieldsToUse.Length; i++)
                    {
                        if (childNode.SelectSingleNode(fieldsToUse[i]) != null && childNode.SelectSingleNode(fieldsToUse[i]).InnerText != null && childNode.SelectSingleNode(fieldsToUse[i]).InnerText != string.Empty)
                        {
                            if (fieldsToUse[i].ToLower().Equals("link"))
                            {
                                XmlCDataSection cData = doc.CreateCDataSection(childNode.SelectSingleNode(fieldsToUse[i]).InnerText);
                                childNode.SelectSingleNode(fieldsToUse[i]).InnerXml = cData.OuterXml;
                                newElement.AppendChild(newDoc.ImportNode(childNode.SelectSingleNode(fieldsToUse[i]), true));
                            }
                            else
                            {
                                newElement.AppendChild(newDoc.ImportNode(childNode.SelectSingleNode(fieldsToUse[i]), true));
                            }
                            newRoot.AppendChild(newElement);
                        }
                    }
                }
                newDoc.AppendChild(newRoot);
            }

            return newDoc.InnerXml;
        }
    }
}