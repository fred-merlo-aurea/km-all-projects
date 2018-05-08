using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class SocialShareTest
    {
        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_Champion_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (campaignItemID, user, getChildren) =>
                 new CampaignItem { CampaignItemType = "champion"};

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_ECNException()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Test Exception");
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_Update_Success()
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
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_DisableRow()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = false;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldBeNull();
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_LongMessage()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Facebook comment cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_LongTitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Facebook title cannot be more than 100 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_LongSubtitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Facebook subtitle cannot be more than 250 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Facebook_SimpleShare_NoPageId()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdFacebook, String.Empty, "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Please select a Facebook page to post to");
        }

        [Test]
        public void SaveWithoutAdvance_Twitter_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdTwitter, "twitterPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdTwitter);
        }

        [Test]
        public void SaveWithoutAdvance_Twitter_SimpleShare_LongMessage()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("cannot be longer than 118 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Linkedin_SimpleShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _simpleShareDetail.ShouldNotBeNull();
            _simpleShareDetail.SocialMediaID.ShouldBe(SocialMediaIdLinkedin);
        }

        [Test]
        public void SaveWithoutAdvance_Linkedin_SimpleShare_LongMessage()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Linkedin comment cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Linkedin_SimpleShare_LongTitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Linkedin title cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Linkedin_SimpleShare_LongSubtitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Linkedin subtitle cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_Linkedin_SimpleShare_NoPageId()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, String.Empty, "message", "title", "subtitle"));
            SetCheckBox("chkSimpleShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Please select a LinkedIn company to post to as ");
        }

        [Test]
        public void SaveWithoutAdvance_F2FSubShare_Success()
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
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdF2F);
        }

        [Test]
        public void SaveWithoutAdvance_FacebookLikeSubShare_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            _enableRowSimpleShare = true;
            InitilizeTestObject(ConfigureSocialConfig(
                SocialMediaIdLinkedin, "linkedinPageId", "message", "title", "subtitle"));
            SetCheckBox("chkSubShare", true);
            SetCheckBox("chkFacebookLikeSubShare", true);

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdFacebookLike);
        }

        [Test]
        public void SaveWithoutAdvance_FacebookLikeSubShare_NoPage()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Please select a Facebook page to use for Facebook Like");
        }

        [Test]
        public void SaveWithoutAdvance_FacebookLikeSubShare_NoUser()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Please select a Facebook account and page for Facebook Like");
        }

        [Test]
        public void SaveWithoutAdvance_FacebookSubShare_Success()
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
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdFacebook);
        }

        [Test]
        public void SaveWithoutAdvance_FacebookSubShare_LongTitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Facebook Subscriber Share title cannot be more than 100 characters");
        }

        [Test]
        public void SaveWithoutAdvance_FacebookSubShare_LongDescription()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Facebook Subscriber Share sub title cannot be more than 250 characters");
        }

        [Test]
        public void SaveWithoutAdvance_LinkedInSubShare_Success()
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
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdLinkedin);
        }

        [Test]
        public void SaveWithoutAdvance_LinkedInSubShare_LongTitle()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("LinkedIn Subscriber Share title cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_LinkedInSubShare_LongDescription()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("LinkedIn Subscriber Share sub title cannot be more than 200 characters");
        }

        [Test]
        public void SaveWithoutAdvance_TwitterSubShare_Success()
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
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeTrue();
            _campaignItemSocialMedia.ShouldNotBeNull();
            _campaignItemSocialMedia.SocialMediaID.ShouldBe(SocialMediaIdTwitter);
        }

        [Test]
        public void SaveWithoutAdvance_TwitterSubShare_HashWithSpace()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Twitter hashtags cannot contain spaces");
        }

        [Test]
        public void SaveWithoutAdvance_TwitterSubShare_LongHash()
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

            // Act
            var result = (Boolean)_testObject.Invoke("SaveWithoutAdvance");

            // Assert
            result.ShouldBeFalse();
            _simpleShareDetail.ShouldBeNull();
            var labelErrorMessage = (Label)_testObject.GetField("lblErrorMessage");
            labelErrorMessage.ShouldNotBeNull();
            labelErrorMessage.Text.ShouldContain("Twitter hashtag length cannot be more than 118 characters");
        }
    }
}
