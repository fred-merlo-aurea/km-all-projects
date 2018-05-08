using System;
using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAD.Web.Admin.Controllers;
using UAD.Web.Admin.Controllers.Fakes;
using UAD.Web.Admin.Models;
using BLFakes = ECN_Framework_BusinessLayer.Application.Fakes;

namespace UAD.Web.Admin.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTest
    {
        private IDisposable _shimObject;
        private ProductController _controller;
        private bool? _isDeleteByPubIDCalled;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _controller = new ProductController();
            _isDeleteByPubIDCalled = null;

            var session = new BLFakes.ShimECNSession().Instance;
            session.CurrentUser = new KMPlatform.Entity.User();
            BLFakes.ShimECNSession.CurrentSession = () => session;
            ShimProductController.AllInstances.ClientConnectionsGet = (_) => new KMPlatform.Object.ClientConnections();
            ShimProductGroup.AllInstances.DeleteClientConnectionsInt32 = (_, __, ___) => true;
            ShimBrandDetails.DeleteByPubIDClientConnectionsInt32 = (_, __) => { _isDeleteByPubIDCalled = true; };
            ShimProductController.AllInstances.LoadGroupsInt32 = (_, __) => new List<Group> { new Group() };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Edit_WhenProductSaved_SavesBrandDetails()
        {
            // Arrange
            var productID = 100;
            var selectedBrandId = 200;

            var productWrapper = new ProductWrapper()
            {
                pub = new Product() { IsActive = true },
                selectedGroupsList = new string[0],
                selectedBrandsList = new string[] { selectedBrandId.ToString() }
            };

            KMPS.MD.Objects.BrandDetails savedBrandDetails = null;
            ShimProduct.AllInstances.SaveProductClientConnections = (_, __, ___) => productID;
            ShimBrandDetails.SaveClientConnectionsBrandDetails = (cconnections, brandDetail) => { savedBrandDetails = brandDetail; return 1; };

            // Act
            _controller.Edit(productWrapper);

            // Assert
            savedBrandDetails.ShouldSatisfyAllConditions(
                () => savedBrandDetails.ShouldNotBeNull(),
                () => savedBrandDetails.BrandID.ShouldBe(selectedBrandId),
                () => savedBrandDetails.PubID.ShouldBe(productID),
                () => _isDeleteByPubIDCalled.ShouldBe(true));
        }

        [Test]
        public void Add_WhenProductAddedSuccessefully_SavesBrandDetails()
        {
            // Arrange
            var productID = 100;
            var selectedBrandId = 200;

            var productWrapper = new ProductWrapper()
            {
                pub = new Product() { IsActive = true },
                selectedGroupsList = new string[0],
                selectedBrandsList = new string[] { selectedBrandId.ToString() }
            };

            KMPS.MD.Objects.BrandDetails savedBrandDetails = null;
            ShimProduct.AllInstances.SaveProductClientConnections = (_, __, ___) => productID;
            ShimBrandDetails.SaveClientConnectionsBrandDetails = (cconnections, brandDetail) => { savedBrandDetails = brandDetail; return 1; };

            // Act
            _controller.Add(productWrapper);

            // Assert
            savedBrandDetails.ShouldSatisfyAllConditions(
                () => savedBrandDetails.ShouldNotBeNull(),
                () => savedBrandDetails.BrandID.ShouldBe(selectedBrandId),
                () => savedBrandDetails.PubID.ShouldBe(productID),
                () => _isDeleteByPubIDCalled.ShouldBe(true));
        }
    }
}



