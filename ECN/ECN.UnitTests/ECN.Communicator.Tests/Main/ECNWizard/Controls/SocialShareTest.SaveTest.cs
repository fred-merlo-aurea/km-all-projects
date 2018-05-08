using System;
using System.Collections.Generic;
using System.Drawing.Fakes;
using System.Net.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class SocialShareTest
    {
        [Test]
        public void Save_Facebook_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void Save_Facebook_SimpleShare_SmallImage()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            ShimWebRequest.CreateString = (path) => new ShimHttpWebRequest();
            ShimHttpWebRequest.AllInstances.GetResponse = (instance) => new ShimHttpWebResponse();
            ShimHttpWebResponse.AllInstances.GetResponseStream = (instance) => null;
            ShimImage.FromStreamStream = (stream) => new System.Drawing.Bitmap(150, 150);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("doesn't meet size requirements. Image must be at least 200px by 200px.");            
        }

        [Test]
        public void Save_Facebook_SimpleShare_Champion_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (campaignItemID, getChildren) =>
                 new CampaignItem { CampaignItemType = "champion", CompletedStep = 1};

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void Save_Facebook_SimpleShare_ECNException()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            ShimCampaignItem.SaveCampaignItemUser = (ci, user) => throw new ECNException(
                new List<ECNError> { new ECNError(Enums.Entity.BlastSocial, Enums.Method.Save, "Test Exception") });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Test Exception");
        }

        [Test]
        public void Save_Facebook_SimpleShare_Update_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            ECN_Framework_Entities.Communicator.Fakes.ShimSimpleShareDetail.AllInstances.SimpleShareDetailIDGet = (instance) => 1;

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void Save_Facebook_SimpleShare_DisableRow()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = false;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldBeNull();
        }

        [Test]
        public void Save_Facebook_SimpleShare_LongMessage()
        {
            // Arrange
            var longMessage = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longMessage += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", longMessage, "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Facebook comment cannot be more than 200 characters");
        }

        [Test]
        public void Save_Facebook_SimpleShare_LongTitle()
        {
            // Arrange
            var longTitle = "This message is longer than 100 charecter";
            for (int i = 0; i < 100; i++)
                longTitle += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", longTitle, "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Facebook title cannot be more than 100 characters");
        }

        [Test]
        public void Save_Facebook_SimpleShare_LongSubtitle()
        {
            // Arrange
            var longSubtitle = "This message is longer than 250 charecter";
            for (int i = 0; i < 250; i++)
                longSubtitle += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", longSubtitle));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Facebook subtitle cannot be more than 250 characters");
        }

        [Test]
        public void Save_Facebook_SimpleShare_NoPageId()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, String.Empty, "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Please select a Facebook page to post to");
        }

        [Test]
        public void Save_Twitter_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdTwitter, "twitterPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdTwitter);
        }

        [Test]
        public void Save_Twitter_SimpleShare_LongMessage()
        {
            // Arrange
            var longMessage = "This message is longer than 118 charecter";
            for (int i = 0; i < 118; i++)
                longMessage += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdTwitter, "twitterPageId", longMessage, "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("cannot be longer than 118 characters");
        }

        [Test]
        public void Save_Linkedin_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdLinkedin);
        }

        [Test]
        public void Save_Linkedin_SimpleShare_LongMessage()
        {
            // Arrange
            var longMessage = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longMessage += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", longMessage, "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Linkedin comment cannot be more than 200 characters");
        }

        [Test]
        public void Save_Linkedin_SimpleShare_LongTitle()
        {
            // Arrange
            var longTitle = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longTitle += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", longTitle, "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Linkedin title cannot be more than 200 characters");
        }

        [Test]
        public void Save_Linkedin_SimpleShare_LongSubtitle()
        {
            // Arrange
            var longSubtitle = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longSubtitle += "*";
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", longSubtitle));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Linkedin subtitle cannot be more than 200 characters");
        }

        [Test]
        public void Save_Linkedin_SimpleShare_NoPageId()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, String.Empty, "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Please select a LinkedIn company to post to as ");
        }

        [Test]
        public void Save_F2FSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkF2FSubShare", true);            

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdF2F);
        }

        [Test]
        public void Save_FacebookLikeSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookLikeSubShare", true);
            ShimSocialMediaHelper.GetUserAccountsString = (accessToken) => new List<SocialMediaHelper.FBAccount>
            {
                new SocialMediaHelper.FBAccount { id = "Account1" }
            };

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdFacebookLike);
        }

        [Test]
        public void Save_FacebookLikeSubShare_NoPage()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookLikeSubShare", true);

            var dropDownList = new DropDownList();
            dropDownList.Items.Add("Like Sub Share");
            dropDownList.Items.Add(String.Empty);
            dropDownList.SelectedIndex = 1;
            _testObject.SetField("ddlFacebookUserAccounts", dropDownList);
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Please select a Facebook page to use for Facebook Like");
        }

        [Test]
        public void Save_FacebookLikeSubShare_NoUser()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookLikeSubShare", true);
            _testObject.SetField("ddlFacebookUserAccounts", new DropDownList());
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Please select a Facebook account and page for Facebook Like");
        }

        [Test]
        public void Save_FacebookSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookSubShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void Save_FacebookSubShare_LongTitle()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookSubShare", true);
            var longTitle = "This message is longer than 100 charecter";
            for (int i = 0; i < 100; i++)
                longTitle += "*";
            _testObject.SetField("txtFBTitleMeta", new TextBox { Text = longTitle });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Facebook Subscriber Share title cannot be more than 100 characters");
        }

        [Test]
        public void Save_FacebookSubShare_LongDescription()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookSubShare", true);
            var longDescription = "This message is longer than 250 charecter";
            for (int i = 0; i < 250; i++)
                longDescription += "*";
            _testObject.SetField("txtFBDescMeta", new TextBox { Text = longDescription });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Facebook Subscriber Share sub title cannot be more than 250 characters");
        }

        [Test]
        public void Save_LinkedInSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkLinkedInSubShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdLinkedin);
        }

        [Test]
        public void Save_LinkedInSubShare_LongTitle()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkLinkedInSubShare", true);
            var longTitle = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longTitle += "*";
            _testObject.SetField("txtLITitleMeta", new TextBox { Text = longTitle });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("LinkedIn Subscriber Share title cannot be more than 200 characters");
        }

        [Test]
        public void Save_LinkedInSubShare_LongDescription()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkLinkedInSubShare", true);
            var longDescription = "This message is longer than 200 charecter";
            for (int i = 0; i < 200; i++)
                longDescription += "*";
            _testObject.SetField("txtLIDescMeta", new TextBox { Text = longDescription });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("LinkedIn Subscriber Share sub title cannot be more than 200 characters");
        }

        [Test]
        public void Save_TwitterSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkTwitterSubShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("Save");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdTwitter);
        }

        [Test]
        public void Save_TwitterSubShare_HashWithSpace()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkTwitterSubShare", true);
            var hashWithSpace = "This message contains spaces";
            _testObject.SetField("txtTWHashMeta", new TextBox { Text = hashWithSpace });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Twitter hashtags cannot contain spaces");
        }

        [Test]
        public void Save_TwitterSubShare_LongHash()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkTwitterSubShare", true);
            var longDescription = "This message is longer than 118 charecter";
            for (int i = 0; i < 118; i++)
                longDescription += "*";
            _testObject.SetField("txtTWHashMeta", new TextBox { Text = longDescription });
            var exceptionOccured = false;
            var exceptionMessage = String.Empty;

            // Act
            try
            {
                var result = (Boolean)_testObject.Invoke("Save");
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exceptionMessage = (e.InnerException as ECNException).ErrorList[0].ErrorMessage;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exceptionMessage.ShouldContain("Twitter hashtag length cannot be more than 118 characters");
        }
    }
}
