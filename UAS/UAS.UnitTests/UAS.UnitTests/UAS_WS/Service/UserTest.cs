using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAS_WS.Service.Common;
using ServiceUser = UAS_WS.Service.User;
using EntityUser = KMPlatform.Entity.User;
using ShimWorker = KMPlatform.BusinessLogic.Fakes.ShimUser;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;

namespace UAS.UnitTests.UAS_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UserTest : Fakes
    {
        private const int ClientGroupId = 100;
        private const int ClientId = 200;
        private const int SecurityGroupId = 300;
        private const int UserId = 400;
        private const int SavedCount = 1;
        private const int SavedCountForError = -1;
        private const string SecurityGroupName = "group1";
        private const string UserEmail = "a@b.com";

        private ServiceUser _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceUser();
        }

        [Test]
        public void Select_IfInternalWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectBoolean = (_, __) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.Select(Guid.Empty, true);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
        }

        [Test]
        public void Select_IncludeObjectsIsTrue_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityUser>();
            ShimWorker.AllInstances.SelectBoolean = (_, __) =>
            {
                list.Add(new EntityUser());
                return list;
            };

            // Act
            var result = _testEntity.Select(Guid.Empty, true);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            list.ShouldNotBeEmpty();
        }

        [Test]
        public void Select_ByClientGroupId_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.Select(Guid.Empty, ClientGroupId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBeNull();
        }

        [Test]
        public void Select_ByClientIdAndSecurityGroupId_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.Select(Guid.Empty, ClientId, SecurityGroupId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBeNull();
        }

        [Test]
        public void Select_ByClientIdAndSecurityGroupName_ReturnsErrorResponse()
        {
            // Arrange, Act
            var result = _testEntity.Select(Guid.Empty, ClientId, SecurityGroupName);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBeNull();
        }

        [Test]
        public void LogIn_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new EntityUser();
            ShimWorker.AllInstances.LogInGuidBoolean = (_, __, ___) => user;

            // Act
            var result = _testEntity.LogIn(Guid.Empty);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(user);
        }

        [Test]
        public void SetUserObjects_ByUserEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new EntityUser();
            ShimWorker.AllInstances.SetUserObjectsUser = (_, __) => user;
            ShimForJsonFunction<EntityUser>();

            // Act
            var result = _testEntity.SetUserObjects(Guid.Empty, user);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(user);
        }

        [Test]
        public void SearchUserName_ByUserName_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new EntityUser();
            ShimWorker.AllInstances.SearchUserNameString = (_, __) => user;

            // Act
            var result = _testEntity.SearchUserName(Guid.Empty, string.Empty);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(user);
        }

        [Test]
        public void SetAuthorizedUserObjects_ByUserEntity_ReturnsErrorResponse()
        {
            // Arrange
            ShimForJsonFunction<EntityUser>();

            // Act
            var result = _testEntity.SetAuthorizedUserObjects(Guid.Empty, new EntityUser());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBeNull();
        }

        [Test]
        public void GetEmailAddress_ByUserName_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetEmailAddressString = (_, __) => UserEmail;

            // Act
            var result = _testEntity.GetEmailAddress(Guid.Empty, string.Empty);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(UserEmail);
        }

        [Test]
        public void EmailExist_ByUserEmail_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.EmailExistString = (_, __) => false;

            // Act
            var result = _testEntity.EmailExist(Guid.Empty, UserEmail);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeFalse();
        }

        [Test]
        public void Search_ByTextAndUserList_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityUser>();
            ShimWorker.AllInstances.SearchStringListOfUser = (_, __, ___) =>
            {
                list.Add(new EntityUser());
                return list;
            };

            // Act
            var result = _testEntity.Search(Guid.Empty, string.Empty, list);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldNotBeEmpty();
        }

        [Test]
        public void SelectUser_ByUserId_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new EntityUser();
            ShimWorker.AllInstances.SelectUserInt32Boolean = (_, __, ___) => user;

            // Act
            var result = _testEntity.SelectUser(Guid.Empty, UserId);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeSameAs(user);
        }

        [Test]
        public void Save_IfWorkerReturnsPositive_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new EntityUser();
            var emailDirect = new EmailDirect();
            EmailDirect parameterEmailDirect = null;
            ShimWorker.AllInstances.SaveUser = (_, __) => SavedCount;
            ShimEmailDirect.SaveEmailDirect = direct =>
            {
                parameterEmailDirect = direct;
                return SavedCount;
            };
            ShimForJsonFunction<EntityUser>();

            // Act
            var result = _testEntity.Save(Guid.Empty, user, emailDirect);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBe(SavedCount);
            parameterEmailDirect.ShouldBeSameAs(emailDirect);
        }

        [Test]
        public void Save_IfWorkerReturnsNegative_ReturnsErrorResponse()
        {
            // Arrange
            var user = new EntityUser();
            var emailDirect = new EmailDirect();
            EmailDirect parameterEmailDirect = null;
            ShimWorker.AllInstances.SaveUser = (_, __) => SavedCountForError;
            ShimEmailDirect.SaveEmailDirect = direct =>
            {
                parameterEmailDirect = direct;
                return SavedCount;
            };
            ShimForJsonFunction<EntityUser>();

            // Act
            var result = _testEntity.Save(Guid.Empty, user, emailDirect);

            // Assert
            result.Status.ShouldBe(ResponseStatus.Error);
            result.Result.ShouldBe(SavedCountForError);
            parameterEmailDirect.ShouldBeNull();
        }

        [Test]
        public void IsKmUser_ByUserEntity_ReturnsSuccessResponse()
        {
            // Arrange, Act
            var result = _testEntity.IsKmUser(Guid.Empty, new EntityUser());

            // Assert
            result.Status.ShouldBe(ResponseStatus.Success);
            result.Result.ShouldBeFalse();
        }
    }
}
