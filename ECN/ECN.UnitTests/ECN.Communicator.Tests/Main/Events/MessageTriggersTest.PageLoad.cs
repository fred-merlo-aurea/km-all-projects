using System.Data;
using System.Reflection;
using System.Web.SessionState;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;

using ecn.communicator.main.ECNWizard.Content;
using ecn.communicator.main.ECNWizard.Content.Fakes;
using ecn.communicator.main.events.Fakes;
using ECN.Common.Fakes;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Events
{
    public partial class MessageTriggersTest
    {
        private const string SelectedValueNo = "N";
        private const int CustomerId = 777;
        private const int UserId = 555;

        [Test]
        public void PageLoad_Called_PropertiesSet()
        {
            // Arrange
            SetupSessionFakes();
            ShimAuthenticationTicket.getTicket = () => typeof(AuthenticationTicket).CreateInstance();
            ShimECNSession.AllInstances.RefreshSession = (x) => { };
            ShimECNSession.AllInstances.ClearSession = (x) => { };

            SetDefaultMembers();

            ShimMessageTriggers.AllInstances.MasterGet = triggers =>
            {
                var master = new ecn.communicator.MasterPages.Communicator();
                return master;
            };

            ShimUserControl.AllInstances.SessionGet = control =>
            {
                var session = new ShimHttpSessionState();
                return (HttpSessionState)session;
            };

            ShimMasterPageEx.AllInstances.HelpTitleSetString = (_, __) => { };
            ShimMasterPageEx.AllInstances.HeadingSetString = (_, __) => { };
            ShimMasterPageEx.AllInstances.HelpContentSetString = (_, __) => { };

            var noopRadioLst = new RadioButtonList();
            noopRadioLst.Items.Add(SelectedValueNo);
            noopRadioLst.SelectedIndex = 0;
            _messageTriggersPrivateObject.SetField("NOOP_RadioList", BindingFlags.Instance | BindingFlags.NonPublic, noopRadioLst);

            ShimlayoutExplorer.AllInstances.enableEditMode = explorer => { };
            ShimlayoutExplorer.AllInstances.enableSelectMode = explorer => { };
            _messageTriggersPrivateObject.SetField("layoutExplorer", new layoutExplorer());
            _messageTriggersPrivateObject.SetField("val_NOOP_EmailFrom", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("imgbtnNOOP", new ImageButton());
            _messageTriggersPrivateObject.SetField("val_NOOP_ReplyTo", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("val_NOOP_EmailFromName", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("val_NOOP_Period", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("val_NOOP_txtHours", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("val_NOOP_txtMinutes", new RequiredFieldValidator());
            _messageTriggersPrivateObject.SetField("rangeval_NOOP_Period", new RangeValidator());
            _messageTriggersPrivateObject.SetField("rangeval_NOOP_txtHours", new RangeValidator());
            _messageTriggersPrivateObject.SetField("rangeval_NOOP_txtMinutes", new RangeValidator());
            _messageTriggersPrivateObject.SetField("rvfCampaingItemNameNO", new RequiredFieldValidator());

            ShimMessageTriggers.AllInstances.BindList = triggers => { };

            ShimLinkAlias.GetLinkAliasDRInt32Int32User = (i, i1, arg3) =>
            {
                var table = new DataTable();
                table.Columns.Add("Alias");
                table.Columns.Add("Link");
                return table;
            };

            // Act 
            _messageTriggersPrivateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            var nopRadioList = _messageTriggersPrivateObject.GetField("NOOP_RadioList") as RadioButtonList;
            var userId = _messageTriggersPrivateObject.GetField("UserId");
            var customerId = _messageTriggersPrivateObject.GetField("CustomerId");
            nopRadioList.ShouldSatisfyAllConditions(
                () => nopRadioList.ShouldNotBeNull(),
                () => nopRadioList.SelectedValue.ShouldBe(SelectedValueNo),
                () => userId.ShouldBe(UserId),
                () => customerId.ShouldBe(CustomerId)
            );
        }

        private static void SetupSessionFakes()
        {
            ShimECNSession.CurrentSession = CreateEcnSession;
        }

        private static ECNSession CreateEcnSession()
        {
            ECNSession session = typeof(ECNSession).CreateInstance();

            Customer customer = typeof(Customer).CreateInstance();
            customer.CustomerID = CustomerId;
            session.SetField(nameof(ECNSession.CurrentCustomer), customer);

            User user = typeof(User).CreateInstance();
            user.UserID = UserId;
            user.CustomerID = CustomerId;
            session.SetField(nameof(ECNSession.CurrentUser), user);

            return session;
        }
    }
}
