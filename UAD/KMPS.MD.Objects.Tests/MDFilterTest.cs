using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using KmFake = KMPS.MD.Objects.Fakes;

namespace KMPS.MD.Objects.Tests
{
    /// <summary>
    /// Unit test for <see cref="MDFilter"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class MDFilterTest
    {
        private const string GetTextValues = "GetTextValues";
        private const string OpenCriteria = "OPEN CRITERIA";
        private const string ClickCriteria = "CLICK CRITERIA";
        private const string VisitCriteria = "VISIT CRITERIA";
        private const string Adhoc = "Adhoc";
        private const string SampleText = "A";
        private MDFilter _mDFilter;
        private PrivateObject _privateObject;
        private PrivateType _privateType;
        private IDisposable _shimsContext;

        [SetUp]
        public void Setup()
        {
            _mDFilter = new MDFilter();
            _privateObject = new PrivateObject(_mDFilter);
            _privateType = new PrivateType(typeof(MDFilter));
            _shimsContext = ShimsContext.Create();
            CreatePageFakeObject();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [TestCase(Enums.ViewType.None)]
        [TestCase(Enums.ViewType.ProductView)]
        public void GetTextValues_FiltersObjectIsNotNullAndClientConnectionsIsNotNull_ReturnsFilterObjectValue(Enums.ViewType viewType)
        {
            // Arrange
            var clientconnection = new KMPlatform.Object.ClientConnections();
            var filters = CreateFiltersObject(viewType, clientconnection);
            var parameters = new object[] { clientconnection, filters };

            // Act
            var resultFilters = _privateType.InvokeStatic(GetTextValues, parameters) as Filters;

            // Assert
            resultFilters.ShouldSatisfyAllConditions(
                () => resultFilters.ShouldNotBeNull(),
                () => resultFilters.AllInterSect.ShouldBe(0),
                () => resultFilters.AllUnion.ShouldBe(0),
                () => resultFilters.Count.ShouldBe(1),
                () => resultFilters.FilterComboList.ShouldBeNull(),
                () => resultFilters.FilterVennList.ShouldBeNull(),
                () => resultFilters.IsReadOnly.ShouldBeFalse()
            );
        }

        private Filters CreateFiltersObject(Enums.ViewType viewType, KMPlatform.Object.ClientConnections clientconnection)
        {
            var filters = new Filters(clientconnection, 1);
            var filter = new Filter();
            var fieldList = new List<Field> { };
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Product, Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Dimension, Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "OPEN CAMPAIGNS", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "CLICK CAMPAIGNS", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "CLICK EMAIL SUBJECT", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "DOMAIN TRACKING", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "VISIT ACTIVITY", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "0" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "2" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "3" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "4" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "5" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "10" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "15" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "20" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = OpenCriteria, Values = "30" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "0" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "2" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "3" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "4" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "5" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "10" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "15" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "20" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = ClickCriteria, Values = "30" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "0" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "2" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "3" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "4" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "5" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "10" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "15" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "20" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = VisitCriteria, Values = "30" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "OPEN BLASTID", Values = "30" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Activity, Name = "CLICK BLASTID", Values = "30" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "CATEGORY TYPE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "CATEGORY CODE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "XACT", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "TRANSACTION CODE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "QSOURCE TYPE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "QSOURCE CODE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "MEDIA", Values = "A,B,O,C" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "QFROM", Values = "A,B,O,C" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "WAVE MAILING", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Circulation, Name = "WAVE MAILING", Values = "0" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Brand, Name = "Brand", Values = "1" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Standard, Name = "COUNTRY", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Standard, Name = "STATE", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Standard, Name = "MEDIA", Values = "A,B,O,C" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Standard, Name = "EMAIL STATUS", Values = "1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Standard, Name = "GEOLOCATED", Values = "1,2,3,0" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Geo, Name = "Geo", SearchCondition = "A|B|C" });

            fieldList.Add(new Field { FilterType = Enums.FiltersType.Adhoc, Name = Adhoc, Group = "m|1" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Adhoc, Name = Adhoc, Group = "e|1|d" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Adhoc, Name = Adhoc, Group = "e|1|b" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Adhoc, Name = Adhoc, Group = "d|[UnitTest]" });
            fieldList.Add(new Field { FilterType = Enums.FiltersType.Adhoc, Name = Adhoc, Group = "b" });
            filter.Fields = fieldList;
            filter.ViewType = viewType;
            filters.Add(filter);
            return filters;
        }

