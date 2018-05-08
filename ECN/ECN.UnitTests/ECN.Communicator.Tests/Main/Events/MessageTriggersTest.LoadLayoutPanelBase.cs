using System.Web.UI.WebControls;
using ecn.communicator.main.events.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Events
{
    public partial class MessageTriggersTest
    {
        private const int SampleLayoutPlanId = 77;
        private const string SampleEmailSubject = "Sample Subject";

        [Test]
        public void LoadLayoutPlan_Called_PropertiesSet()
        {
            // Arrange
            ShimAuthenticationTicket.getTicket = () => typeof(AuthenticationTicket).CreateInstance();
            ShimECNSession.AllInstances.RefreshSession = (x) => { };
            ShimECNSession.AllInstances.ClearSession = (x) => { };

            SetControls();

            SetupShims();

            // Act 
            _messageTriggersPrivateObject.Invoke("LoadLayoutPlan", SampleLayoutPlanId);

            // Assert
            var noopEmailFromName = _messageTriggersInstance.GetFieldValue("NOOP_EmailFromName") as TextBox;
            var noopEmailSubject = _messageTriggersInstance.GetFieldValue("NOOP_EmailSubject") as HiddenField;

            noopEmailSubject.ShouldSatisfyAllConditions(
                () => noopEmailFromName.ShouldNotBeNull(),
                () => noopEmailFromName.Text.ShouldBe(SampleEmailFromName),
                () => noopEmailSubject.ShouldNotBeNull(),
                () => noopEmailSubject.Value.ShouldBe(SampleEmailSubject));
        }

        private static void SetupShims()
        {
            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (i, user) =>
            {
                var plans = new EntitiesCommunicator.LayoutPlans();
                plans.BlastID = 1;
                plans.LayoutID = 2;
                return plans;
            };

            ShimCommunicator.AllInstances.UserSessionGet = communicator =>
            {
                var session = CreateEcnSession();
                return session;
            };

            ShimMessageTriggers.AllInstances.MasterGet = triggers =>
            {
                var master = new ecn.communicator.MasterPages.Communicator();
                return master;
            };

            ShimMessageTriggers.AllInstances.SetNoopControlStatusBoolean = (_, __) => { };

            ShimBlast.GetByBlastIDInt32UserBoolean = (i, user, arg3) =>
            {
                var blast = new EntitiesCommunicator.BlastLayout();
                blast.LayoutID = 1;
                blast.EmailFromName = SampleEmailFromName;
                blast.EmailFrom = SampleEmail;
                blast.EmailSubject = SampleEmailSubject;
                return blast;
            };

            ShimTriggerPlans.GetByRefTriggerIDInt32User = (_, __) =>
            {
                var plan = new EntitiesCommunicator.TriggerPlans();
                plan.BlastID = 1;
                return plan;
            };

            ShimLayout.GetByLayoutIDInt32UserBoolean = (_, __, ___) =>
            {
                var layout = new EntitiesCommunicator.Layout();
                return layout;
            };

            ShimCampaignItem.GetByBlastIDInt32UserBoolean = (i, user, arg3) =>
            {
                var campaignItem = new EntitiesCommunicator.CampaignItem();
                return campaignItem;
            };
        }

        private void SetControls()
        {
            _messageTriggersPrivateObject.SetField("_emailSubject", new HiddenField {Value = "subject"});
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutReply",
                new HiddenField {Value = SampleSelectedLayoutReply.ToString()});
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameTA", new TextBox());
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameNO", new TextBox());
            _messageTriggersPrivateObject.SetField("_emailFromName", new TextBox {Text = SampleEmailFromName});
            _messageTriggersPrivateObject.SetField("_emailFrom", new TextBox {Text = SampleEmail});
            _messageTriggersPrivateObject.SetField("_replyTo", new TextBox());

            _messageTriggersPrivateObject.SetField("NOOP_EmailFrom", new TextBox { });
            _messageTriggersPrivateObject.SetField("NOOP_ReplyTo", new TextBox { });
            _messageTriggersPrivateObject.SetField("NOOP_EmailFromName", new TextBox { });
            _messageTriggersPrivateObject.SetField("NOOP_EmailSubject", new HiddenField { });

            _messageTriggersPrivateObject.SetField("NOOP_Period", new TextBox { });
            _messageTriggersPrivateObject.SetField("NOOP_txtHours", new TextBox { });
            _messageTriggersPrivateObject.SetField("NOOP_txtMinutes", new TextBox { });

            _messageTriggersPrivateObject.SetField("_layoutName", new TextBox());
            _messageTriggersPrivateObject.SetField("_period", new TextBox());
            _messageTriggersPrivateObject.SetField("_txtHours", new TextBox());
            _messageTriggersPrivateObject.SetField("_txtMinutes", new TextBox());
            _messageTriggersPrivateObject.SetField("NOOP_RadioList", new RadioButtonList());

            _messageTriggersPrivateObject.SetField("hfSelectedLayoutTrigger", new HiddenField { });
            _messageTriggersPrivateObject.SetField("lblSelectedLayoutTrigger", new Label { });

            _messageTriggersPrivateObject.SetField("EventType", new DropDownList { });
            _messageTriggersPrivateObject.SetField("_criteria", new DropDownList { });

            _messageTriggersPrivateObject.SetField("lblSelectedLayoutReply", new Label { });
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameTA", new TextBox());

            _messageTriggersPrivateObject.SetField("hfSelectedLayoutNOOPReply", new HiddenField());
            _messageTriggersPrivateObject.SetField("lblSelectedLayoutNOOPReply", new Label());
        }
    }
}
