using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.activityengines.engines.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using AccountFakeDataLayer = ECN_Framework_DataLayer.Accounts.Fakes;
using CommunicatorFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    public partial class SubscriptionManagementTest
    {
        private const int SMID = 10;
        private const string LitHeaderId = "litHeader";
        private const string LitFooterId = "litFooter";
        private const string LblTitleId = "lblTitle";
        private const string LblMSMessageId = "lblMSMessage";
        private const string GvMasterSuppressedId = "gvMasterSuppressed";
        private const string LblReasonMessageId = "lblReasonMessage";
        private const string TxtReasonId = "txtReason";
        private const string DdlReasonId = "ddlReason";
        private const string Email = "DummyEmail";

        [TestCase(true)]
        [TestCase(false)]
        public void Page_Load_Exception_Error(bool isViewStateException)
        {
            // Arrange
            InitCommonForPageLoad();
            var thrownException = isViewStateException
                ? new ViewStateException()
                : new Exception();
            ShimSubscriptionManagement.AllInstances.GetUser = (p) => throw thrownException;

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => _logNonCriticalErrorMethodCallCount.ShouldBe(
                    isViewStateException
                    ? 1
                    : 0),
                () => _logCriticalErrorMethodCallCount.ShouldBe(
                    isViewStateException
                    ? 0
                    : 1));
        }

        [Test]
        public void Page_Load_EmptyEmail_Error()
        {
            // Arrange
            InitCommonForPageLoad(email: string.Empty);

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            //Assert
            _phError.ShouldSatisfyAllConditions(
               () => _phError.Visible.ShouldBeTrue(),
               () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void Page_Load_InvalidSMID_Error()
        {
            // Arrange
            InitCommonForPageLoad(smid: -10);

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            //Assert
            _phError.ShouldSatisfyAllConditions(
               () => _phError.Visible.ShouldBeTrue(),
               () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void Page_Load_NoCurrentSubscription_Error()
        {
            // Arrange
            InitCommonForPageLoad(noCurrentSubscription: true);

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            //Assert
            _phError.ShouldSatisfyAllConditions(
               () => _phError.Visible.ShouldBeTrue(),
               () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [Test]
        public void Page_Load_InvalidSubscriptionId_Error()
        {
            // Arrange
            InitCommonForPageLoad(invalidSubscriptionId: true);

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            //Assert
            _phError.ShouldSatisfyAllConditions(
               () => _phError.Visible.ShouldBeTrue(),
               () => _logNonCriticalErrorMethodCallCount.ShouldBe(1));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Page_Load_Embedded_ControlsInitializedCorrectly(bool includeMSGroups, bool useReasonDropDown)
        {
            // Arrange
            InitTestPageLoadEmbededd(includeMSGroups: includeMSGroups, useReasonDropDown: includeMSGroups);
            var litHeader = Get<Literal>(LitHeaderId);
            var litFooter = Get<Literal>(LitFooterId);
            var lblTitleId = Get<Label>(LblTitleId);
            var gvMasterSuppressed = Get<GridView>(GvMasterSuppressedId);
            var txtReason = Get<TextBox>(TextReasonId);
            var ddlReason = Get<DropDownList>(DdlReasonId);
            var lblMSMessage = Get<Literal>(LblMSMessageId);

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            litHeader.ShouldSatisfyAllConditions(
              () => litHeader.Visible.ShouldBeFalse(),
              () => litFooter.Visible.ShouldBeFalse(),
              () => txtReason.Visible.ShouldBeFalse(),
              () => ddlReason.Visible.ShouldBeFalse(),
              () => gvMasterSuppressed.Visible.ShouldBe(includeMSGroups),
              () => lblMSMessage.Visible.ShouldBe(includeMSGroups),
              () => lblTitleId.Text.ShouldBe($"Manage Subscription for: {Email}"));
        }

        [TestCase("header", "footer")]
        [TestCase("", "")]
        public void Page_Load_NotEmbedded_ControlsInitializedCorrectly(string header, string footer)
        {
            // Arrange
            InitTestPageLoadNotEmbedded(header: header, footer: footer);
            var litHeader = Get<Literal>(LitHeaderId);
            var litFooter = Get<Literal>(LitFooterId);
            var lblTitleId = Get<Label>(LblTitleId);
            var gvMasterSuppressed = Get<GridView>(GvMasterSuppressedId);
            _panelContent.Visible = false;

            // Act
            _subscriptionManagementPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            litHeader.ShouldSatisfyAllConditions(
                () => litHeader.Visible.ShouldBe(!string.IsNullOrWhiteSpace(header)),
                () => litFooter.Visible.ShouldBe(!string.IsNullOrWhiteSpace(footer)),
                () => litHeader.Text.ShouldBe(header),
                () => litFooter.Text.ShouldBe(footer),
                () => _panelContent.Visible.ShouldBeTrue(),
                () => gvMasterSuppressed.Visible.ShouldBeFalse(),
                () => lblTitleId.Text.ShouldBe($"Manage Subscription for: {Email}"));
        }

        private void InitTestPageLoadNotEmbedded(string header, string footer)
        {
            InitCommonForPageLoad(header: header, footer: footer, shimLoadEmailGroupData: false);
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimSubscriptionManagementGroup.GetBySMIDInt32 = (id) => new List<SubscriptionManagementGroup>();
        }

        private void InitTestPageLoadEmbededd(bool includeMSGroups, bool useReasonDropDown)
        {
            InitCommonForPageLoad(shimLoadEmailGroupData: false, isEmbedded: true, includeMSGroups: includeMSGroups, useReasonDropDown: useReasonDropDown);
            var emailGroupsByEmailAddressEnumerator = GetEmailGroupsByEmailAddressForEmbedded().GetEnumerator();
            var emailGroupsByEmailIdEnumerator = GetEmailGroupsByEmailIdForEmbedded().GetEnumerator();
            CommunicatorFakeDataLayer.ShimEmailGroup.GetSqlCommand = (cmd) =>
            {
                if (cmd.CommandText == "e_EmailGroup_Select_EmailAddress_GroupID")
                {
                    emailGroupsByEmailAddressEnumerator.MoveNext();
                    return emailGroupsByEmailAddressEnumerator.Current;
                }
                else if (cmd.CommandText == "e_EmailGroup_Select")
                {
                    emailGroupsByEmailIdEnumerator.MoveNext();
                    return emailGroupsByEmailIdEnumerator.Current;
                }
                return null;
            };
            AccountFakeDataLayer.ShimSubscriptionManagementGroup.GetListSqlCommand = (cmd) => CreateSubscriptionManagementGroupList();
            CommunicatorFakeDataLayer.ShimGroup.GetSqlCommand = (cmd) => new Group();
            CommunicatorFakeDataLayer.ShimGroup.GetSqlCommand = (cmd) => new Group();
        }

        private List<SubscriptionManagementGroup> CreateSubscriptionManagementGroupList()
        {
            var subManagementGrpList = new List<SubscriptionManagementGroup>();
            for (var i = 0; i < 7; i++)
            {
                subManagementGrpList.Add(new SubscriptionManagementGroup());
            }
            return subManagementGrpList;
        }

        private IEnumerable<EmailGroup> GetEmailGroupsByEmailAddressForEmbedded()
        {
            yield return null;
            yield return null;
            yield return null;
            yield return new EmailGroup() { EmailID = 1 };
            yield return new EmailGroup() { SubscribeTypeCode = "M", EmailID = 1, CustomerID = 1 };
            yield return new EmailGroup() { SubscribeTypeCode = "S", EmailID = 1, CustomerID = 1 };
            yield return new EmailGroup() { SubscribeTypeCode = "U", EmailID = 1, CustomerID = 1 };
        }

        private IEnumerable<EmailGroup> GetEmailGroupsByEmailIdForEmbedded()
        {
            yield return null;
            yield return new EmailGroup() { EmailID = 1 };
            yield return new EmailGroup() { EmailID = 2 };
        }

        private void InitCommonForPageLoad(
            string email = Email,
            int smid = SMID,
            bool noCurrentSubscription = false,
            bool isEmbedded = false,
            bool invalidSubscriptionId = false,
            bool includeMSGroups = false,
            bool useReasonDropDown = false,
            bool shimLoadEmailGroupData = true,
            string header = "",
            string footer = "")
        {
            InitializeAllControls(_subscriptionManagementInstance);
            ShimSubscriptionManagement.AllInstances.GetUser = (p) => new User();
            ShimSubscriptionManagement.AllInstances.getEmailAddress = (p) => email;
            ShimSubscriptionManagement.AllInstances.getSMID = (p) => smid;
            ShimSubscriptionManagement.AllInstances.CreateNote = (p) => string.Empty;
            ShimSubscriptionManagement.AllInstances.IsEmbedded = (p) => isEmbedded;
            if (shimLoadEmailGroupData)
            {
                ShimSubscriptionManagement.AllInstances.LoadEmailGroupDataStringSubscriptionManagementUser = (p, emailAddress, sub, user) => { };
            }
            var appSettings = new NameValueCollection();
            appSettings["KMCommon_Application"] = "10";
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (e, method, appId, note, charId, custId) => _logNonCriticalErrorMethodCallCount++;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (e, method, appId, note, charId, custId) => _logCriticalErrorMethodCallCount++;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimSubscriptionManagement.GetBySubscriptionManagementIDInt32 = (id) =>
            {
                if (noCurrentSubscription)
                {
                    return null;
                }
                var subscription = new SubscriptionManagement();
                subscription.SubscriptionManagementID = SMID;
                if (invalidSubscriptionId)
                {
                    subscription.SubscriptionManagementID++;
                }
                subscription.IncludeMSGroups = includeMSGroups;
                subscription.ReasonVisible = true;
                subscription.UseReasonDropDown = useReasonDropDown;
                subscription.Header = header;
                subscription.Footer = footer;
                return subscription;
            };
            AccountFakeDataLayer.ShimSubscriptionManagementReason.GetListSqlCommand = (cmd) =>
             new List<SubscriptionManagementReason>()
             {
                new SubscriptionManagementReason()
             };
            _panelContent = Get<Panel>(PanelContentId);
        }
    }
}