        private void CreatePageFakeObject()
        {
            ShimPubs.GetActiveClientConnections = (x) => { return new List<Pubs> { new Pubs { PubID = 1, PubName = "Unit Test" } }; };
            ShimMasterCodeSheet.GetAllClientConnections = (x) =>
            {
                return new List<MasterCodeSheet>
                {
                    new MasterCodeSheet
                    {
                         MasterID = 1,
                         MasterDesc = SampleText,
                         MasterValue = SampleText
                    }
                };
            };
            KmFake.ShimCountry.GetAll = () =>
            {
                return new List<Country>
                {
                    new Country
                    {
                        CountryID = 1,
                        ShortName = SampleText
                    }
                };

            };
            KmFake.ShimRegion.GetAll = () =>
            {
                return new List<Region>
                {
                    new Region
                    {
                        RegionCode= "1",
                         RegionID=1
                    }
                };
            };
            KmFake.ShimCategory.GetAll = () =>
            {
                return new List<Category>
                {
                    new Category
                    {
                        CategoryCodeID = 1,
                        CategoryCodeName = SampleText,
                        CategoryCodeTypeID =1,
                        IsActive= true
                    }
                };
            };
            KmFake.ShimTransactionCodeType.GetAll = () =>
            {
                return new List<TransactionCodeType>
                {
                    new TransactionCodeType
                    {
                        TransactionCodeTypeID = 1,
                        TransactionCodeTypeName = SampleText
                    }
                };
            };
            KmFake.ShimCode.GetAll = () =>
            {
                return new List<Code>
                {
                     new Code
                     {
                        CodeID=1,
                        CodeName= SampleText,
                        CodeValue = SampleText,
                        DisplayName= SampleText
                     }
                };
            };
            KmFake.ShimEmailStatus.GetAllClientConnections = (x) =>
            {
                return new List<EmailStatus>
                {
                    new EmailStatus
                    {
                        EmailStatusID=1,
                        Status="Success"
                    }
                };
            };
            ShimECNCampaign.GetAllClientConnections = (x) =>
            {
                return new List<ECNCampaign>
                {
                    new ECNCampaign
                    {
                         ECNCampaignID = 1,
                         ECNCampaignName = SampleText
                    }
                };
            };
            ShimCategoryCodeType.AllInstances.Select = (x) =>
            {
                return new List<CategoryCodeType>
                {
                    new CategoryCodeType
                    {
                         CategoryCodeTypeID = 1,
                         CategoryCodeTypeName = SampleText
                    }
                };
            };
            ShimTransactionCode.AllInstances.Select = (x) =>
            {
                return new List<TransactionCode>
                {
                     new TransactionCode
                     {
                         TransactionCodeID = 1,
                         TransactionCodeName = SampleText
                     }
                };
            };

            ShimCodeSheet.GetByPubIDClientConnectionsInt32 = (x, y) =>
            {
                return new List<CodeSheet>
                {
                    new CodeSheet
                    {
                       CodeSheetID = 1,
                       ResponseDesc = SampleText,
                       ResponseValue = SampleText
                    }
                };
            };
            ShimBrand.GetAllClientConnections = (x) =>
            {
                return new List<Brand>
                {
                    new Brand
                    {
                        BrandID = 1,
                        BrandName="Unit Test"
                    }
                };
            };
            ShimDomainTracking.GetClientConnections = (x) =>
            {
                return new List<DomainTracking>
                {
                     new DomainTracking
                     {
                          DomainName= SampleText,
                           DomainTrackingID = 1
                     }
                };
            };
            ShimMasterGroup.GetAllClientConnections = (x) =>
            {
                return new List<MasterGroup>
                {
                    new MasterGroup
                    {
                         MasterGroupID = 1,
                         DisplayName = SampleText
                    }
                };
            };

            ShimPubSubscriptionsExtensionMapper.GetAllClientConnections = (x) =>
            {
                return new List<PubSubscriptionsExtensionMapper>
                {
                    new PubSubscriptionsExtensionMapper
                    {
                        StandardField="1",
                         PubID = 0,
                         CustomField= SampleText,
                    }
                };
            };
            ShimSubscriptionsExtensionMapper.GetAllClientConnections = (x) =>
            {
                return new List<SubscriptionsExtensionMapper>
                {
                    new SubscriptionsExtensionMapper
                    {
                        StandardField = "1",
                         CustomField = SampleText,
                         Active = true
                    }
                };
            };
        }
    }
}
