using System;
using System.Collections.Generic;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Object;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAD.Web.Admin.Controllers;
using UAD.Web.Admin.Controllers.Common.Fakes;
using UAD.Web.Admin.Controllers.Fakes;

namespace UAD.Web.Admin.Tests.Controllers
{
    [TestFixture]
    public class CustomFieldControllerTest
    {
        private IDisposable _shimObject;
        private CustomFieldController _controller;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _controller = new CustomFieldController();

            ShimBaseController.AllInstances.ClientConnectionsGet = (_) => new ClientConnections();
            ShimBaseController.AllInstances.CurrentUserGet = (_) => new KMPlatform.Entity.User();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Delete_WhenIdProvided_CallsDeleteMethodForCodeSheet()
        {
            // Arrange
            var deleteCalled = false;
            ShimSubscriptionsExtensionMapper.AllInstances.DeleteInt32ClientConnections  =
                (_, __, ___) => deleteCalled = true;

            // Act
            _controller.Delete(1);

            // Assert
            deleteCalled.ShouldBeTrue();
        }

        [Test]
        public void Delete_WhenIdProvided_ReturnsMessage()
        {
            // Arrange
            ShimSubscriptionsExtensionMapper.AllInstances.DeleteInt32ClientConnections = (_, __, ___) => { };

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain("Custom Field has been deleted successfully."));
        }

        [Test]
        public void Delete_WhenDeletMethodThrowsUADException_ReturnsErrorMessage()
        {
            // Arrange
            ShimSubscriptionsExtensionMapper.AllInstances.DeleteInt32ClientConnections = (_, __, ___) => { };
            const string ExpectedErrorMessage = "Error Message";
            ShimBaseController.AllInstances.ClientConnectionsGet = (_) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain(ExpectedErrorMessage));
        }
        [Test]
        public void AddEdit_WhenFailsWihtUADException_ReturnsErromMessage()
        {
            const string ExpectedErrorMessage = "Error Message";
            ShimSubscriptionsExtensionMapper.AllInstances.SaveSubscriptionsExtensionMapperClientConnections = (_,__,___)=>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            var model = new Models.CustomFields()
            {
                SubscriptionsExtensionMapperID = 1
            };

            // Act
            var result = _controller.AddEdit(model);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain(ExpectedErrorMessage),
                () => result.Data.ToString().ShouldContain("Success\":false"),
                () => result.JsonRequestBehavior.ShouldBe(System.Web.Mvc.JsonRequestBehavior.AllowGet));
        }
    }
}
