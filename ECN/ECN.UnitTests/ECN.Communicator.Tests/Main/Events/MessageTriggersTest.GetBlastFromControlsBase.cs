using System;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Shouldly;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.Tests.Main.Events
{
    public partial class MessageTriggersTest
    {
        private const int SampleSelectedLayoutReply = 17;
        private const string SampleEmailFromName = "John";
        private const string SampleEmail = "john@.email.com";

        [Test]
        public void GetBlastFromControlsBase_Called_PropertiesSet()
        {
            // Arrange
            var time = DateTime.Now;
            _messageTriggersPrivateObject.SetField("_emailSubject", new HiddenField { Value = "subject" });
            _messageTriggersPrivateObject.SetField("hfSelectedLayoutReply", new HiddenField { Value = SampleSelectedLayoutReply.ToString() });
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameTA", new TextBox());
            _messageTriggersPrivateObject.SetField("txtCampaingItemNameNO", new TextBox());
            _messageTriggersPrivateObject.SetField("_emailFromName", new TextBox { Text = SampleEmailFromName });
            _messageTriggersPrivateObject.SetField("_emailFrom", new TextBox { Text = SampleEmail });
            _messageTriggersPrivateObject.SetField("_replyTo", new TextBox());

            var myBlast = new EntitiesCommunicator.Blast();

            // Act 
            _messageTriggersPrivateObject.Invoke("GetBlastFromControls", new object[] { myBlast });

            // Assert
            myBlast.ShouldSatisfyAllConditions(
                () => myBlast.SendTime.Value.ShouldBeGreaterThanOrEqualTo(time),
                () => myBlast.EmailFromName.ShouldBe(SampleEmailFromName),
                () => myBlast.EmailFrom.ShouldBe(SampleEmail));
        }
    }
}
