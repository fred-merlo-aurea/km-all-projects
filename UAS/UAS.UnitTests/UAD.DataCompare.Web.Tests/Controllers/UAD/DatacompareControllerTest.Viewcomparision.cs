using System.Collections.Generic;
using System.Web.Mvc;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using KM.Platform.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using KMPlatform.Object;
using Shouldly;
using UAD.DataCompare.Web.Models;
using UASEntity = FrameworkUAS.Entity;
using UADEntity = FrameworkUAD_Lookup.Entity;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    public partial class DatacompareControllerTest
    {
        private const string SampleBrandName = "SampleBrandName";
        private const string SamplePubName = "SamplePubName";
        private const string CodeNameBrand = "Brand";
        private const string CodeNameProduct = "Product";
        private const string CodeNameOther = "Other";
        private const string RedirectMainUrl = "/ecn.accounts/main/";
        private const string SomeOtherFileName = "SomeOtherFileName";

        [Test]
        public void Viewcomparision_WhenCodeNameIsBrand_ReturnsViewWithModel()
        {
            // Arrange
            SetFakesForViewComparision();

            // Act
            var result = _controller.Viewcomparision(SourceFileID: 1, targetFilter: 1, scopeFilter: 1, typeFilter: 1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () =>
                {
                    var viewResult = result.ShouldBeOfType<ViewResult>();
                    viewResult.ShouldSatisfyAllConditions(
                        () =>
                        {
                            var model = viewResult.Model.ShouldBeOfType<List<ViewComparisionViewModel>>();
                            model.Count.ShouldBe(1);
                            model[0].FileName.ShouldBe(SampleFileName);
                            model[0].Scope.ShouldBe($"{CodeNameBrand}: {SampleBrandName}");
                            model[0].Query.ShouldContain(CodeNameBrand);
                            model[0].Target.ShouldBe(CodeNameBrand);
                        });
                });
        }

        [Test]
        public void Viewcomparision_WhenCodeNameIsProduct_ReturnsViewWithModel()
        {
            // Arrange
            SetFakesForViewComparision();
            ShimCode.AllInstances.Select = (c) => new List<UADEntity.Code>
            {
                new UADEntity.Code
                {
                    CodeId = 1 ,
                    CodeName = CodeNameProduct,
                }
            };

            // Act
            var result = _controller.Viewcomparision(SourceFileID: 1, targetFilter: 1, scopeFilter: 1, typeFilter: 1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () =>
                {
                    var viewResult = result.ShouldBeOfType<ViewResult>();
                    viewResult.ShouldSatisfyAllConditions(
                        () =>
                        {
                                var model = viewResult.Model.ShouldBeOfType<List<ViewComparisionViewModel>>();
                                model.Count.ShouldBe(1);
                                model[0].FileName.ShouldBe(SampleFileName);
                                model[0].Scope.ShouldBe($"{CodeNameProduct}: {SamplePubName}");
                                model[0].Query.ShouldContain(CodeNameProduct);
                                model[0].Target.ShouldBe(CodeNameProduct);
                        });
                });
        }

        [Test]
        public void Viewcomparision_WhenCodeNameIsOther_ReturnsViewWithModel()
        {
            // Arrange
            SetFakesForViewComparision();
            ShimCode.AllInstances.Select = (c) => new List<UADEntity.Code>
            {
                new UADEntity.Code
                {
                    CodeId = 1 ,
                    CodeName = CodeNameOther,
                }
            };

            // Act
            var result = _controller.Viewcomparision(SourceFileID: 1, targetFilter: 1, scopeFilter: 1, typeFilter: 1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () =>
                    {
                        var viewResult = result.ShouldBeOfType<ViewResult>();
                        viewResult.ShouldSatisfyAllConditions(
                            () => 
                            {
                                    var model = viewResult.Model.ShouldBeOfType<List<ViewComparisionViewModel>>();
                                    model.Count.ShouldBe(1);
                                    model[0].FileName.ShouldBe(SampleFileName);
                                    model[0].Scope.ShouldBe(CodeNameOther);
                                    model[0].Query.ShouldContain(CodeNameOther);
                                    model[0].Target.ShouldBe(CodeNameOther);
                            });
                    });
        }

        [Test]
        public void Viewcomparision_WhenUserHasNoAccess_RedirectsToMainView()
        {
            // Arrange
            SetFakesForViewComparision();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, eu, ed, ey) => false;

            // Act
            var result = _controller.Viewcomparision(SourceFileID: 1, targetFilter: 1, scopeFilter: 1, typeFilter: 1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () =>
                    {
                        var redirectResult = result.ShouldBeOfType<RedirectResult>();
                        redirectResult.Url.ShouldContain(RedirectMainUrl);
                    });
        }

        [Test]
        public void Viewcomparision_WhenClientIdDiffersAndNotVaild_RedirectsToMainView()
        {
            // Arrange
            SetFakesForViewComparision();
            ShimSourceFile.AllInstances.SelectSourceFileIDInt32Boolean = (s, sid, b) => new UASEntity.SourceFile
            {
                SourceFileID = 1,
                FileName = SampleFileName,
                ClientID = 2,
            };

            // Act
            var result = _controller.Viewcomparision(SourceFileID: 1, targetFilter: 1, scopeFilter: 1, typeFilter: 1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () =>
                    {
                        var redirectResult = result.ShouldBeOfType<RedirectResult>();
                        redirectResult.Url.ShouldContain(RedirectMainUrl);
                    });
        }

        private void SetFakesForViewComparision(string fileName = SomeOtherFileName)
        {
            InitializeSessionFakes();
            ShimDataCompareDownloadView.AllInstances.SelectForClientInt32 = (dc, c) => new List<UASEntity.DataCompareDownloadView>
            {
                new UASEntity.DataCompareDownloadView
                {
                    ClientId = 1,
                    DcTargetCodeId = 1,
                    DcTypeCodeId = 1,
                    DcTargetIdUad = 1,
                }
            };
            ShimCode.AllInstances.Select = (c) => new List<UADEntity.Code>
            {
                new UADEntity.Code
                {
                    CodeId = 1 ,
                    CodeName = CodeNameBrand,
                }
            };
            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectInt32Boolean = (c, cid, b) => new Client
            {
                ClientID = 1,
                ClientConnections = new ClientConnections { }
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, eu, ed, ey) => true;

            ShimSourceFile.AllInstances.SelectSourceFileIDInt32Boolean = (s, sid, b) => new UASEntity.SourceFile
            {
                SourceFileID = 1,
                FileName = SampleFileName,
                ClientID = 1,
            };
            ShimSourceFile.AllInstances.SelectBoolean = (s, b) => new List<UASEntity.SourceFile>
            {
                new UASEntity.SourceFile
                {
                    SourceFileID = 1,
                    FileName = fileName,
                    ClientID = 1,
                    IsDeleted = false,
                }
            };
            ShimBrand.AllInstances.SelectClientConnections = (b, c) => new List<FrameworkUAD.Entity.Brand>
            {
                new FrameworkUAD.Entity.Brand { BrandID = 1, BrandName = SampleBrandName }
            };
            ShimProduct.AllInstances.SelectClientConnectionsBoolean = (p, c, b) => new List<FrameworkUAD.Entity.Product>
            {
                new FrameworkUAD.Entity.Product { PubID = 1, PubName = SamplePubName }
            };
        }
    }
}
