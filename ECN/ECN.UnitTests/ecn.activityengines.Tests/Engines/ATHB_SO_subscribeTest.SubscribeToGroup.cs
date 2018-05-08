using System;
using Shouldly;
using NUnit.Framework;
using ecn.activityengines.Fakes;
using ecn.communicator.classes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    public partial class ATHB_SO_subscribeTest
    {
        private const string TestedMethodName_SubscribeToGroup = "SubscribeToGroup";

        [Test]        
        public void ATHB_SO_subscribe_SubscribeToGroup_WithSuccess()
        {
            //Arrange            
            Emails result = null;            

            //Act
            Should.NotThrow(() => result = (Emails)_privateTestedObject.Invoke(TestedMethodName_SubscribeToGroup));

            result.ShouldNotBeNull();
        }

        [Test]
        public void ATHB_SO_subscribe_SubscribeToGroup_WithExceptionCaught()
        {
            //Arrange            
            Emails result = null;
            _groups = null;
            ShimATHB_SO_subscribe.AllInstances.getQSString = (subscribeValue, qs) => throw new Exception();
            ShimATHB_SO_subscribe.AllInstances.getEmailAddress = (subscribeValue) => throw new Exception();
            ShimATHB_SO_subscribe.AllInstances.UpdateToDBStringString = (subscribeValue, xml, xmlUdf) => throw new Exception();

            //Act
            Should.NotThrow(() => result = (Emails)_privateTestedObject.Invoke(TestedMethodName_SubscribeToGroup));

            result.ShouldBeNull();
        }
    }
}
