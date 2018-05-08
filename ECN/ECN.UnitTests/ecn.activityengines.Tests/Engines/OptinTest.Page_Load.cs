using System;
using Shouldly;
using NUnit.Framework;
using ECN_Framework_BusinessLayer.Communicator.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class OptinTest
    {
        private const string TestedMethodName_PageLoad = "Page_Load";

        [Test]
        public void OptinTest_Page_Load_WtihCacheValue_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            _shimCache.ItemGetString = (value) => _user;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            _loggedInNonCriticalError.ShouldBeFalse();
        }

        [Test]
        public void OptinTest_Page_Load_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            _smartFormHistory.Response_UserScreen = UrlValue + MsgSnippet;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            _loggedInNonCriticalError.ShouldBeFalse();
        }

        [Test]
        public void OptinTest_Page_Load_Error_InvalidEmailIsLogged()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            ShimEmail.IsValidEmailAddressString = (emailAddress) => false;

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            _loggedInNonCriticalError.ShouldBeTrue();
        }

        [Test]
        public void OptinTest_Page_Load_Error_InvalidGroupIdIsLogged()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };            
            ShimSmartFormsHistory.GetGroupIDInt32Int32 = (customerID, smartFormID) => DummyInt;
            
            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            _loggedInNonCriticalError.ShouldBeTrue();
        }

    }
}
