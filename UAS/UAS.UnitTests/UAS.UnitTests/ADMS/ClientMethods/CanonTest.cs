using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CanonTest
    {
        [Test]
        public void CanonLookupAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new CanonWrapper();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.CanonLookupAdHocImport(f.Client,f),
                    new[] { "company2", "company"},
                    new[] { "test", "CodeSheetValue_Test" },
                    "test",
                    "CodeSheetValue_Test",
                    "contains");
            }
        }

        [Test]
        public void CannonTop100AdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new CanonWrapper();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.CannonTop100AdHocImport(fileMoved.Client, fileMoved),
                    new[] { "Company" },
                    new[] { "test" },
                    "A",
                    "test",
                    "contains");
            }
        }
    }
}
