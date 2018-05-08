using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ECN_Framework_Common.Objects;
using ecn.communicator.mvc.Infrastructure;
using System.Configuration;

namespace ecn.communicator.mvc.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
        public ActionResult Edit(int EmailID, int GroupID)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(EmailID, GroupID, user);
            EmailWrapper emailWrapperModel = new EmailWrapper(email, GroupID);

            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(user.CurrentClient.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email))
            {
                if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                {
                    LoadDropDowns(emailWrapperModel, user, GroupID);

                    if (EmailID > 0)
                    {
                        LoadLog(emailWrapperModel, user, EmailID);
                        /*if (GroupID == 0)
                        {
                            UserDefLink.NavigateUrl += "?EmailID=" + EmailID.ToString();
                        }
                        else
                        {
                            UserDefLink.NavigateUrl += "?EmailID=" + EmailID.ToString() + "&GroupID=" + GroupID.ToString();
                            FormatPanel.Visible = true;
                        }
                        UserDefLink.Visible = true;
                    }*/
                    }

                    emailWrapperModel.UDFURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailProfileManager.aspx?action=edit&ead=" + email.EmailAddress + "&eid=" + EmailID + "&gid=" + GroupID + "&cid=" + user.CustomerID;
                    //ProfileManagerButton.Attributes.Add("Onclick", "return popManagerWindow('" + profileURL + "');");
                }

                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }

            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            emailWrapperModel.Errors = this.GetTempData("ECNErrors") as List<ECNError>;
            return View(emailWrapperModel);
        }
        [HttpGet]
        public ActionResult Delete(int GroupID, int EmailID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete(GroupID, EmailID, ConvenienceMethods.GetCurrentUser());
            }
            catch (ECNException ex)
            {
                throw ex;
            }
            return RedirectToAction("Edit");
        }
        private void LoadDropDowns(ecn.communicator.mvc.Models.EmailWrapper wrapper, KMPlatform.Entity.User user, int GroupID)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, user);
            if (group != null)
            {
                if (group.MasterSupression == null || group.MasterSupression.Value == 0)
                {
                    wrapper.SubscribeTypeCodes.Clear();
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Subscribes", "S"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Master Suppressed", "M"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Marked as Bad Records", "D"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Pending Subscribes", "P"));
                }
                else
                {
                    wrapper.SubscribeTypeCodes.Clear();
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("UnSubscribes", "U"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Bounce", "B"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Abuse Complaint", "A"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Manual Upload", "M"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Feedback Loop(or Spam Complaint)", "F"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Email Address Change", "E"));
                    wrapper.SubscribeTypeCodes.Add(new Tuple<string, string>("Unknown User", "?"));
                }
            }
        }
        private void LoadLog(ecn.communicator.mvc.Models.EmailWrapper wrapper, KMPlatform.Entity.User user, int EmailID)
        {
            List<ECN_Framework_Entities.Activity.View.BlastActivity> blastActivity = new List<ECN_Framework_Entities.Activity.View.BlastActivity>();
            if (EmailID > 0)
            {
                wrapper.BlastActivity = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetByEmailID(EmailID, user);
            }
        }
        [HttpPost]
        public ActionResult Update(ecn.communicator.mvc.Models.Email externalEmail)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();
            ECN_Framework_Entities.Communicator.Email internalEmail = null;

            internalEmail = externalEmail.ToEmail_Internal(user); // throws uncaught error if email address is empty FIXME
            if (ECN_Framework_BusinessLayer.Communicator.Email.Exists(externalEmail.EmailAddress, user.CustomerID, externalEmail.EmailID))
            {
                Email email1 = externalEmail;
                Email email2 = new Email(ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(externalEmail.EmailAddress, user.CustomerID), externalEmail.CurrentGroupID);
                Session["email1"] = email1;
                Session["email2"] = email2;
                //ECN_Framework_BusinessLayer.Communicator.Email.Save(user, internalEmail, externalEmail.CurrentGroupID, "ecn.communicator.mvc.controllers.GroupController.UpdateEmail");
                return RedirectToAction("MergeEmails", new { emailid1 = email1.EmailID, emailid2 = email2.EmailID, groupid = externalEmail.CurrentGroupID });
            }
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Email.Save(user, internalEmail, externalEmail.CurrentGroupID, "ecn.communicator.mvc.controllers.GroupController.UpdateEmail");
            }
            catch (ECNException ex)
            {
                this.SetTempData("ECNErrors", ex.ErrorList);
                return RedirectToAction("Edit", "Email", new { EmailID = externalEmail.EmailID, GroupID = externalEmail.CurrentGroupID });
            }
            return RedirectToAction("Emails", "Group", new { id = externalEmail.CurrentGroupID });
        }
        [HttpGet]
        public ActionResult MergeEmails(int emailid1, int emailid2, int groupid)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();

            Email externalEmail1 = Session["email1"] as Email;
            Email externalEmail2 = Session["email2"] as Email;
            if (externalEmail1 == null || externalEmail2 == null)
                throw new ECN_Framework_Common.Objects.SecurityException("Attempting to merge non-conflicting emails");
            Tuple<Email, Email> Model = new Tuple<Email, Email>(externalEmail1, externalEmail2);
            return View(Model);
        }
        [HttpPost]
        public ActionResult MergeEmails(int id)
        {
            try
            {
                KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();
                Email email1 = Session["email1"] as Email;
                Email email2 = Session["email2"] as Email;
                if (email1 == null || email2 == null)
                    throw new SecurityException("Attempted email merge with no recorded conflict");

                // Can't merge and make changes simultaneously
                if (email1.EmailID == id)
                {
                    ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(email2.EmailID, id, user);
                }
                else if (email2.EmailID == id)
                {
                    ECN_Framework_BusinessLayer.Communicator.Email.MergeProfiles(email1.EmailID, id, user);
                }
                else
                {
                    throw new ArgumentException("EmailID does not match the conflicting ones");
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ecn.communicator.mvc.Controllers.GroupController.MergeProfiles(int)", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
            return RedirectToAction("Index", "Group");
        }
    }
}