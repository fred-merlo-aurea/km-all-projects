using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MeisterTest
    {
        [Test]
        public void CventOptOuts_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new MeisterWrapper();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => 
                        testObject.CventOptOuts(f.Client, f.SourceFile, null, f),
                    new[] { "Email Address" },
                    new[] { "test@test.com" },
                    dimensionValue: string.Empty,
                    matchValue: "test@test.com",
                    operatorExpected: "equal",
                    updateUADExpected: false);
            }
        }

        [Test]
        public void FipsCounty_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new MeisterWrapper();

                ShimAdHocDimensionGroupPubcodeMap.AllInstances.SaveAdHocDimensionGroupPubcodeMap = (_, __) => true;

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f =>
                        testObject.FipsCounty(f.Client, f.SourceFile, null, f),
                    new[] { "County", "FIPS" },
                    new[] { "test", "CodeSheetValue_Test" },
                    dimensionValue: "test",
                    matchValue: "CodeSheetValue_Test",
                    operatorExpected: "equal",
                    updateUADExpected: false);
            }
        }

        [Test]
        public void RemoveBadPhoneNumber_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new MeisterWrapper();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f =>
                        testObject.RemoveBadPhoneNumber(f.Client, f.SourceFile, null, f),
                    new[] { "HomePhone_Bad"},
                    new[] { "test" },
                    dimensionValue: string.Empty,
                    matchValue: "test",
                    operatorExpected: "equal",
                    updateUADExpected: false);
            }
        }
    }
}
