using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class SocialConfig : UserControl
    {
        private const int SocialMediaIdFacebook = 1;
        private const int SocialMediaIdTwitter = 2;
        private const int SocialMediaIdLinkedIn = 3;
        private const string ErrorMessageUnableToValidate = "Unable to validate credentials.  Please reauthorize your account.";
        private const string YouWillBePostingTo = "You will be posting to ";
        private const string EcnImagesKmNewLinkedInPng = "/ecn.images/KMNew/linkedin.png";
        private const string NoAccounts = "No Accounts";
        private const string Select = "--Select--";
        private const string EcnImagesKmNewTwitterPng = "/ecn.images/KMNew/twitter.png";
        private const string TableHeaderBgColorTwitter = "#45C1F5";
        private const string TableHeaderBgColorLinkedIn = "#427DA5";
        private const string SrcAttribute = "src";
        private const string ReauthCommandArgumentTwitter = "2";
        private const string ReauthCommandArgumentLinkedIn = "3";
        private const string ReauthCommandArgumentFacebook = "1";
        private const string TableHeaderBgColorFacebook = "#417EA3";
        private const string EcnImagesKmNewFacebookPng = "/ecn.images/KMNew/facebook.png";
        private const string AccountsDataTextField = "name";
        private const string AccountsDataValueField = "id";
        private const string InvalidAccountId = "-1";

        public event EventHandler FolderEvent;
        private static string _currentImageButton;
        public string CurrentProfileImage
        {
            get
            {
                if (ViewState["CurrentProfileImage" + this.ClientID.ToString()] != null)
                    return ViewState["CurrentProfileImage" + this.ClientID.ToString()].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["CurrentProfileImage" + this.ClientID.ToString()] = value;
            }
        }
        public string CurrentImageButton
        {
            get
            {
                return _currentImageButton;
            }
            set
            {
                _currentImageButton = value;
            }


        }

        public string ImagePath
        {
            get
            {
                try
                {
                    return hfImagePath.Value;
                }
                catch { return string.Empty; }
            }
            set
            {
                if (imgbtnThumbnail.Attributes[SrcAttribute] != null)
                    imgbtnThumbnail.Attributes[SrcAttribute] = value;
                else
                    imgbtnThumbnail.Attributes.Add(SrcAttribute, value);

                if (imgbtnThumbnail.Attributes["imagepath"] != null)
                    imgbtnThumbnail.Attributes["imagepath"] = value;
                else
                    imgbtnThumbnail.Attributes.Add("imagepath", value);


                imgbtnThumbnail.ImageUrl = value;

                hfImagePath.Value = value;
                ImageURL = value;
            }

        }

        public string Message
        {
            get
            {
                try
                {
                    return txtMessage.Value.Trim();
                }
                catch { return string.Empty; }
            }
            set
            {
                txtMessage.Value = value;
            }
        }

        public string Title
        {
            get
            {
                try
                {
                    return txtPostLink.Value.Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                txtPostLink.Value = value;
            }
        }

        public string Subtitle
        {
            get
            {
                try
                {
                    return txtPostSubTitle.Value.Trim();
                }
                catch { return string.Empty; }
            }
            set
            {
                txtPostSubTitle.Value = value;
            }
        }

        public bool UseThumbnail
        {
            get
            {
                return chkUseThumbNail.Checked;
            }
            set
            {
                chkUseThumbNail.Checked = value;
            }
        }

        public string PageID
        {
            get
            {
                try
                {
                    if (ddlAccounts.SelectedIndex > 0)
                    {
                        return ddlAccounts.SelectedValue.ToString().Trim();
                    }
                    else
                        return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                if (ddlAccounts.Items.Count > 0)
                {
                    ddlAccounts.SelectedValue = value;
                }
            }
        }

        
        public List<ECN_Framework_Common.Objects.SocialMediaHelper.FBAccount> FBAccounts
        {
            get
            {
                if (ViewState["FBAccounts" + this.ClientID.ToString()] != null)
                    return (List<ECN_Framework_Common.Objects.SocialMediaHelper.FBAccount>)ViewState["FBAccounts" + this.ClientID.ToString()];
                else
                    return null;
            }
            set
            {
                ViewState["FBAccounts" + this.ClientID.ToString()] = value;
            }
        }

        public List<ECN_Framework_Common.Objects.SocialMediaHelper.LIAccount> LIAccounts
        {
            get
            {
                if (ViewState["LIAccounts" + this.ClientID.ToString()] != null)
                    return (List<ECN_Framework_Common.Objects.SocialMediaHelper.LIAccount>)ViewState["LIAccounts" + this.ClientID.ToString()];
                else
                    return null;
            }
            set
            {
                ViewState["LIAccounts" + this.ClientID.ToString()] = value;
            }
        }

        public static string ImageURL;

        private static int _SocialMediaID;
        public int SocialMediaID
        {
            get
            {
                if (!string.IsNullOrEmpty(hfSocialMediaID.Value))
                    return Convert.ToInt32(hfSocialMediaID.Value);
                else
                    return -1;
            }
            set
            {
                hfSocialMediaID.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfImagePath.Value))
            {
                if (imgbtnThumbnail.Attributes["imagepath"] == null || string.IsNullOrEmpty(imgbtnThumbnail.Attributes["imagepath"].ToString()))
                {
                    imgbtnThumbnail.ImageUrl = "/ecn.images/images/SelectImage.png";
                    if (imgbtnThumbnail.Attributes[SrcAttribute] != null)
                        imgbtnThumbnail.Attributes[SrcAttribute] = "/ecn.images/images/SelectImage.png";
                    else
                        imgbtnThumbnail.Attributes.Add(SrcAttribute, "/ecn.images/images/SelectImage.png");
                }
                else
                {
                    hfImagePath.Value = imgbtnThumbnail.Attributes["imagepath"].ToString();
                    imgbtnThumbnail.ImageUrl = hfImagePath.Value;
                    if (imgbtnThumbnail.Attributes[SrcAttribute] != null)
                        imgbtnThumbnail.Attributes[SrcAttribute] = hfImagePath.Value;
                    else
                        imgbtnThumbnail.Attributes.Add(SrcAttribute, hfImagePath.Value);
                }
            }
            else
            {
                imgbtnThumbnail.ImageUrl = hfImagePath.Value;
                if (imgbtnThumbnail.Attributes[SrcAttribute] != null)
                    imgbtnThumbnail.Attributes[SrcAttribute] = hfImagePath.Value;
                else
                    imgbtnThumbnail.Attributes.Add(SrcAttribute, hfImagePath.Value);
            }
           // System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "doTwemoji", "pageloaded();", true);
        }

        public void LoadSocialMedia(int SocialMediaID, string profileName, string profileImage, string SubjectLine, string access_token)
        {
            hfSocialMediaID.Value = SocialMediaID.ToString();
            DisplayControls(SocialMediaID, SubjectLine, access_token);

            lblProfileName.Text = profileName;
            if (!string.IsNullOrEmpty(profileImage))
            {
                imgProfile.ImageUrl = profileImage;
                CurrentProfileImage = profileImage;
            }
            else
            {
                imgProfile.Visible = false;
            }
            //imgbtnThumbnail.Attributes.Add("onclientclick", "window.open('~/main/Content/SocialFileManager.aspx?imgcontrol=" + imgbtnThumbnail.ClientID + "', '_blank','menubar=no,titlebar=no,toolbar=no','false');");

            //System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "doTwemoji", "pageloaded();", true);
        }

        private void DisplayControls(int socialMediaId, string subject, string accessToken)
        {
            // TODO: socialMediaId should be enum
            // TODO: should have default case in switch
            switch (socialMediaId)
            {
                case SocialMediaIdFacebook:
                    DisplayControlsFacebook(subject, accessToken);
                    break;
                case SocialMediaIdTwitter:
                    DisplayControlsTwitter(subject);
                    break;
                case SocialMediaIdLinkedIn:
                    DisplayControlsLinkedIn(subject, accessToken);
                    break;
            }
        }

        private void DisplayControlsLinkedIn(string subject, string accessToken)
        {
            txtMessage.Visible = true;
            txtMessage.Value = subject;
            trTitle.Visible = true;
            trSubTitle.Visible = true;
            trThumbnail.Visible = true;
            imgbtnThumbnail.Visible = true;
            imgbtnReauth.CommandArgument = ReauthCommandArgumentLinkedIn;
            LIAccounts = SocialMediaHelper.GetLICompanies(accessToken);
            if (LIAccounts.Count > 0)
            {
                if (LIAccounts[0].id == InvalidAccountId)
                {
                    AccountErrrorMsg.Visible = true;
                    AccountErrrorMsg.Text = ErrorMessageUnableToValidate;
                    ddlAccounts.Visible = true;
                    ddlAccounts.Items.Clear();
                    ddlAccounts.Items.Insert(0, new ListItem() { Text = NoAccounts, Value = InvalidAccountId, Selected = true });
                }
                else
                {
                    ddlAccounts.Visible = true;
                    ddlAccounts.DataSource = LIAccounts;
                    ddlAccounts.DataTextField = AccountsDataTextField;
                    ddlAccounts.DataValueField = AccountsDataValueField;
                    ddlAccounts.DataBind();
                    ddlAccounts.Items.Insert(0, new ListItem() { Text = Select, Value = InvalidAccountId, Selected = true });
                }
            }
            else
            {
                ddlAccounts.Visible = true;
                ddlAccounts.Items.Clear();
                ddlAccounts.Items.Insert(0, new ListItem() { Text = NoAccounts, Value = InvalidAccountId, Selected = true });
            }

            imgSocialMedia.ImageUrl = EcnImagesKmNewLinkedInPng;
            if (imgSocialMedia.Attributes[SrcAttribute] == null)
            {
                imgSocialMedia.Attributes.Add(SrcAttribute, EcnImagesKmNewLinkedInPng);
            }
            else
            {
                imgSocialMedia.Attributes[SrcAttribute] = EcnImagesKmNewLinkedInPng;
            }

            tblHeader.BgColor = TableHeaderBgColorLinkedIn;
            lblSocialMedia.Text = YouWillBePostingTo;
            lblSocialMedia.Visible = true;
        }

        private void DisplayControlsTwitter(string subject)
        {
            txtMessage.Visible = true;
            txtMessage.Value = subject;
            trTitle.Visible = false;
            trSubTitle.Visible = false;
            trThumbnail.Visible = false;
            imgbtnThumbnail.Visible = false;
            imgSocialMedia.ImageUrl = EcnImagesKmNewTwitterPng;
            imgbtnReauth.CommandArgument = ReauthCommandArgumentTwitter;
            if (imgSocialMedia.Attributes[SrcAttribute] == null)
            {
                imgSocialMedia.Attributes.Add(SrcAttribute, EcnImagesKmNewTwitterPng);
            }
            else
            {
                imgSocialMedia.Attributes[SrcAttribute] = EcnImagesKmNewTwitterPng;
            }

            tblHeader.BgColor = TableHeaderBgColorTwitter;
            ddlAccounts.Visible = false;
        }

        private void DisplayControlsFacebook(string subject, string accessToken)
        {
            txtMessage.Visible = true;
            txtMessage.Value = subject;
            trTitle.Visible = true;
            trSubTitle.Visible = true;
            trThumbnail.Visible = true;
            imgbtnThumbnail.Visible = true;
            chkUseThumbNail.Visible = true;
            imgSocialMedia.ImageUrl = EcnImagesKmNewFacebookPng;
            if (imgSocialMedia.Attributes[SrcAttribute] == null)
            {
                imgSocialMedia.Attributes.Add(SrcAttribute, EcnImagesKmNewFacebookPng);
            }
            else
            {
                imgSocialMedia.Attributes[SrcAttribute] = EcnImagesKmNewFacebookPng;
            }

            tblHeader.BgColor = TableHeaderBgColorFacebook;
            imgbtnReauth.CommandArgument = ReauthCommandArgumentFacebook;
            imgbtnReauth.OnClientClick = "fbLogoutUser();";
            FBAccounts = SocialMediaHelper.GetUserAccounts(accessToken);
            if (FBAccounts.Count > 0)
            {
                if (FBAccounts[0].id == InvalidAccountId)
                {
                    AccountErrrorMsg.Visible = true;
                    AccountErrrorMsg.Text = FBAccounts[0]
                        .name;
                    ddlAccounts.Visible = true;
                    ddlAccounts.Items.Clear();
                    ddlAccounts.Items.Insert(0, new ListItem() { Text = NoAccounts, Value = InvalidAccountId, Selected = true });
                }
                else
                {
                    ddlAccounts.Visible = true;
                    ddlAccounts.DataSource = FBAccounts;
                    ddlAccounts.DataTextField = AccountsDataTextField;
                    ddlAccounts.DataValueField = AccountsDataValueField;
                    ddlAccounts.DataBind();
                    ddlAccounts.Items.Insert(0, new ListItem() { Text = Select, Value = InvalidAccountId, Selected = true });
                }
            }
            else
            {
                ddlAccounts.Visible = true;
                ddlAccounts.Items.Clear();
                ddlAccounts.Items.Insert(0, new ListItem() { Text = NoAccounts, Value = InvalidAccountId, Selected = true });
            }

            lblSocialMedia.Text = YouWillBePostingTo;
            lblSocialMedia.Visible = true;
        }

        protected void imgbtnThumbnail_Click(object sender, ImageClickEventArgs e)
        {
            //ecn.editor/ckeditor/filemanager/FileManager.aspx?
            ImageButton imgbtnClicked = (ImageButton)sender;
            CurrentImageButton = imgbtnClicked.ClientID;

            var test = this.Parent;
            var test2 = this.Parent.Parent;
            string windowURL = string.Format("http://" + Request.Url.Host + "/ecn.communicator/main/Content/SocialFileManager.aspx?imgcontrol={0}&cuID={1}&chID={2}&hfToSet={3}&btnToSet={4}", imgbtnClicked.ClientID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, hfImagePath.ClientID, btnAssignImageURL.ClientID);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "imgSelector", "window.open('" + windowURL + "','_blank','menubar=no,titlebar=no,toolbar=no,scrollbars=yes','true');", true);

        }

        protected void btnCloseImageSelector_Click(object sender, EventArgs e)
        {

        }

        protected void btnAssignImageURL_Click(object sender, EventArgs e)
        {
            ImageURL = hfImagePath.Value;
            imgbtnThumbnail.ImageUrl = ImageURL;
            if (imgbtnThumbnail.Attributes[SrcAttribute] != null)
                imgbtnThumbnail.Attributes[SrcAttribute] = ImageURL;
            else
                imgbtnThumbnail.Attributes.Add(SrcAttribute, ImageURL);

            if (imgbtnThumbnail.Attributes["imagepath"] != null)
                imgbtnThumbnail.Attributes["imagepath"] = ImageURL;
            else
                imgbtnThumbnail.Attributes.Add("imagepath", ImageURL);
        }

        protected void ddlAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (SocialMediaID == 1)
            {
                //Facebook
                try
                {
                    if (ddlAccounts.SelectedIndex > 0)
                        imgProfile.ImageUrl = FBAccounts.Find(x => x.id == ddlAccounts.SelectedValue.ToString()).picture;
                    else
                    {
                        if (!string.IsNullOrEmpty(CurrentProfileImage))
                            imgProfile.ImageUrl = CurrentProfileImage;
                    }
                }
                catch { }
            }
            else if (SocialMediaID == 3)
            {
                //Linked In
                try
                {
                    if (ddlAccounts.SelectedIndex > 0)
                        imgProfile.ImageUrl = LIAccounts.Find(x => x.id == ddlAccounts.SelectedValue.ToString()).picture;
                    else
                    {
                        if (!string.IsNullOrEmpty(CurrentProfileImage))
                            imgProfile.ImageUrl = CurrentProfileImage;
                    }
                }
                catch { }
            }

            FolderEvent(sender, e);
            //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "pageloadedInsideConfig();", true);
        }

        protected void imgbtnReauth_Click(object sender, ImageClickEventArgs e)
        {
            //ImageButton imgbtn = (ImageButton)sender;
            //if(imgbtn.CommandArgument.Equals("1"))
            //{
            //    //Facebook
            //    string CallBackURL = Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.IndexOf("?"));
            //    CallBackURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&simple=fb&campaignItemType=" + campaignItemType;
            //    Response.Redirect(string.Format(
            //          "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
            //           FBapp_ID, HttpUtility.UrlEncode(CallBackURL), scope));
            //}
            //else if(imgbtn.CommandArgument.Equals("2"))
            //{
            //    //Twitter
            //    ECN_Framework_Common.Objects.OAuthHelper oauth = new ECN_Framework_Common.Objects.OAuthHelper();
            //    string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
            //    string request_Token = oauth.GetRequestToken(redirectAbsoluteURL.Replace("localhost", "127.0.0.1") + "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=tw");
            //    if (string.IsNullOrEmpty(oauth.oauth_error))
            //    {
            //        Response.Redirect(oauth.GetAuthorizeUrl(request_Token));

            //    }
            //    else
            //    {

            //    }
            //}
            //else if(imgbtn.CommandArgument.Equals("3"))
            //{
            //    //LinkedIn
            //    tempState = Guid.NewGuid();
            //    string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
            //    redirectAbsoluteURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=li";
            //    string redirectURI = string.Format("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id={0}&state={1}&redirect_uri={2}", HttpUtility.UrlEncode(LIapp_ID), tempState.ToString(),
            //        HttpUtility.UrlEncode(redirectAbsoluteURL));
            //    LI_OriginalURI = redirectAbsoluteURL;
            //    Response.Redirect(redirectURI);
            //}

        }
    }

}