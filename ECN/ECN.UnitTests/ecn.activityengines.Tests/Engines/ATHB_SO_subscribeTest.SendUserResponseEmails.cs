using Shouldly;
using NUnit.Framework;
using ecn.activityengines.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class ATHB_SO_subscribeTest
    {
        private const string TestedMethodName_SendUserResponseEmails = "SendUserResponseEmails";

        [Test]
        public void ATHB_SO_subscribe_SendUserResponseEmails_WithSnippetMatch_WithSuccess()
        {
            //Arrange            
            InitializeParameters();
            var methodArguments = new object[] { _groups, _emails};
            ShimATHB_SO_subscribe.AllInstances.ReplaceCodeSnippetsGroupsEmailsString = (subscribe, groups, emails, msg) => SnippetUnsubscribe;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_SendUserResponseEmails, methodArguments));

            //Assert
            _savedEmailDirect.ShouldBeTrue();
        }

        [Test]
        public void ATHB_SO_subscribe_SendUserResponseEmails_WithoutSnippetMatch_WithSuccess()
        {
            //Arrange            
            InitializeParameters();
            var methodArguments = new object[] { _groups, _emails };
            ShimATHB_SO_subscribe.AllInstances.ReplaceCodeSnippetsGroupsEmailsString = (subscribe, groups, emails, msg) => DummyString;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_SendUserResponseEmails, methodArguments));

            //Assert
            _savedEmailDirect.ShouldBeTrue();
        }

        private void InitializeParameters()
        {
            ATHB_SO_subscribe.Response_FromEmail = DummyString;
            ATHB_SO_subscribe.Response_UserMsgSubject = DummyString;
            ATHB_SO_subscribe.Response_UserMsgBody = DummyString;
        }
    }
}
