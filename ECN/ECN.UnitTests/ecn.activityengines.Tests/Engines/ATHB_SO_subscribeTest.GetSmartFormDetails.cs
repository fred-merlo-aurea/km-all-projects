using Shouldly;
using NUnit.Framework;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class ATHB_SO_subscribeTest
    {
        private const string TestedMethodName_GetSmartFormDetails = "getSmartFormDetails";

        [Test]
        public void ATHB_SO_subscribe_GetSmartFormDetails_WithSuccess()
        {
            //Arrange            
            var methodArguments = new object[] { int.Parse(SmartFormIdValue) };

            //Act
            Should.NotThrow(()=>_privateTestedObject.Invoke(TestedMethodName_GetSmartFormDetails, methodArguments));

            //Assert
            ATHB_SO_subscribe.Response_FromEmail.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_UserMsgSubject.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_UserMsgBody.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_UserScreen.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_AdminEmail.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_AdminMsgSubject.ShouldNotBeNullOrEmpty();
            ATHB_SO_subscribe.Response_AdminMsgBody.ShouldNotBeNullOrEmpty();
        }
    }
}
