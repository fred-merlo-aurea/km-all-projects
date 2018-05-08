using System.Diagnostics.CodeAnalysis;
using ADMS.ClientMethods;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LebharTest
    {
        [Test]
        public void TitleAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Lebhar();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TitleAdHocImport(f.Client, f),
                    new[] { "TITLE_CODE", "TITLE" },
                    new[] { "test", "CodeSheetValue_Test" },
                    dimensionValue: "test",
                    matchValue: "CodeSheetValue_Test",
                    operatorExpected: "contains");
            }
        }

        [Test]
        public void CompanyAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Lebhar();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.CompanyAdHocImport(f.Client, f),
                    new[] { "RANK", "COMPANY" },
                    new[] { "test", "CodeSheetValue_Test" },
                    dimensionValue: "test",
                    matchValue: "CodeSheetValue_Test",
                    operatorExpected: "contains");
            }
        }
    }
}
