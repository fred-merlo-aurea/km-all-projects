using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Configuration;
using ECN_Framework_Common.Objects;
using System.Data;
using System.Text.RegularExpressions;
using ECN_Framework_BusinessLayer.Activity.View;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.BusinessLogic;
using Enums = ECN_Framework_Common.Objects.Enums;
using EntitiesCampaignItemSocialMedia = ECN_Framework_Entities.Communicator.CampaignItemSocialMedia;
using EntitiesSocialMediaAuth = ECN_Framework_Entities.Communicator.SocialMediaAuth;
using EntitiesCampaignItemMetaTag = ECN_Framework_Entities.Communicator.CampaignItemMetaTag;

namespace ecn.communicator.main.ECNWizard.OtherControls
{

    public partial class SocialShare : System.Web.UI.UserControl, IECNWizard
    {
        private const string Localhost = "localhost";
        private const string LinkedInAuthorizeUrlTemplate = "https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}";
        private const string FacebookAuthorizeUrlTemplate = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}";
        private const string AccessTokenParamName = "access_token";
        private const string FacebookNetworkName = "Facebook";
        private const string LikeCationToken = "Like";
        private const string CommingSoonToken = "(Coming soon)";
        private const string ImagePath = "/ecn.images/KMNew/";
        private const string FirstNameToken = "first-name";
        private const string LastNameToken = "last-name";
        private const string OAuthToken = "oauth_token";
        private const string OAuthVerifier = "oauth_verifier";
        private const string SimpleToken = "simple";
        private const string FbSimpleValue = "fb";
        private const string LinkedInSimpleValue = "li";
        private const string TwitterSimpleValue = "tw";
        private const string CodeToken = "code";
        private const string StateToken = "state";
        private const string FirstNameTokenFacebook = "first_name";
        private const string LastNameTokenFacebook = "last_name";
        private const string PictureUrlToken = "picture-url";
        private const string Ip127_0_0_1 = "127.0.0.1";
        private const int FacebookLikeId = 4;
        private const int F2FSubShareId = 5;
        private const string SocialMediaAuthIdFieldName = "SocialMediaAuthID";
        private const string ProfileNameFieldName = "ProfileName";
        private const string NoSelectionValueText = "--Select--";
        private const string NoSelectionValue = "-1";
        private const string NameFieldName = "name";
        private const string IdFieldName = "id";
        private const string NoAccountsText = "No accounts";
        private const string TraceCategory = "Warning";
        private const string ExceptionSuppressedMessage = "Exception suppressed";
        private const string TitleTag = "og:title";
        private const string DescriptionTag = "og:description";
        private const string ImageTag = "og:image";
        private const string HashtagsTag = "hashtags";
        private static readonly int KMApplicationId = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);
        
        #region page variables

        private static Guid tempState;
        private static string LI_OriginalURI;
        private static string FBapp_ID = ConfigurationManager.AppSettings["FBAPPID"].ToString();
        private static string scope = ConfigurationManager.AppSettings["FBSCOPE"].ToString();
        private static string LIapp_ID = ConfigurationManager.AppSettings["LIAPPID"].ToString();
        private static string LIapp_Secret = ConfigurationManager.AppSettings["LIAPPSECRET"].ToString();

        private string TWRequest_Token = string.Empty;

        public string campaignItemType
        {
            get { return Request.QueryString["campaignitemtype"].ToString(); }
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

        string _campaignItemType = string.Empty;

        public string CampaignItemType
        {
            set
            {
                _campaignItemType = value;
            }
            get
            {
                return _campaignItemType;
            }
        }

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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            AssignEventHandler();
            if (!string.IsNullOrEmpty(hfFBImagePath.Value))
            {
                imgbtnFBImageMeta.ImageUrl = hfFBImagePath.Value;

                if (imgbtnFBImageMeta.Attributes["src"] != null)
                    imgbtnFBImageMeta.Attributes["src"] = hfFBImagePath.Value;
                else
                    imgbtnFBImageMeta.Attributes.Add("src", hfFBImagePath.Value);
            }
            else
            {
                if (imgbtnFBImageMeta.Attributes["imagepath"] == null || string.IsNullOrEmpty(imgbtnFBImageMeta.Attributes["imagepath"].ToString()))
                {
                    imgbtnFBImageMeta.ImageUrl = "/ecn.images/images/SelectImage.png";
                    if (imgbtnFBImageMeta.Attributes["src"] != null)
                        imgbtnFBImageMeta.Attributes["src"] = "/ecn.images/images/SelectImage.png";
                    else
                        imgbtnFBImageMeta.Attributes.Add("src", "/ecn.images/images/SelectImage.png");
                }
                else
                {
                    hfFBImagePath.Value = imgbtnFBImageMeta.Attributes["imagepath"].ToString();
                    imgbtnFBImageMeta.ImageUrl = hfFBImagePath.Value;
                    if (imgbtnFBImageMeta.Attributes["src"] != null)
                        imgbtnFBImageMeta.Attributes["src"] = hfFBImagePath.Value;
                    else
                        imgbtnFBImageMeta.Attributes.Add("src", hfFBImagePath.Value);
                }

            }
            if (!string.IsNullOrEmpty(hfLIImagePath.Value))
            {
                imgbtnLIImageMeta.ImageUrl = hfLIImagePath.Value;

                if (imgbtnLIImageMeta.Attributes["src"] != null)
                    imgbtnLIImageMeta.Attributes["src"] = hfLIImagePath.Value;
                else
                    imgbtnLIImageMeta.Attributes.Add("src", hfLIImagePath.Value);
            }
            else
            {
                if (imgbtnLIImageMeta.Attributes["imagepath"] == null || string.IsNullOrEmpty(imgbtnLIImageMeta.Attributes["imagepath"].ToString()))
                {
                    imgbtnLIImageMeta.ImageUrl = "/ecn.images/images/SelectImage.png";
                    if (imgbtnLIImageMeta.Attributes["src"] != null)
                        imgbtnLIImageMeta.Attributes["src"] = "/ecn.images/images/SelectImage.png";
                    else
                        imgbtnLIImageMeta.Attributes.Add("src", "/ecn.images/images/SelectImage.png");
                }
                else
                {
                    hfLIImagePath.Value = imgbtnLIImageMeta.Attributes["imagepath"].ToString();
                    imgbtnLIImageMeta.ImageUrl = hfLIImagePath.Value;
                    if (imgbtnLIImageMeta.Attributes["src"] != null)
                        imgbtnLIImageMeta.Attributes["src"] = hfLIImagePath.Value;
                    else
                        imgbtnLIImageMeta.Attributes.Add("src", hfLIImagePath.Value);
                }

            }
        }

        private string StripHTML(string dirty)
        {
            string retString = "";
            Regex htmlStrip = new Regex("<.*?>");
            retString = htmlStrip.Replace(dirty, "");

            return retString;
        }
        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.CampaignItem, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            DoTwemojiOnGrid();
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            
        }

        private void setECNError(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            ECNException ecnException = new ECNException(errorList, Enums.ExceptionLayer.WebSite);
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError2 in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError2.Entity + ": " + ecnError2.ErrorMessage;
            }
            
        }

        private void setECNException(ECNException ecn)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError2 in ecn.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError2.Entity + ": " + ecnError2.ErrorMessage;
            }
            
        }

        private void LoadSocialMedia()
        {
            var listSocialAuth = SocialMediaAuth.GetByCustomerID(ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            var listCism = CampaignItemSocialMedia.GetByCampaignItemID(CampaignItemID);
            var listMeta = CampaignItemMetaTag.GetByCampaignItemID(CampaignItemID);
            if (listSocialAuth.Count > 0)
            {
                LoadSocialAuths(listSocialAuth, listCism);
            }

            if (listCism.Any(x => x.SimpleShareDetailID == null))
            {
                chkSubShare.Checked = true;
                pnlSubShare.Visible = true;

                LoadFacebookControls(listCism, listMeta);

                LoadTwitterControls(listCism, listMeta);

                LoadLinkedInIdControls(listCism, listMeta);

                LoadFacebookLikeControls(listCism, listSocialAuth);
                LoadF2FSubShareControls(listCism);
            }
        }

        private void LoadF2FSubShareControls(IEnumerable<EntitiesCampaignItemSocialMedia> listCism)
        {
            if (listCism.All(x => x.SocialMediaID != F2FSubShareId))
            {
                return;
            }

            chkF2FSubShare.Checked = true;
        }

        private void LoadFacebookLikeControls(List<EntitiesCampaignItemSocialMedia> listCism, List<EntitiesSocialMediaAuth> listSocialAuth)
        {
            if (listCism.All(x => x.SocialMediaID != FacebookLikeId))
            {
                return;
            }

            var campaignItemSocialMedia = listCism.First(x => x.SocialMediaID == FacebookLikeId);
            chkFacebookLikeSubShare.Checked = true;
            ddlFacebookLikeSubShare.Visible = true;
            ddlFacebookUserAccounts.Visible = true;
            ddlFacebookLikeSubShare.DataSource = listSocialAuth.Where(x => x.SocialMediaID == FacebookId);
            ddlFacebookLikeSubShare.DataValueField = SocialMediaAuthIdFieldName;
            ddlFacebookLikeSubShare.DataTextField = ProfileNameFieldName;
            ddlFacebookLikeSubShare.DataBind();

            ddlFacebookLikeSubShare.Items.Insert(0, new ListItem {Text = NoSelectionValueText, Value = NoSelectionValue });
            ddlFacebookLikeSubShare.SelectedValue = campaignItemSocialMedia.SocialMediaAuthID.ToString();

            try
            {
                var sma = SocialMediaAuth.GetBySocialMediaAuthID(campaignItemSocialMedia.SocialMediaAuthID.GetValueOrDefault());
                var listFbAccounts = SocialMediaHelper.GetUserAccounts(sma.Access_Token);
                ddlFacebookUserAccounts.DataSource = listFbAccounts;
                ddlFacebookUserAccounts.DataTextField = NameFieldName;
                ddlFacebookUserAccounts.DataValueField = IdFieldName;
                ddlFacebookUserAccounts.DataBind();

                ddlFacebookUserAccounts.Items.Insert(0, new ListItem {Text = NoSelectionValueText, Selected = true, Value = NoSelectionValue });
                ddlFacebookUserAccounts.SelectedValue = campaignItemSocialMedia.PageID;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
                ddlFacebookUserAccounts.Items.Clear();
                ddlFacebookUserAccounts.Items.Insert(0, new ListItem {Text = NoAccountsText, Selected = true, Value = NoSelectionValue });
            }
        }

        private void LoadLinkedInIdControls(List<EntitiesCampaignItemSocialMedia> listCism, List<EntitiesCampaignItemMetaTag> listMeta)
        {
            if (listCism.All(x => x.SocialMediaID == LinkedInId && x.SimpleShareDetailID == null))
            {
                return;
            }

            chkLinkedInSubShare.Checked = true;
            tblLIMeta.Visible = true;
            if (listMeta.Any(x => x.SocialMediaID == LinkedInId))
            {
                LoadLinkedInMetadataControls(listMeta);
            }
        }

        private void LoadLinkedInMetadataControls(List<EntitiesCampaignItemMetaTag> listMeta)
        {
            try
            {
                txtLITitleMeta.Text = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                         x.Property.Equals(TitleTag, StringComparison.InvariantCultureIgnoreCase)).Content;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                hfLITitleMetaID.Value = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                           x.Property.Equals(TitleTag, StringComparison.InvariantCultureIgnoreCase)).CampaignItemMetaTagID.ToString();
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                txtLIDescMeta.Text = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                        x.Property.Equals(DescriptionTag, StringComparison.InvariantCultureIgnoreCase)).Content;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                hfLIDescMetaID.Value = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                          x.Property.Equals(DescriptionTag, StringComparison.InvariantCultureIgnoreCase)).CampaignItemMetaTagID.ToString();
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            if (listMeta.Exists(x => x.SocialMediaID == LinkedInId &&
                                     x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)) &&
                                     !string.IsNullOrWhiteSpace(listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                     x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)).Content))
            {
                imgbtnLIImageMeta.ImageUrl = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                                x.Property.ToLower().Equals(ImageTag)).Content;
                imgbtnLIImageMeta.Attributes["src"] = imgbtnLIImageMeta.ImageUrl;
                imgbtnLIImageMeta.Attributes.Add("imagepath", imgbtnLIImageMeta.ImageUrl);
                hfLIImagePath.Value = imgbtnLIImageMeta.ImageUrl;
            }

            if (listMeta.Exists(x => x.SocialMediaID == LinkedInId && x.Property.ToLower().Equals(ImageTag)))
            {
                hfLIImageMetaID.Value = listMeta.Find(x => x.SocialMediaID == LinkedInId &&
                                                           x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase))
                                                .CampaignItemMetaTagID.ToString();
            }
        }

        private void LoadTwitterControls(List<EntitiesCampaignItemSocialMedia> listCism, List<EntitiesCampaignItemMetaTag> listMeta)
        {
            if (!listCism.Any(x => x.SocialMediaID == TwitterId && x.SimpleShareDetailID == null))
            {
                return;
            }

            chkTwitterSubShare.Checked = true;
            tblTWMeta.Visible = true;
            if (listMeta.All(x => x.SocialMediaID != TwitterId))
            {
                return;
            }

            try
            {
                txtTWHashMeta.Text = listMeta.Find(x => x.SocialMediaID == TwitterId &&
                                                        x.Property.Equals(HashtagsTag, StringComparison.InvariantCultureIgnoreCase)).Content;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                hfTWHashMeta.Value = listMeta.Find(x => x.SocialMediaID == TwitterId &&
                                                        x.Property.Equals(HashtagsTag, StringComparison.InvariantCultureIgnoreCase)).CampaignItemMetaTagID.ToString();
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }
        }

        private void LoadFacebookControls(List<EntitiesCampaignItemSocialMedia> listCism, List<EntitiesCampaignItemMetaTag> listMeta)
        {
            if (listCism.All(x => x.SocialMediaID == FacebookId && x.SimpleShareDetailID == null))
            {
                return;
            }

            chkFacebookSubShare.Checked = true;
            tblFBMeta.Visible = true;
            if (listMeta.All(x => x.SocialMediaID != FacebookId))
            {
                return;
            }

            LoadFacebookMetadataControls(listMeta);
        }

        private void LoadFacebookMetadataControls(List<EntitiesCampaignItemMetaTag> listMeta)
        {
            try
            {
                txtFBTitleMeta.Text = listMeta
                    .Find(x => x.SocialMediaID == FacebookId && x.Property.Equals(TitleTag, StringComparison.InvariantCultureIgnoreCase))
                    .Content;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                hfFBTitleMetaID.Value = listMeta
                    .Find(x => x.SocialMediaID == FacebookId && x.Property.Equals(TitleTag, StringComparison.InvariantCultureIgnoreCase))
                    .CampaignItemMetaTagID.ToString();
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                txtFBDescMeta.Text = listMeta
                    .Find(x => x.SocialMediaID == FacebookId && x.Property.Equals(DescriptionTag, StringComparison.InvariantCultureIgnoreCase))
                    .Content;
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            try
            {
                hfFBDescMetaID.Value = listMeta
                    .Find(x => x.SocialMediaID == FacebookId && x.Property.Equals(DescriptionTag, StringComparison.InvariantCultureIgnoreCase))
                    .CampaignItemMetaTagID.ToString();
            }
            catch (Exception ex)
            {
                Trace.Warn(TraceCategory, ExceptionSuppressedMessage, ex);
            }

            if (listMeta.Exists(x => x.SocialMediaID == 1 && x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)) &&
                !string.IsNullOrEmpty(listMeta.Find(x => x.SocialMediaID == FacebookId &&
                                                         x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)).Content))
            {
                imgbtnFBImageMeta.ImageUrl = listMeta
                    .Find(x => x.SocialMediaID == FacebookId &&
                               x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)).Content;
                imgbtnFBImageMeta.Attributes["src"] = imgbtnFBImageMeta.ImageUrl;
                imgbtnFBImageMeta.Attributes.Add("imagepath", imgbtnFBImageMeta.ImageUrl);
                hfFBImagePath.Value = imgbtnFBImageMeta.ImageUrl;
            }

            if (listMeta.Exists(x => x.SocialMediaID == FacebookId &&
                                     x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase)))
            {
                hfFBImageMetaID.Value = listMeta.Find(x => x.SocialMediaID == FacebookId &&
                                                           x.Property.Equals(ImageTag, StringComparison.InvariantCultureIgnoreCase))
                    .CampaignItemMetaTagID.ToString();
            }
        }

        private void LoadSocialAuths(List<EntitiesSocialMediaAuth> listSocialAuth, List<EntitiesCampaignItemSocialMedia> listCism)
        {
            gvSimpleShare.DataSource = listSocialAuth.Where(x => x.IsDeleted == false).ToList();
            gvSimpleShare.DataBind();
            CampaignItem.GetByCampaignItemID(CampaignItemID, ECNSession.CurrentSession().CurrentUser, false);

            if (listCism.All(x => x.SimpleShareDetailID == null))
            {
                return;
            }

            foreach (GridViewRow gridViewRow in gvSimpleShare.Rows)
            {
                if (gridViewRow.RowType != DataControlRowType.DataRow)
                    {
                        continue;
                    }

                LoadSocialAuthRow(listCism, gridViewRow);
            }
        }

        private void LoadSocialAuthRow(List<EntitiesCampaignItemSocialMedia> listCism, GridViewRow gridViewRow)
        {
            int socialAuthId;
            if (!int.TryParse(gvSimpleShare.DataKeys[gridViewRow.RowIndex]?.Value.ToString(), out socialAuthId))
            {
                return;
            }

            var socialMediaAuth = SocialMediaAuth.GetBySocialMediaAuthID(socialAuthId);
            var chkEnable = gridViewRow.FindControl("chkEnableSimpleShare") as CheckBox;
            if (chkEnable == null)
            {
                return;
            }

            chkEnable.Enabled = true;
            var campaignItemSocialMedia = listCism.FirstOrDefault(x => x.SocialMediaAuthID == socialMediaAuth.SocialMediaAuthID && x.SimpleShareDetailID != null);
            if (campaignItemSocialMedia == null)
            {
                return;
            }

            chkSimpleShare.Checked = true;
            gvSimpleShare.Visible = true;
            var upConfig = gridViewRow.FindControl("upSocialConfig") as UpdatePanel;
            if (!chkEnable.Enabled || upConfig == null)
            {
                return;
            }

            chkEnable.Checked = true;
            upConfig.Visible = true;
            var scConfig = gridViewRow.FindControl("scConfig") as SocialConfig;
            if (scConfig == null)
            {
                return;
            }

            var ssd = SimpleShareDetail.GetBySimpleShareDetailID(campaignItemSocialMedia.SimpleShareDetailID.GetValueOrDefault());
            scConfig.Title = ssd.Title;
            scConfig.Subtitle = ssd.SubTitle;
            scConfig.ImagePath = !string.IsNullOrEmpty(ssd.ImagePath)
                ? ssd.ImagePath
                : "/ecn.images/images/SelectImage.png";
            scConfig.Message = ssd.Content;
            scConfig.UseThumbnail = ssd.UseThumbnail.GetValueOrDefault();
            scConfig.SocialMediaID = ssd.SocialMediaID;

            var ddlAccounts = (DropDownList) scConfig.FindControl("ddlAccounts");
            if (campaignItemSocialMedia.SocialMediaID == FacebookId || campaignItemSocialMedia.SocialMediaID == LinkedInId)
            {
                ddlAccounts.SelectedValue = ssd.PageID;
            }
        }

        public void Initialize()
        {
            if (User.HasAccess(
                ECNSession.CurrentSession().CurrentUser,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit))
            {
                phError.Visible = false;
                int fbUserId;
                int.TryParse(ConfigurationManager.AppSettings["FBUserID"], out fbUserId);
                if (fbUserId > 0 && ECNSession.CurrentSession().CurrentUser.UserID != fbUserId)
                {
                    RenderFacebookInactive();
                }
                else if (fbUserId > 0)
                {
                    RenderFacebookActive();
                }

                if (!Page.IsPostBack)
                {
                    InitializeSocialNetworks();
                }
                LoadSocialMedia();
                DoTwemojiOnGrid();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void InitializeSocialNetworks()
        {
            if (Request.QueryString[SimpleToken]?.Equals(FbSimpleValue, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                InitializeFacebook();
            }
            else if (Request.QueryString[SimpleToken]?.Equals(LinkedInSimpleValue, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                InitializeLinkedIn();
            }
            else if (Request.QueryString[SimpleToken]?.Equals(TwitterSimpleValue, StringComparison.InvariantCultureIgnoreCase) == true &&
                     (Request.QueryString[OAuthToken] != null &&
                      Request.QueryString[OAuthVerifier] != null))
            {
                InitializeTwitter();
            }
        }

        private void InitializeTwitter()
        {
            var oauthToken = Request.QueryString[OAuthToken];
            var oauthVerifier = Request.QueryString[OAuthVerifier];

            var oauthHelper = new OAuthHelper();
            var callbackUrl = Request.Url.AbsoluteUri.Contains(Localhost)
                ? Request.Url.AbsoluteUri.Replace(Localhost, Ip127_0_0_1)
                : Request.Url.AbsoluteUri;
            callbackUrl = $"{callbackUrl}&simple=tw";

            oauthHelper.GetUserTwAccessToken(oauthToken, oauthVerifier, callbackUrl);
            var dirtyProfile = oauthHelper.GetTwitterProfile(oauthHelper.user_id,
                oauthHelper.oauth_access_token,
                oauthHelper.oauth_access_token_secret);

            var twUserProfile = SocialMediaHelper.GetJSONDict(dirtyProfile);

            if (string.IsNullOrWhiteSpace(oauthHelper.oauth_error))
            {
                UserCheck(
                    oauthHelper.user_id,
                    TwitterId,
                    oauthHelper.oauth_access_token,
                    oauthHelper.oauth_access_token_secret,
                    twUserProfile[NameFieldName] ?? string.Empty);
            }
        }

        private void InitializeLinkedIn()
        {
            if (Request.QueryString[CodeToken] == null ||
                Request.QueryString[StateToken]?.Equals(tempState.ToString()) != true)
            {
                return;
            }

            var authCode = Request.QueryString[CodeToken];
            var url = string.Format(
                LinkedInAuthorizeUrlTemplate,
                authCode,
                HttpUtility.UrlEncode(LI_OriginalURI),
                LIapp_ID,
                LIapp_Secret);

            var tokens = GetLinkedInTokens(url);
            if (tokens == null)
            {
                return;
            }

            var accessToken = tokens[AccessTokenParamName] ?? string.Empty;
            var liUserProfile = SocialMediaHelper.GetLIUserProfile(accessToken);
            var fullName = (liUserProfile[FirstNameToken] ?? string.Empty) +
                           (liUserProfile[LastNameToken] != null
                               ? Whitespace + liUserProfile[LastNameToken]
                               : string.Empty);
            UserCheck(liUserProfile[IdFieldName],
                LinkedInId,
                accessToken,
                string.Empty,
                fullName);
        }

        private static Dictionary<string, string> GetLinkedInTokens(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                return null;
            }

            request.Method = "POST";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    return new Dictionary<string, string>();
                }

                var reader = new StreamReader(response.GetResponseStream());
                return SocialMediaHelper.GetJSONDict(reader.ReadToEnd());
            }
        }

        private void InitializeFacebook()
        {
            if (Request.QueryString[CodeToken] == null)
            {
                Response.Redirect(string.Format(
                    FacebookAuthorizeUrlTemplate,
                    FBapp_ID,
                    HttpUtility.UrlEncode($"{Request.Url.AbsoluteUri}%26simple=fb%26campaignitemtype={campaignItemType}"),
                    scope));
            }
            else
            {
                var accessToken = string.Empty;
                var tokens = SocialMediaHelper.GetFBAccessToken(Request.QueryString[CodeToken], Request.Url.AbsoluteUri);
                accessToken = ExchangeFacebookAccessToken(tokens, accessToken);

                //Get FB User ID to see if we already have them registered
                var fbProfile = new Dictionary<string, string>();
                try
                {
                    fbProfile = SocialMediaHelper.GetFBUserProfile(accessToken);
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                        ex,
                        "get fb Profile",
                        KMApplicationId);
                }

                try
                {
                    var fullName = (fbProfile[FirstNameTokenFacebook] ?? string.Empty) +
                                   (fbProfile[LastNameTokenFacebook] != null ? Whitespace + fbProfile[LastNameTokenFacebook] : string.Empty);
                    UserCheck(
                        fbProfile[IdFieldName],
                        FacebookId,
                        accessToken,
                        string.Empty,
                        fullName);
                    ScriptManager.RegisterStartupScript(this, GetType(), "fbLogOUT", "fbLogoutUser();", true);
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                        ex,
                        "User Check",
                        KMApplicationId);
                }
            }
        }

        private static string ExchangeFacebookAccessToken(IReadOnlyDictionary<string, string> tokens, string accessToken)
        {
            var longlivedTokens = new Dictionary<string, string>();
            //exchange the access token for a long lived token
            try
            {
                longlivedTokens = SocialMediaHelper.GetFBLongLivedToken(tokens[AccessTokenParamName]);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                    ex,
                    "get long lived token with short lived token",
                    KMApplicationId);
            }

            try
            {
                accessToken = tokens[AccessTokenParamName];
                try
                {
                    accessToken = longlivedTokens[AccessTokenParamName];
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                        ex,
                        "get long lived token from longlivedTokens dict",
                        KMApplicationId);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                    ex,
                    "getaccesstoken from tokens dict",
                    KMApplicationId);
            }

            return accessToken;
        }

        private void RenderFacebookActive()
        {
            imgbtnFacebookSimple.ImageUrl = $"{ImagePath}facebook.png";
            imgbtnFacebookSimple.Enabled = true;
            lblFacebookSimple.Text = FacebookNetworkName;
            chkFacebookLikeSubShare.Enabled = true;
            chkFacebookLikeSubShare.Text = $"{FacebookNetworkName} {LikeCationToken}";
        }

        private void RenderFacebookInactive()
        {
            imgbtnFacebookSimple.ImageUrl = $"{ImagePath}facebook_inactive.png";
            imgbtnFacebookSimple.Enabled = false;
            lblFacebookSimple.Text = $"{FacebookNetworkName}{CommingSoonToken}";
            chkFacebookLikeSubShare.Enabled = false;
            chkFacebookLikeSubShare.Text = $"{FacebookNetworkName} {LikeCationToken}{CommingSoonToken}";
        }

        private void UserCheck(string userIDToCheck, int SocialMediaID, string access_token, string access_secret, string profileName)
        {
            //this checks if they exist for the current customer
            if (ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.ExistsByUserID(userIDToCheck, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, SocialMediaID))
            {
                //if they do, grab all of the records with the same UserID/SocialMediaID for all customers in the base channel and update with new access_token
                //this method only asks for customerid but finds all records for the base channel
                List<ECN_Framework_Entities.Communicator.SocialMediaAuth> smaList = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetByUserID_CustomerID_SocialMediaID(userIDToCheck, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, SocialMediaID);

                foreach (ECN_Framework_Entities.Communicator.SocialMediaAuth sma in smaList)
                {
                    sma.Access_Token = access_token;
                    sma.Access_Secret = access_secret;
                    sma.ProfileName = profileName;
                    ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(sma, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
            }
            else
            {
                //if they don't exist for the current customer, still update all of the records with the same UserID/SocialMediaID
                //this method only asks for customerid but finds all records for the base channel
                List<ECN_Framework_Entities.Communicator.SocialMediaAuth> smaList = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetByUserID_CustomerID_SocialMediaID(userIDToCheck, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, SocialMediaID);

                foreach (ECN_Framework_Entities.Communicator.SocialMediaAuth sma in smaList)
                {
                    sma.Access_Token = access_token;
                    sma.Access_Secret = access_secret;
                    sma.ProfileName = profileName;
                    ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(sma, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                //Then add a new record for the current customer
                ECN_Framework_Entities.Communicator.SocialMediaAuth newSMA = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
                newSMA.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                newSMA.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                newSMA.IsDeleted = false;
                newSMA.SocialMediaID = SocialMediaID;
                newSMA.Access_Token = access_token;
                newSMA.UserID = userIDToCheck;
                newSMA.Access_Secret = access_secret;
                newSMA.ProfileName = profileName;
                ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(newSMA, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
        }

        #region Grid View events

        private void AssignEventHandler()
        {
            foreach(GridViewRow gvr in gvSimpleShare.Rows)
            {
                SocialConfig scFB = (SocialConfig)gvr.FindControl("scConfig");
                scFB.FolderEvent += new EventHandler(DoTwemojiOnGridHandler);
            }
        }

        protected void gvSimpleShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            try
            {
                var chConfigure =  e.Row.FindControl("chkEnableSimpleShare") as CheckBox;
                if (chConfigure != null)
                {
                    chConfigure.Attributes.Add("index", e.Row.RowIndex.ToString());
                    chConfigure.Enabled = true;
                }

                var socialMediaAuth = SocialMediaAuth.GetBySocialMediaAuthID(
                    Convert.ToInt32(gvSimpleShare.DataKeys[e.Row.RowIndex]?.Value));
                if (socialMediaAuth == null)
                {
                    return;
                }

                var sm = SocialMedia.GetSocialMediaByID(socialMediaAuth.SocialMediaID);
                var lblSocialNetwork = (Label) e.Row.FindControl("lblSocialNetwork");
                lblSocialNetwork.Text = sm.DisplayName;

                var emailSubject = GetEmailSubject();

                switch (sm.SocialMediaID)
                {
                    case FacebookId:
                    {
                        BindFacebookRow(e, socialMediaAuth, emailSubject, chConfigure);
                        break;
                    }
                    case TwitterId:
                    {
                        BindTwitterRow(e, socialMediaAuth, emailSubject, chConfigure);
                        break;
                    }
                    case LinkedInId:
                    {
                        BindLinkedInRow(e, socialMediaAuth, emailSubject, chConfigure);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // POSSIBLE BUG: exception should be logged
                Trace.Warn("Error", "Unable to retrieve the image: {0}", ex);
            }
        }

        private string GetEmailSubject()
        {
            if (campaignItemType.Equals(ChampionCampainName, StringComparison.InvariantCultureIgnoreCase))
            {
                var campaignItem = CampaignItem.GetByCampaignItemID(
                    CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser,
                    false);
                var sample = Sample.GetBySampleID(campaignItem.SampleID.Value, ECNSession.CurrentSession().CurrentUser);
                var dtSample = BlastActivity.ChampionByProc(
                    campaignItem.SampleID.Value,
                    false,
                    ECNSession.CurrentSession().CurrentUser,
                    sample.ABWinnerType);
                return dtSample.Select("Winner = 'true'")[0]["EmailSubject"].ToString();
            }

            var cib = CampaignItemBlast.GetByCampaignItemID(
                CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                false);
            return cib.Count > 0 ? cib[0].EmailSubject : string.Empty;
        }

        private void BindLinkedInRow(GridViewRowEventArgs e, ECN_Framework_Entities.Communicator.SocialMediaAuth sma, string emailSubject, CheckBox chConfigure)
        {
            var lblAccountName = (Label) e.Row.FindControl("lblAccountName");
            var imgbtnDelete = (ImageButton) e.Row.FindControl("imgbtnDelete");
            var scLi = (SocialConfig) e.Row.FindControl("scConfig");
            scLi.FolderEvent += DoTwemojiOnGridHandler;
            try
            {
                lblAccountName.Text = sma.ProfileName;
                imgbtnDelete.CommandArgument = sma.SocialMediaAuthID.ToString();
                var tokens = SocialMediaHelper.GetLIUserProfile(sma.Access_Token);

                if (!tokens.ContainsKey(PictureUrlToken))
                {
                    tokens.Add(PictureUrlToken, string.Empty);
                }

                scLi.LoadSocialMedia(sma.SocialMediaID,
                    sma.ProfileName,
                    tokens[PictureUrlToken]?.Replace(@"\", string.Empty) ?? string.Empty,
                    emailSubject,
                    sma.Access_Token);
                chConfigure.Enabled = true;
            }
            catch (Exception ex)
            {
                lblAccountName.Text = $"Unable to get profile for {sma.ProfileName}";
                chConfigure.Enabled = false;
                chConfigure.Checked = false;
            }
        }

        private void BindTwitterRow(GridViewRowEventArgs e, ECN_Framework_Entities.Communicator.SocialMediaAuth sma, string emailSubject, CheckBox chConfigure)
        {
            var scTW = (SocialConfig) e.Row.FindControl("scConfig");
            var lblAccountName = (Label) e.Row.FindControl("lblAccountName");
            var imgbtnDelete = (ImageButton) e.Row.FindControl("imgbtnDelete");
            scTW.FolderEvent += DoTwemojiOnGridHandler;
            try
            {
                lblAccountName.Text = sma.ProfileName;

                imgbtnDelete.CommandArgument = sma.SocialMediaAuthID.ToString();
                var oauth = new OAuthHelper();
                var dirtyProfile = oauth.GetTwitterProfile(sma.UserID, sma.Access_Token, sma.Access_Secret);
                var twUserProfile = SocialMediaHelper.GetJSONDict(dirtyProfile);

                scTW.LoadSocialMedia(sma.SocialMediaID,
                    sma.ProfileName,
                    twUserProfile["profile_image_url_https"]?.Replace(@"\", string.Empty) ?? string.Empty,
                    emailSubject,
                    sma.Access_Token);
                chConfigure.Enabled = true;
            }
            catch (Exception ex)
            {
                lblAccountName.Text = $"Unable to get profile for {sma.ProfileName}";
                chConfigure.Checked = false;
                chConfigure.Enabled = false;
            }
        }

        private void BindFacebookRow(GridViewRowEventArgs e, ECN_Framework_Entities.Communicator.SocialMediaAuth sma, string emailSubject, CheckBox chConfigure)
        {
            var lblAccountName = (Label) e.Row.FindControl("lblAccountName");
            var imgbtnDelete = (ImageButton) e.Row.FindControl("imgbtnDelete");
            var scFB = (SocialConfig) e.Row.FindControl("scConfig");
            scFB.FolderEvent += DoTwemojiOnGridHandler;
            try
            {
                lblAccountName.Text = sma.ProfileName;
                imgbtnDelete.CommandArgument = sma.SocialMediaAuthID.ToString();
                var tokens = SocialMediaHelper.GetFBUserProfile(sma.Access_Token);

                scFB.LoadSocialMedia(sma.SocialMediaID,
                    sma.ProfileName,
                    tokens["url"]?.Replace(@"\", string.Empty) ?? string.Empty,
                    emailSubject,
                    sma.Access_Token);
                chConfigure.Enabled = true;
            }
            catch (Exception ex)
            {
                lblAccountName.Text = $"Unable to get profile for {sma.ProfileName}";
                chConfigure.Checked = false;
                chConfigure.Enabled = false;
            }
        }

        private void DoTwemojiOnGridHandler(object sender, EventArgs e)
        {
            DoTwemojiOnGrid();
        }

        protected void gvSimpleShare_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletesocialmedia"))
            {
                if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    //have to uncheck the profiles enable check box otherwise it will get saved and not pass the UsedInBlasts check
                    foreach (GridViewRow gvr in gvSimpleShare.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            string dataKey = gvSimpleShare.DataKeys[gvr.RowIndex].Value.ToString();
                            if (dataKey == e.CommandArgument.ToString())
                            {
                                CheckBox chkEnable = (CheckBox)gvr.FindControl("chkEnableSimpleShare");
                                chkEnable.Checked = false;
                                SocialConfig scFB = (SocialConfig)gvr.FindControl("scConfig");
                                scFB.Visible = false;
                            }
                        }
                    }
                    if (SaveWithoutAdvance())
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.UsedInBlasts(Convert.ToInt32(e.CommandArgument.ToString())))
                        {
                            lblPermission.Text = "Cannot delete profile as it is used in pending or active blasts";
                            pnlDelete.Visible = false;
                            pnlPermission.Visible = true;
                            mpeDeleteProfile.Show();
                        }
                        else
                        {
                            pnlDelete.Visible = true;
                            btnDeleteProfile.CommandArgument = e.CommandArgument.ToString();
                            pnlPermission.Visible = false;
                            mpeDeleteProfile.Show();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    lblPermission.Text = "You must have administrative privileges to perform this action";
                    pnlDelete.Visible = false;
                    pnlPermission.Visible = true;
                    mpeDeleteProfile.Show();
                }

            }
            else if (e.CommandName.ToLower().Equals("reauth"))
            {
                if (e.CommandArgument.Equals("1"))
                {
                    //Facebook
                    string CallBackURL = Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.IndexOf("?"));
                    CallBackURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&simple=fb&campaignItemType=" + campaignItemType;
                    Response.Redirect(string.Format(
                          "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
                           FBapp_ID, HttpUtility.UrlEncode(CallBackURL), scope));
                }
                else if (e.CommandArgument.Equals("2"))
                {
                    //Twitter
                    ECN_Framework_Common.Objects.OAuthHelper oauth = new ECN_Framework_Common.Objects.OAuthHelper();
                    string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
                    string request_Token = oauth.GetRequestToken(redirectAbsoluteURL.Replace(Localhost, Ip127_0_0_1) + "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=tw");
                    if (string.IsNullOrEmpty(oauth.oauth_error))
                    {
                        Response.Redirect(oauth.GetAuthorizeUrl(request_Token));

                    }
                    else
                    {

                    }
                }
                else if (e.CommandArgument.Equals("3"))
                {
                    //LinkedIn
                    tempState = Guid.NewGuid();
                    string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
                    redirectAbsoluteURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=li";
                    string redirectURI = string.Format("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id={0}&state={1}&redirect_uri={2}", HttpUtility.UrlEncode(LIapp_ID), tempState.ToString(),
                        HttpUtility.UrlEncode(redirectAbsoluteURL));
                    LI_OriginalURI = redirectAbsoluteURL;
                    Response.Redirect(redirectURI);
                }
            }

            DoTwemojiOnGrid();
        }
        #endregion

        #region control events

        protected void chkEnableSimpleShare_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkConfig = (CheckBox)sender;
            int index = Convert.ToInt32(chkConfig.Attributes["index"].ToString());
            GridViewRow gvr = gvSimpleShare.Rows[index];

            if (chkConfig.Checked)
            {
                UpdatePanel upConfig = (UpdatePanel)gvr.FindControl("upSocialConfig");
                upConfig.Visible = true;
            }
            else
            {
                UpdatePanel upConfig = (UpdatePanel)gvr.FindControl("upSocialConfig");
                upConfig.Visible = false;
            }

            DoTwemojiOnGrid();
        }



        private void DoTwemojiOnGrid()
        {
            bool progressShown = false;
            foreach (GridViewRow gvr in gvSimpleShare.Rows)
            {

                if(!progressShown)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "ShowProgress('" + UpdateProgress2.ClientID + "');", true);
                    progressShown = true;
                }

                string controlClientIDMessage = "";
                string controlClientIDTitle = "";
                string controlClientIDSubTitle = "";
                int socialMedia = -1;
                try
                {
                    UpdatePanel upConfig = (UpdatePanel)gvr.FindControl("upSocialConfig");
                    if (upConfig != null && upConfig.Visible == true)
                    {
                        ecn.communicator.main.ECNWizard.OtherControls.SocialConfig scConfig = (ecn.communicator.main.ECNWizard.OtherControls.SocialConfig)upConfig.FindControl("scConfig");
                        socialMedia = scConfig.SocialMediaID;
                        HiddenField hf = (HiddenField)scConfig.FindControl("txtMessage");
                        if (hf != null && hf.Visible == true)
                            controlClientIDMessage = hf.ClientID;
                        HiddenField hfTitle = (HiddenField)scConfig.FindControl("txtPostLink");
                        if (hfTitle != null && hfTitle.Visible == true)
                            controlClientIDTitle = hfTitle.ClientID;
                        HiddenField hfSubTitle = (HiddenField)scConfig.FindControl("txtPostSubTitle");
                        if (hfSubTitle != null && hfSubTitle.Visible == true)
                            controlClientIDSubTitle = hfSubTitle.ClientID;
                    }
                }
                catch { }

                string MaxLengthMessage = "";
                string MaxLengthTitle = "255";
                string MaxLengthSubTitle = "255";
                bool isFB = false;
                switch(socialMedia)
                {
                    case 1:
                        //facebook limit - 100
                        MaxLengthMessage = "200";
                        MaxLengthTitle = "100";
                        MaxLengthSubTitle = "250";
                        isFB = true;
                        break;
                    case 2:
                        //twitter limit - 118
                        MaxLengthMessage = "118";
                        break;
                    case 3:
                        //linked in limit - 255
                        MaxLengthMessage = "255";
                        break;
                }

                if (!string.IsNullOrEmpty(controlClientIDMessage))
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "assignTwemoji('" + controlClientIDMessage + "'," + MaxLengthMessage + ", false);", true);
                if (!string.IsNullOrEmpty(controlClientIDTitle))
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "assignTwemoji('" + controlClientIDTitle + "'," + MaxLengthTitle + ", " + isFB.ToString().ToLower() + ");", true);
                if (!string.IsNullOrEmpty(controlClientIDSubTitle))
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "assignTwemoji('" + controlClientIDSubTitle + "'," + MaxLengthSubTitle + ", " + isFB.ToString().ToLower() + ");", true);
            }
            if(progressShown)
                System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "HideProgress('" + UpdateProgress2.ClientID + "');", true);

        }

        protected void chkSubShare_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSubShare.Checked)
            {
                pnlSubShare.Visible = true;
            }
            else
                pnlSubShare.Visible = false;
            DoTwemojiOnGrid();
        }

        protected void chkFacebookLikeSubShare_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFacebookLikeSubShare.Checked)
            {
                ddlFacebookLikeSubShare.Visible = true;
                List<ECN_Framework_Entities.Communicator.SocialMediaAuth> listFBAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetByCustomerID_SocialMediaID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, 1);
                ddlFacebookLikeSubShare.DataSource = listFBAuth;
                ddlFacebookLikeSubShare.DataValueField = SocialMediaAuthIdFieldName;
                ddlFacebookLikeSubShare.DataTextField = "ProfileName";
                ddlFacebookLikeSubShare.DataBind();

                ddlFacebookLikeSubShare.Items.Insert(0, new ListItem(NoSelectionValueText, NoSelectionValue));
            }
            else
            {
                ddlFacebookLikeSubShare.Visible = false;
                ddlFacebookUserAccounts.Visible = false;
            }
            DoTwemojiOnGrid();
        }

        protected void ddlFacebookLikeSubShare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFacebookLikeSubShare.SelectedIndex > 0)
            {
                ECN_Framework_Entities.Communicator.SocialMediaAuth sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(Convert.ToInt32(ddlFacebookLikeSubShare.SelectedValue.ToString()));
                List<ECN_Framework_Common.Objects.SocialMediaHelper.FBAccount> listFBAccounts = ECN_Framework_Common.Objects.SocialMediaHelper.GetUserAccounts(sma.Access_Token);
                ddlFacebookUserAccounts.DataSource = listFBAccounts;
                ddlFacebookUserAccounts.DataTextField = NameFieldName;
                ddlFacebookUserAccounts.DataValueField = IdFieldName;
                ddlFacebookUserAccounts.DataBind();
                ddlFacebookUserAccounts.Items.Insert(0, new ListItem() { Text = NoSelectionValueText, Selected = true, Value = NoSelectionValue });
                ddlFacebookUserAccounts.Visible = true;
            }
            DoTwemojiOnGrid();
        }
        protected void chkSimpleShare_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSimpleShare.Checked)
            {
                gvSimpleShare.Visible = true;
                foreach (GridViewRow gvr in gvSimpleShare.Rows)
                {
                    CheckBox chkEnable = (CheckBox)gvr.FindControl("chkEnableSimpleShare");
                    chkEnable.Enabled = true;

                }

            }
            else
            {

                foreach (GridViewRow gvr in gvSimpleShare.Rows)
                {
                    CheckBox chkEnable = (CheckBox)gvr.FindControl("chkEnableSimpleShare");
                    chkEnable.Enabled = false;
                }
                gvSimpleShare.Visible = false;
            }
            DoTwemojiOnGrid();
            
        }

        protected void imgbtnFacebookSimple_Click(object sender, ImageClickEventArgs e)
        {
            pnlStartSimple.Visible = false;

            if (SaveWithoutAdvance())
            {
                string CallBackURL = Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.IndexOf("?"));
                CallBackURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&simple=fb&campaignItemType=" + campaignItemType;
                Response.Redirect(string.Format(
                      "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
                       FBapp_ID, HttpUtility.UrlEncode(CallBackURL), scope));
            }
            else
            {
                DoTwemojiOnGrid();
                return;
            }
            
        }

        protected void imgbtnTwitterSimple_Click(object sender, ImageClickEventArgs e)
        {
            pnlStartSimple.Visible = false;

            if (SaveWithoutAdvance())
            {
                ECN_Framework_Common.Objects.OAuthHelper oauth = new ECN_Framework_Common.Objects.OAuthHelper();
                string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
                string request_Token = oauth.GetRequestToken(redirectAbsoluteURL.Replace(Localhost, Ip127_0_0_1) + "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=tw");
                if (string.IsNullOrEmpty(oauth.oauth_error))
                {
                    Response.Redirect(oauth.GetAuthorizeUrl(request_Token));

                }
                else
                {
                    DoTwemojiOnGrid();
                }
            }
            else
            {
                DoTwemojiOnGrid();
                return;
            }

        }

        protected void imgbtnLinkedInSimple_Click(object sender, ImageClickEventArgs e)
        {
            if (SaveWithoutAdvance())
            {
                pnlStartSimple.Visible = false;
                tempState = Guid.NewGuid();
                string redirectAbsoluteURL = Request.Url.AbsoluteUri.Replace(Request.Url.Query, "");
                redirectAbsoluteURL += "?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemtype=" + campaignItemType + "&simple=li";
                string redirectURI = string.Format("https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id={0}&state={1}&redirect_uri={2}", HttpUtility.UrlEncode(LIapp_ID), tempState.ToString(),
                    HttpUtility.UrlEncode(redirectAbsoluteURL));
                LI_OriginalURI = redirectAbsoluteURL;
                Response.Redirect(redirectURI);
            }
            else
            {
                DoTwemojiOnGrid();
                return;
            }

        }
        protected void btnAddSocialNetwork_Click(object sender, EventArgs e)
        {

            mpeSimpleShare.Show();
            DoTwemojiOnGrid();
        }
        #endregion

        protected void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(Convert.ToInt32(btnDeleteProfile.CommandArgument.ToString()));

            if (sma.SocialMediaID == 1)
            {
                sma.IsDeleted = true;
                ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(sma, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            }
            else if (sma.SocialMediaID == 2)
            {
                sma.IsDeleted = true;
                ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(sma, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            else if (sma.SocialMediaID == 3)
            {
                sma.IsDeleted = true;
                ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.Save(sma, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            //btnDeleteProfile.CommandArgument = string.Empty;
            //mpeDeleteProfile.Hide();
            //LoadSocialMedia();
            Response.Redirect("wizardsetup.aspx?CampaignItemID=" + CampaignItemID.ToString() + "&campaignItemType=" + campaignItemType.ToString());
        }

        protected void btnOkayPermission_Click(object sender, EventArgs e)
        {
            btnDeleteProfile.CommandArgument = string.Empty;
            mpeDeleteProfile.Hide();
            DoTwemojiOnGrid();
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            mpeDeleteProfile.Hide();
            DoTwemojiOnGrid();
        }

        protected void chkFacebookSubShare_CheckedChanged(object sender, EventArgs e)
        {
            tblFBMeta.Visible = chkFacebookSubShare.Checked;
            DoTwemojiOnGrid();
        }

        protected void chkTwitterSubShare_CheckedChanged(object sender, EventArgs e)
        {
            tblTWMeta.Visible = chkTwitterSubShare.Checked;
            DoTwemojiOnGrid();
        }

        protected void chkLinkedInSubShare_CheckedChanged(object sender, EventArgs e)
        {
            tblLIMeta.Visible = chkLinkedInSubShare.Checked;
            DoTwemojiOnGrid();
        }

        protected void imgbtnMetaImage_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtnClicked = (ImageButton)sender;
            string CurrentImageButton = imgbtnClicked.ClientID;
            HiddenField hfImagePath = new HiddenField();
            Button btnToClick = new Button();
            if (imgbtnClicked.ID.ToLower().Equals("imgbtnfbimagemeta"))
            {
                hfImagePath = hfFBImagePath;
                btnToClick = btnFBImage;
            }
            else
            {
                btnToClick = btnLIImage;
                hfImagePath = hfLIImagePath;
            }

            string windowURL = string.Format("http://" + Request.Url.Host + "/ecn.communicator/main/Content/SocialFileManager.aspx?imgcontrol={0}&cuID={1}&chID={2}&hfToSet={3}&btnToSet={4}", imgbtnClicked.ClientID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, hfImagePath.ClientID, btnToClick.ClientID);
            DoTwemojiOnGrid();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "imgSelector", "window.open('" + windowURL + "','_blank','menubar=no,titlebar=no,toolbar=no,scrollbars=yes','true');", true);
        }

        protected void btnLIImage_Click(object sender, EventArgs e)
        {
            imgbtnLIImageMeta.ImageUrl = hfLIImagePath.Value;
            if (imgbtnLIImageMeta.Attributes["src"] != null)
                imgbtnLIImageMeta.Attributes["src"] = hfLIImagePath.Value;
            else
                imgbtnLIImageMeta.Attributes.Add("src", hfLIImagePath.Value);

            if (imgbtnLIImageMeta.Attributes["imagepath"] != null)
                imgbtnLIImageMeta.Attributes["imagepath"] = hfLIImagePath.Value;
            else
                imgbtnLIImageMeta.Attributes.Add("imagepath", hfLIImagePath.Value);
            DoTwemojiOnGrid();
        }

        protected void btnFBImage_Click(object sender, EventArgs e)
        {
            imgbtnFBImageMeta.ImageUrl = hfFBImagePath.Value;
            if (imgbtnFBImageMeta.Attributes["src"] != null)
                imgbtnFBImageMeta.Attributes["src"] = hfFBImagePath.Value;
            else
                imgbtnFBImageMeta.Attributes.Add("src", hfFBImagePath.Value);

            if (imgbtnFBImageMeta.Attributes["imagepath"] != null)
                imgbtnFBImageMeta.Attributes["imagepath"] = hfFBImagePath.Value;
            else
                imgbtnFBImageMeta.Attributes.Add("imagepath", hfFBImagePath.Value);
            DoTwemojiOnGrid();
        }



    }
}