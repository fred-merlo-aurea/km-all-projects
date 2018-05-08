using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.digitaledition.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Publisher.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.DigitalEdition;
using ECN_Framework_Entities.Publisher;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using WebUIFakes = System.Web.UI.Fakes;

namespace ECN.Digitaledition.Test
{
    public partial class MagazineTest
    {
        [Test]
        public void Page_Load_WithValidUserSessionAndPublication_SetsPageControlValues()
        {
            // Arrange
            SetPageLoadFakes();

            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);
            var lblMessage = Get<Label>(_privateObject, "lblMessage");
            var lblEditionID = Get<Label>(_privateObject,"lblEditionID");
            var lblIsSecured = Get<Label>(_privateObject, "lblIsSecured");
            var txtpageno = Get<TextBox>(_privateObject, "txtpageno");
            var phContact = Get<PlaceHolder>(_privateObject, "phContact");
            var phSubscribe = Get<PlaceHolder>(_privateObject, "phSubscribe");

            // Assert
            lblMessage.ShouldNotBeNull();
            lblMessage.Text.ShouldBeNullOrWhiteSpace();
            lblMessage.Visible.ShouldBeFalse();
            lblIsSecured.ShouldNotBeNull();
            lblEditionID.ShouldNotBeNull();
            lblEditionID.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes["style"].ShouldBe("display:none");
            lblEditionID.Attributes["style"].ShouldBe("display:none");
            txtpageno.ShouldNotBeNull();
            txtpageno.Attributes.Keys.ShouldBe(new []{"onKeyPress","style" });
            txtpageno.Text.ShouldBe("1");
            lblEditionID.Text.ShouldBe("1");
            lblIsSecured.Text.ShouldBe("1");
            Get<HyperLink>(_privateObject, "hlLogo").Target.ShouldBe("_blank");
            Get<HyperLink>(_privateObject, "hlLogo").NavigateUrl.ShouldBe("http://www.knowledgemarketing.com");
            Get<string>(_privateObject, "_Theme").ShouldBe("default");
            Get<HyperLink>(_privateObject, "hlpowered").NavigateUrl.ShouldContain("www.mplanet2009.com/home.shtml");
            phSubscribe.Visible.ShouldBeFalse();
            phSubscribe.Controls.Count.ShouldBe(1);
            phSubscribe.Controls[0].ShouldBeOfType(typeof(LiteralControl));
            phContact.Controls[0].ShouldBeOfType(typeof(LiteralControl));
            phContact.Controls.Count.ShouldBe(1);
            Get<PlaceHolder>(_privateObject, "phsbf2f").Visible.ShouldBeFalse();
            Get<PlaceHolder>(_privateObject, "phf2f").Visible.ShouldBeFalse();
            Get<HyperLink>(_privateObject, "lnkContactEmail").Text.ShouldBe(SampleEmail);
            Get<Panel>(_privateObject, "pnlContactPhone").Visible.ShouldBeTrue();
            Get<Panel>(_privateObject, "pnlEdition").Visible.ShouldBeFalse();
            Get<Panel>(_privateObject, "pnlLogin").Visible.ShouldBeTrue();
            Get<Label>(_privateObject, "lblContactPhone").Text.ShouldBe(SamplePhone);
            Get<Panel>(_privateObject, "pnlContactAddress").Visible.ShouldBeTrue();
            Get<HtmlAnchor>(_privateObject, "download").Visible.ShouldBeFalse();
            Get<Label>(_privateObject, "lblContactAddress1").Text.ShouldBe(SampleAddress);
        }

        [Test]
        public void Page_Load_WithLogoUrlAndSubscriptionLinkSet_SetsPageControlValues()
        {
            // Arrange
            SetPageLoadFakes();
            SetPublicationFakes();
            SetDigitalSeesionFakes();

            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);
            var lblMessage = Get<Label>(_privateObject, "lblMessage");
            var lblEditionID = Get<Label>(_privateObject, "lblEditionID");
            var lblIsSecured = Get<Label>(_privateObject, "lblIsSecured");
            var txtpageno = Get<TextBox>(_privateObject, "txtpageno");
            var phContact = Get<PlaceHolder>(_privateObject, "phContact");
            var phSubscribe = Get<PlaceHolder>(_privateObject, "phSubscribe");

