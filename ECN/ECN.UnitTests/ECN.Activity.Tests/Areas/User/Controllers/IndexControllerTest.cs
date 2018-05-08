using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Linq;
using ecn.activity.Areas.User.Controllers;
using ecn.activity.Areas.User.Controllers.Fakes;
using ecn.activity.Areas.User.Models;
using ECN.TestHelpers;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using SecurityGroupOptIn = KMPlatform.Entity.SecurityGroupOptIn;
using Entity = KMPlatform.Entity;

namespace ecn.activity.Tests.Areas.User.Controllers
{
    /// <summary>
    ///     Unit Tests for <see cref="IndexController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class IndexControllerTest
    {
        private const int ClientId = 25;
        private const int SecurityGroupID = 10;
        private const int UserId = 15;
        private const string UserName = "username";
        private const string Password = "password";
        private const int ClientGroupId = 11;
        private const string KmPlatformLogin = "Login";

        private IDisposable _context;
        private IndexController _controller;
        private Guid _setId;
        private SecurityGroupOptIn _securityGroupsToAccept;
        private Entity.UserClientSecurityGroupMap _userClientSg;
        private Entity.User _currentUser;
        private Entity.User _oldUser;
        private int _userId;

        private bool _validateUser;
        private bool _roleExist;
        private Entity.ClientGroupClientMap _clientGrpMap;
        private UserAcceptModel _userAcceptModel;
        private bool _returnOldUser;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _controller = new IndexController();

            _setId = Guid.NewGuid();
            _validateUser = false;
            _roleExist = false;
            _returnOldUser = false;
            _userId = UserId;

            _currentUser = typeof(Entity.User).CreateInstance();
            _currentUser.UserID = UserId;
            _currentUser.IsPlatformAdministrator = false;
            _currentUser.UserName = UserName;
            _currentUser.IsActive = true;
            _currentUser.Status = KMPlatform.Enums.UserStatus.Active;
            _currentUser.Password = Password;
            _currentUser.IsAccessKeyValid = true;

            _oldUser = typeof(Entity.User).CreateInstance();

            _securityGroupsToAccept = typeof(SecurityGroupOptIn).CreateInstance();
            _securityGroupsToAccept.ClientID = ClientId;
            _securityGroupsToAccept.SecurityGroupID = SecurityGroupID;
            _securityGroupsToAccept.SecurityGroupOptInID = SecurityGroupID;

            _userClientSg = typeof(Entity.UserClientSecurityGroupMap).CreateInstance();
            _userClientSg.ClientID = ClientId;
            _userClientSg.SecurityGroupID = SecurityGroupID;
            _currentUser.DefaultClientGroupID = ClientGroupId;

            _clientGrpMap = typeof(Entity.ClientGroupClientMap).CreateInstance();
            _clientGrpMap.ClientGroupID = ClientGroupId;
            _clientGrpMap.ClientID = ClientId;

            ConfigurationManager.AppSettings["KMPlatformLogin"] = KmPlatformLogin;

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void SetupFakes()
        {
            ShimDateTime.NowGet = () => new DateTime(2018, 3, 10, 10, 10, 10);
            ShimSecurityGroupOptIn.AllInstances.GetBySetIDGuid = (_, setId) =>
            {
                setId.ShouldBe(_setId);

                return new List<SecurityGroupOptIn>
                {
                   _securityGroupsToAccept
                };
            };

            ShimUserClientSecurityGroupMap.AllInstances.SelectForUserInt32 = (_, id) =>
            {
                id.ShouldBe(UserId);

                return new List<Entity.UserClientSecurityGroupMap>
                {
                    _userClientSg
                };
            };

            ShimUser.AllInstances.SelectUserInt32Boolean = (_, id, includeObjects) =>
            {
                includeObjects.ShouldBeFalse();
                id.ShouldBe(UserId);

                return _returnOldUser ?
                        _oldUser :
                        _currentUser;
            };

            ShimUser.AllInstances.Validate_UserNameStringInt32 = (_, name, id) =>
            {
                id.ShouldBe(_userId);
                name.ShouldBe(UserName);

                return _validateUser;
            };

            ShimIndexController.AllInstances.RoleCheckListOfSecurityGroupOptInListOfUserClientSecurityGroupMap =
                (_, sec, __) =>
                {
                    sec.First().IsContentMatched(_securityGroupsToAccept).ShouldBeTrue();

                    return _roleExist;
                };

            ShimSecurityGroupOptIn.AllInstances.MarkAsAcceptedInt32 = (_, id) =>
            {
                id.ShouldBe(SecurityGroupID);
            };

            ShimUser.AllInstances.SaveUser = (_, user) =>
            {
                _currentUser.DefaultClientID = ClientId;
                user.IsContentMatched(_currentUser, nameof(Entity.User.AccessKey)).ShouldBeTrue();

                return UserId;
            };

            ShimUserClientSecurityGroupMap.AllInstances.SaveUserClientSecurityGroupMap = (map, user) =>
            {
                user.IsContentMatched(_userClientSg).ShouldBeTrue();

                return user.UserID;
            };

            ShimClientGroupClientMap.AllInstances.SelectForClientIDInt32 = (_, id) =>
            {
                id.ShouldBe(ClientId);

                return new List<Entity.ClientGroupClientMap>
                {
                    _clientGrpMap
                };
            };

            ShimIndexController.AllInstances.SendConfirmationEmailsUserAcceptModel = (_, model) =>
            {
                model.ShouldBe(_userAcceptModel);
            };

            ShimUser.AllInstances.SearchUserNameString = (_, name) =>
            {
                name.ShouldBe(UserName);

                return _currentUser;
            };

            ShimSecurityGroupOptIn.AllInstances.SaveSecurityGroupOptIn = (_, result) =>
            {
                result.IsContentMatched(_securityGroupsToAccept).ShouldBeTrue();
                return result.UserID;
            };

            ShimIndexController.AllInstances.DeleteOldUserInt32 = (_, id) =>
            {
                id.ShouldBe(UserId);
            };
        }
    }
}
