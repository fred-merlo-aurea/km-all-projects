using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI.WebControls;
using ecn.communicator.main.Helpers;
using ecn.communicator.main.Salesforce.Controls;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Salesforce;
using KM.Common;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardSFCampaign : System.Web.UI.UserControl, IECNWizard
    {

        Hashtable hUpdatedRecords = new Hashtable();
        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        private void loadLicense()
        {

            ECN_Framework_Entities.Accounts.License lic =
            ECN_Framework_BusinessLayer.Accounts.License.GetCurrentLicensesByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeCode.emailblock10k);

            if (lic.LicenseOption == ECN_Framework_Common.Objects.Accounts.Enums.LicenseOption.unlimited)
            {
                BlastLicensed.Text = "UNLIMITED";
                BlastAvailable.Text = "N/A";
            }
            else
            {
                BlastLicensed.Text = lic.Allowed.ToString();
                BlastAvailable.Text = lic.Available.ToString();
            }
            BlastUsed.Text = lic.Used.ToString();
            UpdateLicenseCount();

        }
        
        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
                SF_Authentication.RefreshSalesForceToken(sfs.RefreshToken, sfs.ConsumerSecret, sfs.ConsumerKey, sfs.SandboxMode.Value);
                List<SF_Campaign> listCamp = SF_Campaign.GetList(SF_Authentication.Token.access_token, string.Empty).OrderBy(x => x.Name).ToList();
                drpSFCampaigns.DataSource = listCamp;
                drpSFCampaigns.DataTextField = "Name";
                drpSFCampaigns.DataValueField = "ID";
                drpSFCampaigns.DataBind();
                drpSFCampaigns.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "-1" });
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                if (ci.SFCampaignID != null && ci.SFCampaignID != "-1")
                {
                    try
                    {
                        drpSFCampaigns.SelectedValue = ci.SFCampaignID.ToString();
                    }
                    catch
                    {
                        ci.SFCampaignID = "-1";
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        throwECNException("Unable to find the Salesforce Campaign selected during the time of the setup.");
                    }
                }
                loadLicense();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
        }


        #region ErrorHandling
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }
        #endregion

        public bool Save()
        {
            if (drpSFCampaigns.SelectedValue != "-1")
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                if (ci.SFCampaignID != null && ci.SFCampaignID == drpSFCampaigns.SelectedValue)
                {
                    return true;
                }
                int GroupID = CreateGroupFromSfCampaign(drpSFCampaigns.SelectedValue);
                ci.SFCampaignID = drpSFCampaigns.SelectedValue;                    
                List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
                ECN_Framework_Entities.Communicator.CampaignItemBlast CampaignItemBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                CampaignItemBlast.CampaignItemID = CampaignItemID;
                CampaignItemBlast.GroupID = GroupID;
                //CampaignItemBlast.FilterID = null;
                if (ci.BlastList.Count > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast_old = ci.BlastList[0];
                    CampaignItemBlast.LayoutID = ciBlast_old.LayoutID;
                    CampaignItemBlast.EmailSubject = ciBlast_old.EmailSubject;
                    CampaignItemBlast.DynamicFromName = ciBlast_old.DynamicFromName;
                    CampaignItemBlast.DynamicFromEmail = ciBlast_old.DynamicFromEmail;
                    CampaignItemBlast.DynamicReplyTo = ciBlast_old.DynamicReplyTo;
                    CampaignItemBlast.AddOptOuts_to_MS = ciBlast_old.AddOptOuts_to_MS;
                    CampaignItemBlast.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    CampaignItemBlast.CreatedUserID = ciBlast_old.CreatedUserID;
                }
                else
                {
                    CampaignItemBlast.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                }
                CampaignItemBlast.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                ciBlastList.Add(CampaignItemBlast);
                if (ci.CompletedStep < 2)
                    ci.CompletedStep = 2;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 10, 0)))
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ci.BlastList)
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(ciBlast.CampaignItemBlastID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    }
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(CampaignItemID, ciBlastList, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);              
                    ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    scope.Complete();
                }
                return true;
            }
            else
            {
                throwECNException("Please seelct a Salesforce Campaign");
                return false;
            }                   
        }


        private ECN_Framework_Entities.Communicator.Group CreateECNGroup(string groupName)
        {
            ECN_Framework_Entities.Communicator.Group retGroup = new ECN_Framework_Entities.Communicator.Group();

            int folderID = 0;
            try
            {
                retGroup.GroupName = SF_Utilities.CleanStringSqlInjection(groupName);
                retGroup.FolderID = folderID;
                retGroup.AllowUDFHistory = "Y";
                retGroup.OwnerTypeCode = "customer";
                retGroup.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                retGroup.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                retGroup.PublicFolder = 1;
                retGroup.IsSeedList = false;
                retGroup.GroupID = ECN_Framework_BusinessLayer.Communicator.Group.Save(retGroup, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                retGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(retGroup.GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                return retGroup = null;
            }
            return retGroup;
        }

        private Hashtable CreateUDF(int groupID)
        {
            try
            {

                ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = groupID;
                gdf.IsPublic = "N";
                gdf.LongName = "SalesforceID";
                gdf.ShortName = "SFID";
                gdf.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                gdf.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;

                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (Exception ex)
            {
                //SF_Utilities.LogException(ex);
            }
            try
            {
                ECN_Framework_Entities.Communicator.GroupDataFields gdf2 = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf2.GroupID = groupID;
                gdf2.IsPublic = "N";
                gdf2.LongName = "Salesforce Type";
                gdf2.ShortName = "SFType";
                gdf2.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                gdf2.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf2, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            catch (Exception ex)
            {
                //SF_Utilities.LogException(ex);
            }

            Hashtable hGDFFields = getUDFsForList(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            return hGDFFields;
        }

        private Hashtable getUDFsForList(int groupID, KMPlatform.Entity.User user)
        {
            Hashtable fields = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user);
            if (gdfList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    fields.Add("user_" + gdf.ShortName.ToLower(), gdf.GroupDataFieldsID);
                }
            }

            return fields;
        }

        private int CreateGroupFromSfCampaign(string sfCampaignId)
        {
            hUpdatedRecords = new Hashtable();
            var newGrpName = Guid.NewGuid().ToString("N");
            var ecnGroup = CreateECNGroup(newGrpName);

            var builder = new EmailBatchBuilder(
                CreateUDF(ecnGroup.GroupID),
                SF_Contact.GetCampaignMembers(SF_Authentication.Token.access_token, sfCampaignId),
                SF_Lead.GetCampaignMembers(SF_Authentication.Token.access_token, sfCampaignId));

            foreach (var batch in builder.Build())
            {
                try
                {
                    UpdateToDB(ecnGroup.GroupID, batch.XmlProfileUdf, batch.XmlUdf);
                }
                catch (Exception ex)
                {
                    SF_Utilities.LogException(ex);
                    throwECNException($"ERROR:Import Unsuccessful -{Message.Message_Icon.error}");
                }
            }

            return ecnGroup.GroupID;
        }

        private void UpdateToDB(int groupID, string xmlProfile, string xmlUDF)
        {
            DataTable dtResults = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, groupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "HTML", "S", false, "", "Ecn.communicator.main.ecnwizard.controls.wizardSFcampaign.UpdateToDB");

            if (dtResults.Rows.Count > 0)
            {
                foreach (DataRow dr in dtResults.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                        hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                    else
                    {
                        int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                    }
                }

            }
        }

        protected void drpSFCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLicenseCount();
        }

        private void UpdateLicenseCount()
        {
            if (drpSFCampaigns.SelectedValue != "-1")
            {
                ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
                SF_Authentication.RefreshSalesForceToken(sfs.RefreshToken, sfs.ConsumerSecret, sfs.ConsumerKey, sfs.SandboxMode.Value);
                List<SF_Campaign> listCamp = SF_Campaign.GetList(SF_Authentication.Token.access_token, "WHERE ID ='" + drpSFCampaigns.SelectedValue.ToString() + "'").OrderBy(x => x.Name).ToList();
                BlastThis.Text = (listCamp[0].NumberOfContacts + listCamp[0].NumberOfLeads).ToString();
            }
        }
    }
}