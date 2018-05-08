using Shouldly;
using NUnit.Framework;
using ecn.communicator.classes;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public partial class EventOrganizerTest
    {
        private LayoutPlans _myPlan;
        private EmailActivityLog _logEvent;       
        private const string TestedMethodName_FireEvent = "FireEvent";        

        [Test]
        public void FireEvent_EventTypeUnsubscribe_WithSuccess()
        {
            //Arrange
            InitializeParameters();
            _eventType = EventTypeUnsubscribe;
            var methodArguments = new object[]
            {
                _myPlan,
                _logEvent
            };

            //Act and Assert
            Should.NotThrow(() => _testedClass.Invoke(TestedMethodName_FireEvent, methodArguments));
        }

        [Test]
        public void FireEvent_EventTypeSubscribe_WithSuccess()
        {
            //Arrange
            InitializeParameters();
            _eventType= EventTypeSubscribe;

            var methodArguments = new object[]
            {
                _myPlan,
                _logEvent
            };

            //Act and Assert
            Should.NotThrow(() => _testedClass.Invoke(TestedMethodName_FireEvent, methodArguments));
        }

        private void InitializeParameters()
        {
            _myPlan = new LayoutPlans(LayoutPlanID);
            _logEvent = new EmailActivityLog(LayoutPlanID);
        }
    }
}
