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
    public class CodeSheetControllerTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
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
            var controller = new CodeSheetController();
            var deleteCalled = false;
            ShimCodeSheet.AllInstances.DeleteClientConnectionsInt32 =
                (_, __, ___) => deleteCalled = true;
            ShimBaseController.AllInstances.ClientConnectionsGet = (_) => new ClientConnections();

            // Act
            controller.Delete(1);

            // Assert
            deleteCalled.ShouldBeTrue();
        }

        [Test]
        public void Delete_WhenIdProvided_ReturnsMessage()
        {
            // Arrange
            var controller = new CodeSheetController();
            ShimCodeSheet.AllInstances.DeleteClientConnectionsInt32 =  (_, __, ___) => true;
            ShimBaseController.AllInstances.ClientConnectionsGet = (_) => new ClientConnections();

            // Act
            var result = controller.Delete(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain("Code Sheet has been deleted successfully"));
        }

        [Test]
        public void Delete_WhenDeletMethodThrowsUADException_ReturnsErrorMessage()
        {
            // Arrange
            var controller = new CodeSheetController();
            const string ExpectedErrorMessage = "Error Message";
            ShimCodeSheet.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => true;
            ShimBaseController.AllInstances.ClientConnectionsGet = (_) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } } );

            // Act
            var result = controller.Delete(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.ToString().ShouldContain(ExpectedErrorMessage));
        }
    }
}