            // Assert
            lblMessage.ShouldNotBeNull();
            lblMessage.Text.ShouldBeNullOrWhiteSpace();
            lblMessage.Visible.ShouldBeFalse();
            lblIsSecured.ShouldNotBeNull();
            lblEditionID.ShouldNotBeNull();
            lblEditionID.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes["style"].ShouldBe("display:none");
            lblEditionID.Attributes["style"].ShouldBe("display:none");
            txtpageno.ShouldNotBeNull();
            txtpageno.Attributes.Keys.ShouldBe(new[] { "onKeyPress", "style" });
            txtpageno.Text.ShouldBe("1");
            lblEditionID.Text.ShouldBe("1");
            lblIsSecured.Text.ShouldBe("0");
            Get<HyperLink>(_privateObject, "hlLogo").Target.ShouldBe("_blank");
            Get<HyperLink>(_privateObject, "hlLogo").NavigateUrl.ShouldBe(SampleLogoLink);
            Get<string>(_privateObject, "_Theme").ShouldBe("default");
            Get<HyperLink>(_privateObject, "hlpowered").NavigateUrl.ShouldContain("www.mplanet2009.com/home.shtml");
            phSubscribe.Visible.ShouldBeTrue();
            phSubscribe.Controls.Count.ShouldBe(1);
            phSubscribe.Controls[0].ShouldBeOfType(typeof(LiteralControl));
            phContact.Controls[0].ShouldBeOfType(typeof(LiteralControl));
            phContact.Controls.Count.ShouldBe(1);
            Get<PlaceHolder>(_privateObject, "phsbf2f").Visible.ShouldBeTrue();
            Get<PlaceHolder>(_privateObject, "phf2f").Visible.ShouldBeTrue();
            Get<HyperLink>(_privateObject, "lnkContactEmail").Text.ShouldBeNullOrWhiteSpace();
            Get<Panel>(_privateObject, "pnlContactPhone").Visible.ShouldBeTrue();
            Get<Panel>(_privateObject, "pnlEdition").Visible.ShouldBeTrue();
            Get<Panel>(_privateObject, "pnlLogin").Visible.ShouldBeFalse();
            Get<Label>(_privateObject, "lblContactPhone").Text.ShouldBeNullOrWhiteSpace();
            Get<Panel>(_privateObject, "pnlContactAddress").Visible.ShouldBeTrue();
            Get<HtmlAnchor>(_privateObject, "download").Visible.ShouldBeTrue();
            Get<Label>(_privateObject, "lblContactAddress1").Text.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void Page_Load_WithEmptySubscriptionAddressAndPhone_Test()
        {
            // Arrange
            SetPageLoadFakes();
            SetPublicationFakes(isEnableSubscription: false, defaultPhone: string.Empty, defaultAddress: string.Empty, defaultContactFormLink: string.Empty);
            SetDigitalSeesionFakes();

            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);
            var lblMessage = Get<Label>(_privateObject, "lblMessage");
            var lblEditionID = Get<Label>(_privateObject, "lblEditionID");
            var lblIsSecured = Get<Label>(_privateObject, "lblIsSecured");
            var txtpageno = Get<TextBox>(_privateObject, "txtpageno");
            var phContact = Get<PlaceHolder>(_privateObject, "phContact");
            var phSubscribe = Get<PlaceHolder>(_privateObject, "phSubscribe");

            // Assert
            lblMessage.ShouldNotBeNull();
            lblMessage.Text.ShouldBeNullOrWhiteSpace();
            lblMessage.Visible.ShouldBeFalse();
            lblIsSecured.ShouldNotBeNull();
            lblEditionID.ShouldNotBeNull();
            lblEditionID.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes.Keys.ShouldBe(new[] { "style" });
            lblIsSecured.Attributes["style"].ShouldBe("display:none");
            lblEditionID.Attributes["style"].ShouldBe("display:none");
            txtpageno.ShouldNotBeNull();
            txtpageno.Attributes.Keys.ShouldBe(new[] { "onKeyPress", "style" });
            txtpageno.Text.ShouldBe("1");
            lblEditionID.Text.ShouldBe("1");
            lblIsSecured.Text.ShouldBe("0");
            Get<HyperLink>(_privateObject, "hlLogo").Target.ShouldBe("_blank");
            Get<HyperLink>(_privateObject, "hlLogo").NavigateUrl.ShouldBe(SampleLogoLink);
            Get<string>(_privateObject, "_Theme").ShouldBe("default");
            Get<HyperLink>(_privateObject, "hlpowered").NavigateUrl.ShouldContain("www.mplanet2009.com/home.shtml");
            phSubscribe.Visible.ShouldBeTrue();
            phSubscribe.Controls.Count.ShouldBe(0);
            phContact.Controls.Count.ShouldBe(1);
            phContact.Controls[0].ShouldBeOfType(typeof(LiteralControl));
            Get<PlaceHolder>(_privateObject, "phsbf2f").Visible.ShouldBeTrue();
            Get<PlaceHolder>(_privateObject, "phf2f").Visible.ShouldBeTrue();
            Get<HyperLink>(_privateObject, "lnkContactEmail").Text.ShouldBe(SampleEmail);
            Get<Panel>(_privateObject, "pnlContactPhone").Visible.ShouldBeFalse();
            Get<Panel>(_privateObject, "pnlEdition").Visible.ShouldBeTrue();
            Get<Panel>(_privateObject, "pnlLogin").Visible.ShouldBeFalse();
            Get<Label>(_privateObject, "lblContactPhone").Text.ShouldBeNullOrWhiteSpace();
            Get<Panel>(_privateObject, "pnlContactAddress").Visible.ShouldBeFalse();
            Get<HtmlAnchor>(_privateObject, "download").Visible.ShouldBeTrue();
            Get<Label>(_privateObject, "lblContactAddress1").Text.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void Page_Load_WithIsPostBackTrue_SetsSessionValues()
        {
            // Arrange
            WebUIFakes.ShimPage.AllInstances.IsPostBackGet = (p) => true;
            var lblEditionID = (Label)_privateObject.GetFieldOrProperty("lblEditionID");
            lblEditionID.Text = "1";
            SetPageLoadFakes();
            SetPublicationFakes();
            SetDigitalSeesionFakes();

            // Act
            _privateObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            _testEntity.Session[SessionKeyIpAddress].ShouldNotBeNull();
            _testEntity.Session[SessionKeyIpAddress].ShouldBe(SampleIpAddress);
        }

