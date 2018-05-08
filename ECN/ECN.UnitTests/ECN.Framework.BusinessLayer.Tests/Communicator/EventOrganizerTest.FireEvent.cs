using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using Entities = ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public partial class EventOrganizerTest
    {
        private const string FireEventMethodName = "FireEvent";
        private const int DefaultId = 1;
        private List<Entities.BlastSingle> _lstSavedBlastSingleForFireEvent;
        private const string StatusYes = "Y";
        private string _savedErrorMessage;

        [Test]
        [TestCase("Subscribe", 1, 0, 1, 1)]
        [TestCase("Subscribe", 1, 0, 0, 1)]
        public void FireEvent_MyPlanGroupIDWithEmailGroupEmailID_SavesBlastSingle(string planEventType, int myPlanGroupID, int myPlanEmailId, int emailGroupEmailId, int logEventEmailId)
        {
            // Arrange
            Entities.LayoutPlans myPlan;
            Entities.EmailActivityLog logEvent;
            User user;
            Entities.BlastSingle expectedBlastSingleForFireEvent;
            SetupFireEventTest(planEventType, myPlanGroupID, emailGroupEmailId, logEventEmailId, false, out myPlan, out logEvent, out user, out expectedBlastSingleForFireEvent);

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEventMethodName, myPlan, logEvent, user);

            // Assert
            _lstSavedBlastSingleForFireEvent.ShouldSatisfyAllConditions(
                    () => _lstSavedBlastSingleForFireEvent.First().SendTime.ShouldBe(expectedBlastSingleForFireEvent.SendTime),
                    () => _lstSavedBlastSingleForFireEvent.First().BlastID.ShouldBe(expectedBlastSingleForFireEvent.BlastID),
                    () => _lstSavedBlastSingleForFireEvent.First().EmailID.ShouldBe(expectedBlastSingleForFireEvent.EmailID),
                    () => _lstSavedBlastSingleForFireEvent.First().LayoutPlanID.ShouldBe(expectedBlastSingleForFireEvent.LayoutPlanID),
                    () => _lstSavedBlastSingleForFireEvent.First().RefBlastID.ShouldBe(expectedBlastSingleForFireEvent.RefBlastID)
                );
        }

        [Test]
        [TestCase("Subscribe", 1, 0, 1, 1)]
        [TestCase("Subscribe", 1, 0, 0, 1)]
        public void FireEvent_MyPlanGroupIDWithEmailGroupEmailID_LogsException(string planEventType, int myPlanGroupID, int myPlanEmailId, int emailGroupEmailId, int logEventEmailId)
        {
            // Arrange
            Entities.LayoutPlans myPlan;
            Entities.EmailActivityLog logEvent;
            User user;
            Entities.BlastSingle expectedBlastSingleForFireEvent;
            SetupFireEventTest(planEventType, myPlanGroupID, emailGroupEmailId, logEventEmailId, true, out myPlan, out logEvent, out user, out expectedBlastSingleForFireEvent);
            var expectedErrorMessage = "BlastSingle.Insert failed validation";

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEventMethodName, myPlan, logEvent, user);

            // Assert
            _savedErrorMessage.ShouldBe(expectedErrorMessage);
        }

        [Test]
        [TestCase("Subscribe", 0, 0, 0, 0)]
        [TestCase("Subscribe", 0, 0, 0, 1)]
        public void FireEvent_MyPlanGroupIDWithLogEventEmailID_SavesBlastSingle(string planEventType, int myPlanGroupID, int myPlanEmailId, int emailGroupEmailId, int logEventEmailId)
        {
            // Arrange
            Entities.LayoutPlans myPlan;
            Entities.EmailActivityLog logEvent;
            User user;
            Entities.BlastSingle expectedBlastSingleForFireEvent;
            SetupFireEventTest(planEventType, myPlanGroupID, emailGroupEmailId, logEventEmailId, false, out myPlan, out logEvent, out user, out expectedBlastSingleForFireEvent);

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEventMethodName, myPlan, logEvent, user);

            // Assert
            if (logEventEmailId == 0)
            {
                _lstSavedBlastSingleForFireEvent.ShouldSatisfyAllConditions(
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.EmailID == logEventEmailId).ShouldBe(1),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.EmailID == DefaultId).ShouldBe(1)
                  );
            }
            else
            {
                _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.EmailID == DefaultId).ShouldBe(2);
            }
            _lstSavedBlastSingleForFireEvent.ShouldSatisfyAllConditions(
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.SendTime == expectedBlastSingleForFireEvent.SendTime).ShouldBe(2),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.BlastID == expectedBlastSingleForFireEvent.BlastID).ShouldBe(2),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.LayoutPlanID == expectedBlastSingleForFireEvent.LayoutPlanID).ShouldBe(2),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.RefBlastID == expectedBlastSingleForFireEvent.RefBlastID).ShouldBe(2),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.CustomerID == expectedBlastSingleForFireEvent.CustomerID).ShouldBe(2),
                    () => _lstSavedBlastSingleForFireEvent.Count(blastSingle => blastSingle.CreatedUserID == expectedBlastSingleForFireEvent.CreatedUserID).ShouldBe(2)
                );
        }

        [Test]
        [TestCase("Subscribe", 0, 0, 0, 0)]
        [TestCase("Subscribe", 0, 0, 0, 1)]
        public void FireEvent_MyPlanGroupIDWithLogEventEmailID_LogsException(string planEventType, int myPlanGroupID, int myPlanEmailId, int emailGroupEmailId, int logEventEmailId)
        {
            // Arrange
            Entities.LayoutPlans myPlan;
            Entities.EmailActivityLog logEvent;
            User user;
            Entities.BlastSingle expectedBlastSingleForFireEvent;
            SetupFireEventTest(planEventType, myPlanGroupID, emailGroupEmailId, logEventEmailId, true, out myPlan, out logEvent, out user, out expectedBlastSingleForFireEvent);
            var expectedErrorMessage = "BlastSingle.Insert failed validation";

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEventMethodName, myPlan, logEvent, user);

            // Assert
            _savedErrorMessage.ShouldBe(expectedErrorMessage);
        }
        private void SetupFireEventTest(string planEventType, int myPlanGroupID, int emailGroupEmailId, 
                                         int logEventEmailId, bool willThrowException,
                                         out Entities.LayoutPlans myPlan, out Entities.EmailActivityLog logEvent,
                                         out User user, out Entities.BlastSingle expectedBlastSingleForFireEvent)
        {
            var defaultDate = new DateTime(2000, 1, 1);
            myPlan = new Entities.LayoutPlans
            {
                LayoutPlanID = DefaultId,
                Status = StatusYes,
                BlastID = DefaultId,
                EventType = planEventType,
                GroupID = myPlanGroupID
            };
            logEvent = new Entities.EmailActivityLog
            {
                BlastID = DefaultId,
                EmailID = logEventEmailId
            };
            user = new User();

            ShimDateTime.NowGet = () =>
            {
                return defaultDate;
            };
            expectedBlastSingleForFireEvent = new Entities.BlastSingle
            {
                SendTime = DateTime.Now,
                BlastID = DefaultId,
                EmailID = logEventEmailId,
                LayoutPlanID = myPlan.LayoutPlanID,
                RefBlastID = logEvent.BlastID,
                CustomerID = DefaultId,
                CreatedUserID = DefaultId
            };
            _lstSavedBlastSingleForFireEvent = new List<Entities.BlastSingle>();

            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (blastID, getChildren) =>
            {
                return new Entities.BlastAB
                {
                    BlastID = DefaultId,
                    BlastType = "LAYOUT",
                    GroupID = DefaultId,
                    LayoutID = DefaultId,
                    CustomerID = DefaultId,
                    CreatedUserID = DefaultId
                };
            };
            ShimEmail.GetByEmailID_NoAccessCheckInt32 = (emailId) =>
            {
                return new Entities.Email
                {
                    EmailID = logEventEmailId
                };
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (emailId, groupId) =>
            {
                return new Entities.Email();
            };
            ShimEmailGroup.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (emailId, groudId) =>
            {
                return new Entities.EmailGroup
                {
                    EmailID = emailGroupEmailId
                };
            };
            ShimBlastSingle.ExistsByBlastEmailLayoutPlanInt32Int32Int32Int32 = (blastID, emailID, layoutPlanID, customerID) =>
            {
                return false;
            };
            ShimBlastSingle.Insert_NoAccessCheckBlastSingle = (blastSingle) =>
            {
                if (willThrowException)
                {
                    throw new ECNException(new List<ECNError>());
                }

                _lstSavedBlastSingleForFireEvent.Add(blastSingle);
                return DefaultId;
            };
            ShimEmailHistory.FindMergedEmailIDInt32 = (emailId) =>
            {
                return DefaultId;
            };
            ShimTriggerPlans.GetNoOpenByBlastID_NoAccessCheckInt32 = (blastId) =>
            {
                return new List<Entities.TriggerPlans>
                {
                    new Entities.TriggerPlans
                    {
                        BlastID = DefaultId,
                        TriggerPlanID = DefaultId
                    }
                };
            };
            ShimEmailGroup.GetEmailIDFromWhatEmail_NoAccessCheckInt32Int32String = (groupID, customerID, emailAddress) =>
            {
                return -1;
            };
            ShimBlastSingle.GetRefBlastIDInt32Int32Int32String = (blastID, emailID, customerID, BlastType) =>
            {
                return DefaultId;
            };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 =
                (error, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _savedErrorMessage = error;
                    return DefaultId;
                };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var namedValueCollection = new NameValueCollection();
                namedValueCollection.Add("KMCommon_Application", DefaultId.ToString());

                return namedValueCollection;
            };
        }
    }
}