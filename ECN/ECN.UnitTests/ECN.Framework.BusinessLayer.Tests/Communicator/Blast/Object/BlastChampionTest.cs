using System;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using EcnEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class BlastChampionTest : BlastObjectTestsBase<BlastChampion>
    {
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
    }
}
