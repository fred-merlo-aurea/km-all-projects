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
    public class MasterCodeSheetControllerTest
    {
        private IDisposable _shimObject;
        private MasterCodeSheetController _controller;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _controller = new MasterCodeSheetController();

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
            ShimMasterCodeSheet.AllInstances.DeleteMasterIDClientConnectionsInt32 =
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
            ShimMasterCodeSheet.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, ___) => true;

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain("Master Code Sheet has been deleted successfully."));
        }

        [Test]
        public void Delete_WhenDeletMethodThrowsUADException_ReturnsErrorMessage()
        {
            // Arrange
            ShimMasterCodeSheet.AllInstances.DeleteMasterIDClientConnectionsInt32 = (_, __, ___) => true;
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
            ShimMasterCodeSheet.AllInstances.SaveMasterCodeSheetClientConnections = (_, __, ___) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            var model = new Models.MasterCodeSheets()
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
