using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.ClientMethods;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TenMissionsTest
    {
        [Test]
        public void TMCompanyStartsWithAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TMCompanyStartsWithAdHocImport(f),
                    new[] { "Company Name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "starts_with",
                    false,
                    "99");
            }
        }

        [Test]
        public void TMCompanyStartsWith_AdHocImportWithSpecifics_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();
                List<AdHocDimensionGroupPubcodeMap> mapList = null;

                ShimAdHocDimensionGroupPubcodeMap.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroupPubcodeMap =
                    (_, maps) =>
                    {
                        mapList = maps;
                        return true;
                    };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TMCompanyStartsWithAdHocImport(f),
                    new[] { "Company Name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "starts_with",
                    true,
                    "99");

                mapList.ShouldNotBeNull();
                mapList.Count.ShouldBe(1);
                mapList.First().Pubcode.ShouldBe("PubCode_test");
            }
        }

        [Test]
        public void VehicleTypesServicedMatching_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.VehicleTypesServicedMatching(f),
                    new[] { "If term is anywhere in company name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "contains",
                    false,
                    string.Empty);
            }
        }

        [Test]
        public void VehicleTypesServicedMatching_AdHocDimensionWithSpecifics_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();
                List<AdHocDimensionGroupPubcodeMap> mapList = null;

                ShimAdHocDimensionGroupPubcodeMap.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroupPubcodeMap =
                    (_, maps) =>
                    {
                        mapList = maps;
                        return true;
                    };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.VehicleTypesServicedMatching(f),
                    new[] { "If term is anywhere in company name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "contains",
                    true,
                    string.Empty);

                mapList.ShouldNotBeNull();
                mapList.Count.ShouldBe(1);
                mapList.First().Pubcode.ShouldBe("PubCode_test");
            }
        }

        [Test]
        public void TMCompanyContainsAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TMCompanyContainsAdHocImport(f),
                    new[] { "Company Name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "contains",
                    false,
                    "99");
            }
        }

        [Test]
        public void TMCompanyContainsAdHocImport_AdHocImportWithSpecifics_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new TenMissions();
                List<AdHocDimensionGroupPubcodeMap> mapList = null;

                ShimAdHocDimensionGroupPubcodeMap.AllInstances.SaveBulkSqlInsertListOfAdHocDimensionGroupPubcodeMap =
                    (_, maps) =>
                    {
                        mapList = maps;
                        return true;
                    };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TMCompanyContainsAdHocImport(f),
                    new[] { "Company Name", "Master Code", "PubCode" },
                    new[] { "test", "CodeSheetValue_Test", "PubCode_test" },
                    "CodeSheetValue_Test",
                    "test",
                    "contains",
                    true,
                    "99");

                mapList.ShouldNotBeNull();
                mapList.Count.ShouldBe(1);
                mapList.First().Pubcode.ShouldBe("PubCode_test");
            }
        }
    }
}
