using System;
using System.Data;
using System.Data.SqlClient;
using KMPlatform.DataAccess;
using KMPlatform.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.KMPlatform.Tests.DataAccess
{
    [TestFixture]
    public class ApplicationTest
    {
        private const string ParameterServiceID = "@ServiceID";
        private const string ParameterUserID = "@UserID";
        private const string ParameterSecurityGroupID = "@SecurityGroupID";
        private readonly int ServiceId = 9;
        private readonly int UserId = 4;
        private readonly int SecurityGroupId = 2;

        private IDisposable _shimContext;

        [SetUp]
        public void SetUp()
        {
            _shimContext = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void SelectOnlyEnabledForService_ValidParameter_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.SelectOnlyEnabledForService(ServiceId);

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_SelectOnlyEnabled_ServiceID"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(1),
                () => sqlCommand.Parameters[0].ParameterName.ShouldBe(ParameterServiceID),
                () => sqlCommand.Parameters[0].Value.ShouldBe(ServiceId));
        }

        [Test]
        public void SelectOnlyEnabledForService_ValidParameters_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.SelectOnlyEnabledForService(ServiceId, UserId);

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_SelectOnlyEnabled_ServiceID_UserID"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(2),
                () => sqlCommand.Parameters[0].ParameterName.ShouldBe(ParameterServiceID),
                () => sqlCommand.Parameters[0].Value.ShouldBe(ServiceId),
                () => sqlCommand.Parameters[1].ParameterName.ShouldBe(ParameterUserID),
                () => sqlCommand.Parameters[1].Value.ShouldBe(UserId));
        }

        [Test]
        public void SelectForService_ValidParameter_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.SelectForService(ServiceId);

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_Select_ServiceID"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(1),
                () => sqlCommand.Parameters[0].ParameterName.ShouldBe(ParameterServiceID),
                () => sqlCommand.Parameters[0].Value.ShouldBe(ServiceId));
        }

        [Test]
        public void SelectForSecurityGroup_ValidParameter_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.SelectForSecurityGroup(SecurityGroupId);

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_Select_SecurityGroupID"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(1),
                () => sqlCommand.Parameters[0].ParameterName.ShouldBe(ParameterSecurityGroupID),
                () => sqlCommand.Parameters[0].Value.ShouldBe(SecurityGroupId));
        }

        [Test]
        public void SelectForUser_ValidParameter_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.SelectForUser(UserId);

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_Select_UserID"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(1),
                () => sqlCommand.Parameters[0].ParameterName.ShouldBe(ParameterUserID),
                () => sqlCommand.Parameters[0].Value.ShouldBe(UserId));
        }

        [Test]
        public void Select_ValidParameter_ProperlySetupSqlCommand()
        {
            // Arrange
            SqlCommand sqlCommand = null;
            ShimApplication.GetListSqlCommand = (commandParameter) =>
            {
                sqlCommand = commandParameter;
                return null;
            };

            // Act
            Application.Select();

            // Assert
            sqlCommand.ShouldSatisfyAllConditions(
                () => sqlCommand.ShouldNotBeNull(),
                () => sqlCommand.CommandText.ShouldBe("e_Application_Select"),
                () => sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => sqlCommand.Parameters.ShouldNotBeNull(),
                () => sqlCommand.Parameters.Count.ShouldBe(0));
        }
    }
}
