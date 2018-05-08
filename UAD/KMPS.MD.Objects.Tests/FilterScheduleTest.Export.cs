using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FilterScheduleTest
    {
        private const string SampleExportColumn = "SampleExportColumn";
        private const string SampleSuppressionOperation = "SampleSuppressionOperation";
        private const string SampleFilterName = "SampleFilterName";
        private const string SampleFilterQuery = "SampleFilterQuery";
        private const string SampleColumn1 = "SampleColumn2";
        private const string SampleColumn2 = "SampleColumn2";
        private const string MasterColumn1 = "MasterColumn1";
        private const string MasterColumn2 = "MasterColumn2";
        private const string CustomColumn = "CustomColumn";
        private const string StandardColumn = "StandardColumn";
        private const string EmailField = "EMAIL";
        private const string FirstNameField = "FIRSTNAME";
        private const string SampleMapperColumnValue = "SampleMapperColumnValue";

        private IDisposable _shims;

        [SetUp]
        public void SetUp()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shims.Dispose();
        }

        [Test]
        [TestCase(EmailField)]
        [TestCase(FirstNameField)]
        public void Export_WhenDataMaskError_ReturnsEmptyTuple(string maskField)
        {
            // Arrange
            const int FilterScheduleId = 1;
            var connections = new ClientConnections();
            SetFakesForExportMethod();
            ShimFilterSchedule.GetByIDClientConnectionsInt32 = (_, __) => new FilterSchedule
            {
                FilterScheduleID = 1,
                FilterID = 1,
                FilterName = SampleFilterName,
                ExportTypeID = Enums.ExportType.ECN,
            };
            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (_, __) => new List<UserDataMask>
            {
                new UserDataMask { MaskField = maskField }
            };
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (conn, __, ___) => new Filters(conn, 1)
            {
                GetFilter(Enums.ViewType.None)
            };

            // Act
            var resultTuple = FilterSchedule.Export(connections, FilterScheduleId);

            // Assert
            resultTuple.ShouldSatisfyAllConditions(
                () => resultTuple.Item1.Rows.Count.ShouldBe(0),
                () => resultTuple.Item2.ShouldBeNullOrWhiteSpace(),
                () => 
                    {
                        if (maskField == EmailField)
                        {   
                            resultTuple.Item4.ShouldBeTrue();
                        }
                        else
                        {
                            resultTuple.Item4.ShouldBeFalse();
                        }
                    },
                () => resultTuple.Item3.Rows.Count.ShouldBe(0));
        }

        [Test]
        [TestCase(Enums.ViewType.None)]
        [TestCase(Enums.ViewType.ProductView)]
        public void Export_WhenDataMaskNoError_ReturnsTuple(Enums.ViewType viewType)
        {
            // Arrange
            const int FilterScheduleId = 1;
            var connections = new ClientConnections();
            SetFakesForExportMethod();
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (conn, __, ___) => new Filters(conn, 1)
            {
                GetFilter(viewType)
            };

            // Act
            var resultTuple = FilterSchedule.Export(connections, FilterScheduleId);

            // Assert
            AssertResultTuple(resultTuple);
        }

        [Test]
        public void Export_WhenFilterGroupSelectedIsNull_ReturnsTuple()
        {
            // Arrange
            const int FilterScheduleId = 1;
            var connections = new ClientConnections();
            SetFakesForExportMethod();
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (conn, __, ___) => new Filters(conn, 1)
            {
                GetFilter(Enums.ViewType.None)
            };
            var filterSchedile = GetFilterSchedule();
            filterSchedile.FilterGroupID_Selected = null;
            ShimFilterSchedule.GetByIDClientConnectionsInt32 = (_, __) => filterSchedile;

            // Act
            var resultTuple = FilterSchedule.Export(connections, FilterScheduleId);

            // Assert
            AssertResultTuple(resultTuple);
        }

        [Test]
        public void Export_WhenFilterGroupSelectedIsEmpty_ReturnsTuple()
        {
            // Arrange
            const int FilterScheduleId = 1;
            var connections = new ClientConnections();
            SetFakesForExportMethod();
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (conn, __, ___) => new Filters(conn, 1)
            {
                GetFilter(Enums.ViewType.None)
            };
            var filterSchedile = GetFilterSchedule();
            filterSchedile.FilterGroupID_Selected = new List<int>();
            ShimFilterSchedule.GetByIDClientConnectionsInt32 = (_, __) => filterSchedile;

            // Act
            var resultTuple = FilterSchedule.Export(connections, FilterScheduleId);

            // Assert
            AssertResultTuple(resultTuple);
        }

        private void SetFakesForExportMethod()
        {
            ShimFilterSchedule.GetByIDClientConnectionsInt32 = (_, __) => GetFilterSchedule();
            ShimUser.GetByUserIDInt32Boolean = (_, __) => new User
            {
                UserID = 1,
            };

            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (_, __) => new List<UserDataMask>
            {
                new UserDataMask { MaskField = SampleFilterName }
            };
            
            ShimFilter.getFilterQueryFilterStringStringClientConnections = (_, __, ___, ____) => SampleFilterQuery;

            ShimFilterExportField.getByFilterScheduleIDClientConnectionsInt32 = (_, __) => new List<FilterExportField>
            {
                new FilterExportField  { IsCustomValue = false, IsDescription = false }
            };
            ShimUtilities.GetSelectedStandardExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) => 
                new List<string> { SampleColumn1, SampleColumn2  };

            ShimUtilities.GetSelectedMasterGroupExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) =>
                new Tuple<List<string>, List<string>>(new List<string> { MasterColumn1, MasterColumn2 }, new List<string> { });

            ShimUtilities.GetSelectedSubExtMapperExportColumnsClientConnectionsListOfString = (_, __) => 
                new List<string> { SampleMapperColumnValue };

            ShimUtilities.GetStandardExportColumnFieldNameIListOfStringEnumsViewTypeInt32Boolean = (_, __, ___, _____) => 
                new List<string> { StandardColumn  };

            ShimUtilities.GetSelectedCustomExportColumnsListOfString = (_) => new List<string> { CustomColumn };

            ShimSubscriber.GetSubscriberDataClientConnectionsStringBuilderListOfStringListOfStringListOfStringListOfStringListOfStringInt32ListOfInt32BooleanInt32String =
                (e, r, t, y, u, i, a, s, d, f, g, h) => GetDataTable();
            ShimSubscriber.GetProductDimensionSubscriberDataClientConnectionsStringBuilderListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Int32 =
                (e, r, t, y, u, i, a, s, d, f) => GetDataTable();
            ShimUtilities.GetSelectedPubSubExtMapperExportColumnsClientConnectionsListOfStringInt32 = (_, __, ___) => new List<string>();

            ShimUtilities.GetSelectedResponseGroupStandardExportColumnsClientConnectionsListOfStringInt32BooleanBoolean = (u, f, t, w, y) =>
            new Tuple<List<string>, List<string>, List<string>>(new List<string>(), new List<string>(), new List<string>());


            ShimFilterExportField.getDisplayNameClientConnectionsInt32 = (_, __) => new List<FilterExportField>
            {
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "FNAME" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "LNAME" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "ISLATLONVALID" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "Default" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "ADDRESS1" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "REGIONCODE" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "ZIPCODE" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "PUBTRANSACTIONDATE" },
                new FilterExportField { ExportColumn = SampleExportColumn, DisplayName = "QUALIFICATIONDATE" },
            };

            ShimProfileFieldMask.MaskDataClientConnectionsObjectUser = (_, __, ___) => GetDataTable();
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("FNAME");
            dataTable.Columns.Add("LNAME");
            dataTable.Columns.Add("ISLATLONVALID");
            dataTable.Columns.Add("Address");
            dataTable.Columns.Add("State");
            dataTable.Columns.Add("Zip");
            dataTable.Columns.Add("TransactionDate");
            dataTable.Columns.Add("QDate");

            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("GeoLocated");
            dataTable.Columns.Add("ADDRESS1");
            dataTable.Columns.Add("REGIONCODE");
            dataTable.Columns.Add("ZIPCODE");
            dataTable.Columns.Add("PUBTRANSACTIONDATE");
            dataTable.Columns.Add("QUALIFICATIONDATE");
            dataTable.Columns.Add("SubscriptionID");
            dataTable.Columns.Add("Default");

            return dataTable;
        }

        private static Filter GetFilter(Enums.ViewType viewType)
        {
            return new Filter
            {
                FilterGroupID = 1,
                FilterNo = 1,
                Fields = new List<Field>
                    {
                        new Field
                        {
                            Group = "OPENCRITERIA",
                            SearchCondition = "SEARCH SELECTED PRODUCTS",
                            Name = "PRODUCT",
                            Values = "1,2"
                        }
                    },
                ViewType = viewType,
                PubID = 1
            };
        }

        private static FilterSchedule GetFilterSchedule()
        {
            return new FilterSchedule
            {
                FilterScheduleID = 1,
                FilterID = 1,
                FilterName = SampleFilterName,
                ExportTypeID = Enums.ExportType.FTP,
                FilterGroupID_Selected = new List<int> { 1 },
                FilterGroupID_Suppressed = new List<int> { 1 },
                ShowHeader = true,
                SuppressedOperation = SampleSuppressionOperation

            };
        }

        private void AssertResultTuple(Tuple<DataTable, string, DataTable, bool> resultTuple)
        {
            resultTuple.ShouldSatisfyAllConditions(
                () => resultTuple.Item1.Rows.Count.ShouldBe(0),
                () => resultTuple.Item1.Columns.Count.ShouldBe(17),
                () =>
                {
                    var dataTable = GetDataTable();
                    dataTable.Columns.Remove("SubscriptionID");
                    foreach (DataColumn item in dataTable.Columns)
                    {
                        resultTuple.Item1.Columns.Contains(item.ColumnName).ShouldBeTrue();
                    }
                },
                () => resultTuple.Item2.ShouldNotBeNullOrWhiteSpace(),
                () => resultTuple.Item2.ShouldContain("Operations = Single"),
                () => resultTuple.Item2.ShouldContain("Filters In\r\nPRODUCT = "),
                () => resultTuple.Item2.ShouldContain("Filters NotIn\r\nPRODUCT = "),
                () => resultTuple.Item2.ShouldContain($"Operations Not In = {SampleSuppressionOperation}"),
                () => resultTuple.Item3.Rows.Count.ShouldBe(0),
                () => resultTuple.Item4.ShouldBeFalse());
        }
    }
}
