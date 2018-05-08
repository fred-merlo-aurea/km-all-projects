using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using ADMS.ClientMethods;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StagnitoTest
    {
        [Test]
        public void EnsambleCompanyIDAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new StagnitoWrapper();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(
                    fileMoved => testObject.EnsambleCompanyIDAdHocImport(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                    new[] { "CompanyID", "Match Name", "Match Domain" },
                    new[] { "Title_TEST", "NewTitle_TEST", "domain_test" },
                    "Title_TEST",
                    "NewTitle_TEST:domain_test",
                    "equal");
            }
        }

        [Test]
        public void ViolaCompanyIDAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new StagnitoWrapper();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(
                    fileMoved => testObject.ViolaCompanyIDAdHocImport(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                    new[] { "Company_ID", "company_name", "domain" },
                    new[] { "Title_TEST", "NewTitle_TEST", "domain_test" },
                    "Title_TEST",
                    "NewTitle_TEST:domain_test",
                    "equal");
            }
        }
    }
}
