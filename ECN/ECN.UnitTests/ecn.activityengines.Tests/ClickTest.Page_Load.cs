using System;
using System.Collections.Specialized;
using System.Web.Fakes;
using Shouldly;
using NUnit.Framework;
using KM.Common.Entity.Fakes;
using ecn.activityengines.Fakes;

namespace ecn.activityengines.Tests
{
    [TestFixture]
    public partial class ClickTest
    {
        private const string TestedMethodName_PageLoad = "Page_Load";

        [Test]
        public void Click_Page_Load_ValidParams_WithSuccess()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            
            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            ResponseWriteText.ShouldSatisfyAllConditions(()=> ResponseWriteText.ShouldNotBeEmpty(),
                                                         ()=> ResponseWriteText.ShouldContain(UdfNewValue));            
        }

        [Test]
        public void Click_Page_Load_Error_WithErroLogged()
        {
            //Arrange            
            var sender = new object();
            var eventArgs = new EventArgs();
            var methodArguments = new object[] { sender, eventArgs };
            var errorLogged = false;

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, sourceMethod, applicationId, note, charityId, customerId) =>
            {
                errorLogged = true;
                return DummyInt;
            };

            ShimClick.AllInstances.NotifyOfMissingTopicUDF = (click) => throw new Exception();

            ShimHttpRequest.AllInstances.ServerVariablesGet = (req) => 
            {
                return new NameValueCollection()
                {
                    {HttpPostKey, DummyString}
                };
            };

            //Act
            Should.NotThrow(() => _privateTestedObject.Invoke(TestedMethodName_PageLoad, methodArguments));

            //Assert 
            errorLogged.ShouldBeTrue();
        }
    }
}
