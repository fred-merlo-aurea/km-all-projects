using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAD.DataCompare.Web.Controllers.UAD;
using UAD.DataCompare.Web.Controllers.UAD.Fakes;
using UAD.DataCompare.Web.Models;
using static FrameworkUAD.BusinessLogic.Enums;
using Product = FrameworkUAD.Entity.Product;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class DataComparerControllerTestLoadFilterDetails
    {
        private const string Separator = ",";
        private const string ViewName = "~/Views/DataCompare/Partials/ViewFilters/_FilterDetailView.cshtml";
        private const int DcDownloadId = 10;
        private const int DcTargetCodeId = 20;
        private const string Product = "Product";
        private const int DefaultId = 123;
        private IDisposable _shimsContext;
        private DatacompareController _controller;
        private PrivateObject _controllerPrivate;
        private string _targetCodeName;
        private string _dataCompareDownloadFilterDetailValue;
        private List<DataCompareDownloadFilterDetail> _dcFilterDetails;
        private Brand _brand;
        private Product _product;
        private MasterCodeSheet _materCodeSheet;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _brand = new Brand();
            _product = new Product();
            _materCodeSheet = new MasterCodeSheet();
            _targetCodeName = "Any Dummy String";
            _dataCompareDownloadFilterDetailValue = string.Empty;
            _dcFilterDetails = new List<DataCompareDownloadFilterDetail>();
            _shimsContext = ShimsContext.Create();
            CommonShims();
            _controller = new DatacompareController();
            _controllerPrivate = new PrivateObject(_controller);
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void LoadFilterDetails_FilterTypeBrand_ShouldReturnRightFiler()
        {
            //Arrange
            _brand.BrandID = GetNumber();
            _brand.BrandName = GetString();
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Brand,
                Values = _brand.BrandID.ToString(),
                Name = fieldName
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.ShouldSatisfyAllConditions(
                () => filterDetails.Field.ShouldBe(fieldName),
                () => filterDetails.Values.ShouldBe(_brand.BrandName));
        }

        [Test]
        public void LoadFilterDetails_FilterTypeProduct_ShouldReturnRightFilter()
        {
            //Arrange
            _product.PubID = GetNumber();
            _product.PubName = GetString();
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Product,
                Values = _product.PubID.ToString(),
                Name = fieldName
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.ShouldSatisfyAllConditions(
                () => filterDetails.Field.ShouldBe(fieldName),
                () => filterDetails.Values.ShouldBe(_product.PubName));
        }

        [Test]
        public void LoadFilterDetails_FilterTypeDimension_ShouldReturnRightFilter()
        {
            //Arrange
            var description = GetString();
            var masterValue = GetString();
            _materCodeSheet.MasterID = GetNumber();
            _materCodeSheet.MasterDesc = description;
            _materCodeSheet.MasterValue = masterValue;
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Dimension,
                Values = _materCodeSheet.MasterID.ToString(),
                Name = fieldName
            });
            var expectedValue = $"{description} ({masterValue})";
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.ShouldSatisfyAllConditions(
                () => filterDetails.Field.ShouldBe(fieldName),
                () => filterDetails.Values.ShouldBe(expectedValue));
        }

        [Test]
        public void LoadFilterDetails_FilterTypeDimensionWithTargetProduct_ShouldReturnRightFilter()
        {
            //Arrange
            _targetCodeName = Product;
            var description = GetString();
            var masterValue = GetString();
            _materCodeSheet.MasterID = GetNumber();
            _materCodeSheet.MasterDesc = description;
            _materCodeSheet.MasterValue = masterValue;
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Dimension,
                Values = _materCodeSheet.MasterID.ToString(),
                Name = fieldName
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.Field.ShouldBe(fieldName);
        }

        [Test]
        [TestCaseSource(nameof(StandardFieldNameCases))]
        public void LoadFilterDetails_FilterTypeStandard_ShouldReturnRightFilter(string fieldName, string value)
        {
            //Arrange
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Standard,
                Values = value,
                Name = fieldName
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.Field.ShouldBe(fieldName);
        }

        [Test]
        public void LoadFilterDetails_FilterTypeGeo_ShouldReturnRightFilter()
        {
            //Arrange
            var fieldName = GetString();
            var firstValue = GetNumber();
            var secondValue = GetNumber();
            var thirdValue = GetNumber();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Geo,
                Values = _materCodeSheet.MasterID.ToString(),
                Name = fieldName,
                SearchCondition = $"{firstValue}|{secondValue}|{thirdValue}"
            });
            var expectedValue = $"{firstValue} & {secondValue} miles - {thirdValue} miles";
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.ShouldSatisfyAllConditions(
                () => filterDetails.Field.ShouldBe(fieldName),
                () => filterDetails.Values.ShouldBe(expectedValue));
        }

        [Test]
        [TestCaseSource(nameof(ActivityFieldsCases))]
        public void LoadFilterDetails_FilterTypeActivity_ShouldReturnRightFilter(string fieldName, string value)
        {
            //Arrange            
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Activity,
                Values = value,
                Name = fieldName,
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.Field.ShouldBe(fieldName);
        }

        [Test]
        [TestCaseSource(nameof(AdhocFieldsCases))]
        public void LoadFilterDetails_FilterTypeAdhoc_ShouldReturnRightFilter(string group, string target)
        {
            //Arrange      
            _targetCodeName = target;
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Adhoc,
                Group = group,
                Name = fieldName,
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.Field.ShouldBe(fieldName);
        }

        [Test]
        public void LoadFilterDetails_FilterTypeCirculation_ShouldReturnRightFilter()
        {
            //Arrange
            var fieldName = GetString();
            _dcFilterDetails.Add(new DataCompareDownloadFilterDetail
            {
                FilterType = (int)FiltersType.Circulation,
                Name = fieldName,
            });
            //Act
            var model = CallLoadFilterDetailsAndGetModel();
            //Assert
            model.ShouldHaveSingleItem();
            var filterDetails = model.Single();
            filterDetails.ShouldNotBeNull();
            filterDetails.Field.ShouldBe(fieldName);
        }

        private static IEnumerable<string[]> AdhocFieldsCases()
        {
            const string AnyString = "{06D45483-FC27-4D47-B7BF-9423DE64690D}";
            var targets = new[] { Product, AnyString };
            var options = new[] { string.Empty, $"|{AnyString}" };
            var result = new List<string[]>
            {
                new[] { $"m|{DefaultId}", AnyString}
            };
            var productGroups = new string[][]
            {
                new [] { "e", string.Empty , "d" },
                new [] { "e", string.Empty , "b" },
                new [] { "e", string.Empty , "i" },
                new [] { "e", string.Empty , "f" },
                new [] { "e", string.Empty , "z" }
            };
            var productGroupsAndTargets = productGroups
                .SelectMany(groups =>
                {
                    return targets.Select(target => new[] { string.Join("|", groups), target });
                });
            result.AddRange(productGroupsAndTargets);
            var optionalSecondItemGroups = new string[]
            {
                "d",
                "b",
                "i",
                "f",
                "z"
            };
            var optionalGroups = optionalSecondItemGroups
                .SelectMany(group =>
                {
                    return options.Select(option => new[] { $"{group}{option}", AnyString });
                });
            result.AddRange(optionalGroups);
            return result;
        }

        private static IEnumerable<string[]> ActivityFieldsCases()
        {
            var defaultValue = DefaultId.ToString();
            var numbers = new[] { 0, 1, 2, 3, 4, 5, 5, 10, 15, 20, 30 };
            var numbersFieldNames = new[]
            {
                "OPEN CRITERIA",
                "CLICK CRITERIA",
                "VISIT CRITERIA",
            };
            var result = new List<string[]>
            {
                new [] { "OPEN ACTIVITY", defaultValue },
                new [] { "OPEN BLASTID", defaultValue },
                new [] { "OPEN CAMPAIGNS", defaultValue },
                new [] {"OPEN EMAIL SUBJECT", defaultValue },
                new [] {"OPEN EMAIL SENT DATE", defaultValue },
                new [] {"LINK", defaultValue },
                new [] {"CLICK ACTIVITY", defaultValue },
                new [] {"CLICK BLASTID", defaultValue },
                new [] {"CLICK CAMPAIGNS", defaultValue },
                new [] {"CLICK EMAIL SUBJECT", defaultValue },
                new [] {"CLICK EMAIL SENT DATE", defaultValue },
                new [] {"DOMAIN TRACKING", defaultValue },
                new [] {"URL", defaultValue },
                new [] {"VISIT ACTIVITY", defaultValue },
            };
            var numberFieldsAndValues = numbersFieldNames
                    .SelectMany(field => numbers.Select(number => new[] { field, number.ToString() }));
            result.AddRange(numberFieldsAndValues);
            return result;
        }

        private static IEnumerable<string[]> StandardFieldNameCases()
        {
            var defaultValue = $"{DefaultId}";
            var booleanValues = "0,1,_";
            var booleanFileds = new[]
            {
                "EMAIL",
                "PHONE",
                "FAX",
                "MAILPERMISSION",
                "FAXPERMISSION",
                "PHONEPERMISSION",
                "OTHERPRODUCTSPERMISSION",
                "THIRDPARTYPERMISSION",
                "EMAILRENEWPERMISSION",
                "TEXTPERMISSION",
                "GEOLOCATED",
                "EMAIL STATUS"
            };
            var result = new List<string[]>
            {
                new []{ "STATE", defaultValue },
                new []{ "COUNTRY", defaultValue },
                new []{ "MEDIA", "A,B,O,C" },
            };
            result.AddRange(booleanFileds.Select(field => new[] { field, booleanValues }));
            return result.ToList();
        }

        private List<FilterDetailsModel> CallLoadFilterDetailsAndGetModel()
        {
            var result = _controller.LoadFilterDetails(DcDownloadId) as PartialViewResult;
            result.ShouldNotBeNull();
            result.ViewName.ShouldBe(ViewName);
            var model = result.Model as List<FilterDetailsModel>;
            model.ShouldNotBeNull();
            return model;
        }

        private void CommonShims()
        {
            ShimClient.AllInstances.SelectInt32Boolean = (instance, clientId, includeObjects) =>
            {
                return new Client
                {
                    ClientConnections = new ClientConnections()
                };
            };
            ShimDataCompareDownloadView.AllInstances.SelectForClientInt32 = (instance, clinetId) =>
            {
                return new List<DataCompareDownloadView>
                {
                    new DataCompareDownloadView
                    {
                        DcDownloadId = DcDownloadId,
                        DcTargetCodeId  = DcTargetCodeId
                    }
                };
            };
            ShimCode.AllInstances.Select = instance =>
            {
                return new List<Code>
                {
                    new Code
                    {
                        CodeId = DcTargetCodeId,
                        CodeName = _targetCodeName
                    }
                };
            };
            ShimMasterCodeSheet.AllInstances.SelectClientConnections = (instance, clientConnections) =>
            {
                return new List<MasterCodeSheet>
                {
                    _materCodeSheet
                };
            };
            ShimDataCompareDownloadFilterGroup.AllInstances.SelectForDownloadInt32 = (instance, dcDownloadId) =>
            {
                return new List<DataCompareDownloadFilterGroup>
                {
                    new DataCompareDownloadFilterGroup
                    {
                        DcFilterDetails = _dcFilterDetails
                    }
                };
            };
            ShimDatacompareController.AllInstances.CurrentClientIDGet = instance =>
            {
                return 100;
            };
            ShimBrand.AllInstances.SelectClientConnections = (instance, clientConnections) =>
            {
                return new List<Brand>
                {
                    _brand
                };
            };
            ShimProduct.AllInstances.SelectClientConnectionsBoolean =
                (instance, clientConnections, inlcudeProperties) =>
                {
                    return new List<Product>
                    {
                        _product
                    };
                };
            ShimCountry.AllInstances.Select = instance => new List<Country>
            {
                new Country
                {
                    CountryID = DefaultId,
                    ShortName = GetString()
                }
            };
            ShimRegion.AllInstances.Select = instacne => new List<Region>
            {
                new Region
                {
                    RegionCode = DefaultId.ToString()
                }
            };
            ShimECNCampaign.AllInstances.SelectClientConnections = (instance, clientConnections) => new List<ECNCampaign>
            {
                new ECNCampaign
                {
                    ECNCampaignID = DefaultId,
                    ECNCampaignName =GetString()
                }
            };
            ShimProductSubscriptionsExtensionMapper.AllInstances.SelectAllClientConnections =
                (instance, clientConnections) => new List<ProductSubscriptionsExtensionMapper>
                {
                    new ProductSubscriptionsExtensionMapper
                    {
                        StandardField = DefaultId.ToString(),
                        PubID = DefaultId
                    }
                };
            ShimSubscriptionsExtensionMapper.AllInstances.SelectAllClientConnections = (instance, clientConnections) =>
                new List<SubscriptionsExtensionMapper>
                {
                    new SubscriptionsExtensionMapper
                    {
                        StandardField = DefaultId.ToString(),
                        CustomField = GetString()
                    }
                };
            ShimMasterGroup.AllInstances.SelectClientConnections = (instance, clientConnections) =>
                new List<MasterGroup>
                {
                    new MasterGroup
                    {
                        MasterGroupID = DefaultId,
                        DisplayName = GetString()
                    }
                };
            ShimDomainTracking.AllInstances.SelectClientConnections = (instance, clientConnections) =>
                new List<DomainTracking>
                {
                    new DomainTracking
                    {
                        DomainTrackingID = DefaultId
                    }
                };
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _controllerPrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
