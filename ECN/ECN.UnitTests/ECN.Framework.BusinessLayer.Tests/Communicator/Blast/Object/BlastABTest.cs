using System;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using EcnEntities = ECN_Framework_Entities.Communicator;
using KMFakes = KM.Platform.Fakes;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class BlastABTest : BlastObjectTestsBase<BlastAB>
    {
        private const string TestBlastValueN = "N";
        private const string DummyText = "DummyText";
        private const string HTMLText = "<html>";
        private const int IdValue = 1;
        private DateTime _nowDateTime = DateTime.Now;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            ShimCustomer.ExistsInt32 = (_) => { return true; };
            ShimBlastSchedule.ExistsInt32 = (_) => { return false; };

            ShimGroup.SuppressionGroupsExistsStringInt32 = (_, __) => { return false; };
            ShimGroup.ExistsInt32Int32 = (_, __) => { return true; };
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (_, __, ___) => { };

            ShimBlast.RefBlastsExistsStringInt32DateTime = (_, __, ___) => { return false; };

            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };
            ShimLayout.ValidateLayoutContentInt32 = (_) => { return new List<string>(); };

            KMFakes.ShimUser.IsSystemAdministratorUser = (_) => { return true; };
            KMPlatformFakes.ShimUser.GetByUserIDInt32Boolean = (_, __) => { return new User(); };

            ShimEmail.IsValidEmailAddressString = (_) => { return false; };
        }
        
        [Test]
        public void ValidateNoAccessCheck_NullCustomerIDButNotNullCodeID_ThrowsExceptionWithThreeErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() => 
                _testEntity.Validate_NoAccessCheck(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CodeID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future",
                "CodeID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void ValidateNoAccessCheck_NotNullCustomerIDAndSendTimeAndBlastSuppressionAndRefBlastID_ThrowsExceptionWithFourteenErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate_NoAccessCheck(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CustomerID = IdValue,
                        SendTime = _nowDateTime,
                        BlastSuppression = DummyText,
                        RefBlastID = DummyText
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "EmailFrom cannot be empty",
                "EmailFromName cannot be empty",
                "StatusCode is invalid",
                "BlastType is invalid",
                "LayoutID is invalid",
                "GroupID is invalid",
                "ReplyTo cannot by empty",
                "TestBlast is invalid",
                "BlastSuppression is invalid",
                "RefBlastID is invalid",
                "BlastAmount is invalid",
                "BlastAmountType is invalid",
                "EmailSubject is invalid",
                "SampleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void ValidateNoAccessCheck_ValidPropertiesWithHtmlEmailSubject_ThrowsExceptionWithTwoErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate_NoAccessCheck(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CustomerID = IdValue,
                        SendTime = _nowDateTime,
                        EmailFrom = DummyText,
                        EmailFromName = DummyText,
                        StatusCode = BlastStatusCode.Pending.ToString(),
                        LayoutID = IdValue,
                        GroupID = IdValue,
                        ReplyTo = DummyText,
                        BlastType = BlastType.Sample.ToString(),
                        TestBlast = TestBlastValueN,
                        OverrideAmount = IdValue,
                        OverrideIsAmount = false,
                        EmailSubject = HTMLText
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                string.Format(
                    "{0}{1}",
                    "Email Subject has the following issues, ",
                    "html elements(invisible characters)"),
                "SampleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void ValidateNoAccessCheck_NotNullCodeIDNotNullScheduleId_ThrowsExceptionWithFourErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate_NoAccessCheck(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CodeID = IdValue,
                        BlastScheduleID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future",
                "CodeID is invalid",
                "BlastScheduleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void ValidateNoAccessCheck_UnInitializedBlast_ThrowsExceptionWithTwoErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate_NoAccessCheck(new ECN_Framework_Entities.Communicator.BlastAB(), null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_UnInitializedBlast_ThrowsExceptionWithTwoErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(new ECN_Framework_Entities.Communicator.BlastAB(), null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_NullCustomerIDButNotNullCodeID_ThrowsExceptionWithThreeErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CodeID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future",
                "CodeID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_NullEmailFromButNotNullBlastSuppressionAndRefBlastID_ThrowsExceptionWithFourteenErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CustomerID = IdValue,
                        SendTime = _nowDateTime,
                        BlastSuppression = DummyText,
                        RefBlastID = DummyText,
                        CreatedUserID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "EmailFrom cannot be empty",
                "EmailFromName cannot be empty",
                "StatusCode is invalid",
                "BlastType is invalid",
                "LayoutID is invalid",
                "GroupID is invalid",
                "ReplyTo cannot by empty",
                "TestBlast is invalid",
                "BlastSuppression is invalid",
                "RefBlastID is invalid",
                "BlastAmount is invalid",
                "BlastAmountType is invalid",
                "EmailSubject is invalid",
                "SampleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_NotNullEmailFromAndBlastSuppressionAndRefBlastID_ThrowsExceptionWithFourteenErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CustomerID = IdValue,
                        SendTime = _nowDateTime,
                        BlastSuppression = DummyText,
                        RefBlastID = DummyText,
                        CreatedUserID = IdValue,
                        EmailFrom = DummyText,
                        ReplyTo = DummyText
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "EmailFrom is invalid",
                "EmailFromName cannot be empty",
                "StatusCode is invalid",
                "BlastType is invalid",
                "LayoutID is invalid",
                "GroupID is invalid",
                "ReplyTo is invalid",
                "TestBlast is invalid",
                "BlastSuppression is invalid",
                "RefBlastID is invalid",
                "BlastAmount is invalid",
                "BlastAmountType is invalid",
                "EmailSubject is invalid",
                "SampleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_ValidPropertiesWithHtmlEmailSubject_ThrowsExceptionWithFourErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CustomerID = IdValue,
                        SendTime = _nowDateTime,
                        EmailFrom = DummyText,
                        EmailFromName = DummyText,
                        StatusCode = BlastStatusCode.Pending.ToString(),
                        LayoutID = IdValue,
                        GroupID = IdValue,
                        ReplyTo = DummyText,
                        BlastType = BlastType.Sample.ToString(),
                        TestBlast = TestBlastValueN,
                        OverrideAmount = IdValue,
                        OverrideIsAmount = false,
                        EmailSubject = HTMLText,
                        CreatedUserID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "EmailFrom is invalid",
                "ReplyTo is invalid",
                string.Format(
                    "{0}{1}",
                    "Email Subject has the following issues, ",
                    "html elements(invisible characters)"),
                "SampleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Validate_NotNullCodeIDNotNullSchedule_ThrowsExceptionWithFourErrors()
        {
            // Arrange, Act
            var exception = Should.Throw<ECNException>(() =>
                _testEntity.Validate(
                    new ECN_Framework_Entities.Communicator.BlastAB()
                    {
                        CodeID = IdValue,
                        BlastScheduleID = IdValue
                    },
                    null));

            // Assert
            var exceptionMessages = new string[]
            {
                "CustomerID is invalid",
                "SendTime must be in the future",
                "CodeID is invalid",
                "BlastScheduleID is invalid"
            };
            AssertExceptionInformation(exception, exceptionMessages);
        }

        [Test]
        public void Save_IfUserDoesNotHaveEditPermission_ThrowsSecurityException()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            var blastAbstractEntity = blastAbstractEntityMock.Object;

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(false);

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var saveAction = new Action(() => { blastBusiness.Save(blastAbstractEntity, user); });

            // Assert
            saveAction.ShouldThrow<SecurityException>();
        }

        [Test]
        public void Save_IfCanAccessByCustomerReturnsFalse_ThrowsSecurityException()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            var blastAbstractEntity = blastAbstractEntityMock.Object;

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(true);

            ShimAccessCheck.CanAccessByCustomerOf1M0User<EcnEntities.BlastAbstract>((_, __) => { return false; });

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var saveAction = new Action(() => { blastBusiness.Save(blastAbstractEntity, user); });

            // Assert
            saveAction.ShouldThrow<SecurityException>();
        }

        [Test]
        public void Save_IfBlastFieldsPropertyIsNullAfterBlastSave_BlastFieldsSaveNotCalled()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            blastAbstractEntityMock.SetupAllProperties();

            var blastAbstractEntity = blastAbstractEntityMock.Object;
            blastAbstractEntity.BlastID = BlastId;
            blastAbstractEntity.CustomerID = CustomerId;
            blastAbstractEntity.CreatedUserID = CreatedUserId;
            blastAbstractEntity.UpdatedUserID = UpdatedUserId;
            blastAbstractEntity.Fields = null;

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(true);

            ShimAccessCheck.CanAccessByCustomerOf1M0User<EcnEntities.BlastAbstract>((_, __) => { return true; });
            ShimBlast.SaveBlastAbstractUser = (blastAbstractParam, userParam) => { return blastAbstractParam.BlastID; };

            var blastFieldsSaveCalled = false;
            ShimBlastFields.SaveBlastFieldsUser = (blastFieldsParam, userParam) => { blastFieldsSaveCalled = true; };

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var returnedValue = blastBusiness.Save(blastAbstractEntity, user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(blastAbstractEntity.BlastID),
                () => blastFieldsSaveCalled.ShouldBeFalse(),
                () => blastAbstractEntity.Fields.ShouldBeNull());
        }

        [Test]
        public void Save_IfBlastFieldsPropertyIsNotNullAfterBlastSave_CallsBlastFieldsSave()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            blastAbstractEntityMock.SetupAllProperties();

            var blastAbstractEntity = blastAbstractEntityMock.Object;
            blastAbstractEntity.BlastID = BlastId;
            blastAbstractEntity.CustomerID = CustomerId;
            blastAbstractEntity.CreatedUserID = CreatedUserId;
            blastAbstractEntity.UpdatedUserID = UpdatedUserId;
            blastAbstractEntity.Fields = new EcnEntities.BlastFields();

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(true);

            ShimAccessCheck.CanAccessByCustomerOf1M0User<EcnEntities.BlastAbstract>((_, __) => { return true; });
            ShimBlast.SaveBlastAbstractUser = (blastAbstractParam, userParam) => { return blastAbstractParam.BlastID; };

            var blastFieldsSaveCalled = false;
            ShimBlastFields.SaveBlastFieldsUser = (blastFieldsParam, userParam) => { blastFieldsSaveCalled = true; };

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var returnedValue = blastBusiness.Save(blastAbstractEntity, user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(blastAbstractEntity.BlastID),
                () => blastFieldsSaveCalled.ShouldBeTrue(),
                () => blastAbstractEntity.Fields.ShouldNotBeNull(),
                () => blastAbstractEntity.Fields.BlastID.ShouldBe(blastAbstractEntity.BlastID),
                () => blastAbstractEntity.Fields.CustomerID.ShouldBe(blastAbstractEntity.CustomerID),
                () => blastAbstractEntity.Fields.CreatedUserID.ShouldBe(blastAbstractEntity.CreatedUserID),
                () => blastAbstractEntity.Fields.UpdatedUserID.ShouldBe(blastAbstractEntity.UpdatedUserID));
        }

        [Test]
        public void SaveNoAccessCheck_IfBlastFieldsPropertyIsNullAfterBlastSave_BlastFieldsSaveNotCalled()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            blastAbstractEntityMock.SetupAllProperties();

            var blastAbstractEntity = blastAbstractEntityMock.Object;
            blastAbstractEntity.BlastID = BlastId;
            blastAbstractEntity.CustomerID = CustomerId;
            blastAbstractEntity.CreatedUserID = CreatedUserId;
            blastAbstractEntity.UpdatedUserID = UpdatedUserId;
            blastAbstractEntity.Fields = null;

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate_NoAccessCheck(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));

            ShimBlast.SaveBlastAbstractUser = (blastAbstractParam, userParam) => { return blastAbstractParam.BlastID; };
            var blastFieldsSaveCalled = false;
            ShimBlastFields.Save_NoAccessCheckBlastFieldsUser = (blastFieldsParam, userParam) => { blastFieldsSaveCalled = true; };

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var returnedValue = blastBusiness.Save_NoAccessCheck(blastAbstractEntity, user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(blastAbstractEntity.BlastID),
                () => blastFieldsSaveCalled.ShouldBeFalse(),
                () => blastAbstractEntity.Fields.ShouldBeNull());
        }

        [Test]
        public void SaveNoAccessCheck_IfBlastFieldsPropertyIsNotNullAfterBlastSave_CallsBlastFieldsSave()
        {
            // Arrange
            var user = new User();
            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            blastAbstractEntityMock.SetupAllProperties();

            var blastAbstractEntity = blastAbstractEntityMock.Object;
            blastAbstractEntity.BlastID = BlastId;
            blastAbstractEntity.CustomerID = CustomerId;
            blastAbstractEntity.CreatedUserID = CreatedUserId;
            blastAbstractEntity.UpdatedUserID = UpdatedUserId;
            blastAbstractEntity.Fields = new EcnEntities.BlastFields();

            var blastBusinessMock = new Mock<BlastChampion>();
            blastBusinessMock.Setup(b => b.Validate_NoAccessCheck(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));

            ShimBlast.SaveBlastAbstractUser = (blastAbstractParam, userParam) => { return blastAbstractParam.BlastID; };

            var blastFieldsSaveCalled = false;
            ShimBlastFields.Save_NoAccessCheckBlastFieldsUser = (blastFieldsParam, userParam) => { blastFieldsSaveCalled = true; };

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var returnedValue = blastBusiness.Save_NoAccessCheck(blastAbstractEntity, user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(blastAbstractEntity.BlastID),
                () => blastFieldsSaveCalled.ShouldBeTrue(),
                () => blastAbstractEntity.Fields.ShouldNotBeNull(),
                () => blastAbstractEntity.Fields.BlastID.ShouldBe(blastAbstractEntity.BlastID),
                () => blastAbstractEntity.Fields.CustomerID.ShouldBe(blastAbstractEntity.CustomerID),
                () => blastAbstractEntity.Fields.CreatedUserID.ShouldBe(blastAbstractEntity.CreatedUserID),
                () => blastAbstractEntity.Fields.UpdatedUserID.ShouldBe(blastAbstractEntity.UpdatedUserID));
        }

        private void AssertExceptionInformation(ECNException exception, string[] messages)
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ErrorList, Is.Not.Null);
            Assert.That(messages, Is.EqualTo(exception.ErrorList.Select(x => x.ErrorMessage).ToArray()));
        }
    }
}
