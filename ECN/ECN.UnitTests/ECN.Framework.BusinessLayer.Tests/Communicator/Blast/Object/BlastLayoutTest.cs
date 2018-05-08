﻿using System;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using EcnEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class BlastLayoutTest : BlastObjectTestsBase<BlastLayout>
    {
        private User _user;
        private EcnEntities.BlastAbstract _blastAbstractEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            ShimBlast.PreValidateBlastAbstract = (_) => { };
            ShimBlast.PreValidate_NoAccessCheckBlastAbstract = (_) => { };

            var blastAbstractEntityMock = new Mock<EcnEntities.BlastAbstract>();
            blastAbstractEntityMock.SetupAllProperties();

            _user = new User();
            _blastAbstractEntity = blastAbstractEntityMock.Object;
        }

        [Test]
        public void Validate_IfEmailFromIsEmpty_ThrowsEcnExceptionWithEmailFromErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailFromErrorMessage));
        }

        [Test]
        public void Validate_IfEmailFromNameIsEmpty_ThrowsEcnExceptionWithEmailFromNameErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFromName = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailFromNameErrorMessage));
        }

        [Test]
        public void Validate_IfStatusCodeIsNotSystem_ThrowsEcnExceptionWithStatusCodeErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.StatusCode = BlastStatusCode.Unknown.ToString();

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == StatusCodeErrorMessage));
        }

        [Test]
        public void Validate_IfBlastTypeIsNotBlastLayout_ThrowsEcnExceptionWithBlastTypeErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.BlastType = BlastType.Unknown.ToString();

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == BlastTypeErrorMessage));
        }

        [Test]
        public void Validate_IfLayoutIdIsNull_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void Validate_IfCustomerIdIsNull_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.CustomerID = null;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => { return true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void Validate_IfLayoutDoesNotExist_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.CustomerID = CustomerId;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => { return false; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void Validate_IfReplyToIsEmpty_ThrowsEcnExceptionWithReplyToErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.ReplyTo = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == ReplyToErrorMessage));
        }

        [Test]
        public void Validate_IfTestBlastIsInvalid_ThrowsEcnExceptionWithTestBlastErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.TestBlast = TestBlastValueX;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == TestBlastErrorMessage));
        }

        [Test]
        public void Validate_IfEmailSubjectIsEmpty_ThrowsEcnExceptionWithEmailSubjectErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailSubject = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailSubjectErrorMessage));
        }

        [Test]
        public void Validate_IfEmailFieldsAreNotValid_ThrowsEcnExceptionWith4Errors()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = string.Empty;
            _blastAbstractEntity.EmailFromName = string.Empty;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = string.Empty;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = string.Empty;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.Count.ShouldBe(4),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailFromErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailFromNameErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailSubjectErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == ReplyToErrorMessage),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void Validate_IfStatusCodeBlastTypeAndTestBlastFieldsAreInvalid_ThrowsEcnExceptionWith3rrors()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.Unknown.ToString();
            _blastAbstractEntity.BlastType = BlastType.Unknown.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueX;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.Count.ShouldBe(3),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == StatusCodeErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == BlastTypeErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == TestBlastErrorMessage),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void Validate_IfAllChecksAreValidExceptGroupId_DoesNotThrowException()
        {
            // Arrange
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => action.ShouldNotThrow(),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void Validate_IfAllChecksAreValidAndGroupIdAndLayoutIdNotNull_ValidateBlastContentIsCalled()
        {
            // Arrange
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = GroupId;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => action.ShouldNotThrow(),
                () => validateBlastContentCalled.ShouldBeTrue());
        }

        [Test]
        public void ValidateNoAccessCheck_IfEmailFromIsEmpty_ThrowsEcnExceptionWithEmailFromErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailFromErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfEmailFromNameIsEmpty_ThrowsEcnExceptionWithEmailFromNameErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFromName = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailFromNameErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfStatusCodeIsNotSystem_ThrowsEcnExceptionWithStatusCodeErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.StatusCode = BlastStatusCode.Unknown.ToString();

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == StatusCodeErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfBlastTypeIsNotBlastLayout_ThrowsEcnExceptionWithBlastTypeErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.BlastType = BlastType.Unknown.ToString();

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == BlastTypeErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfLayoutIdIsNull_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfCustomerIdIsNull_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.CustomerID = null;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => { return true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfLayoutDoesNotExist_ThrowsEcnExceptionWithLayoutErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.CustomerID = CustomerId;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => { return false; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == LayoutErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfReplyToIsEmpty_ThrowsEcnExceptionWithReplyToErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.ReplyTo = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == ReplyToErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfTestBlastIsInvalid_ThrowsEcnExceptionWithTestBlastErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.TestBlast = TestBlastValueX;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == TestBlastErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfEmailSubjectIsEmpty_ThrowsEcnExceptionWithEmailSubjectErrorMessage()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailSubject = string.Empty;

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.ShouldContain(err => err.ErrorMessage == EmailSubjectErrorMessage));
        }

        [Test]
        public void ValidateNoAccessCheck_IfEmailFieldsAreNotValid_ThrowsEcnExceptionWith4Errors()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = string.Empty;
            _blastAbstractEntity.EmailFromName = string.Empty;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = string.Empty;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = string.Empty;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.Count.ShouldBe(4),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailFromErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailFromNameErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == EmailSubjectErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == ReplyToErrorMessage),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void ValidateNoAccessCheck_IfStatusCodeBlastTypeAndTestBlastFieldsAreInvalid_ThrowsEcnExceptionWith3rrors()
        {
            // Arrange
            var throwedException = default(ECNException);
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.Unknown.ToString();
            _blastAbstractEntity.BlastType = BlastType.Unknown.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueX;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => throwedException = action.ShouldThrow<ECNException>(),
                () => throwedException.ErrorList.Count.ShouldBe(3),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == StatusCodeErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == BlastTypeErrorMessage),
                () => throwedException.ErrorList.ShouldContain(ex => ex.ErrorMessage == TestBlastErrorMessage),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void ValidateNoAccessCheck_IfAllChecksAreValidExceptGroupId_DoesNotThrowException()
        {
            // Arrange
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = null;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => action.ShouldNotThrow(),
                () => validateBlastContentCalled.ShouldBeFalse());
        }

        [Test]
        public void ValidateNoAccessCheck_IfAllChecksAreValidAndGroupIdAndLayoutIdNotNull_ValidateBlastContentIsCalled()
        {
            // Arrange
            _blastAbstractEntity.EmailFrom = EmailFrom;
            _blastAbstractEntity.EmailFromName = EmailFromName;
            _blastAbstractEntity.StatusCode = BlastStatusCode.System.ToString();
            _blastAbstractEntity.BlastType = BlastType.Layout.ToString();
            _blastAbstractEntity.LayoutID = LayoutId;
            _blastAbstractEntity.GroupID = GroupId;
            _blastAbstractEntity.CustomerID = CustomerId;
            _blastAbstractEntity.ReplyTo = ReplyTo;
            _blastAbstractEntity.TestBlast = TestBlastValueN;
            _blastAbstractEntity.EmailSubject = EmailSubject;
            ShimLayout.ExistsInt32Int32 = (_, __) => { return true; };

            var validateBlastContentCalled = false;
            ShimBlast.ValidateBlastContentBlastAbstractUser = (_, __) => { validateBlastContentCalled = true; };

            // Act
            var action = new Action(() => { _testEntity.Validate_NoAccessCheck(_blastAbstractEntity, _user); });

            // Assert
            action.ShouldSatisfyAllConditions(
                () => action.ShouldNotThrow(),
                () => validateBlastContentCalled.ShouldBeTrue());
        }

        [Test]
        public void Save_IfUserDoesNotHaveEditPermission_ThrowsSecurityException()
        {
            // Arrange
            var blastBusinessMock = new Mock<BlastLayout>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(false);

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var saveAction = new Action(() => { blastBusiness.Save(_blastAbstractEntity, _user); });

            // Assert
            saveAction.ShouldThrow<SecurityException>();
        }

        [Test]
        public void Save_IfCanAccessByCustomerReturnsFalse_ThrowsSecurityException()
        {
            // Arrange
            var blastBusinessMock = new Mock<BlastLayout>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(true);

            ShimAccessCheck.CanAccessByCustomerOf1M0User<EcnEntities.BlastAbstract>((_, __) => { return false; });

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var saveAction = new Action(() => { blastBusiness.Save(_blastAbstractEntity, _user); });

            // Assert
            saveAction.ShouldThrow<SecurityException>();
        }

        [Test]
        public void Save_IfNoExceptionOccured_SavesBlastAndReturnsBlastId()
        {
            // Arrange
            _blastAbstractEntity.BlastID = BlastId;

            var blastBusinessMock = new Mock<BlastLayout>();
            blastBusinessMock.Setup(b => b.Validate(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));
            blastBusinessMock.Setup(b => b.HasPermission(It.IsAny<KMPlatform.Enums.Access>(), It.IsAny<User>())).Returns(true);
            var blastBusiness = blastBusinessMock.Object;

            ShimAccessCheck.CanAccessByCustomerOf1M0User<EcnEntities.BlastAbstract>((_, __) => { return true; });

            var blastSaveCalled = false;
            ShimBlast.SaveBlastAbstractUser = (argBlastAbstract, argUser) =>
            {
                blastSaveCalled = true;
                return argBlastAbstract.BlastID;
            };

            // Act
            var returnedValue = blastBusiness.Save(_blastAbstractEntity, _user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(_blastAbstractEntity.BlastID),
                () => blastSaveCalled.ShouldBeTrue());
        }

        [Test]
        public void SaveNoAccessCheck_WhenCalled_SavesBlastAndReturnsBlastId()
        {
            // Arrange
            _blastAbstractEntity.BlastID = BlastId;

            var blastBusinessMock = new Mock<BlastLayout>();
            blastBusinessMock.Setup(b => b.Validate_NoAccessCheck(It.IsAny<EcnEntities.BlastAbstract>(), It.IsAny<User>()));

            var blastSaveCalled = false;
            ShimBlast.SaveBlastAbstractUser = (argBlastAbstract, argUser) =>
            {
                blastSaveCalled = true;
                return argBlastAbstract.BlastID;
            };

            var blastBusiness = blastBusinessMock.Object;

            // Act
            var returnedValue = blastBusiness.Save_NoAccessCheck(_blastAbstractEntity, _user);

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(_blastAbstractEntity.BlastID),
                () => blastSaveCalled.ShouldBeTrue());
        }
    }
}
