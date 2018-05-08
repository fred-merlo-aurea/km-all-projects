using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using ecn.activity.Areas.User.Models;
using KM.Common;
using KMPlatform.BusinessLogic.Interfaces;
using KMPlatform.Entity;
using BusinessLogic = KMPlatform.BusinessLogic;
using EntityUser = KMPlatform.Entity.User;
using ApplicationLog = KM.Common.Entity.ApplicationLog;

namespace ecn.activity.Areas.User.Controllers
{
    public class IndexController : Controller
    {
        private const string UrlAccept = "~/Areas/User/Views/Main/Accept.cshtml";
        private const string UrlThankYou = "~/Areas/User/Views/Main/ThankYou.cshtml";
        private const string ControllerError = "Error";
        private const string ActionError = "Error";
        private const string ErrorInvalidLink = "InvalidLink";
        private const string AreaDefault = "";
        private const string SourceMethodUserAccept = "User.Accept(Unknown Issue)";
        private const string AppSettingApplication = "KMCommon_Application";
        private const string ErrorHard = "HardError";
        private const string AppSettingsLogin = "KMPlatformLogin";
        private const string ErrorParseIntTemplate = "Coudn't parse {0} into int.";
        private const string ErrorRoleExists = 
            "A role already exists for this username/customer combination. Please contact your administrator for assistance.";
        private const string ErrorSystemAdministrator = 
            "User is a System Administrator and cannot have roles assigned to them.";
        private const string ErrorUserIsLocked = "User is locked and cannot have roles assigned to them.";
        private const string ErrorUserPasswordInvalid = "Username/password is invalid";
        private const int UserIdNone = -1;
        private const string ErrorUsernameExists = "Username already exists";
        private const string ErrorInvalidFormat = 
            "Invalid format. Please remove any leading and trailing white spaces from Username.";

        //
        // GET: /User/Index/

        public ActionResult Index()
        {
            return View();
        }

        #region controller view methods
        [HttpGet]
        public ActionResult Accept(string query)
        {
            string unencrypted = "";
            string rawURL = Request.ServerVariables["HTTP_URL"].ToString();
            query = rawURL.Substring(rawURL.ToLower().IndexOf("accept/") + 7);
            string redirectURL = string.Empty;
            try
            {
                KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(query), ecLink);

                unencrypted = unencrypted.StartsWith("?") ? unencrypted.Substring(1) : unencrypted;
            }
            catch
            {
                unencrypted = query.StartsWith("?") ? query.Substring(1) : query;
            }

