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

namespace UAD.Web.Admin.Tests.Controllers
{
    [TestFixture]
    public class MasterGroupControllerTest
    {
        private IDisposable _shimObject;
        private MasterGroupController _controller;
        private bool? _wasDeleteCalled;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _controller = new MasterGroupController();
            _wasDeleteCalled = null;
            ShimMasterGroup.AllInstances.DeleteInt32ClientConnections =
                (_, __, ___) =>
                {
                    _wasDeleteCalled = true;
                    return true;
                };

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
            // Act
            _controller.Delete(1);

            // Assert
            _wasDeleteCalled.ShouldBe(true);
        }

        [Test]
        public void Delete_WhenIdProvided_ReturnsMessage()
        {
            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Data.ToString().ShouldContain("Master Group has been deleted successfully.");
        }

        [Test]
        public void Delete_WhenDeletMethodThrowsUADException_ReturnsErorrMessage()
        {
            // Arrange
            const string ExpectedErrorMessage = "Error Message";
            ShimBaseController.AllInstances.ClientConnectionsGet = (_) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Data.ToString().ShouldContain(ExpectedErrorMessage);
        }

        [Test]
        public void AddEdit_WhenFailsWihtUADException_ReturnsErromMessage()
        {
            const string ExpectedErrorMessage = "Error Message";
            ShimMasterGroup.AllInstances.SaveMasterGroupClientConnections = (_, __, ___) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            var model = new Models.MasterGroups()
            {
                MasterGroupID = 1
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
