using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.UnitTests.BusinessLogic.Common;
using NUnit.Framework;
using Shouldly;
using ObjectCircImportExportTest = FrameworkUAD.Object.CircImportExport;

namespace FrameworkUAD.UnitTests.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CircImportExportTest : Fakes
    {
        private const int CountryId = 1;
        private const string Plus4CodeTest = "";
        private readonly CircImportExport _circImportExport = new CircImportExport();

        [SetUp]
        public void Initialize()
        {
            SetupFakes();
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("000-1")]
        [TestCase("0")]
        [TestCase("01")]
        [TestCase("0123")]
        [TestCase("01234")]
        [TestCase("012345678")]
        [TestCase("0123456789")]
        public void FormatZipCode_WhenParameterIsCircImportExport_ReturnsCorrectedValues(string zipCode)
        {
            // Arrange
            VerifyZipCodeTransformation(zipCode, Plus4CodeTest, out var zipCodeResult, out var plus4Result);
            var circImportExport = SetCircImportExport(CountryId, CountryUnitedStates, zipCode);

            // Act
            var result = _circImportExport.FormatZipCode(circImportExport);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Country.ShouldBe(CountryUnitedStates),
                () => result.ZipCode.ShouldBe(zipCodeResult),
                () => result.Plus4.ShouldBe(plus4Result));
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("000-1")]
        [TestCase("0")]
        [TestCase("01")]
        [TestCase("0123")]
        [TestCase("01234")]
        [TestCase("012345678")]
        [TestCase("0123456789")]
        public void FormatZipCode_WhenParameterIsCircImportExportList_ReturnsCorrectedValues(string zipCode)
        {
            // Arrange
            VerifyZipCodeTransformation(zipCode, Plus4CodeTest, out var zipCodeResult, out var plus4Result);
            var circImportExportList = new List<ObjectCircImportExportTest>
            {
                SetCircImportExport(CountryId, CountryUnitedStates, zipCode)
            };

            // Act
            var result = _circImportExport.FormatZipCode(circImportExportList);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result[0].Country.ShouldBe(CountryUnitedStates),
                () => result[0].ZipCode.ShouldBe(zipCodeResult),
                () => result[0].Plus4.ShouldBe(plus4Result));
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("000-1")]
        [TestCase("0")]
        [TestCase("01")]
        [TestCase("0123")]
        [TestCase("01234")]
        [TestCase("012345678")]
        [TestCase("0123456789")]
        public void FormatZipCode_WhenParameterIsDataTable_ReturnsCorrectedValues(string zipCode)
        {
            // Arrange
            SetDataTableColumns(CountryId, CountryUnitedStates, zipCode);
            VerifyZipCodeTransformation(zipCode, Plus4CodeTest, out var zipCodeResult, out var plus4Result);

            // Act
            var result = _circImportExport.FormatZipCode(DataTable);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => GetValueByKeyFromDatable(result, 0, CountryKey).ShouldBe(CountryUnitedStates),
                () => GetValueByKeyFromDatable(result, 0, ZipCodeKey).ShouldBe(zipCodeResult),
                () => GetValueByKeyFromDatable(result, 0, Plus4Key).ShouldBe(plus4Result));
        }

        private ObjectCircImportExportTest SetCircImportExport(int countryId, string countryName, string zipCode)
        {
            return new ObjectCircImportExportTest
            {
                CountryID = countryId,
                Country = countryName,
                ZipCode = zipCode,
                Plus4 = string.Empty
            };
        }
    }
}
