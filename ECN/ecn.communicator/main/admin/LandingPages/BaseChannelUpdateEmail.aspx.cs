using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ecn.Communicator.Main.Admin.Helpers;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_Common.Objects;
using EntitiesAccounts = ECN_Framework_Entities.Accounts;

namespace ecn.communicator.main.admin.LandingPages
{
    using Role = KM.Platform.User;

    public partial class BaseChannelUpdateEmail : System.Web.UI.Page
    {
        private const string BaseChannelMainPage = "BaseChannelMain.aspx";
        private const string InvalidCodeSnippetError = "Invalid codesnippet, only %%basechannelname%% and %%pagename%% are allowed";
        private string[] SnippetsToMatch = new string[]
        {
                "%%basechannelname%%",
                "%%pagename%%",
                "%%groupdescription%%"
        };
        private BaseChannelOperationsHandler _landingPagesOperations = null;

        public BaseChannelUpdateEmail()
        {
            _landingPagesOperations = new BaseChannelOperationsHandler(null);
        }

        private static ECN_Framework_Entities.Accounts.LandingPageAssign LPA = null;
        private static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> listOptions = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BASECHANNEL;
            Master.SubMenu = "landing pages";
            Master.Heading = "Change Email Page Settings";
            //Master.Heading = "Manage Content & Messages";
            //Master.HelpContent = "<b>Updating Email Address:</b><br/><div id='par1'>Update an Email Address to a new Email Address across the Base Channel.</div>";
            Master.HelpTitle = "Update Emails";