        private void SetPageLoadFakes()
        {
            QueryString.Add("pno", "1");
            QueryString.Add("cd", TestPublicationCode);
            QueryString.Add("REMOTE_ADDR", SampleIpAddress);

            var configSettings = new NameValueCollection();
            configSettings.Add("ECNEngineAccessKey", Guid.Empty.ToString());
            configSettings.Add("KMPSLogoChannels", $"{SampleChannel},1");
            ShimConfigurationManager.AppSettingsGet = () => configSettings;

            ShimUser.GetByAccessKeyStringBoolean = (k, b) => new User();

            ShimMagazine.CreateUserSessionStringInt32User = (p, e, u) => new DigitalSession
            {
                EditionID = 1,
                Totalpages = 1,
                IsLoginRequired = true
            };

            ShimMagazine.AllInstances.getBlastID = (m) => 1;
            ShimMagazine.AllInstances.getEmailID = (m) => 0;
            ShimMagazine.AllInstances.getEmailAddress = (m) => SampleEmail;
            ShimMagazine.SubscribeToGroupGroupStringStringStringStringStringStringStringStringStringStringInt32 =
                (p, o, i, u, l, j, g, f, d, s, a, c) => 1;

            ShimPublication.GetByPublicationIDInt32User = (e, u) => new Publication
            {
                GroupID = 1,
                CustomerID = 2022,
                EnableSubscription = true,
                ContactEmail = SampleEmail,
                ContactPhone = SamplePhone,
                ContactAddress1 = SampleAddress
            };
            ShimEdition.GetByEditionIDInt32User = (e, u) => new Edition
            {
                CustomerID = 1,
                EditionID = 1,
                FileName = SampleFileName
            };
            ShimEdition.GetByPublicationIDInt32User = (i, u) => new List<Edition>()
            {
                new Edition{ EditionID = 2,EditionName= SampleEdition }
            };
            ShimGroup.GetByGroupIDInt32User = (g, u) => new Group();
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { BaseChannelID = 1 };
        }

        private void SetPublicationFakes(bool isEnableSubscription = true,
            string defaultPhone = SamplePhone,
            string defaultAddress = SampleAddress,
            string defaultContactFormLink = DefaultContactFormLink)
        {
            ShimPublication.GetByPublicationIDInt32User = (e, u) => new Publication
            {
                GroupID = 1,
                CustomerID = 2022,
                EnableSubscription = isEnableSubscription,
                ContactEmail = SampleEmail,
                ContactPhone = defaultPhone,
                ContactAddress1 = defaultAddress,
                LogoLink = SampleLogoLink,
                LogoURL = SampleLogoURL,
                SubscriptionFormLink = SampleSubscriptionLink,
                ContactFormLink = defaultContactFormLink
            };
        }

        private void SetDigitalSeesionFakes()
        {
            ShimMagazine.CreateUserSessionStringInt32User = (p, e, u) => new DigitalSession
            {
                EditionID = 1,
                Totalpages = 1,
                IsLoginRequired = false
            };
        }

    }
}
