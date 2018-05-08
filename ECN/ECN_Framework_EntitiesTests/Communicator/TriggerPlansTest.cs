using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class TriggerPlansTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int triggerPlanID = -1;
            string eventType = string.Empty;
            string criteria = string.Empty;
            string actionName = string.Empty;
            string status = string.Empty;        

            // Act
            TriggerPlans triggerPlans = new TriggerPlans();    

            // Assert
            triggerPlans.TriggerPlanID.ShouldBe(triggerPlanID);
            triggerPlans.refTriggerID.ShouldBeNull();
            triggerPlans.BlastID.ShouldBeNull();
            triggerPlans.CustomerID.ShouldBeNull();
            triggerPlans.GroupID.ShouldBeNull();
            triggerPlans.EventType.ShouldBe(eventType);
            triggerPlans.Period.ShouldBeNull();
            triggerPlans.Criteria.ShouldBe(criteria);
            triggerPlans.ActionName.ShouldBe(actionName);
            triggerPlans.Status.ShouldBe(status);
            triggerPlans.CreatedUserID.ShouldBeNull();
            triggerPlans.CreatedDate.ShouldBeNull();
            triggerPlans.UpdatedUserID.ShouldBeNull();
            triggerPlans.UpdatedDate.ShouldBeNull();
            triggerPlans.IsDeleted.ShouldBeNull();
        }
    }
}