            if(!Page.IsPostBack)
            {
                if (Role.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    LoadData();
                }
                else
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    Label1.Text = "You do not have access to this page.";
                }
                
            }
        }

        private void LoadData()
        {
            LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value, 5);
            
            if(LPA != null)
            {
                chkOverrideDefault.Checked = LPA.BaseChannelDoesOverride.HasValue ? LPA.BaseChannelDoesOverride.Value : false;
                listOptions = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(LPA.LPAID);

                txtPageHeader.Text = LPA.Header;
                txtPageFooter.Text = LPA.Footer;
                txtPageText.Text = listOptions.FirstOrDefault(x => x.LPOID == 18).Display;

                txtOldEmailLabel.Text = listOptions.FirstOrDefault(x => x.LPOID == 19).Display;
                txtNewEmailLabel.Text = listOptions.FirstOrDefault(x => x.LPOID == 20).Display;
                txtButtonLabel.Text = listOptions.FirstOrDefault(x => x.LPOID == 21).Display;
                chkReEntryRequired.Checked = listOptions.FirstOrDefault(x => x.LPOID == 22).Display.ToLower().Equals("true") ? true : false;
                txtReEnterEmailLabel.Text = listOptions.FirstOrDefault(x => x.LPOID == 23).Display;
                txtConfirmationPageText.Text = listOptions.FirstOrDefault(x => x.LPOID == 24).Display;
                txtEmailHeader.Text = listOptions.FirstOrDefault(x => x.LPOID == 25).Display;
                txtEmailFooter.Text = listOptions.FirstOrDefault(x => x.LPOID == 26).Display;
                txtEmailBody.Text = listOptions.FirstOrDefault(x => x.LPOID == 27).Display;
                txtFromEmail.Text = listOptions.FirstOrDefault(x => x.LPOID == 28).Display;
                txtEmailSubject.Text = listOptions.FirstOrDefault(x => x.LPOID == 29).Display;
                txtFinalConfirmationText.Text = listOptions.FirstOrDefault(x => x.LPOID == 30) != null ? listOptions.FirstOrDefault(x => x.LPOID == 30).Display : string.Empty;

                lblURL.Text = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/UpdateEmailAddress.aspx?bcid=" + LPA.BaseChannelID.ToString();

               // btnPreview.Attributes.Add("onclientclick", "window.open('" + ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/UpdateEmailAddress.aspx?preview=" + LPA.LPAID.ToString() + "', 'popup_window', 'width=1000,height=750,resizable=yes');");

            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (LPA == null)
                {
                    LPA = new EntitiesAccounts.LandingPageAssign();
                    LPA.LPID = 5;
                    LPA.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    LPA.BaseChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                }

                var codeSnippetsAndTheirErrorMessages = new Tuple<string, string>[]
                {
                    new Tuple<string, string>(txtPageHeader.Text, "Page Header"),
                    new Tuple<string, string>(txtPageFooter.Text, "Page Footer"),
                    new Tuple<string, string>(txtPageText.Text, "Page Text"),
                    new Tuple<string, string>(txtConfirmationPageText.Text, "Confirmation Page Text"),
                    new Tuple<string, string>(txtFinalConfirmationText.Text, "Final Confirmation Text"),
                    new Tuple<string, string>(txtEmailHeader.Text, "Email Header"),
                    new Tuple<string, string>(txtEmailFooter.Text, "Email Footer"),
                    new Tuple<string, string>(txtEmailBody.Text, "Email Body"),
                };

                if (!ValidateCodeSnippets(codeSnippetsAndTheirErrorMessages))
                {
                    return;
                }

                LPA.Header = txtPageHeader.Text.Trim();
                LPA.Footer = txtPageFooter.Text.Trim();
                LPA.BaseChannelDoesOverride = chkOverrideDefault.Checked;

                LandingPageAssign.Save(LPA, Master.UserSession.CurrentUser);
                LandingPageAssignContent.Delete(LPA.LPAID, Master.UserSession.CurrentUser);

                var landingPageIDsAndTheirControlsToUpdate = new Tuple<int, WebControl>[]
                {
                    new Tuple<int, WebControl>(18, txtPageText),
                    new Tuple<int, WebControl>(19, txtOldEmailLabel),
                    new Tuple<int, WebControl>(20, txtNewEmailLabel),
                    new Tuple<int, WebControl>(21, txtButtonLabel),
                    new Tuple<int, WebControl>(22, chkReEntryRequired),
                    new Tuple<int, WebControl>(23, txtReEnterEmailLabel),
                    new Tuple<int, WebControl>(24, txtConfirmationPageText),
                    new Tuple<int, WebControl>(25, txtEmailHeader),
                    new Tuple<int, WebControl>(26, txtEmailFooter),
                    new Tuple<int, WebControl>(27, txtEmailBody),
                    new Tuple<int, WebControl>(28, txtFromEmail),
                    new Tuple<int, WebControl>(29, txtEmailSubject),
                    new Tuple<int, WebControl>(30, txtFinalConfirmationText)
                };

                SaveLandingPageAssignContents(landingPageIDsAndTheirControlsToUpdate);
                Response.Redirect(BaseChannelMainPage);
            }
            catch (ECNException ecn)
            {
                setECNError(ecn);
            }
        }

        private bool ValidateCodeSnippets(Tuple<string, string>[] codeSnippetsAndTheirErrorMessages)
        {
            if (codeSnippetsAndTheirErrorMessages == null)
            {
                throw new ArgumentNullException(nameof(codeSnippetsAndTheirErrorMessages));
            }

            foreach (var codeSnippetAndErrorMessage in codeSnippetsAndTheirErrorMessages)
            {
                var codeSnippetError = validCodeSnippets(codeSnippetAndErrorMessage.Item1);
                if (!codeSnippetError.valid)
                {
                    ThrowECNException(string.Format("{0} in {1}", codeSnippetError.message, codeSnippetAndErrorMessage.Item2));
                    return false;
                }
            }

            return true;
        }

        private void SaveLandingPageAssignContents(Tuple<int, WebControl>[] landingPageIDsAndTheirTextBoxes)
        {
            if (landingPageIDsAndTheirTextBoxes == null)
            {
                throw new ArgumentNullException(nameof(landingPageIDsAndTheirTextBoxes));
            }

            foreach (var idAndTextBox in landingPageIDsAndTheirTextBoxes)
            {
                SaveLandingPageAssignContent(idAndTextBox.Item1, idAndTextBox.Item2);
            }
        }

        private void SaveLandingPageAssignContent(int landingPageObjectID, WebControl displayControl)
        {
            if (displayControl == null)
            {
                throw new ArgumentNullException(nameof(displayControl));
            }

            var landingPageAssignContent = new EntitiesAccounts.LandingPageAssignContent();
            landingPageAssignContent.LPAID = LPA.LPAID;
            landingPageAssignContent.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            landingPageAssignContent.LPOID = landingPageObjectID;
            if (displayControl is ITextControl)
            {
                landingPageAssignContent.Display = (displayControl as ITextControl)?.Text;
            }
            else if (displayControl is ICheckBoxControl)
            {
                landingPageAssignContent.Display = (displayControl as ICheckBoxControl)?.Checked.ToString();
            }

            LandingPageAssignContent.Save(landingPageAssignContent, Master.UserSession.CurrentUser);
        }

        private void ThrowECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.LandingPage, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }

        }

        private CodeSnippetError validCodeSnippets(string text)
        {
            return _landingPagesOperations.ValidCodeSnippets(text, SnippetsToMatch.ToList(), InvalidCodeSnippetError);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseChannelMain.aspx");
        }



        protected void chkReEntryRequired_CheckedChanged(object sender, EventArgs e)
        {
            rfvReEnterEmailLabel.Enabled = chkReEntryRequired.Checked;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "open_window", "window.open('" + ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/UpdateEmailAddress.aspx?preview=" + LPA.LPAID.ToString() + "', 'popup_window', 'width=1000,height=750,resizable=yes');", true);
        }
    }
}