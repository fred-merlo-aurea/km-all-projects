using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Object.Fakes;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FiltersTests
    {
        private const string FilterDescriptionAllIntersect = "ALL INTERSECT";
        private const string FilterDescriptionAllUnion = "ALL UNION";
        private const int UserID = -1;
        private const int FilterNoOne = 1;
        private const int FilterNoTwo = 2;
        private const int FilterComboCount = 5;
        private const string AllFilterNumbers = "ALL";
        private const string ColumnNameTotal = "Total";
        private const string ColumnNameFilterNo = "FilterNo";
        private const string FilterOperationSingle = "SINGLE";
        private Filters _filters;

        [SetUp]
        public void Setup()
        {
            _filters = new Filters(new ClientConnections(), UserID);
        }

        [Test]
        public void GetIntersectCrossTabData_EmptyFilterCol_ReturnsSingleRow()
        {
            // Arrange
            _filters.FilterComboList = new List<FilterCombo>()
            {
                new FilterCombo()
                {
                    FilterDescription = FilterDescriptionAllIntersect,
                    Count = FilterComboCount
                }
            };

            // Act
            var dataTable = _filters.GetIntersectCrossTabData();

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Columns.ShouldNotBeNull(),
                () => dataTable.Columns.Count.ShouldBe(2),
                () => dataTable.Columns[0].ColumnName.ShouldBe(ColumnNameFilterNo),
                () => dataTable.Columns[1].ColumnName.ShouldBe(ColumnNameTotal),
                () => dataTable.Rows.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][0].ShouldBe(AllFilterNumbers),
                () => dataTable.Rows[0][1].ShouldBe(FilterComboCount.ToString()));
        }

        [Test]
        public void GetIntersectCrossTabData_TwoFilterComboItems_ReturnsFourRows()
        {
            // Arrange
            _filters.FilterComboList = new List<FilterCombo>()
            {
                new FilterCombo()
                {
                    FilterDescription = FilterDescriptionAllIntersect,
                    Count = FilterComboCount
                },
                new FilterCombo()
                {
                    SelectedFilterOperation = FilterOperationSingle,
                    Count = FilterComboCount,
                    SelectedFilterNo = FilterNoOne.ToString(),
                    FilterDescription = string.Format("{0}(intersect)", FilterNoOne.ToString())
                },
                new FilterCombo()
                {
                    SelectedFilterOperation = FilterOperationSingle,
                    Count = FilterComboCount * 2,
                    SelectedFilterNo = FilterNoTwo.ToString(),
                    FilterDescription = string.Format("{0},{1}(intersect)", FilterNoOne.ToString(), FilterNoTwo.ToString())
                }
            };

            _filters.SetField("FilterCol", new List<Filter>()
            {
                new Filter()
                {
                    Count = FilterComboCount,
                    FilterNo = FilterNoOne
                },
                new Filter()
                {
                    Count = FilterComboCount,
                    FilterNo = FilterNoTwo
                }
            });

            // Act
            var dataTable = _filters.GetIntersectCrossTabData();

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Columns.ShouldNotBeNull(),
                () => dataTable.Columns.Count.ShouldBe(4),
                () => dataTable.Columns[0].ColumnName.ShouldBe(ColumnNameFilterNo),
                () => dataTable.Columns[1].ColumnName.ShouldBe(FilterNoOne.ToString()),
                () => dataTable.Columns[2].ColumnName.ShouldBe(FilterNoTwo.ToString()),
                () => dataTable.Rows.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(3),
                () => dataTable.Rows[0][0].ShouldBe(FilterNoOne.ToString()),
                () => dataTable.Rows[1][0].ShouldBe(FilterNoTwo.ToString()),
                () => dataTable.Rows[0][1].ShouldBe(FilterComboCount.ToString()),
                () => dataTable.Rows[1][1].ShouldBe((FilterComboCount * 2).ToString()));
        }

        [Test]
        public void GetUnionCrossTabData_EmptyFilterCol_ReturnsSingleRow()
        {
            // Arrange
            _filters.FilterComboList = new List<FilterCombo>()
            {
                new FilterCombo()
                {
                    FilterDescription = FilterDescriptionAllUnion,
                    Count = FilterComboCount
                }
            };

            // Act
            var dataTable = _filters.GetUnionCrossTabData();

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Columns.ShouldNotBeNull(),
                () => dataTable.Columns.Count.ShouldBe(2),
                () => dataTable.Columns[0].ColumnName.ShouldBe(ColumnNameFilterNo),
                () => dataTable.Columns[1].ColumnName.ShouldBe(ColumnNameTotal),
                () => dataTable.Rows.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][0].ShouldBe(AllFilterNumbers),
                () => dataTable.Rows[0][1].ShouldBe(FilterComboCount.ToString()));
        }

        [Test]
        public void GetUnionCrossTabData_TwoFilterComboItems_ReturnsFourRows()
        {
            // Arrange
            _filters.FilterComboList = new List<FilterCombo>()
            {
                new FilterCombo()
                {
                    FilterDescription = FilterDescriptionAllUnion,
                    Count = FilterComboCount
                },
                new FilterCombo()
                {
                    SelectedFilterOperation = FilterOperationSingle,
                    Count = FilterComboCount,
                    SelectedFilterNo = FilterNoOne.ToString(),
                    FilterDescription = string.Format("{0}(union)", FilterNoOne.ToString())
                },
                new FilterCombo()
                {
                    SelectedFilterOperation = FilterOperationSingle,
                    Count = FilterComboCount * 2,
                    SelectedFilterNo = FilterNoTwo.ToString(),
                    FilterDescription = string.Format("{0},{1}(union)", FilterNoOne.ToString(), FilterNoTwo.ToString())
                }
            };

            _filters.SetField("FilterCol", new List<Filter>()
            {
                new Filter()
                {
                    Count = FilterComboCount,
                    FilterNo = FilterNoOne
                },
                new Filter()
                {
                    Count = FilterComboCount,
                    FilterNo = FilterNoTwo
                }
            });

            // Act
            var dataTable = _filters.GetUnionCrossTabData();

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Columns.ShouldNotBeNull(),
                () => dataTable.Columns.Count.ShouldBe(4),
                () => dataTable.Columns[0].ColumnName.ShouldBe(ColumnNameFilterNo),
                () => dataTable.Columns[1].ColumnName.ShouldBe(FilterNoOne.ToString()),
                () => dataTable.Columns[2].ColumnName.ShouldBe(FilterNoTwo.ToString()),
                () => dataTable.Rows.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(3),
                () => dataTable.Rows[0][0].ShouldBe(FilterNoOne.ToString()),
                () => dataTable.Rows[1][0].ShouldBe(FilterNoTwo.ToString()),
                () => dataTable.Rows[0][1].ShouldBe(FilterComboCount.ToString()),
                () => dataTable.Rows[1][1].ShouldBe((FilterComboCount * 2).ToString()));
        }
    }
}
