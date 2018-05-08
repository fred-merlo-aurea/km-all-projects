using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using System.Web.Routing;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using UAD.DataCompare.Web.Controllers.UAD;
using UAD.DataCompare.Web.Models;
using ShimCode = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode;
using ShimTransactionCodeType = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimTransactionCodeType;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class DatacompareControllerTest
    {
        private DatacompareController _controller;
        private DataCompareDownloadFilterDetail _filterDetail;
        private FileDetails _fileDetails;
        private List<ColumnMap> _lstColumnMapper;
        private KMPlatform.Object.ClientConnections _clientConnections;
        private Mock<HttpSessionStateBase> session;

        private int _counter;
        private int _codeId1;
        private int _codeId2;
        private int _idTest1;
        private int _idTest2;

        private const int Value1 = 1;
        private const int Value2 = 2;
        private const int Value3 = 3;
        private const int Value4 = 4;

        private const string SampleText1 = "Sample1";
        private const string SampleText2 = "Sample2";
        private const string DeletedElement = "Delete";
        private const string KeyProfileColumnList = "ProfileColumnList";

        private const string FilterEmailStatus = "EMAIL STATUS";
        private const string FilterCountry = "COUNTRY";
        private const string FilterCategoryCode = "CATEGORY CODE";
        private const string FilterXact = "XACT";
        private const string FilterQsource = "QSOURCE";

        private const string CountryUnitedStates = "United States";
        private const string CountryBrazil = "Brazil";
        private const string ValuesSeparator = ",";
        private const char CharSeparator = ',';
        private const string TempPath = "App_Data";

        private List<SelectListItem> _selectListForSession;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _controller = new DatacompareController();
            _clientConnections = new KMPlatform.Object.ClientConnections();
            session = new Mock<HttpSessionStateBase>();

            _counter = 1;
            _codeId1 = 1;
            _codeId2 = 2;
            _idTest1 = 1;
            _idTest2 = 2;

            _filterDetail = new DataCompareDownloadFilterDetail()
            {
                Name = string.Empty,
                Values = string.Join(ValuesSeparator, _idTest1, _idTest2)
            };

            _selectListForSession = new List<SelectListItem>
            {
                new SelectListItem { Text = SampleText1, Value = SampleText1 },
                new SelectListItem { Text = SampleText2, Value = SampleText2 }
            };

            _lstColumnMapper = new List<ColumnMap>();

            SetupFakesForCountry();
            SetupFakesForUad();
            SetupFakesForUadLookup();
        }

        [TearDown]
        public void CleanUp()
        {
            var fullTestPath = $"{TestContext.CurrentContext.TestDirectory}\\{TempPath}";
            if (Directory.Exists(fullTestPath))
            {
                Directory.Delete(fullTestPath, true);
            }
            _context.Dispose();
        }

        [Test]
        public void GetEmailStatusFilterValue_WhenClientConnectionsAreNull_ThrowsException()
        {
            // Arrange
            _clientConnections = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.GetEmailStatusFilterValue(_clientConnections, _filterDetail);
            }); 
        }

        [Test]
        public void GetEmailStatusFilterValue_WhenFilterDetailIsNull_ThrowsException()
        {
            // Arrange
            _filterDetail = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.GetEmailStatusFilterValue(_clientConnections, _filterDetail);
            }); 
        }

        [Test]
        public void GetEmailStatusFilterValue_WithFilterNameEqualsEmailStatus_ReturnsExpectedText()
        {
            // Arrange
            _filterDetail.Name = FilterEmailStatus;
            var selectedText = string.Join(ValuesSeparator, SampleText1, SampleText2);

            // Act
            var result = _controller.GetEmailStatusFilterValue(_clientConnections, _filterDetail);

            // Assert
            result.ShouldBe(selectedText); 
        }

        [Test]
        public void SelectItemDisplayName_WhenFilterNameIsNull_ReturnsStringEmpty()
        {
            // Arrange
            var item = _filterDetail.Values.Split(CharSeparator)[0];

            // Act
            var result = _controller.SelectItemDisplayName(_filterDetail.Name, item);

            // Assert
            result.ShouldBeNullOrEmpty();
        }

        [Test]
        public void SelectItemDisplayName_WithFilterNameEqualsCountry_ReturnsCountryShortName()
        {
            // Arrange
            _filterDetail.Name = FilterCountry;
            var item = _filterDetail.Values.Split(CharSeparator)[0];

            // Act
            var result = _controller.SelectItemDisplayName(_filterDetail.Name, item);

            // Assert
            result.ShouldBe(CountryUnitedStates);
        }

        [Test]
        public void SelectItemDisplayName_WithFilterNameEqualsCategoryCode_ReturnsCategoryCodeName()
        {
            // Arrange
            _filterDetail.Name = FilterCategoryCode;
            var item = _filterDetail.Values.Split(CharSeparator)[0];

            // Act
            var result = _controller.SelectItemDisplayName(_filterDetail.Name, item);

            // Assert
            result.ShouldBe(SampleText1);
        }

        [Test]
        public void SelectItemDisplayName_WithFilterNameEqualsXact_ReturnsTransactionCodeTypeName()
        {
            // Arrange
            _filterDetail.Name = FilterXact;
            var item = _filterDetail.Values.Split(CharSeparator)[0];

            // Act
            var result = _controller.SelectItemDisplayName(_filterDetail.Name, item);

            // Assert
            result.ShouldBe(SampleText1);
        }

        [Test]
        public void SelectItemDisplayName_WithFilterNameEqualsQsource_ReturnsDisplayName()
        {
            // Arrange
            _filterDetail.Name = FilterQsource;
            var item = _filterDetail.Values.Split(CharSeparator)[0];

            // Act
            var result = _controller.SelectItemDisplayName(_filterDetail.Name, item);

            // Assert
            result.ShouldBe(SampleText1);
        }

        [Test]
        public void RebuildColumnMap_WhenFileIsNull_ThrowsException()
        {
            // Arrange
            _fileDetails = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.RebuildColumnMap(_fileDetails, _lstColumnMapper);
            });
        }

        [Test]
        public void RebuildColumnMap_WhenLstColumnMapperIsNull_ThrowsException()
        {
            // Arrange
            _lstColumnMapper = null;

            _fileDetails = new FileDetails
            {
                ColumnMapping = new List<ColumnMap>
                {
                    new ColumnMap { MappedColumn = SampleText1 }
                }
            };

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _controller.RebuildColumnMap(_fileDetails, _lstColumnMapper);
            });
        }

        [Test]
        public void RebuildColumnMap_WhenSessionIsNull_ReturnsAddedColumnMapping()
        {
            // Arrange
            _fileDetails = new FileDetails
            {
                ColumnMapping = new List<ColumnMap>
                {
                    new ColumnMap { MappedColumn = SampleText1 },
                    new ColumnMap { MappedColumn = SampleText2 }
                }
            };

            // Act
            var result = _controller.RebuildColumnMap(_fileDetails, _lstColumnMapper);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result[0].MappedColumn.ShouldBe(SampleText1),
                () => result[1].MappedColumn.ShouldBe(SampleText2)
            );
        }

        [Test]
        public void RebuildColumnMap_WithSessionNullWhenMappedColumnIsDeleteShouldNotBeAdded_ReturnsCorrectElements()
        {
            // Arrange
            _fileDetails = new FileDetails
            {
                ColumnMapping = new List<ColumnMap>
                {
                    new ColumnMap { FieldMapID = Value1, MappedColumn = SampleText1 },
                    new ColumnMap { FieldMapID = Value2, MappedColumn = DeletedElement },
                    new ColumnMap { FieldMapID = Value3, MappedColumn = DeletedElement },
                    new ColumnMap { FieldMapID = Value4, MappedColumn = SampleText2 },
                }
            };

            // Act
            var result = _controller.RebuildColumnMap(_fileDetails, _lstColumnMapper);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result[0].FieldMapID.ShouldBe(Value1),
                () => result[1].FieldMapID.ShouldBe(Value4)
            );
        }

        [Test]
        public void RebuildColumnMap_WhenSessionIsNotNullShouldAddProfileColumnList_ReturnsCorrectElements()
        {
            // Arrange
            SetupControllerContext();

            _fileDetails = new FileDetails
            {
                ColumnMapping = new List<ColumnMap>
                {
                    new ColumnMap { FieldMapID = Value1, MappedColumn = SampleText1 },
                    new ColumnMap { FieldMapID = Value2, MappedColumn = DeletedElement },
                    new ColumnMap { FieldMapID = Value3, MappedColumn = DeletedElement },
                    new ColumnMap { FieldMapID = Value4, MappedColumn = SampleText2 },
                }
            };

            // Act
            var result = _controller.RebuildColumnMap(_fileDetails, _lstColumnMapper);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result[0].FieldMapID.ShouldBe(Value1),
                () => result[0].ProfileColumnList.Count.ShouldBe(_selectListForSession.Count),
                () => result[1].FieldMapID.ShouldBe(Value4),
                () => result[1].ProfileColumnList.Count.ShouldBe(_selectListForSession.Count)
            );
        }

        private void SetupFakesForCountry()
        {
            ShimCountry.AllInstances.Select =
                _ => new List<Country>
                {
                    new Country
                    {
                        CountryID = _idTest1,
                        ShortName = CountryUnitedStates
                    },
                    new Country
                    {
                        CountryID = _idTest2,
                        ShortName = CountryBrazil
                    }
                };
        }

        private void SetupFakesForUad()
        {
            ShimEmailStatus.AllInstances.SelectClientConnections =
                (_, connection) => new List<FrameworkUAD.Entity.EmailStatus>
                {
                    new FrameworkUAD.Entity.EmailStatus
                    {
                        EmailStatusID = _idTest1,
                        Status = SampleText1
                    },
                    new FrameworkUAD.Entity.EmailStatus
                    {
                        EmailStatusID = _idTest2,
                        Status = SampleText2
                    }
                };

            ShimCodeSheet.AllInstances.SelectClientConnections =
                (_, connection) => new List<FrameworkUAD.Entity.CodeSheet>
                {
                    new FrameworkUAD.Entity.CodeSheet
                    {
                        CodeSheetID = _idTest1,
                        ResponseDesc = SampleText1,
                        ResponseValue = _idTest1.ToString()
                    },
                    new FrameworkUAD.Entity.CodeSheet
                    {
                        CodeSheetID = _idTest2,
                        ResponseDesc = SampleText2,
                        ResponseValue = _idTest2.ToString()
                    }
                };
        }

        private void SetupFakesForUadLookup()
        {
            ShimCategoryCode.AllInstances.Select =
                _ => new List<CategoryCode>
                {
                    new CategoryCode
                    {
                        CategoryCodeID = _idTest1,
                        CategoryCodeName = SampleText1
                    },
                    new CategoryCode
                    {
                        CategoryCodeID = _idTest2,
                        CategoryCodeName = SampleText2
                    }
                };

            ShimTransactionCodeType.AllInstances.Select =
                _ => new List<TransactionCodeType>
                {
                    new TransactionCodeType
                    {
                        TransactionCodeTypeID = _idTest1,
                        TransactionCodeTypeName = SampleText1
                    },
                    new TransactionCodeType
                    {
                        TransactionCodeTypeID = _idTest2,
                        TransactionCodeTypeName = SampleText2
                    }
                };

            ShimCode.AllInstances.Select =
                _ => new List<Code>
                {
                    new Code
                    {
                        CodeId = _codeId1,
                        DisplayName = SampleText1
                    },
                    new Code
                    {
                        CodeId = _codeId2,
                        DisplayName = SampleText2
                    }
                };
        }
        
        private void SetupControllerContext()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(x => x.HttpMethod).Returns("GET");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Request).Returns(request.Object);

            session.Setup(x => x[KeyProfileColumnList]).Returns(_selectListForSession);
            mockHttpContext.Setup(x => x.Session).Returns(session.Object);
            
            _controller.ControllerContext = new ControllerContext(
                mockHttpContext.Object,
                new RouteData(),
                new Mock<ControllerBase>().Object);

            ShimHttpServerUtilityWrapper.AllInstances.MapPathString = (h, path) =>
            {
                path = path.Replace("~", string.Empty);
                return $"{TestContext.CurrentContext.TestDirectory}\\{path}";
            };
            ShimController.AllInstances.ServerGet = (c) => new ShimHttpServerUtilityWrapper();
        }
    }
}
