using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using FrameworkEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    /// <summary>
    /// Unit test for <see cref="EventOrganizer"/> class;
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class EventOrganizerTest
    {
        private const string FireEvent = "FireEvent";
        private const string MessageError = "object reference not set to an instance";
        private const string KMCommonApplication = "KMCommon_Application";
        private readonly NameValueCollection _appSettings = new NameValueCollection();
        private EventOrganizer _eventOrganizer;
        private PrivateObject _privateObject;
        private PrivateType _privateEventOrganizerType;
        private IDisposable _shimObject;
        private bool _insertNoAccessCheck = false;


        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _eventOrganizer = new EventOrganizer();
            _privateObject = new PrivateObject(_eventOrganizer);
            _privateEventOrganizerType = new PrivateType(typeof(EventOrganizer));
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [TestCase(ActionTypeCode.Subscribe, 1, 1)]
        [TestCase(ActionTypeCode.Subscribe, 0, 1)]
        [TestCase(ActionTypeCode.Subscribe, 1, 0)]
        [TestCase(ActionTypeCode.Subscribe, 0, 0)]
        public void FireEvent_LayoutPlansIsNotNullAndUserIsNotNull_UpdateValueInDatabase(ActionTypeCode actionTypeCode, int emailId, int groupId)
        {
            // Arrange
            var layoutPlans = CreateLayoutPlansObject(groupId);
            var logEvent = CreateEmailActivityLogObject();
            var user = new KMPlatform.Entity.User();
            var parameters = new object[] { layoutPlans, logEvent, user };
            CommonPageFakeObject(emailId);

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEvent, parameters);

            // Assert
            _insertNoAccessCheck.ShouldBeTrue();
        }

        [TestCase(ActionTypeCode.Subscribe, 1, 1)]
        [TestCase(ActionTypeCode.Subscribe, 0, 1)]
        [TestCase(ActionTypeCode.Subscribe, 1, 0)]
        [TestCase(ActionTypeCode.Subscribe, 0, 0)]
        public void FireEvent_BlastSingleIsNull_ThrowException(ActionTypeCode actionTypeCode, int emailId, int groupId)
        {
            // Arrange
            var layoutPlans = CreateLayoutPlansObject(groupId);
            var erroMessage = string.Empty;
            var logEvent = CreateEmailActivityLogObject();
            var user = new KMPlatform.Entity.User();
            var parameters = new object[] { layoutPlans, logEvent, user };
            CommonPageFakeObject(emailId);
            var errorList = new List<ECNError>();
            ShimBlastSingle.Insert_NoAccessCheckBlastSingle = (x) =>
            {
                errorList = new List<ECNError>();
                errorList.Add(new ECNError
                {
                    Entity = Enums.Entity.BaseChannel,
                    ErrorMessage = MessageError
                });
                throw new ECNException(errorList);
            };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (error, sourceMethod, applicationId, note, charityId, customerId) =>
            {
                erroMessage = error;
                return 1;
            };

            // Act
            _privateEventOrganizerType.InvokeStatic(FireEvent, parameters);

            // Assert
            errorList.ShouldSatisfyAllConditions(
                () => errorList.ShouldNotBeNull(),
                () => errorList.Any().ShouldBeTrue(),
                () => errorList.Count.ShouldBe(1),
                () => errorList.FirstOrDefault().Entity.ShouldBe(Enums.Entity.BaseChannel),
                () => errorList.FirstOrDefault().ErrorMessage.ShouldBe(MessageError)
            );
            erroMessage.ShouldNotBeNullOrEmpty();
        }

        private void CommonPageFakeObject(int emailId)
        {
            _appSettings.Clear();
            _appSettings.Add(KMCommonApplication, "1");
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                return BlastAbstractMockObject();
            };
            ShimEmailGroup.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (enailID, groupID) =>
            {
                if (enailID > 0)
                {
                    return CreateEmailGroupObject(emailId);
                }
                return null;
            };
            ShimBlastSingle.ExistsByBlastEmailLayoutPlanInt32Int32Int32Int32 = (blastID, emailID, layoutPlanID, customerID) => { return false; };

            ShimBlastSingle.Insert_NoAccessCheckBlastSingle = (x) =>
            {
                _insertNoAccessCheck = true;
                return 1;
            };
            ShimEmailHistory.FindMergedEmailIDInt32 = (x) => { return 1; };
            ShimBlastSingle.GetRefBlastIDInt32Int32Int32String = (blastID, emailID, customerID, blastType) => { return 1; };
            ShimTriggerPlans.GetNoOpenByBlastID_NoAccessCheckInt32 = (blastID) =>
            {
                return CreateTriggerPlansListObject();
            };
            ShimEmail.GetByEmailID_NoAccessCheckInt32 = (enailID) =>
            {
                return CreateEmailObject(emailId, enailID);
            };
        }

        private FrameworkEntities.Email CreateEmailObject(int emailId, int enailID)
        {
            if (enailID > 0)
            {
                return new FrameworkEntities.Email
                {
                    CustomerID = 1,
                    EmailID = emailId,
                    SubscribeTypeCode = "a"
                };
            }
            return null;
        }

        private List<FrameworkEntities.TriggerPlans> CreateTriggerPlansListObject()
        {
            return new List<FrameworkEntities.TriggerPlans>
            {
                new FrameworkEntities.TriggerPlans
                {
                    BlastID = 1,
                    TriggerPlanID =1,
                    Period =1,
                    CustomerID = 1,
                    CreatedUserID =1 ,
                    GroupID = 1,
                    IsDeleted =false,
                }
            };
        }

        private FrameworkEntities.EmailGroup CreateEmailGroupObject(int emailId)
        {
            return new FrameworkEntities.EmailGroup
            {
                CustomerID = 1,
                EmailGroupID = 1,
                EmailID = emailId,
                CreatedOn = DateTime.Now,
                GroupID = 1,
                SMSEnabled = true,
                SubscribeTypeCode = "a"
            };
        }

        private FrameworkEntities.EmailActivityLog CreateEmailActivityLogObject()
        {
            return new FrameworkEntities.EmailActivityLog
            {
                ActionValue = "a",
                EmailID = 1,
                BlastID = 1,
                ActionDate = DateTime.Now,
                ActionNotes = "Unt Test",
                ActionTypeCode = ActionTypeCode.Subscribe.ToString(),
                CustomerID = 1,
                EAID = 1,
                ErrorList = new List<ECNError>(),
                Processed = "Yes",
            };
        }

        private FrameworkEntities.LayoutPlans CreateLayoutPlansObject(int groupId)
        {
            // Arrange
            return new FrameworkEntities.LayoutPlans
            {
                LayoutPlanID = 1,
                LayoutID = 1,
                Status = "Y",
                BlastID = 1,
                Criteria = "a",
                GroupID = groupId,
                EventType = ActionTypeCode.Subscribe.ToString(),
                Period = 1,
                IsDeleted = false,
                UpdatedUserID = 1,
                CampaignItemID = 1,
                ActionName = "Add",
                CreatedDate = DateTime.Now,
                CreatedUserID = 1,
                CustomerID = 1,
                SmartFormID = 1,
                TokenUID = Guid.NewGuid(),
                UpdatedDate = DateTime.Now,
            };
        }

        private FrameworkEntities.BlastAbstract BlastAbstractMockObject()
        {
            var blastAbstractMockObject = new Mock<FrameworkEntities.BlastAbstract>().Object;
            blastAbstractMockObject.BlastType = "LAYOUT";
            blastAbstractMockObject.GroupID = 1;
            blastAbstractMockObject.LayoutID = 1;
            blastAbstractMockObject.CustomerID = 1;
            blastAbstractMockObject.BlastID = 1;
            blastAbstractMockObject.CreatedUserID = 1;
            return blastAbstractMockObject;
        }
    }
}