            //Handling sys admins differently because they won't have a "valid" setID;
            string[] queryParams = unencrypted.Split('&');
            string setID = "";
            int currentUserID = -1;
            bool existing = false;
            if (unencrypted.ToLower().Contains("&userid"))
            {
                foreach (string s in queryParams)
                {
                    string[] nameValue = s.Split('=');
                    if (nameValue[0].ToLower().Equals("setid"))
                    {
                        setID = nameValue[1];
                    }
                    else if (nameValue[0].ToLower().Equals("userid"))
                    {
                        currentUserID = Convert.ToInt32(nameValue[1]);
                    }
                    else if (nameValue[0].ToLower().Equals("existing"))
                    {
                        existing = Convert.ToBoolean(nameValue[1]);
                    }
                }
            }
            else//leaving this for opt in emails that have already been sent out.
            {
                setID = unencrypted;
            }
            ecn.activity.Areas.User.Models.UserAcceptModel uam = new Models.UserAcceptModel();
            try
            {

                Guid SetID = new Guid();
                if (Guid.TryParse(setID, out SetID))
                {

                    uam.setID = SetID;
                    uam.SecurityGroupsToAccept = (new KMPlatform.BusinessLogic.SecurityGroupOptIn()).GetBySetID(uam.setID.Value);
                    KMPlatform.Entity.User currentUser = new KMPlatform.BusinessLogic.User().SelectUser(currentUserID, false);

                    if (currentUser.Status != KMPlatform.Enums.UserStatus.Locked)
                    {
                        if (uam.SecurityGroupsToAccept != null && uam.SecurityGroupsToAccept.Count > 0)
                        {
                            if (uam.SecurityGroupsToAccept.Any(x => x.HasAccepted == true))
                            {
                                uam.ValidForAccepting = false;
                            }
                            uam.PlatformLoginURL = ConfigurationManager.AppSettings["KMPlatformLogin"].ToString();
                            uam.UserID = uam.SecurityGroupsToAccept[0].UserID;
                            uam.CreatedByUserID = uam.SecurityGroupsToAccept[0].CreatedByUserID;
                            uam.IsSysAdmin = false;
                        }
                        else if (currentUser.IsPlatformAdministrator)//User must be a sys admin
                        {
                            uam.IsSysAdmin = true;
                            if (!currentUser.IsActive)
                            {
                                uam.UserID = currentUserID;
                                uam.ValidForAccepting = true;
                                uam.PlatformLoginURL = ConfigurationManager.AppSettings["KMPlatformLogin"].ToString();
                                uam.CreatedByUserID = currentUser.CreatedByUserID;
                            }
                            else
                            {
                                uam.ValidForAccepting = false;
                                return View("~/Areas/User/Views/Main/Accept.cshtml", uam);
                            }
                        }


                        uam.UserName = currentUser.UserName;
                        if (existing)
                        {
                            uam.NewUser = false;
                            uam.IsExistingUser = true;

                        }
                        else
                        {

                            if (currentUser.IsActive)
                            {
                                uam.NewUser = false;
                                uam.IsExistingUser = true;
                            }
                            else
                            {
                                uam.NewUser = true;
                                uam.IsExistingUser = false;
                            }
                        }
                        return View("~/Areas/User/Views/Main/Accept.cshtml", uam);
                    }
                    else
                    {
                        uam.ValidForAccepting = false;
                        uam.UserIsLocked = true;
                        return View("~/Areas/User/Views/Main/Accept.cshtml", uam);
                    }
                }
                else
                {
                    return RedirectToRoute(new { controller = "Error", action = "Error", error = "InvalidLink", area = "" });// errorMsgPanel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "User.Accept(Unknown Issue)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote(uam));
                return RedirectToRoute(new { controller = "Error", action = "Error", error = "HardError", area = "" });// errorMsgPanel.Visible = true;
            }

        }

