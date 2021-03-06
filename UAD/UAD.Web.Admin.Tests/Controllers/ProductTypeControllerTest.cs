﻿using System;
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
    public class ProductTypeControllerTest
    {
        private IDisposable _shimObject;
        private ProductTypeController _controller;
        private bool? _wasDeleteCalled;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _controller = new ProductTypeController();
            _wasDeleteCalled = null;
            ShimProductTypes.AllInstances.DeleteClientConnectionsInt32 =
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
            result.Data.ToString().ShouldContain("Product Type has been deleted successfully.");
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
            ShimSubscriptionsExtensionMapper.AllInstances.DeleteInt32ClientConnections = (_, __, ___) => { };
            const string ExpectedErrorMessage = "Error Message";
            ShimProductTypes.AllInstances.SaveProductTypesClientConnections = (_, __, ___) =>
                    throw new UADException(new List<UADError>() { new UADError() { ErrorMessage = ExpectedErrorMessage } });

            var model = new Models.ProductTypes ()
            {
                 PubTypeID = 1
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
