using System;
using System.Collections.Generic;
using System.Linq;
using KM.Common.Export;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Export
{
    [TestFixture]
    public class HelperTest
    {
        private const int DefaultBrandId = 0;
        private const int PositiveBrandId = 42;
        private const string InputNameScore = "SCORE";
        private const string InputNameAddress = "ADDRESS1";
        private const string InputNameRegionCode = "REGIONCODE";
        private const string InputNameZipCode = "ZIPCODE";
        private const string InputNamePubTransactionDate = "PUBTRANSACTIONDATE";
        private const string InputNameQualificationDate = "QUALIFICATIONDATE";
        private const string InputNameEmailStatus = "EMAILSTATUS";
        private const string InputNameCategoryId = "CATEGORYID";
        private const string InputNameTransactionStatus = "TRANSACTIONSTATUS";
        private const string InputNameTransactionId = "TRANSACTIONID";
        private const string InputNameQSourceId = "QSOURCEID";
        private const string InputNameParc3C = "PAR3C";
        private const string InputNameTransactionName = "TRANSACTIONNAME";
        private const string InputNameQSourceName = "QSOURCENAME";
        private const string InputNameLastOpenEdDate = "LASTOPENEDDATE";
        private const string InputNameLastOpenPubCode = "LASTOPENEDPUBCODE";
        private const string InputNameIgrpNo = "IGRP_NO";
        private const string InputNameCgRpNo = "CGRP_NO";
        private const string InputNameBatch = "BATCH";
        private const string InputNameDefault = "";
        private const string InputNameFName = "FNAME";
        private const string InputNameLName = "LNAME";
        private const string InputNameLatLonValid = "ISLATLONVALID";
        private const string SubscriptionIdColumnProductView = "ps.SUBSCRIPTIONID|Default";
        private const string SubscriptionIdColumnNonProductView = "s.SUBSCRIPTIONID|Default";
        private const string TestInputBrandId = "SCORE|Merkur";
        private const string TestOutputBrandId = "bs.Score|Merkur";
        private const char PipeSeparator = '|';
        private const string ExportTransactionName = "tc.TransactionCodeName as TransactionName";
        private const string ExportDisplayOrderAsParc3C = "cp.DisplayOrder as Par3C";
        private const string ExportQSourceId = "c.CodeValue as QSourceID";
        private const string ExportTransactionStatus = "tct.TransactionCodeTypename as Transactionstatus";
        private const string ExportCategoryId = "cc.CategoryCodeValue as CategoryID";
        private const string ExportEmailStatus = "es.Status as Emailstatus";
        private const string ExportQualifictionDate = "CONVERT(VARCHAR(10),ps.QualificationDate,1) as QDate";
        private const string ExportPubTransactionDate = "ps.PubTransactionDATE as TransactionDate";
        private const string ExportZipCode = "ps.ZipCode as Zip";
        private const string ExportRegionCode = "ps.RegionCode as State";
        private const string ExportAddress = "ps.Address1 as Address";
        private const string ExportScore = "s.Score";
        private const string ExportBatch = "ps.Batch";
        private const string ExportQSourceName = "c.DisplayName  as QSourceName";
        private const string ExportTransactionId = "tc.TransactionCodeValue as TransactionID";
        private const string ExportFirstName = "s.FNAME as FirstName";
        private const string ExportLastName = "s.LNAME as LastName";
        private const string ExportLatLonValid = "s.IsLatLonValid as GeoLocated";
        private const string ExportParc3CNonProductView = "cp.CodeValue as Par3C";

        private static readonly string[] GroupExportsProductView =
        {
            "ps.EMAIL|Default",
            "ps.FIRSTNAME|Default",
            "ps.LASTNAME|Default",
            "ps.COMPANY|Default",
            "ps.TITLE|Default",
            "ps.ADDRESS1 as Address|Default",
            "ps.CITY|Default",
            "ps.REGIONCODE as State|Default",
            "ps.ZIPCODE as Zip|None",
            "ps.COUNTRY|Default",
            "ps.PHONE|None",
            "ps.FAX|None",
            "ps.MOBILE|None"
        };

        private static readonly string[] GroupExportsNonProductView =
        {
            "s.EMAIL|Default",
            "s.FNAME as FirstName|Default",
            "s.LNAME as LastName|Default",
            "s.COMPANY|Default",
            "s.TITLE|Default",
            "s.ADDRESS|Default",
            "s.CITY|Default",
            "s.STATE|Default",
            "s.ZIP|Default",
            "s.COUNTRY|Default",
            "s.PHONE|None",
            "s.FAX|None",
            "s.MOBILE|None"
        };

        private static IEnumerable<string> ColumnNamesForProductView =>
            new List<string>
            {
                InputNameScore,
                InputNameAddress,
                InputNameRegionCode,
                InputNameZipCode,
                InputNamePubTransactionDate,
                InputNameQualificationDate,
                InputNameEmailStatus,
                InputNameCategoryId,
                InputNameTransactionStatus,
                InputNameTransactionId,
                InputNameQSourceId,
                InputNameParc3C,
                InputNameTransactionName,
                InputNameQSourceName,
                InputNameIgrpNo,
                InputNameCgRpNo,
                InputNameBatch,
                InputNameDefault
            };

        private static IEnumerable<string> ColumnNamesForNonProductView =>
            new List<string>
            {
                InputNameScore,
                InputNameFName,
                InputNameLName,
                InputNameLatLonValid,
                InputNameCategoryId,
                InputNameTransactionStatus,
                InputNameTransactionId,
                InputNameQSourceId,
                InputNameParc3C,
                InputNameTransactionName,
                InputNameQSourceName,
                InputNameDefault
            };

        private static IEnumerable<string> ColumnNamesForProductViewNoOutput =>
            new List<string>
            {
                InputNameLastOpenEdDate,
                InputNameLastOpenPubCode,
            };

        private static IEnumerable<string> ColumnNamesForNonProductViewNoOutput =>
            new List<string>
            {
                InputNameLastOpenEdDate,
                InputNameLastOpenPubCode,
            };

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetStandardExportColumnFieldNameForViewType_EmptyInput_EmptyOutput(bool isProductView)
        {
            // Arrange
            var inputColumns = new List<string>();

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputColumns,
                isProductView,
                DefaultBrandId,
                false,
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(0));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetStandardExportColumnFieldName_BrandIdSet_ReturnsSubscriptionIdColumn(bool isProductViewe)
        {
            // Arrange
            var inputList = new List<string> 
                                {
                                    TestInputBrandId
                                };

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                isProductViewe,
                PositiveBrandId,
                false,
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(1),
                () => exportColumns.First().ShouldBe(TestOutputBrandId));
        }

        [Test]
        public void GetStandardExportColumnFieldNameForViewType_NullInput_ArgumentException()
        {
            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                {
                   Helper.GetStandardExportColumnFieldName(
                        null,
                        false,
                        DefaultBrandId,
                        false,
                        true);
                });
        }

        [Test]
        [TestCaseSource(nameof(GetValidInputTestDataForProductView))]
        public void GetStandardExportColumnFieldNameForProductView_ValidInput_ReturnsExportColumns(Tuple<string, string> testInput)
        {
            // Arrange
            var inputList = new List<string>() { testInput.Item1 };

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                true,
                DefaultBrandId,
                false, 
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.Count.ShouldBe(1),
                () => exportColumns.First().ShouldBe(testInput.Item2));
        }

        [Test]
        [TestCaseSource(nameof(GetInputTestDataForProductViewNoOutput))]
        public void GetStandardExportColumnFieldNameForProductView_NoValidInput_ReturnsEmptyList(Tuple<string, string> testInput)
        {
            // Arrange
            var inputList = new List<string>() { testInput.Item1 };

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                true,
                DefaultBrandId,
                false, 
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(0));
        }

        [Test]
        public void GetStandardExportColumnFieldNameForProductView_GroupExport_ReturnsGroupExportColumns()
        {
            // Arrange
            var inputList = new List<string>();

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                true,
                DefaultBrandId,
                true, 
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(GroupExportsProductView.Length),
                () => exportColumns
                    .ToList()
                    .ForEach(column => GroupExportsProductView.Contains(column).ShouldBeTrue()));
        }

        [Test]
        public void GetStandardExportColumnFieldNameForProductView_IssueNotSplit_ReturnsSubscriptionIdColumn()
        {
            // Arrange
            var inputList = new List<string>();

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                true,
                DefaultBrandId,
                false);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(1),
                () => exportColumns.First().ShouldBe(SubscriptionIdColumnProductView));
        }

        [Test]
        [TestCaseSource(nameof(GetValidInputTestDataForNonProductView))]
        public void GetStandardExportColumnFieldNameForNonProductView_ValidInput_ReturnsExportColumns(Tuple<string, string> testInput)
        {
            // Arrange
            var inputList = new List<string>() { testInput.Item1 };

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                false,
                DefaultBrandId,
                false, 
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.Count.ShouldBe(1),
                () => exportColumns.First().ShouldBe(testInput.Item2));
        }

        [Test]
        [TestCaseSource(nameof(GetInputTestDataForNonProductViewNoOutput))]
        public void GetStandardExportColumnFieldNameForNonProductView_NoValidInput_ReturnsEmptyList(Tuple<string, string> testInput)
        {
            // Arrange
            var inputList = new List<string>() { testInput.Item1 };

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                false,
                DefaultBrandId,
                false,
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(0));
        }

        [Test]
        public void GetStandardExportColumnFieldNameForNonProductView_GroupExport_ReturnsGroupExportColumns()
        {
            // Arrange
            var inputList = new List<string>();

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                false,
                DefaultBrandId,
                true,
                true);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(GroupExportsNonProductView.Length),
                () => exportColumns
                .ToList()
                    .ForEach(column => GroupExportsNonProductView.Contains(column).ShouldBeTrue()));
        }

        [Test]
        public void GetStandardExportColumnFieldNameForNonProductView_IssueNotSplit_ReturnsSubscriptionIdColumn()
        {
            // Arrange
            var inputList = new List<string>();

            // Act
            var exportColumns = Helper.GetStandardExportColumnFieldName(
                inputList,
                false,
                DefaultBrandId,
                false);

            // Assert
            exportColumns.ShouldSatisfyAllConditions(
                () => exportColumns.ShouldBeOfType(typeof(List<string>)),
                () => exportColumns.Count.ShouldBe(1),
                () => exportColumns.First().ShouldBe(SubscriptionIdColumnNonProductView));
        }

        private static IEnumerable<Tuple<string, string>> GetValidInputTestDataForProductView()
        {
            foreach (var columName in ColumnNamesForProductView)
            {
                yield return GetValidInputColumnAndExpectedOutputColumnName(columName);
            }
        }

        private static IEnumerable<Tuple<string, string>> GetInputTestDataForProductViewNoOutput()
        {
            foreach (var columName in ColumnNamesForProductViewNoOutput)
            {
                yield return GetValidInputColumnAndExpectedOutputColumnName(columName);
            }
        }

        private static IEnumerable<Tuple<string, string>> GetValidInputTestDataForNonProductView()
        {
            foreach (var columName in ColumnNamesForNonProductView)
            {
                yield return GetValidInputColumnAndExpectedOutputColumnName(columName, false);
            }
        }

        private static IEnumerable<Tuple<string, string>> GetInputTestDataForNonProductViewNoOutput()
        {
            foreach (var columName in ColumnNamesForNonProductViewNoOutput)
            {
                yield return GetValidInputColumnAndExpectedOutputColumnName(columName, false);
            }
        }

        private static Tuple<string, string> GetValidInputColumnAndExpectedOutputColumnName(string columnName, bool productView = true)
        {
            var columnReverse = $"{new string(columnName.Reverse().ToArray())}";
            var inputColumnName = $"{columnName}{PipeSeparator}{columnReverse}";

            string expectedExportColumn;

            if (productView)
            {
                expectedExportColumn = ExpectedExportColumnForProductView(columnName, columnReverse);
            }
            else
            {
                expectedExportColumn = ExpectedExportColumnForNonProductView(columnName, columnReverse);
            }

            return new Tuple<string, string>(inputColumnName, expectedExportColumn);
        }

        private static string BuildExportColumn(string prefix, string columnNamePartTwo)
        {
            return $"{prefix}{PipeSeparator}{columnNamePartTwo}";
        }

        private static string ExpectedExportColumnForProductView(string columnName, string columnNamePartTwo)
        {
            string expectedExportColumn = null;

            switch (columnName)
            {
                case InputNameScore:
                    expectedExportColumn = BuildExportColumn(ExportScore, columnNamePartTwo);
                    break;
                case InputNameAddress:
                    expectedExportColumn = BuildExportColumn(ExportAddress, columnNamePartTwo);
                    break;
                case InputNameRegionCode:
                    expectedExportColumn = BuildExportColumn(ExportRegionCode, columnNamePartTwo);
                    break;
                case InputNameZipCode:
                    expectedExportColumn = BuildExportColumn(ExportZipCode, columnNamePartTwo);
                    break;
                case InputNamePubTransactionDate:
                    expectedExportColumn = BuildExportColumn(ExportPubTransactionDate, columnNamePartTwo);
                    break;
                case InputNameQualificationDate:
                    expectedExportColumn =
                        BuildExportColumn(ExportQualifictionDate, columnNamePartTwo);
                    break;
                case InputNameEmailStatus:
                    expectedExportColumn = BuildExportColumn(ExportEmailStatus, columnNamePartTwo);
                    break;
                case InputNameCategoryId:
                    expectedExportColumn = BuildExportColumn(ExportCategoryId, columnNamePartTwo);
                    break;
                case InputNameTransactionStatus:
                    expectedExportColumn = BuildExportColumn(ExportTransactionStatus, columnNamePartTwo);
                    break;
                case InputNameTransactionId:
                    expectedExportColumn = BuildExportColumn(ExportTransactionId, columnNamePartTwo);
                    break;
                case InputNameQSourceId:
                    expectedExportColumn = BuildExportColumn(ExportQSourceId, columnNamePartTwo);
                    break;
                case InputNameParc3C:
                    expectedExportColumn = BuildExportColumn(ExportDisplayOrderAsParc3C, columnNamePartTwo);
                    break;
                case InputNameTransactionName:
                    expectedExportColumn = BuildExportColumn(ExportTransactionName, columnNamePartTwo);
                    break;
                case InputNameQSourceName:
                    expectedExportColumn = BuildExportColumn(ExportQSourceName, columnNamePartTwo);
                    break;
                case InputNameLastOpenEdDate:
                    break;
                case InputNameLastOpenPubCode:
                    break;
                case InputNameIgrpNo:
                case InputNameCgRpNo:
                    expectedExportColumn = BuildExportColumn($"s.{columnName}", columnNamePartTwo);
                    break;
                case InputNameBatch:
                    expectedExportColumn = BuildExportColumn(ExportBatch, columnNamePartTwo);
                    break;
                default:
                    expectedExportColumn = BuildExportColumn($"ps.{columnName}", columnNamePartTwo);
                    break;
            }

            return expectedExportColumn;
        }

        private static string ExpectedExportColumnForNonProductView(string columnName, string columnNamePartTwo)
        {
            string expectedExportColumn = null;

            switch (columnName)
            {
                case InputNameScore:
                    expectedExportColumn = BuildExportColumn(ExportScore, columnNamePartTwo);
                    break;
                case InputNameFName:
                    expectedExportColumn = BuildExportColumn(ExportFirstName, columnNamePartTwo);
                    break;
                case InputNameLName:
                    expectedExportColumn = BuildExportColumn(ExportLastName, columnNamePartTwo);
                    break;
                case InputNameLatLonValid:
                    expectedExportColumn = BuildExportColumn(ExportLatLonValid, columnNamePartTwo);
                    break;
                case InputNameCategoryId:
                    expectedExportColumn = BuildExportColumn(ExportCategoryId, columnNamePartTwo);
                    break;
                case InputNameTransactionStatus:
                    expectedExportColumn =
                        BuildExportColumn(ExportTransactionStatus, columnNamePartTwo);
                    break;
                case InputNameTransactionId:
                    expectedExportColumn = BuildExportColumn(ExportTransactionId, columnNamePartTwo);
                    break;
                case InputNameQSourceId:
                    expectedExportColumn = BuildExportColumn(ExportQSourceId, columnNamePartTwo);
                    break;
                case InputNameParc3C:
                    expectedExportColumn = BuildExportColumn(ExportParc3CNonProductView, columnNamePartTwo);
                    break;
                case InputNameTransactionName:
                    expectedExportColumn = BuildExportColumn(ExportTransactionName, columnNamePartTwo);
                    break;
                case InputNameQSourceName:
                    expectedExportColumn = BuildExportColumn(ExportQSourceName, columnNamePartTwo);
                    break;
                case InputNameLastOpenEdDate:
                    break;
                case InputNameLastOpenPubCode:
                    break;
                default:
                    expectedExportColumn = BuildExportColumn($"s.{columnName}", columnNamePartTwo);
                    break;
            }

            return expectedExportColumn;
        }
    }
}