        [HttpPost]
        public ActionResult Accept(UserAcceptModel acceptModel)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));

            acceptModel.ErrorMessage = string.Empty;
            try
            {
                if (acceptModel.setID.HasValue)
                {
                    ActionResult accept;
                    return ProcessSetId(acceptModel, out accept) ? accept : View(UrlThankYou);
                }
                else
                {
                    return RedirectToRoute(new
                    {
                        controller = ControllerError,
                        action = ActionError,
                        error = ErrorInvalidLink,
                        area = AreaDefault
                    });
                }
            }
            catch (Exception exception)
            {
                ApplicationLog.LogCriticalError(
                    exception,
                    SourceMethodUserAccept,
                    ToInt32WithThrows(ConfigurationManager.AppSettings[AppSettingApplication]), 
                    CreateNote(acceptModel));
                return RedirectToRoute(new
                {
                    controller = ControllerError,
                    action = ActionError,
                    error = ErrorHard,
                    area = AreaDefault
                });
            }
        }

        private bool ProcessSetId(UserAcceptModel acceptModel, out ActionResult accept)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));

            var groupMap = new BusinessLogic.UserClientSecurityGroupMap();

            var lstucsgm = groupMap.SelectForUser(acceptModel.UserID);
            acceptModel.SecurityGroupsToAccept =
                new BusinessLogic.SecurityGroupOptIn().GetBySetID(acceptModel.setID.Value);
            var userWorker = new BusinessLogic.User();
            if (acceptModel.NewUser)
            {
                if (!acceptModel.IsExistingUser)
                {
                    if (ProcessNonExistingUser(acceptModel, out accept, lstucsgm))
                    {
                        return true;
                    }
                }
                else
                {
                    if (ProcessExistingUser(acceptModel, out accept, userWorker, lstucsgm))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (ProcessExistingUser(acceptModel, out accept, lstucsgm))
                {
                    return true;
                }
            }

            SendConfirmationEmails(acceptModel);
            ViewBag.LoginURL = ConfigurationManager.AppSettings[AppSettingsLogin];

            accept = null;
            return false;
        }

        private bool ProcessExistingUser(
            UserAcceptModel acceptModel, 
            out ActionResult accept, 
            IReadOnlyCollection<UserClientSecurityGroupMap> securityGroupMaps)
        {
            var uWorker = new BusinessLogic.User();
            var existUser = uWorker.SearchUserName(acceptModel.UserName);
            if (existUser?.UserID > 0 && existUser.Password == acceptModel.Password)
            {
                if (existUser.Status != KMPlatform.Enums.UserStatus.Locked)
                {
                    if (!existUser.IsPlatformAdministrator)
                    {
                        var existingRoles =
                            new BusinessLogic.UserClientSecurityGroupMap().SelectForUser(existUser.UserID);
                        if (!RoleCheck(acceptModel.SecurityGroupsToAccept, existingRoles))
                        {
                            ProcessNotInRole(acceptModel, securityGroupMaps, existUser, existingRoles);
                        }
                        else
                        {
                            acceptModel.ErrorMessage = ErrorRoleExists;
                            accept = View(UrlAccept, acceptModel);
                            return true;
                        }
                    }
                    else
                    {
                        acceptModel.ErrorMessage = ErrorSystemAdministrator;
                        accept = View(UrlAccept, acceptModel);
                        return true;
                    }
                }
                else
                {
                    acceptModel.ErrorMessage = ErrorUserIsLocked;
                    accept = View(UrlAccept, acceptModel);
                    return true;
                }
            }
            else
            {
                acceptModel.ErrorMessage = ErrorUserPasswordInvalid;
                accept = View(UrlAccept, acceptModel);
                return true;
            }

            accept = null;
            return false;
        }

        private void ProcessNotInRole(
            UserAcceptModel acceptModel, 
            IReadOnlyCollection<UserClientSecurityGroupMap> securityGroupMaps,
            EntityUser existUser,
            IReadOnlyCollection<UserClientSecurityGroupMap> existingRoles)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));
            Guard.NotNull(existUser, nameof(existUser));

            var sgoiWorker = new BusinessLogic.SecurityGroupOptIn();
            var ucsgmWorker = new BusinessLogic.UserClientSecurityGroupMap();
            var uWorker = new BusinessLogic.User();
            foreach (var groupOptIn in acceptModel.SecurityGroupsToAccept)
            {
                groupOptIn.UserID = existUser.UserID;
                groupOptIn.HasAccepted = true;
                sgoiWorker.Save(groupOptIn);
                var groupMap = securityGroupMaps.First(
                    x => x.ClientID == groupOptIn.ClientID && x.SecurityGroupID == groupOptIn.SecurityGroupID);

                if (groupMap != null)
                {
                    if (existingRoles.Any(x =>
                        x.SecurityGroupID == groupMap.SecurityGroupID && x.ClientID == groupMap.ClientID))
                    {
                        groupMap = existingRoles.First(x =>
                            x.SecurityGroupID == groupMap.SecurityGroupID && x.ClientID == groupMap.ClientID);
                    }
                    else
                    {
                        groupMap.UserID = existUser.UserID;
                    }

                    groupMap.IsActive = true;
                    groupMap.InactiveReason = string.Empty;
                    ucsgmWorker.Save(groupMap);
                }
            }

            existUser.IsActive = true;
            existUser.Status = KMPlatform.Enums.UserStatus.Active;
            uWorker.Save(existUser);
            DeleteOldUser(acceptModel.UserID);
            acceptModel.UserID = existUser.UserID;
        }

        private bool ProcessExistingUser(
            UserAcceptModel acceptModel, 
            out ActionResult accept, 
            BusinessLogic.User userWorker,
            IReadOnlyCollection<UserClientSecurityGroupMap> lstucsgm)
        {
            Guard.NotNull(userWorker, nameof(userWorker));

            var sgoiWorker = new BusinessLogic.SecurityGroupOptIn();
            var ucsgmWorker = new BusinessLogic.UserClientSecurityGroupMap();

            if (!userWorker.Validate_UserName(acceptModel.UserName, UserIdNone))
            {
                var userId = ProcessExistingUserFill(acceptModel, userWorker);

                foreach (var groupOptIn in acceptModel.SecurityGroupsToAccept)
                {
                    sgoiWorker.MarkAsAccepted(groupOptIn.SecurityGroupOptInID);
                    var clientSecurityGroupMap = lstucsgm.First(
                        x => x.ClientID == groupOptIn.ClientID && x.SecurityGroupID == groupOptIn.SecurityGroupID);
                    if (clientSecurityGroupMap != null)
                    {
                        clientSecurityGroupMap.UserID = userId;
                        clientSecurityGroupMap.IsActive = true;
                        clientSecurityGroupMap.InactiveReason = string.Empty;
                        ucsgmWorker.Save(clientSecurityGroupMap);
                    }
                }

                DeleteOldUser(acceptModel.UserID);
            }
            else
            {
                acceptModel.ErrorMessage = ErrorUsernameExists;
                accept = View(UrlAccept, acceptModel);
                return true;
            }

            accept = null;
            return false;
        }

        private static int ProcessExistingUserFill(UserAcceptModel acceptModel, BusinessLogic.User userWorker)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));
            Guard.NotNull(userWorker, nameof(userWorker));

            var currentUser = new KMPlatform.Entity.User();
            var oldUser = userWorker.SelectUser(acceptModel.UserID, false);
            currentUser.Status = KMPlatform.Enums.UserStatus.Active;
            currentUser.UserName = acceptModel.UserName;
            currentUser.Password = acceptModel.Password;
            currentUser.IsActive = true;
            currentUser.EmailAddress = oldUser.EmailAddress;
            currentUser.FirstName = oldUser.FirstName;
            currentUser.LastName = oldUser.LastName;
            currentUser.CreatedByUserID = oldUser.CreatedByUserID;
            currentUser.IsPlatformAdministrator = oldUser.IsPlatformAdministrator;
            currentUser.Phone = oldUser.Phone;
            currentUser.AccessKey = Guid.NewGuid();
            currentUser.IsAccessKeyValid = true;
            try
            {
                var selectForClientGroup =
                    new BusinessLogic.ClientGroupClientMap().SelectForClientGroup(
                        acceptModel.SecurityGroupsToAccept.First(
                            x => x.ClientGroupID > 0).ClientGroupID.Value);
                currentUser.DefaultClientGroupID = selectForClientGroup[0].ClientGroupID;
                currentUser.DefaultClientID = selectForClientGroup[0].ClientID;
            }
            catch
            {
                var selectForClientId =
                    new BusinessLogic.ClientGroupClientMap().SelectForClientID(acceptModel
                        .SecurityGroupsToAccept.First(x => x.ClientID > 0).ClientID);
                currentUser.DefaultClientGroupID = selectForClientId[0].ClientGroupID;
                currentUser.DefaultClientID = selectForClientId[0].ClientID;
            }

            var userId = userWorker.Save(currentUser);
            acceptModel.UserID = userId;
            return userId;
        }

        private bool ProcessNonExistingUser(
            UserAcceptModel acceptModel, 
            out ActionResult accept, 
            IReadOnlyCollection<UserClientSecurityGroupMap> clientSecurityGroupMaps)
        {
            var userWorker = new BusinessLogic.User();

            var currentUser = CreateCurrentUser(acceptModel, userWorker);
            const string regex = @"^[ \s]+|[ \s]+$";
            var match = Regex.Match(currentUser.UserName, regex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                if (!userWorker.Validate_UserName(currentUser.UserName.Trim(), currentUser.UserID))
                {
                    List<UserClientSecurityGroupMap> existingRoles;
                    var changeDefaults = CheckExistingRoles(currentUser, out existingRoles);

                    if (!RoleCheck(acceptModel.SecurityGroupsToAccept, existingRoles))
                    {
                        userWorker.Save(currentUser);
                        if (!currentUser.IsPlatformAdministrator)
                        {
                            var newDefaultClientId = ApplyGroupsToUser(
                                acceptModel, 
                                clientSecurityGroupMaps);
                            SaveCurrentUser(acceptModel, changeDefaults, newDefaultClientId, currentUser);
                        }
                    }
                    else
                    {
                        acceptModel.ErrorMessage = ErrorRoleExists;
                        accept = View(UrlAccept, acceptModel);
                        return true;
                    }
                }
                else
                {
                    acceptModel.ErrorMessage = ErrorUsernameExists;
                    accept = View(UrlAccept, acceptModel);
                    return true;
                }
            }
            else
            {
                acceptModel.ErrorMessage = ErrorInvalidFormat;
                accept = View(UrlAccept, acceptModel);
                return true;
            }

            accept = null;
            return false;
        }

        private static bool CheckExistingRoles(
            EntityUser currentUser, 
            out List<UserClientSecurityGroupMap> existingRoles)
        {
            Guard.NotNull(currentUser, nameof(currentUser));

            var changeDefaults = false;
            existingRoles = new BusinessLogic.UserClientSecurityGroupMap().SelectForUser(currentUser.UserID);
            if (!existingRoles.Exists(x => x.ClientID == currentUser.DefaultClientID && x.IsActive))
            {
                changeDefaults = true;
            }

            return changeDefaults;
        }

        private static EntityUser CreateCurrentUser(UserAcceptModel acceptModel, BusinessLogic.User userWorker)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));
            Guard.NotNull(userWorker, nameof(userWorker));

            var currentUser = userWorker.SelectUser(acceptModel.UserID, false);
            currentUser.Status = KMPlatform.Enums.UserStatus.Active;
            currentUser.UserName = acceptModel.UserName;
            currentUser.Password = acceptModel.Password;
            currentUser.IsActive = true;
            currentUser.IsAccessKeyValid = true;
            return currentUser;
        }

        private static void SaveCurrentUser(
            UserAcceptModel acceptModel, 
            bool changeDefaults, 
            int newDefaultClientId,
            EntityUser currentUser)
        {
            var userWorker = new BusinessLogic.User();
            if (changeDefaults && newDefaultClientId > 0)
            {
                currentUser.DefaultClientID = newDefaultClientId;
                currentUser.DefaultClientGroupID =
                    new BusinessLogic.ClientGroupClientMap().SelectForClientID(
                        newDefaultClientId)[0].ClientGroupID;
                currentUser.Password = acceptModel.Password;
                userWorker.Save(currentUser);
            }
        }

        private static int ApplyGroupsToUser(
            UserAcceptModel acceptModel, 
            IReadOnlyCollection<UserClientSecurityGroupMap> clientSecurityGroupMaps)
        {
            Guard.NotNull(acceptModel, nameof(acceptModel));

            var sgoiWorker = new BusinessLogic.SecurityGroupOptIn();
            var ucsgmWorker = new BusinessLogic.UserClientSecurityGroupMap();

            var newDefaultClientId = UserIdNone;
            foreach (var groupOptIn in acceptModel.SecurityGroupsToAccept)
            {
                sgoiWorker.MarkAsAccepted(groupOptIn.SecurityGroupOptInID);
                var groupMap = clientSecurityGroupMaps.First(
                    x => x.ClientID == groupOptIn.ClientID && x.SecurityGroupID == groupOptIn.SecurityGroupID);
                if (groupMap != null)
                {
                    newDefaultClientId = newDefaultClientId < 0 ? groupMap.ClientID : newDefaultClientId;
                    groupMap.IsActive = true;
                    groupMap.InactiveReason = string.Empty;
                    groupMap.DateUpdated = DateTime.Now;
                    groupMap.UpdatedByUserID = acceptModel.CreatedByUserID;
                    ucsgmWorker.Save(groupMap);
                }
            }

            return newDefaultClientId;
        }

        #endregion

        private void DeleteOldUser(int oldUserID)
        {
            List<KMPlatform.Entity.UserClientSecurityGroupMap> lstUCSGM = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap().SelectForUser(oldUserID);

            if (lstUCSGM == null && lstUCSGM.Count == 0)
            {
                new KMPlatform.BusinessLogic.User().Delete(oldUserID);
            }
        }

        private bool RoleCheck(List<KMPlatform.Entity.SecurityGroupOptIn> lstucsgm, List<KMPlatform.Entity.UserClientSecurityGroupMap> existingRoles)
        {
            bool exists = false;
            KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
            KMPlatform.BusinessLogic.ClientGroupClientMap cWorker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
            existingRoles = existingRoles.Where(x => x.IsActive == true).ToList();
            List<KMPlatform.Entity.SecurityGroup> existingSG = new List<KMPlatform.Entity.SecurityGroup>();
            foreach (KMPlatform.Entity.UserClientSecurityGroupMap ucsgm in existingRoles)
            {
                KMPlatform.Entity.SecurityGroup sgExist = sgWorker.Select(ucsgm.SecurityGroupID, false, false);
                if (!existingSG.Contains(sgExist))
                    existingSG.Add(sgExist);
            }

            foreach (KMPlatform.Entity.SecurityGroupOptIn ucsgm in lstucsgm)
            {
                //Get security Group for roles we're opting in to
                KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(ucsgm.SecurityGroupID, false);
                if (sg.ClientID > 0)
                {
                    //check existing roles for clientid
                    if (existingSG.Any(x => x.ClientID == sg.ClientID))
                    {
                        exists = true;
                        break;
                    }
                    else
                    {
                        //check existing roles for client groupid of clientid were opting in to
                        List<KMPlatform.Entity.ClientGroupClientMap> c = cWorker.SelectForClientID(sg.ClientID);
                        if (c != null && c.Count > 0)
                        {
                            foreach (KMPlatform.Entity.SecurityGroup cgCheck in existingSG)
                            {

                                if (cgCheck.ClientGroupID > 0 && c.Any(x => x.ClientGroupID == cgCheck.ClientGroupID))//.First().ClientGroupID.Equals(sgCGCheck.ClientGroupID))
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (exists)
                                break;

                        }

                    }

                }
                else if (sg.ClientGroupID > 0)//role is a client group role
                {
                    //Get Clients for Client Group
                    List<KMPlatform.Entity.ClientGroupClientMap> c = cWorker.SelectForClientGroup(sg.ClientGroupID);
                    foreach (KMPlatform.Entity.SecurityGroup cgCheck in existingSG)
                    {

                        if (cgCheck.ClientGroupID > 0 && cgCheck.ClientGroupID == sg.ClientGroupID)
                        {
                            exists = true;
                            break;
                        }
                        else
                        {
                            foreach (KMPlatform.Entity.ClientGroupClientMap cgcm in c)
                            {
                                if (existingRoles.Any(x => x.ClientID == cgcm.ClientID))
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (exists)
                                break;
                        }
                    }
                    if (exists)
                        break;
                }
            }

            return exists;
        }

        #region Sending email stuff
        private void SendConfirmationEmails(Models.UserAcceptModel uam)
        {
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();

            KMPlatform.Entity.User adminUser = uWorker.SelectUser(uam.CreatedByUserID);
            KMPlatform.Entity.User currentUser = uWorker.SelectUser(uam.UserID);
            int customerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(adminUser.DefaultClientID, false).CustomerID;
            //Admin
            ECN_Framework_Entities.Communicator.EmailDirect edAdmin = new ECN_Framework_Entities.Communicator.EmailDirect();
            edAdmin.EmailAddress = adminUser.EmailAddress;
            edAdmin.EmailSubject = "New KM Platform User Account";
            edAdmin.ReplyEmailAddress = "info@knowledgemarketing.com";
            edAdmin.FromName = "KM Platform";
            edAdmin.Content = GetAdminConfirmationEmail(currentUser, adminUser, uam.SecurityGroupsToAccept);
            edAdmin.SendTime = DateTime.Now;
            edAdmin.CreatedUserID = adminUser.UserID;
            edAdmin.CustomerID = customerID;
            edAdmin.Process = "KMPlatform Admin Email";
            edAdmin.Source = "KMPlatform";
            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(edAdmin);

            //currentUser
            ECN_Framework_Entities.Communicator.EmailDirect edUser = new ECN_Framework_Entities.Communicator.EmailDirect();
            edUser.EmailAddress = currentUser.EmailAddress;
            edUser.EmailSubject = "Confirm KM Platform User Account";
            edUser.ReplyEmailAddress = "info@knowledgemarketing.com";
            edUser.FromName = "Knowledge Marketing";
            edUser.Content = GetUserConfirmationEmail(currentUser, adminUser);
            edUser.SendTime = DateTime.Now;
            edUser.CreatedUserID = adminUser.UserID;
            edUser.CustomerID = customerID;
            edUser.Process = "KMPlatform User Email";
            edUser.Source = "KMPlatform";
            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(edUser);


        }

        private string GetUserConfirmationEmail(KMPlatform.Entity.User currentUser, KMPlatform.Entity.User adminUser)
        {
            string content = @"<div style='width:60%; margin:0 auto;'>
    <table style='width:100%;'>
        <tbody>
            <tr>
                <td colspan='3' style='text-align:center;'><img alt='' src='http://images.ecn5.com/KMNew/KMNewWebLogo.jpg' /></td>
            </tr>
            <tr>
                <td>
                    <p>Dear %%FirstName%%,<br /><br />You have successfully created a KM Platform User Account. Please click the link below to login.<br />&nbsp;</p>
                    <div id='redirectbutton' style='width:200px;text-align:center;margin-left:auto;margin-right:auto; border-radius:8px;  background-color: #FFFFFF;  moz-border-radius: 8px;  -webkit-border-radius: 8px;  border: 2px solid #000000;  '>
                        <p style='width:200px;height:100%;margin-left:auto;margin-right:auto;'><a href='%%RedirectLink%%' style='color:black;text-decoration:none;width:100%;height:100%;'>KM Platform Login</a></p>
                    </div>
                    <br />
                    <p>This is an automated notification. Please do not reply to this email. If you have questions or need assistance in setting up your user account, please contact %%CreatedByUserName%% at %%CreatedByUserEmailAddress%%</p>
                </td>
            </tr>
        </tbody>
    </table>
</div>";

            content = content.Replace("%%FirstName%%", currentUser.FirstName);
            content = content.Replace("%%RedirectLink%%", ConfigurationManager.AppSettings["KMPlatformLogin"].ToString());
            content = content.Replace("%%CreatedByUserName%%", adminUser.FirstName + " " + adminUser.LastName);
            content = content.Replace("%%CreatedByUserEmailAddress%%", adminUser.EmailAddress);

            return content;
        }

        private string GetAdminConfirmationEmail(KMPlatform.Entity.User currentUser, KMPlatform.Entity.User adminUser, List<KMPlatform.Entity.SecurityGroupOptIn> roles)
        {
            string content = @"<div style='text-align:center;'>
                                <div style='width:60%; margin:0 auto;'>
                                <table style='width:100%;'>
                                <tr>
                                <td colspan='3' style='text-align:center;'>
		                                <img src='http://images.ecn5.com/KMNew/KMNewWebLogo.jpg' alt=''>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                <p>New KM Platform User %%UserName%% has been established for %%FirstName%% %%LastName%% for the following role(s):<br />
                                %%Roles%%<br />
                                </p>
                                </td>
                                </tr>
                                </table>
                                </div>
                                </div>";

            content = content.Replace("%%UserName%%", currentUser.UserName);
            content = content.Replace("%%FirstName%%", currentUser.FirstName);
            content = content.Replace("%%LastName%%", currentUser.LastName);

            if (!currentUser.IsPlatformAdministrator)
            {
                KMPlatform.BusinessLogic.SecurityGroup sgWorker = new KMPlatform.BusinessLogic.SecurityGroup();
                StringBuilder sbRoles = new StringBuilder();
                List<int> rolesDone = new List<int>();
                foreach (KMPlatform.Entity.SecurityGroupOptIn sgoi in roles)
                {
                    if (!rolesDone.Contains(sgoi.SecurityGroupID))
                    {
                        KMPlatform.Entity.SecurityGroup sg = sgWorker.Select(sgoi.SecurityGroupID, false);
                        sbRoles.AppendLine(sg.SecurityGroupName + "<br />");
                        rolesDone.Add(sgoi.SecurityGroupID);
                    }
                }
                content = content.Replace("%%Roles%%", sbRoles.ToString());
            }
            else
            {
                content = content.Replace("%%Roles%%", "System Administrator");
            }

            return content;
        }

        private string CreateNote(ecn.activity.Areas.User.Models.UserAcceptModel uam)
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>UserID: " + uam.UserID.ToString());

                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
                if (Request.UrlReferrer != null)
                {
                    adminEmailVariables.AppendLine("<BR>Referring URL: " + Request.UrlReferrer.ToString());
                }
                adminEmailVariables.AppendLine("<BR>HEADERS");
                var headers = String.Empty;
                foreach (var key in Request.Headers.AllKeys)
                    headers += "<BR>" + key + ":" + Request.Headers[key];
                adminEmailVariables.AppendLine(headers);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        #endregion

        private int ToInt32WithThrows(string numberStr)
        {
            int result;
            if (int.TryParse(numberStr, out result))
            {
                return result;
            }
            throw new InvalidOperationException(string.Format(ErrorParseIntTemplate, numberStr));
        }
    }
}
