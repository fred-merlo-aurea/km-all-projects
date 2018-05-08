using System;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using CampaignItemMetaTag = ECN_Framework_Entities.Communicator.CampaignItemMetaTag;
using Image = System.Drawing.Image;
using SimpleShareDetail = ECN_Framework_Entities.Communicator.SimpleShareDetail;
using EntitiesCampaignItemSocialMedia = ECN_Framework_Entities.Communicator.CampaignItemSocialMedia;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using BusinessCampaignItemMetaTag = ECN_Framework_BusinessLayer.Communicator.CampaignItemMetaTag;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class SocialShare
    {
        private const string ChampionCampainName = "champion";
        private const string SubscriberModeName = "Subscriber";
        private const string Whitespace = " ";
        private const string TwitterHashtagSpaceError = "Twitter hashtags cannot contain spaces";
        private const string HashtagsProperty = "hashtags";
        private const string TitleProperty = "og:title";
        private const string DescriptionProperty = "og:description";
        private const string ImageProperty = "og:image";
        private const string TwitterHashtagLengthError = "Twitter hashtag length cannot be more than 118 characters";

        private const string LinkedInTitleLengthError =
            "LinkedIn Subscriber Share title cannot be more than 200 characters";

        private const string LinkedInSubTitleLengthError =
            "LinkedIn Subscriber Share sub title cannot be more than 200 characters";

        private const string DefaultImage = "/ecn.images/images/SelectImage.png";

        private const string FacebookTitleLengthError =
            "Facebook Subscriber Share title cannot be more than 100 characters";

        private const string FaceboolSubtitleLengthError =
            "Facebook Subscriber Share sub title cannot be more than 250 characters";

        private const string PageOrAccountNotSelectedError =
            "Please select a Facebook account and page for Facebook Like";

        private const string PageNotSelectedError = "Please select a Facebook page to use for Facebook Like";
        private const string SimpleModeName = "Simple";

        private const string LinkedInCommentLengthError =
            "LinkedIn comment cannot be more than 200 characters for profile {0}";

        private const string LinkedInItemTitleLengthError =
            "LinkedIn title cannot be more than 200 characters for profile {0}";

        private const string LinkedInItemSubtitleLengthError =
            "LinkedIn subtitle cannot be more than 200 characters for profile {0}";

        private const string LinkedInCompanyNotSelectedError = "Please select a LinkedIn company to post to as {0}";
        private const string TwitterLengthError = "Tweet for {0} cannot be longer than 118 characters";

        private const string FacebookCommentLengthError =
            "Facebook comment cannot be more than 200 characters for profile {0}";

        private const string FacebookItemTitleLengthError =
            "Facebook title cannot be more than 100 characters for profile {0}";

        private const string FacebookItemSubtitleLengthError =
            "Facebook subtitle cannot be more than 250 characters for profile {0}";

        private const string IncorrectImageFormatError =
            "Image selected for {0} doesn't meet size requirements. Image must be at least 200px by 200px.";

        private const string FacebookPageNotSelectedError = "Please select a Facebook page to post to as {0}";
        private const string Nbsp = "&nbsp;";
        private const int TwitterMaxLength = 118;
        private const int TwitterId = 2;
        private const int LinkedInMaxLength = 200;
        private const int LinkedInId = 3;
        private const int FbMetaMaxLength = 100;
        private const int FbDescMaxLength = 250;
        private const int FacebookId = 1;
        private const int FbFeedId = 5;
        private const int FbLikeId = 4;
        private const int FacebookMessageMaxLength = 200;
        private const int MinImageSize = 200;

        public bool Save()
        {
            return Save(true);
        }

        private bool Save(bool withAdvance)
        {
            if (!withAdvance)
            {
                DoTwemojiOnGrid();
            }

            return SaveSimpleShare(withAdvance) &&
                   SaveShareToSubscribers(withAdvance) &&
                   SaveCompainStatus(withAdvance);
        }

        private bool SaveCompainStatus(bool withAdvance)
        {
            if (!withAdvance)
            {
                return SaveCompainStatusWithoutAdvance();
            }

            SaveCampainStatusWithAdvance();
            return true;

        }

        private bool SaveCompainStatusWithoutAdvance()
        {
            var campaignItem = CampaignItem.GetByCampaignItemID(
                CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                true);
            if (!campaignItem.CampaignItemType.Equals(ChampionCampainName, StringComparison.InvariantCultureIgnoreCase))
            {
                campaignItem.CompletedStep = 3;
            }
            else if (campaignItem.CampaignItemType.Equals(ChampionCampainName, StringComparison.InvariantCultureIgnoreCase))
            {
                campaignItem.CompletedStep = 2;
            }

            campaignItem.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;

            try
            {
                CampaignItem.Save(campaignItem, ECNSession.CurrentSession().CurrentUser);
            }
            catch (ECNException ecn)
            {
                setECNException(ecn);
                return false;
            }

            return true;
        }

        private void SaveCampainStatusWithAdvance()
        {
            var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            if (campaignItem.CompletedStep < 4 && 
                !campaignItem.CampaignItemType.Equals(ChampionCampainName, StringComparison.InvariantCultureIgnoreCase))
            {
                campaignItem.CompletedStep = 4;
            }
            else if (campaignItem.CampaignItemType.Equals(ChampionCampainName, StringComparison.InvariantCultureIgnoreCase) &&
                     campaignItem.CompletedStep < 3)
            {
                campaignItem.CompletedStep = 3;
            }

            campaignItem.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            CampaignItem.Save(campaignItem, ECNSession.CurrentSession().CurrentUser);
        }

        private bool SaveShareToSubscribers(bool withAdvance)
        {
            if (chkSubShare.Checked)
            {
                var userId = ECNSession.CurrentSession().CurrentUser.UserID;
                CampaignItemSocialMedia.Delete(CampaignItemID, SubscriberModeName);
                SaveShareToFbFeedSubscribers();

                if (!SaveShareToFbLikeSubscribers(withAdvance))
                {
                    return false;
                }

                if (!ValidateAndSaveShareToFbSubscribers(withAdvance, userId))
                {
                    return false;
                }

                if (!ValidateAndSaveShareToLinkedInSubscribers(withAdvance, userId))
                {
                    return false;
                }

                if (!ValidateAndSaveShareToTwitterSubscribers(withAdvance, userId))
                {
                    return false;
                }
            }
            else
            {
                CampaignItemSocialMedia.Delete(CampaignItemID, SubscriberModeName);
                BusinessCampaignItemMetaTag.Delete_CampaignItemID(CampaignItemID,
                    ECNSession.CurrentSession().CurrentUser.UserID);
            }

            return true;
        }

        private bool ValidateAndSaveShareToTwitterSubscribers(bool withAdvance, int userId)
        {
            if (!chkTwitterSubShare.Checked)
            {
                BusinessCampaignItemMetaTag.Delete_SocialMediaID_CampaignItemID(TwitterId,
                    CampaignItemID,
                    userId);
                return true;
            }

            var twHashMetaTrimmed = txtTWHashMeta.Text.Trim();
            if (twHashMetaTrimmed.Length >= TwitterMaxLength)
            {
                return GenerateEcnError(withAdvance, TwitterHashtagLengthError);
            }

            var splitSpace = txtTWHashMeta.Text.Split(',');            
            return splitSpace.Select(s => s.Trim()).Any(clean => clean.Contains(Whitespace)) 
                ? GenerateEcnError(withAdvance, TwitterHashtagSpaceError) 
                : SaveShareToTwitterSubscribers(userId, twHashMetaTrimmed);
        }

        private bool SaveShareToTwitterSubscribers(int userId, string twHashMetaTrimmed)
        {
            var campaignItemSocialMedia = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SocialMediaID = TwitterId,
                SimpleShareDetailID = null,
                Status = CommunicatorEnums.SocialMediaStatusType.Created.ToString()
            };
            CampaignItemSocialMedia.Save(campaignItemSocialMedia);

            if (string.IsNullOrWhiteSpace(twHashMetaTrimmed))
            {
                return true;
            }

            var cimtTwHash = new CampaignItemMetaTag
            {
                SocialMediaID = TwitterId,
                CampaignItemID = CampaignItemID,
                CampaignItemMetaTagID = Convert.ToInt32(hfTWHashMeta.Value),
                Property = HashtagsProperty,
                Content = twHashMetaTrimmed,
                CreatedUserID = userId,
                UpdatedUserID = userId,
                IsDeleted = false
            };
            hfTWHashMeta.Value = BusinessCampaignItemMetaTag.Save(cimtTwHash).ToString();
            return true;
        }

        private bool ValidateAndSaveShareToLinkedInSubscribers(bool withAdvance, int userId)
        {
            if (!chkLinkedInSubShare.Checked)
            {
                BusinessCampaignItemMetaTag.Delete_SocialMediaID_CampaignItemID(LinkedInId, CampaignItemID, userId);
                return true;
            }

            if (txtLITitleMeta.Text.Length > LinkedInMaxLength && !GenerateEcnError(withAdvance, LinkedInTitleLengthError))
            {
                return false;
            }

            if (txtLIDescMeta.Text.Length > LinkedInMaxLength && !GenerateEcnError(withAdvance, LinkedInSubTitleLengthError))
            {
                return false;
            }

            SaveShareToLinkedInSubscribers(userId);

            return true;
        }

        private void SaveShareToLinkedInSubscribers(int userId)
        {
            var cism = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SocialMediaID = LinkedInId,
                SimpleShareDetailID = null,
                Status = CommunicatorEnums.SocialMediaStatusType.Created.ToString()
            };
            CampaignItemSocialMedia.Save(cism);

            //For meta tags
            var liTitleMetaTrimmed = txtLITitleMeta.Text.Trim();
            if (!string.IsNullOrWhiteSpace(liTitleMetaTrimmed))
            {
                var cimtLiTitle = new CampaignItemMetaTag
                {
                    SocialMediaID = LinkedInId,
                    CampaignItemID = CampaignItemID,
                    CampaignItemMetaTagID = Convert.ToInt32(hfLITitleMetaID.Value),
                    Property = TitleProperty,
                    Content = liTitleMetaTrimmed,
                    CreatedUserID = userId,
                    UpdatedUserID = userId,
                    IsDeleted = false
                };
                hfLITitleMetaID.Value = BusinessCampaignItemMetaTag.Save(cimtLiTitle).ToString();
            }

            var liDescMetaTrimmed = txtLIDescMeta.Text.Trim();
            if (!string.IsNullOrWhiteSpace(liDescMetaTrimmed))
            {
                var cimtLiDesc = new CampaignItemMetaTag
                {
                    SocialMediaID = LinkedInId,
                    CampaignItemID = CampaignItemID,
                    CampaignItemMetaTagID = Convert.ToInt32(hfLIDescMetaID.Value),
                    Property = DescriptionProperty,
                    Content = liDescMetaTrimmed,
                    CreatedUserID = userId,
                    UpdatedUserID = userId,
                    IsDeleted = false
                };
                hfLIDescMetaID.Value = BusinessCampaignItemMetaTag.Save(cimtLiDesc).ToString();
            }

            if (!imgbtnLIImageMeta.ImageUrl.Equals(DefaultImage))
            {
                var cimtLiImage = new CampaignItemMetaTag
                {
                    SocialMediaID = LinkedInId,
                    CampaignItemID = CampaignItemID,
                    CampaignItemMetaTagID = Convert.ToInt32(hfLIImageMetaID.Value),
                    Property = ImageProperty,
                    Content = imgbtnLIImageMeta.ImageUrl.Trim(),
                    CreatedUserID = userId,
                    UpdatedUserID = userId,
                    IsDeleted = false
                };
                hfLIImageMetaID.Value = BusinessCampaignItemMetaTag.Save(cimtLiImage).ToString();
            }
        }

        private bool ValidateAndSaveShareToFbSubscribers(bool withAdvance, int userId)
        {
            if (!chkFacebookSubShare.Checked)
            {
                BusinessCampaignItemMetaTag.Delete_SocialMediaID_CampaignItemID(FacebookId, CampaignItemID, userId);
                return true;
            }

            if (txtFBTitleMeta.Text.Length > FbMetaMaxLength && !GenerateEcnError(withAdvance, FacebookTitleLengthError))
            {
                return false;
            }

            if (txtFBDescMeta.Text.Length > FbDescMaxLength && !GenerateEcnError(withAdvance, FaceboolSubtitleLengthError))
            {
                return false;
            }

            return SaveShareToFbSubscribers(userId);
        }

        private bool SaveShareToFbSubscribers(int userId)
        {
            var cism = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SocialMediaID = FacebookId,
                SimpleShareDetailID = null,
                Status =
                    CommunicatorEnums.SocialMediaStatusType.Created.ToString()
            };
            CampaignItemSocialMedia.Save(cism);

            //For meta tags
            if (!string.IsNullOrWhiteSpace(txtFBTitleMeta.Text.Trim()))
            {
                var cimtFbTitle = new CampaignItemMetaTag
                {
                    SocialMediaID = FacebookId,
                    CampaignItemID = CampaignItemID,
                    CampaignItemMetaTagID = Convert.ToInt32(hfFBTitleMetaID.Value),
                    Property = TitleProperty,
                    Content = txtFBTitleMeta.Text.Trim(),
                    CreatedUserID = userId,
                    UpdatedUserID = userId,
                    IsDeleted = false
                };
                hfFBTitleMetaID.Value = BusinessCampaignItemMetaTag
                    .Save(cimtFbTitle)
                    .ToString();
            }

            if (!string.IsNullOrWhiteSpace(txtFBDescMeta.Text.Trim()))
            {
                var cimtFbDesc = new CampaignItemMetaTag
                {
                    SocialMediaID = FacebookId,
                    CampaignItemID = CampaignItemID,
                    CampaignItemMetaTagID = Convert.ToInt32(hfFBDescMetaID.Value),
                    Property = DescriptionProperty,
                    Content = txtFBDescMeta.Text.Trim(),
                    CreatedUserID = userId,
                    UpdatedUserID = userId,
                    IsDeleted = false
                };
                hfFBDescMetaID.Value = BusinessCampaignItemMetaTag
                    .Save(cimtFbDesc)
                    .ToString();
            }

            if (imgbtnFBImageMeta.ImageUrl.Trim().Equals(DefaultImage))
            {
                return true;
            }

            var cimtFbImage = new CampaignItemMetaTag
            {
                SocialMediaID = FacebookId,
                CampaignItemID = CampaignItemID,
                CampaignItemMetaTagID = Convert.ToInt32(hfFBImageMetaID.Value),
                Property = ImageProperty,
                Content = imgbtnFBImageMeta.ImageUrl.Trim(),
                CreatedUserID = userId,
                UpdatedUserID = userId,
                IsDeleted = false
            };
            hfFBImageMetaID.Value = BusinessCampaignItemMetaTag.Save(cimtFbImage).ToString();

            return true;
        }

        private bool SaveShareToFbLikeSubscribers(bool withAdvance)
        {
            if (!chkFacebookLikeSubShare.Checked)
            {
                return true;
            }

            if (ddlFacebookLikeSubShare.SelectedIndex <= 0 || ddlFacebookUserAccounts.SelectedIndex <= 0)
            {
                return GenerateEcnError(withAdvance, PageOrAccountNotSelectedError);
            }

            var cism = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SocialMediaID = FbLikeId,
                SocialMediaAuthID = Convert.ToInt32(ddlFacebookLikeSubShare.SelectedValue),
                SimpleShareDetailID = null,
                Status = CommunicatorEnums.SocialMediaStatusType.Created.ToString(),
                PageID = ddlFacebookUserAccounts.SelectedValue
            };

            if (string.IsNullOrWhiteSpace(cism.PageID))
            {
                return GenerateEcnError(withAdvance, PageNotSelectedError);
            }

            if (cism.SocialMediaAuthID != null)
            {
                var sma = SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.Value);
                var listAccounts = SocialMediaHelper.GetUserAccounts(sma.Access_Token);
                var selectedPage = listAccounts.Find(x => x.id == cism.PageID);
                if (selectedPage != null)
                {
                    cism.PageAccessToken = selectedPage.access_token;
                }
            }

            CampaignItemSocialMedia.Save(cism);
            return true;
        }

        private void SaveShareToFbFeedSubscribers()
        {
            if (!chkF2FSubShare.Checked)
            {
                return;
            }

            var cism = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SocialMediaID = FbFeedId,
                SimpleShareDetailID = null,
                Status = CommunicatorEnums.SocialMediaStatusType.Created.ToString()
            };
            CampaignItemSocialMedia.Save(cism);
        }

        private bool SaveSimpleShare(bool withAdvance)
        {
            if (!chkSimpleShare.Checked)
            {
                CampaignItemSocialMedia.Delete(CampaignItemID, SimpleModeName);
                return true;
            }

            CampaignItemSocialMedia.Delete(CampaignItemID, SimpleModeName);
            foreach (GridViewRow gvr in gvSimpleShare.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                {
                    continue;
                }

                if (!SaveSimpleShareRow(withAdvance, gvr))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SaveSimpleShareRow(bool withAdvance, GridViewRow gvr)
        {
            var chkSocial = gvr.FindControl("chkEnableSimpleShare") as CheckBox;
            if (!chkSocial?.Checked == true)
            {
                var smaid = Convert.ToInt32(gvSimpleShare.DataKeys[gvr.RowIndex]?.Value.ToString());
                ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail.DeleteFromCampaignItem(smaid,
                    CampaignItemID);
                return true;
            }

            var sc = (SocialConfig) gvr.FindControl("scConfig");

            if (withAdvance)
            {
                sc.Message = StripHTML(sc.Message);
                sc.Title = StripHTML(sc.Title);
                sc.Subtitle = StripHTML(sc.Subtitle);
            }

            var ssd = new SimpleShareDetail
            {
                CampaignItemID = CampaignItemID,
                CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                SocialMediaID = sc.SocialMediaID,
                IsDeleted = false,
                SocialMediaAuthID = Convert.ToInt32(gvSimpleShare.DataKeys[gvr.RowIndex]?.Value)
            };
            var sma = SocialMediaAuth.GetBySocialMediaAuthID(ssd.SocialMediaAuthID);
            if (!BuildSimpleShareDetail(withAdvance, ssd, sc, sma))
            {
                return false;
            }

            if (ssd.SimpleShareDetailID > 0)
            {
                ssd.UpdatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            }
            else
            {
                ssd.CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID;
            }

            SaveSimpleShareDetail(ssd);
            return true;
        }

        private bool BuildSimpleShareDetail(bool withAdvance, SimpleShareDetail ssd, SocialConfig sc, ECN_Framework_Entities.Communicator.SocialMediaAuth sma)
        {
            switch (ssd.SocialMediaID)
            {
                case FacebookId:
                    if (!BuildSimpleShareDetailForFbMesage(withAdvance, sc, sma, ssd))
                    {
                        return false;
                    }

                    break;
                case TwitterId:
                    if (!BuildSimpleShareDetailForTwitter(withAdvance, sc, sma, ssd))
                    {
                        return false;
                    }

                    break;
                case LinkedInId:
                    if (!BuildSimpleShareDetailForLinkedIn(withAdvance, sc, sma, ssd))
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }

        private void SaveSimpleShareDetail(SimpleShareDetail ssd)
        {
            var simpleShareDetailId =
                ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail.Save(ssd);
            var newCism = new EntitiesCampaignItemSocialMedia
            {
                CampaignItemID = CampaignItemID,
                SimpleShareDetailID = simpleShareDetailId,
                SocialMediaAuthID = ssd.SocialMediaAuthID,
                SocialMediaID = ssd.SocialMediaID,
                Status = CommunicatorEnums.SocialMediaStatusType.Created
                    .ToString(),
                PageAccessToken = ssd.PageAccessToken,
                PageID = ssd.PageID
            };
            CampaignItemSocialMedia.Save(newCism);
        }

        private bool BuildSimpleShareDetailForLinkedIn(
            bool withAdvance,
            SocialConfig sc,
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma,
            SimpleShareDetail ssd)
        {
            if (sc.Message.Length > LinkedInMaxLength)
            {
                if (!GenerateEcnError(
                    withAdvance,
                    string.Format(LinkedInCommentLengthError, sma.ProfileName)))
                {
                    return false;
                }
            }
            else if (sc.Title.Length > LinkedInMaxLength)
            {
                if (!GenerateEcnError(withAdvance, string.Format(LinkedInItemTitleLengthError, sma.ProfileName)))
                {
                    return false;
                }
            }
            else if (sc.Subtitle.Length > LinkedInMaxLength &&
                     !GenerateEcnError(withAdvance, string.Format(LinkedInItemSubtitleLengthError, sma.ProfileName)))
            {
                return false;
            }

            ssd.Content = sc.Message.Replace(Nbsp, Whitespace);
            ssd.Title = sc.Title.Replace(Nbsp, Whitespace);
            ssd.SubTitle = sc.Subtitle.Replace(Nbsp, Whitespace);
            ssd.ImagePath = sc.ImagePath;
            ssd.UseThumbnail = sc.UseThumbnail;
            ssd.PageID = sc.PageID;
            return !string.IsNullOrWhiteSpace(ssd.PageID) ||
                   GenerateEcnError(withAdvance, string.Format(LinkedInCompanyNotSelectedError, sma.ProfileName));
        }

        private bool BuildSimpleShareDetailForTwitter(
            bool withAdvance,
            SocialConfig sc,
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma,
            SimpleShareDetail ssd)
        {
            if (sc.Message.Length > TwitterMaxLength &&
                !GenerateEcnError(withAdvance, string.Format(TwitterLengthError, sma.ProfileName)))
            {
                return false;
            }

            ssd.Content = sc.Message.Replace(Nbsp, Whitespace);
            return true;
        }

        private bool BuildSimpleShareDetailForFbMesage(
            bool withAdvance,
            SocialConfig sc,
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma,
            SimpleShareDetail ssd)
        {
            if (sc.Message.Length > FacebookMessageMaxLength)
            {
                if (!GenerateEcnError(withAdvance, string.Format(FacebookCommentLengthError, sma.ProfileName)))
                {
                    return false;
                }
            }
            else if (sc.Title.Length > FbMetaMaxLength)
            {
                if (!GenerateEcnError(withAdvance, string.Format(FacebookItemTitleLengthError, sma.ProfileName)))
                {
                    return false;
                }
            }
            else if (sc.Subtitle.Length > FbDescMaxLength &&
                     !GenerateEcnError(withAdvance, string.Format(FacebookItemSubtitleLengthError, sma.ProfileName)))
            {
                return false;
            }

            ssd.Title = sc.Title.Replace(Nbsp, Whitespace);
            ssd.SubTitle = sc.Subtitle.Replace(Nbsp, Whitespace);
            ssd.Content = sc.Message.Replace(Nbsp, Whitespace);
            ssd.ImagePath = sc.ImagePath;
            ssd.UseThumbnail = sc.UseThumbnail;
            if (withAdvance && ssd.UseThumbnail.Value)
            {
                Image img = null;
                try
                {
                    var wReq = (HttpWebRequest) WebRequest.Create(sc.ImagePath);
                    var wRes = (HttpWebResponse) wReq.GetResponse();
                    var str = wRes.GetResponseStream();
                    img = Image.FromStream(str);
                }
                catch (Exception ex)
                {
                    // POSSIBLE BUG: no exception logging
                    Trace.Warn("Error", "Unable to retrieve the image: {0}", ex);
                }

                if (img != null && (img.Width < MinImageSize || img.Height < MinImageSize))
                {
                    throwECNException(string.Format(IncorrectImageFormatError, sma.ProfileName));
                }

                img?.Dispose();
            }

            ssd.PageID = sc.PageID;
            if (!string.IsNullOrWhiteSpace(ssd.PageID))
            {
                var listAccounts = SocialMediaHelper.GetUserAccounts(sma.Access_Token);
                var selectedPage = listAccounts.Find(x => x.id == sc.PageID);
                if (selectedPage != null)
                {
                    ssd.PageAccessToken = selectedPage.access_token;
                }
            }
            else
            {
                return GenerateEcnError(withAdvance, string.Format(FacebookPageNotSelectedError, sma.ProfileName));
            }

            return true;
        }

        private bool SaveWithoutAdvance()
        {
            return Save(false);
        }

        private bool GenerateEcnError(bool withAdvance, string errorMessage)
        {
            if (withAdvance)
            {
                throwECNException(errorMessage);
            }
            else
            {
                setECNError(errorMessage);
                return false;
            }

            return true;
        }
    }
}