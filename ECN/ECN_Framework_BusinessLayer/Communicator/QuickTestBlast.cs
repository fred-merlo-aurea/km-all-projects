using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using KMPlatform.Entity;
using Client = KMPlatform.BusinessLogic.Client;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class QuickTestBlast
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Blast;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.QuickTestBlast;


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

        public static void Validate(string emailsToAdd, int layoutId, bool emailPreview, string emailFrom, string replyTo, string fromName, string emailSubject, int customerId, int? groupId, string groupName, int baseChannelId, int? campaignId, string campaignName, int? campaignItemId, string campaignItemName, User currentUser, ECN_Framework_Entities.Communicator.QuickTestBlastConfig qtb)
        {
            //get test blast limit and verify
            var errorList = new List<string>();
            var testBlastLimit = 10;

            var channelId = BaseChannel.GetByBaseChannelID(baseChannelId);
            if (channelId.TestBlastLimit.HasValue && channelId.TestBlastLimit.Value > 0)
            {
                testBlastLimit = channelId.TestBlastLimit.Value;
            }

            var customer = Customer.GetByCustomerID(customerId, false);
            if (customer.TestBlastLimit.HasValue && customer.TestBlastLimit.Value > 0)
            {
                testBlastLimit = customer.TestBlastLimit.Value;
            }
            
            emailsToAdd = ValidateEmailsToAdd(emailsToAdd, groupId, groupName, qtb, errorList, testBlastLimit);

            // Campaign/CampaignItem validation
            ValidateCampaign(campaignId, campaignName, campaignItemId, campaignItemName, currentUser, errorList);

            // Group/Emails validation
            ValidateGroupEmails(emailsToAdd, customerId, groupId, groupName, currentUser, qtb, errorList, testBlastLimit);

            // Validate links for email preview
            ValidateEmailPreviewLinks(layoutId, emailPreview, customerId, errorList);

            // validate envelope info
            ValidateEnvelopeInfo(emailFrom, replyTo, fromName, emailSubject, errorList);

            // validate layout
            ValidateLayout(layoutId, customerId, currentUser, errorList);

            if (errorList.Count > 0)
            {
                throw new ECNException(
                    errorList
                        .Select(message => new ECNError(Enums.Entity.QuickTestBlast, Enums.Method.Validate, message))
                        .ToList());
            }
        }

        private static string ValidateEmailsToAdd(string emailsToAdd, int? groupId, string groupName, ECN_Framework_Entities.Communicator.QuickTestBlastConfig qtb, List<string> errorList, int testBlastLimit)
        {
            if (emailsToAdd == null)
            {
                emailsToAdd = string.Empty;
            }

            emailsToAdd = emailsToAdd.TrimEnd(',');
            emailsToAdd = emailsToAdd.Replace("\r\n", ",").Replace("\n", ",");

            var tokenizer = new StringTokenizer(emailsToAdd, ',');
            if (!groupId.HasValue || groupId.Value <= 0)
            {
                if (!qtb.AllowAdhocEmails.Value)
                {
                    errorList.Add("Configuration does not allow using Ad-Hoc emails");
                }
                else if (qtb.AutoCreateGroup.Value && !string.IsNullOrWhiteSpace(groupName))
                {
                    errorList.Add("Configuration does not allow specifying a group name");
                }
                else if (!qtb.AutoCreateGroup.Value && string.IsNullOrWhiteSpace(groupName))
                {
                    errorList.Add("Configuration requires you to specify a group name");
                }
                else if (tokenizer.CountTokens() > testBlastLimit)
                {
                    errorList.Add("Emails count exceeds test blast limit");
                }
                else if (string.IsNullOrWhiteSpace(emailsToAdd))
                {
                    errorList.Add("Addresses cannot be empty");
                }

                while (tokenizer.HasMoreTokens())
                {
                    var email = tokenizer.NextToken().Trim();

                    if (!Email.IsValidEmailAddress(email))
                    {
                        errorList.Add("Invalid Email Address: " + email);
                    }
                }
            }

            return emailsToAdd;
        }

        private static void ValidateCampaign(int? campaignId, string campaignName, int? campaignItemId, string campaignItemName, User currentUser, List<string> errorList)
        {
            if (campaignId.HasValue && campaignId.Value > 0)
            {
                var camp = Campaign.GetByCampaignID(campaignId.Value, currentUser, false);
                if (camp == null)
                {
                    errorList.Add("CampaignID does not exist");
                }
                else if (camp.IsArchived.HasValue && camp.IsArchived.Value)
                {
                    errorList.Add("Campaign is archived");
                }
            }

            if (campaignId.HasValue && campaignId.Value > 0 && campaignItemId.HasValue && campaignItemId.Value > 0)
            {
                errorList.Add("Cannot supply CampaignID and CampaignItemID");
            }
            else if (campaignId.HasValue && campaignId.Value > 0 && !string.IsNullOrWhiteSpace(campaignName))
            {
                errorList.Add("Cannot supply CampaignID and CampaignName");
            }
            else if (campaignItemId.HasValue && campaignItemId.Value > 0 && !string.IsNullOrWhiteSpace(campaignItemName))
            {
                errorList.Add("Cannot supply CampaignItemID and CampaignItemName");
            }
            else if (campaignItemId.HasValue && campaignItemId.Value > 0 && !string.IsNullOrEmpty(campaignName))
            {
                errorList.Add("Cannot supply CampaignItemID and CampaignName");
            }

            if (campaignItemId.HasValue && campaignItemId.Value > 0)
            {
                var campaignItem = CampaignItem.GetByCampaignItemID(campaignItemId.Value, currentUser, false);
                if (campaignItem == null)
                {
                    errorList.Add("CampaignItemID does not exist");
                }
            }

            if (campaignItemName.Length > 50)
            {
                errorList.Add("CampaignItemName must be less than or equal to 50 characters");
            }
        }

        private static void ValidateGroupEmails(string emailsToAdd, int customerId, int? groupId, string groupName, User currentUser, ECN_Framework_Entities.Communicator.QuickTestBlastConfig qtb, List<string> errorList, int testBlastLimit)
        {
            if (groupId.HasValue && groupId.Value > 0 && !string.IsNullOrWhiteSpace(groupName))
            {
                errorList.Add("Cannot supply GroupID and GroupName");
            }
            else if (groupId.HasValue && groupId.Value > 0 && !string.IsNullOrWhiteSpace(emailsToAdd))
            {
                errorList.Add("Cannot supply GroupID and EmailAddresses");
            }
            else if ((!groupId.HasValue || groupId <= 0) && string.IsNullOrWhiteSpace(emailsToAdd))
            {
                errorList.Add("Group not selected");
            }
            else if ((!groupId.HasValue || groupId.Value <= 0) && !qtb.AutoCreateGroup.Value && string.IsNullOrWhiteSpace(groupName))
            {
                errorList.Add("Group name cannot be empty");
            }
            else if (groupId.HasValue && groupId.Value > 0 && !Group.Exists(groupId.Value, customerId))
            {
                errorList.Add("Selected group does not exist");
            }
            else if (groupId.HasValue && groupId.Value > 0 && Group.IsArchived(groupId.Value, customerId))
            {
                errorList.Add("Selected group is archived");
            }
            else
            {
                if (groupId.HasValue && groupId.Value > 0)
                {
                    //verify group sub count is less than allow limit
                    var subsCount = EmailGroup.GetSubscriberCount(groupId.Value, customerId, currentUser);
                    if (subsCount > testBlastLimit)
                    {
                        errorList.Add("Selected group exceeds the test blast limit");
                    }
                }
            }

            if (!qtb.AllowAdhocEmails.Value && !string.IsNullOrWhiteSpace(emailsToAdd))
            {
                errorList.Add("Configuration does not allow Ad Hoc emails");
            }
        }

        private static void ValidateEmailPreviewLinks(int layoutId, bool emailPreview, int customerId, List<string> errorList)
        {
            if (emailPreview)
            {
                var cust = Customer.GetByCustomerID(customerId, false);
                if (!Client.HasServiceFeature(cust.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreview))
                {
                    errorList.Add("Email Preview functionality not enabled");
                }
                else
                {
                    try
                    {
                        Content.ValidateLinks(layoutId);
                    }
                    catch (ECNException ecn)
                    {
                        foreach (var r in ecn.ErrorList)
                        {
                            errorList.Add(r.ErrorMessage);
                        }
                    }
                }
            }
        }

        private static void ValidateEnvelopeInfo(string emailFrom, string replyTo, string fromName, string emailSubject, List<string> errorList)
        {
            if (emailFrom == "Select From Email" || replyTo == "Select Reply To Email" || fromName == "Select From Name" || emailFrom == "" || replyTo == "" || fromName == "")
            {
                errorList.Add("Incomplete Envelope Information");
            }

            if (emailFrom != string.Empty && !Email.IsValidEmailAddress(emailFrom))
            {
                errorList.Add("FromEmail is invalid");
            }

            if (replyTo != string.Empty && !Email.IsValidEmailAddress(replyTo))
            {
                errorList.Add("ReplyTo is invalid");
            }

            if (emailSubject == "")
            {
                errorList.Add("EmailSubject cannot be empty");
                throw new ECNException(
                    errorList
                        .Select(message => new ECNError(Enums.Entity.QuickTestBlast, Enums.Method.Validate, message))
                        .ToList());
            }

            var cleanEmailSubject = CleanAndStripHTML(emailSubject);
            cleanEmailSubject = cleanEmailSubject.Replace("&nbsp;", " ").Trim();
            if (cleanEmailSubject.Length > 255)
            {
                errorList.Add("EmailSubject cannot be more than 255 characters");
            }
        }

        private static void ValidateLayout(int layoutId, int customerId, User currentUser, List<string> errorList)
        {
            if (layoutId <= 0)
            {
                errorList.Add("Message Layout not selected");
            }
            else
            {
                if (!Layout.Exists(layoutId, customerId))
                {
                    errorList.Add("Message does not exist");
                }
                else
                {
                    var lToCheck = Layout.GetByLayoutID(layoutId, currentUser, false);
                    if (lToCheck.Archived.HasValue && lToCheck.Archived.Value)
                    {
                        errorList.Add("Message is archived");
                    }
                }
            }
        }

        public static int CreateQuickTestBlast(int customerID, int baseChannelID, int? groupID, string groupName, string emailsInGroup, int layoutID, int? campaignItemID, string campaignItemName, int? campaignID, string campaignName, bool emailPreview, bool enableCacheBuster, bool sendTextVersion, string emailFrom, string replyTo, string fromName, string emailSubject, KMPlatform.Entity.User CurrentUser)
        {
            if (!HasPermission(KMPlatform.Enums.Access.Edit, CurrentUser))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            ECN_Framework_Entities.Communicator.QuickTestBlastConfig QTB = ECN_Framework_BusinessLayer.Communicator.QuickTestBlastConfig.GetByBaseChannelID(baseChannelID);
            if (QTB != null && QTB.CustomerCanOverride.HasValue && QTB.CustomerCanOverride.Value)
            {
                ECN_Framework_Entities.Communicator.QuickTestBlastConfig QTBcust = ECN_Framework_BusinessLayer.Communicator.QuickTestBlastConfig.GetByCustomerID(customerID);
                if (QTBcust != null && QTBcust.CustomerDoesOverride.HasValue && QTBcust.CustomerDoesOverride.Value)
                {
                    QTB = QTBcust;
                    QTB.BaseChannelDoesOverride = true;
                }
            }
            if (QTB != null && QTB.BaseChannelDoesOverride.HasValue && QTB.BaseChannelDoesOverride.Value)
            {

            }
            else
            {
                QTB = ECN_Framework_BusinessLayer.Communicator.QuickTestBlastConfig.GetKMDefaultConfig();
            }



            Validate(emailsInGroup, layoutID, emailPreview, emailFrom, replyTo, fromName, emailSubject, customerID, groupID, groupName, baseChannelID, campaignID, campaignName, campaignItemID, campaignItemName, CurrentUser, QTB);

            ECN_Framework_Entities.Communicator.Layout lForName = null;
            if (string.IsNullOrWhiteSpace(groupName))
            {
                try
                {
                    if (QTB.AutoCreateGroup.Value && !string.IsNullOrEmpty(emailsInGroup))
                    {
                        lForName = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, CurrentUser, false);

                        string layoutTriggerTxt = lForName.LayoutName.Length > 28 ? lForName.LayoutName.Substring(0, 28) : lForName.LayoutName;
                        groupName = "QTB_" + layoutTriggerTxt + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss", CultureInfo.InvariantCulture);
                    }

                }
                catch { }
            }

            StringBuilder xmlInsert = new StringBuilder();
            ECN_Framework_Entities.Communicator.Campaign camp = new ECN_Framework_Entities.Communicator.Campaign();
            ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
            ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
            bool createCampaign = false;
            bool saveCIB = false;
            if (!campaignItemID.HasValue || (campaignItemID.HasValue && campaignItemID.Value <= 0))//no campaign item id so this is a new campaign/campaign item
            {
                if (campaignID.HasValue && campaignID.Value > 0)
                {
                    camp = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(campaignID.Value, CurrentUser, false);
                }
                else
                {

                    createCampaign = true;
                    camp.CampaignName = campaignName;
                    camp.CustomerID = customerID;
                    camp.CreatedDate = DateTime.Now;
                    camp.CreatedUserID = CurrentUser.UserID;

                }

            }
            else
            {
                ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID.Value, CurrentUser, true);

                camp = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(ci.CampaignID.Value, CurrentUser, false);

            }


            string emailAddressToAdd = emailsInGroup;
            if (!string.IsNullOrEmpty(emailAddressToAdd) && !groupID.HasValue)
            {

            }

            int folderID = -1;
            if (!ECN_Framework_BusinessLayer.Communicator.Folder.Exists(-1, "Quick Test Groups", 0, customerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString()))
            {
                ECN_Framework_Entities.Communicator.Folder newFolder = new ECN_Framework_Entities.Communicator.Folder();
                newFolder.CreatedDate = DateTime.Now;
                newFolder.CreatedUserID = CurrentUser.UserID;
                newFolder.CustomerID = customerID;
                newFolder.FolderDescription = "Quick Test Groups";
                newFolder.FolderName = "Quick Test Groups";
                newFolder.FolderType = "GRP";
                newFolder.IsDeleted = false;
                newFolder.IsSystem = false;
                newFolder.ParentID = 0;

                folderID = ECN_Framework_BusinessLayer.Communicator.Folder.Save(newFolder, CurrentUser);
            }
            else
            {
                // Method for getting folder by name/type
                folderID = ECN_Framework_BusinessLayer.Communicator.Folder.GetFolderIDByName(customerID, "Quick Test Groups", "GRP");
            }

            if (!groupID.HasValue)//we're creating a new group and inserting emails for it
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",").Replace("\n", ",");
                if (emailAddressToAdd.LastIndexOf(",") == (emailAddressToAdd.Length - 1))
                    emailAddressToAdd = emailAddressToAdd.Substring(0, emailAddressToAdd.Length - 1);
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                group.GroupName = groupName;
                group.GroupDescription = groupName;
                group.PublicFolder = 1; //pub_folder;
                group.AllowUDFHistory = "N";
                group.OwnerTypeCode = "customer";
                group.CreatedUserID = CurrentUser.UserID;
                group.CustomerID = customerID;
                group.FolderID = folderID;
                group.IsSeedList = false;

                //Add emails to group                
                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                while (st.HasMoreTokens())
                {
                    string email = st.NextToken().Trim();
                    xmlInsert.Append("<Emails><emailaddress>" + email + "</emailaddress></Emails>");
                }

            }
            else
            {
                group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID.Value, CurrentUser);
            }

            string cleanEmailSubject = CleanAndStripHTML(emailSubject);
            cleanEmailSubject = cleanEmailSubject.Replace("&nbsp;", " ").Trim();// txtSubject.Text;
            if (campaignItemID.HasValue)
            {

                //ci.FromEmail = emailFrom.Trim();
                //ci.FromName = fromName.Trim();
                //ci.ReplyTo = replyTo.Trim();

                //ci.CampaignID = camp.CampaignID;
                ci.UpdatedUserID = CurrentUser.UserID;
                ci.CustomerID = customerID;
                ci.EnableCacheBuster = enableCacheBuster;
                cib = ci.BlastList.OrderByDescending(u => u.CampaignItemBlastID).FirstOrDefault();
                if (cib == null)
                {
                    cib = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    cib.CampaignItemID = campaignItemID.Value;
                    cib.EmailSubject = cleanEmailSubject;
                    cib.FromName = ci.FromName;
                    cib.EmailFrom = ci.FromEmail;
                    cib.ReplyTo = ci.ReplyTo;
                    cib.LayoutID = layoutID;
                    
                    //cib.GroupID = group.GroupID;
                    cib.CreatedUserID = CurrentUser.UserID;
                    cib.CustomerID = CurrentUser.CustomerID;
                    ci.BlastList.Add(cib);
                }
            }
            else
            {
                ci.FromEmail = emailFrom.Trim();
                ci.FromName = fromName.Trim();
                ci.ReplyTo = replyTo.Trim();
                //ci.CampaignID = camp.CampaignID;
                ci.IgnoreSuppression = false;
                ci.CampaignItemFormatType = "HTML";
                ci.CampaignItemName = campaignItemName.Trim();
                ci.CampaignItemType = "Regular";
                ci.CompletedStep = 0;

                ci.CreatedUserID = CurrentUser.UserID;
                ci.CustomerID = customerID;
                ci.EnableCacheBuster = enableCacheBuster;
                ci.IsDeleted = false;
                ci.CampaignItemNameOriginal = campaignItemName.Trim();
                ci.IsHidden = false;

                // Campaign Item Blast
                if (ci.BlastList == null || ci.BlastList.Count == 0)
                {
                    cib.EmailSubject = cleanEmailSubject;
                    cib.FromName = ci.FromName;
                    cib.EmailFrom = ci.FromEmail;
                    cib.ReplyTo = ci.ReplyTo;
                    cib.LayoutID = layoutID;
                    cib.GroupID = group.GroupID;
                    cib.CreatedUserID = CurrentUser.UserID;
                    cib.CustomerID = CurrentUser.CustomerID;
                    ci.BlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
                    ci.BlastList.Add(cib);
                    saveCIB = true;
                }
            }

            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTest = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast();

            ciTest.HasEmailPreview = emailPreview;
            ciTest.CreatedUserID = CurrentUser.UserID;
            ciTest.CustomerID = customerID;

            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTestText = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast();

            if (sendTextVersion)
            {
                ciTestText.GroupID = group.GroupID;
                ciTestText.HasEmailPreview = emailPreview;
                ciTestText.CreatedUserID = CurrentUser.UserID;
                ciTestText.CustomerID = customerID;
                ciTestText.CampaignItemTestBlastType = "TEXT";
            }
            int campaignItemTestBlastID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                if (createCampaign)
                    camp.CampaignID = ECN_Framework_BusinessLayer.Communicator.Campaign.Save(camp, CurrentUser);

                if ((!groupID.HasValue || groupID.Value <= 0) && !string.IsNullOrEmpty(emailsInGroup))
                {
                    // Create Group
                    group.GroupID = ECN_Framework_BusinessLayer.Communicator.Group.Save(group, CurrentUser);
                    DataTable resultsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(CurrentUser, customerID, group.GroupID, xmlInsert.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "html", "S", true, customerID + " " + Guid.NewGuid().ToString(), "QuickTestBlast.Save - SubscribeType S");

                    // Create necesary UDFs
                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(layoutID, false);
                    ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                    List<string> templateCheck = new List<string>();
                    templateCheck.Add(template.TemplateSource);
                    templateCheck.Add(template.TemplateText);
                    List<string> listNoExist = new List<string>();
                    listNoExist.AddRange(GetShortNamesForLayoutTemplate(templateCheck, group.GroupID, CurrentUser));

                    List<string> listLY = ECN_Framework_BusinessLayer.Communicator.Layout.ValidateLayoutContent(layoutID);
                    if (listLY == null)
                        listLY = new System.Collections.Generic.List<string>();
                    if (cib.DynamicFromName.Trim().Length > 0)
                    {
                        listLY.Add(cib.DynamicFromName.Trim().ToLower());
                    }
                    if (cib.DynamicFromEmail.Trim().Length > 0)
                    {
                        listLY.Add(cib.DynamicFromEmail.Trim().ToLower());
                    }
                    if (cib.DynamicReplyTo.Trim().Length > 0)
                    {
                        listLY.Add(cib.DynamicReplyTo.Trim().ToLower());
                    }
                    if (cib.EmailSubject.Trim().Length > 0)
                    {
                        listLY.Add(cib.EmailSubject.Trim().ToLower());
                    }
                    listNoExist.AddRange(GetShortNamesForDynamicStrings(listLY, group.GroupID, CurrentUser));

                    if (listNoExist.Count > 0)
                    {
                        foreach (string s in listNoExist)
                        {
                            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFeilds = new ECN_Framework_Entities.Communicator.GroupDataFields();
                            groupDataFeilds.ShortName = s;
                            groupDataFeilds.LongName = s;
                            groupDataFeilds.GroupID = group.GroupID;
                            groupDataFeilds.IsPublic = "N";
                            groupDataFeilds.CreatedUserID = CurrentUser.UserID;
                            groupDataFeilds.CustomerID = customerID;

                            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFeilds, CurrentUser);
                        }
                    }
                }
                
                ci.CampaignID = camp.CampaignID;
                ci.CampaignItemID = ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, CurrentUser);
                if(saveCIB)
                {
                    cib.CampaignItemID = ci.CampaignItemID;
                    cib.GroupID = group.GroupID;
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, CurrentUser, false);
                }
                ciTest.CampaignItemID = ci.CampaignItemID;
                ciTest.GroupID = group.GroupID;
                ciTest.EmailSubject = cleanEmailSubject;
                ciTest.FromName = fromName.Trim();
                ciTest.FromEmail = emailFrom.Trim();
                ciTest.ReplyTo = replyTo.Trim();
                ciTest.LayoutID = layoutID;
                ciTestText.GroupID = group.GroupID;
                ciTestText.CampaignItemID = ci.CampaignItemID;
                ciTestText.EmailSubject = cleanEmailSubject;
                ciTestText.FromName = fromName.Trim();
                ciTestText.FromEmail = emailFrom.Trim();
                ciTestText.ReplyTo = replyTo.Trim();
                ciTestText.LayoutID = layoutID;
                //if (campaignItemID == null && cib.CampaignItemBlastID < 0)
                //{
                //    cib.GroupID = group.GroupID;
                //    cib.EmailSubject = cleanEmailSubject;
                //    cib.FromName = ci.FromName;
                //    cib.EmailFrom = ci.FromEmail;
                //    cib.ReplyTo = ci.ReplyTo;
                //    cib.LayoutID = layoutID;
                //    cib.CreatedUserID = CurrentUser.UserID;
                //    cib.CampaignItemID = ci.CampaignItemID;
                //    cib.CampaignItemBlastID = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, CurrentUser);
                //}
                //else
                //{
                //    cib.UpdatedUserID = CurrentUser.UserID;
                //    cib.CampaignItemID = ci.CampaignItemID;
                //    cib.CampaignItemBlastID = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(cib, CurrentUser);
                //}


                campaignItemTestBlastID = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.Insert(ciTest, CurrentUser, true);
                if (sendTextVersion)
                {
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.Insert(ciTestText, CurrentUser, true);
                }

                scope.Complete();
            }

            if (!string.IsNullOrEmpty(groupName) && QTB.AutoArchiveGroup.Value)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ECN_Framework_BusinessLayer.Communicator.Group.Archive(group.GroupID, true, customerID, CurrentUser);
                    scope.Complete();
                }
            }

            ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemTestBlastID(campaignItemTestBlastID, CurrentUser, false);

            return b.BlastID;
        }


        private static List<string> GetShortNamesForLayoutTemplate(List<string> listLY, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Group;
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            DataTable dtEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            System.Collections.Generic.List<string> listCS = new System.Collections.Generic.List<string>();

            foreach (DataRow dr in dtEmail.Rows)
            {
                listCS.Add(dr["columnName"].ToString().ToLower());
            }
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
            {
                listCS.Add(gdf.ShortName.ToLower());
            }

            listCS.Add("emailaddress");
            listCS.Add("formattypecode");
            listCS.Add("subscribetypecode");
            listCS.Add("title");
            listCS.Add("firstname");
            listCS.Add("lastname");
            listCS.Add("fullname");
            listCS.Add("company");
            listCS.Add("occupation");
            listCS.Add("address");
            listCS.Add("address2");
            listCS.Add("city");
            listCS.Add("state");
            listCS.Add("zip");
            listCS.Add("country");
            listCS.Add("voice");
            listCS.Add("mobile");
            listCS.Add("fax");
            listCS.Add("website");
            listCS.Add("age");
            listCS.Add("income");
            listCS.Add("gender");
            listCS.Add("user1");
            listCS.Add("user2");
            listCS.Add("user3");
            listCS.Add("user4");
            listCS.Add("user5");
            listCS.Add("user6");
            listCS.Add("birthdate");
            listCS.Add("userevent1");
            listCS.Add("userevent1date");
            listCS.Add("userevent2");
            listCS.Add("userevent2date");
            listCS.Add("notes");

            System.Collections.Generic.List<string> subLY = new System.Collections.Generic.List<string>();
            foreach (string s in listLY)
            {
                #region Badly Formed Snippets
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        //return error
                        errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {
                            //return error
                            errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                        }
                    }
                }
                #endregion

                //%% and ##
                System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList1 = reg1.Matches(s);

                foreach (System.Text.RegularExpressions.Match m in MatchList1)
                {
                    if (!string.IsNullOrEmpty(m.Value.ToString()))
                    {
                        if (!subLY.Contains(m.Value.ToString().ToLower().Replace("%%", string.Empty)))
                            subLY.Add(m.Value.ToString().ToLower().Replace("%%", string.Empty));
                    }
                }
            }
            System.Collections.Generic.List<string> listNoExist = new System.Collections.Generic.List<string>();
            foreach (string s in subLY)
            {
                if (!listCS.Contains(s))
                    listNoExist.Add(s);
            }

            listNoExist.Remove("blastid");
            listNoExist.Remove("groupid");
            listNoExist.Remove("groupname");
            listNoExist.Remove("emailtofriend");
            listNoExist.Remove("conversiontrkcde");
            listNoExist.Remove("unsubscribelink");
            listNoExist.Remove("lastchanged");
            listNoExist.Remove("createdon");
            listNoExist.Remove("publicview");
            listNoExist.Remove("company_address");
            listNoExist.Remove("surveytitle");
            listNoExist.Remove("surveylink");
            listNoExist.Remove("currdate");
            listNoExist.Remove("reportabuselink");
            listNoExist.Remove("profilepreferences");
            listNoExist.Remove("emailfromaddress");

            listNoExist.Remove("customer_name");
            listNoExist.Remove("customer_address");
            listNoExist.Remove("customer_webaddress");

            listNoExist.Remove("customer_udf1");
            listNoExist.Remove("customer_udf2");
            listNoExist.Remove("customer_udf3");
            listNoExist.Remove("customer_udf4");
            listNoExist.Remove("customer_udf5");

            listNoExist.Remove("slot1");
            listNoExist.Remove("slot2");
            listNoExist.Remove("slot3");
            listNoExist.Remove("slot4");
            listNoExist.Remove("slot5");
            listNoExist.Remove("slot6");
            listNoExist.Remove("slot7");
            listNoExist.Remove("slot8");
            listNoExist.Remove("slot9");
            listNoExist.Remove("slot10");

            return listNoExist;
        }

        private static List<string> GetShortNamesForDynamicStrings(List<string> listLY, int groupID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Group;
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            DataTable dtEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(groupID);

            System.Collections.Generic.List<string> listCS = new System.Collections.Generic.List<string>();

            foreach (DataRow dr in dtEmail.Rows)
            {
                listCS.Add(dr["columnName"].ToString().ToLower());
            }
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
            {
                listCS.Add(gdf.ShortName.ToLower());
            }

            System.Collections.Generic.List<string> subLY = new System.Collections.Generic.List<string>();
            foreach (string s in listLY)
            {
                #region Badly Formed Snippets
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        //return error
                        errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {
                            //return error
                            errorList.Add(new ECNError(Entity, Method, "Incorrectly formed code snippet"));
                        }
                    }
                }
                #endregion

                //%% and ##
                System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("%%.+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchCollection MatchList1 = reg1.Matches(s);

                foreach (System.Text.RegularExpressions.Match m in MatchList1)
                {
                    if (!string.IsNullOrEmpty(m.Value.ToString()))
                    {
                        if (!subLY.Contains(m.Value.ToString().Replace("%%", string.Empty)))
                            subLY.Add(m.Value.ToString().Replace("%%", string.Empty));
                    }
                }
            }
            System.Collections.Generic.List<string> listNoExist = new System.Collections.Generic.List<string>();
            foreach (string s in subLY)
            {
                if (!listCS.Contains(s))
                    listNoExist.Add(s);
            }

            listNoExist.Remove("blastid");
            listNoExist.Remove("groupid");
            listNoExist.Remove("groupname");
            listNoExist.Remove("emailtofriend");
            listNoExist.Remove("conversiontrkcde");
            listNoExist.Remove("unsubscribelink");
            listNoExist.Remove("lastchanged");
            listNoExist.Remove("createdon");
            listNoExist.Remove("publicview");
            listNoExist.Remove("company_address");
            listNoExist.Remove("surveytitle");
            listNoExist.Remove("surveylink");
            listNoExist.Remove("currdate");
            listNoExist.Remove("reportabuselink");

            listNoExist.Remove("customer_name");
            listNoExist.Remove("customer_address");
            listNoExist.Remove("customer_webaddress");

            listNoExist.Remove("customer_udf1");
            listNoExist.Remove("customer_udf2");
            listNoExist.Remove("customer_udf3");
            listNoExist.Remove("customer_udf4");
            listNoExist.Remove("customer_udf5");

            return listNoExist;
        }
        private static string CleanAndStripHTML(string dirty)
        {
            string retString = "";
            Regex htmlStrip = new Regex("<.*?>");
            retString = htmlStrip.Replace(dirty, "");
            retString = retString.Replace("&gt;", ">");
            retString = retString.Replace("&lt;", "<");

            return retString;
        }


    }
}
