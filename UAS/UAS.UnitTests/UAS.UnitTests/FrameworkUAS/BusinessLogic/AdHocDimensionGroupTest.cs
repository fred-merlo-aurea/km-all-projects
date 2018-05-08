using System;
using System.Collections.Generic;
using FrameworkUAS.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using AdHocDimensionGroup = FrameworkUAS.BusinessLogic.AdHocDimensionGroup;
using BusinessLogicFakes = FrameworkUAS.BusinessLogic.Fakes;
using EntityAdHocDimension = FrameworkUAS.Entity.AdHocDimension;
using EntityAdHocDimensionGroup = FrameworkUAS.Entity.AdHocDimensionGroup;
using EntityAdHocDimensionGroupPubcodeMap = FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap;

namespace UAS.UnitTests.FrameworkUAS.BusinessLogic
{
    [TestFixture]
    public class AdHocDimensionGroupTest
    {
        private const string TestDimensionGroupName = "TestGroupName";
        private const int TestDimensionGroupId = 42;
        private const int TestDimensionSourceFileId = 7027;

        private IDisposable shims;

        [SetUp]
        public void SetUp()
        {
            shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            shims?.Dispose();
        }

        [Test]
        public void SelectByAdHocDimensionGroupId_DoNotIncludeCustomProperties_Success()
        {
            // Arrange
            SetupShims();

            // Act
            var dimensionGroup = new AdHocDimensionGroup();
            var entityDimensionGroup = dimensionGroup.SelectByAdHocDimensionGroupId(TestDimensionGroupId);

            // Assert
            entityDimensionGroup.ShouldSatisfyAllConditions(
                () => entityDimensionGroup.AdHocDimensionGroupId.ShouldBe(TestDimensionGroupId),
                () => entityDimensionGroup.AdHocDimensionGroupName.ShouldBeEmpty(),
                () => entityDimensionGroup.AdHocDimensions.Count.ShouldBe(0),
                () => entityDimensionGroup.DimensionGroupPubcodeMappings.Count.ShouldBe(0));
        }

        [Test]
        public void SelectByAdHocDimensionGroupId_IncludeCustomProperties_Success()
        {
            // Arrange
            SetupShims();

            // Act
             var dimensionGroup = new AdHocDimensionGroup();
             var entityDimensionGroup = dimensionGroup.SelectByAdHocDimensionGroupId(TestDimensionGroupId, true);

            // Assert
            entityDimensionGroup.ShouldSatisfyAllConditions(
                () => entityDimensionGroup.AdHocDimensionGroupId.ShouldBe(TestDimensionGroupId),
                () => entityDimensionGroup.AdHocDimensionGroupName.ShouldBeEmpty(),
                () => entityDimensionGroup.AdHocDimensions.Count.ShouldBe(2),
                () => entityDimensionGroup.DimensionGroupPubcodeMappings.Count.ShouldBe(3));
        }

        [Test]
        public void SelectBySourceFileId_DoNotIncludeCustomProperties_Success()
        {
            // Arrange
            SetupShims();

            // Act
            var dimensionGroup = new AdHocDimensionGroup();
            var entityDimensionGroup = dimensionGroup.Select(TestDimensionGroupId, TestDimensionSourceFileId, TestDimensionGroupName);

            // Assert
            entityDimensionGroup.ShouldSatisfyAllConditions(
                () => entityDimensionGroup.AdHocDimensionGroupId.ShouldBe(TestDimensionGroupId),
                () => entityDimensionGroup.AdHocDimensionGroupName.ShouldBe(TestDimensionGroupName),
                () => entityDimensionGroup.SourceFileID.ShouldBe(TestDimensionSourceFileId),
                () => entityDimensionGroup.AdHocDimensions.Count.ShouldBe(0),
                () => entityDimensionGroup.DimensionGroupPubcodeMappings.Count.ShouldBe(0));
        }

        [Test]
        public void SSelectBySourceFileId_IncludeCustomProperties_Success()
        {
            // Arrange
            SetupShims();

            // Act
             var dimensionGroup = new AdHocDimensionGroup();
             var entityDimensionGroup = dimensionGroup.Select(TestDimensionGroupId, TestDimensionSourceFileId, TestDimensionGroupName, true);

            // Assert
            entityDimensionGroup.ShouldSatisfyAllConditions(
                () => entityDimensionGroup.AdHocDimensionGroupId.ShouldBe(TestDimensionGroupId),
                () => entityDimensionGroup.AdHocDimensionGroupName.ShouldBe(TestDimensionGroupName),
                () => entityDimensionGroup.SourceFileID.ShouldBe(TestDimensionSourceFileId),
                () => entityDimensionGroup.AdHocDimensions.Count.ShouldBe(2),
                () => entityDimensionGroup.DimensionGroupPubcodeMappings.Count.ShouldBe(3));
        }

        private static void SetupShims()
        {
            ShimAdHocDimensionGroup.SelectByAdHocDimensionGroupIdInt32 = adHocDimensionGroupId =>
                {
                    var dimensionGroup = new EntityAdHocDimensionGroup
                                             {
                                                 AdHocDimensionGroupId = adHocDimensionGroupId,
                                             };

                    return dimensionGroup;
                };

            ShimAdHocDimensionGroup.SelectInt32Int32String = (groupId, sourceFileId, groupName) =>
                {
                    var dimensionGroup = new EntityAdHocDimensionGroup
                                             {
                                                 AdHocDimensionGroupId = groupId,
                                                 AdHocDimensionGroupName = groupName,
                                                 SourceFileID = sourceFileId
                    };

                    return dimensionGroup;
                };

            BusinessLogicFakes.ShimAdHocDimension.AllInstances.SelectInt32 = (adHocDimension, groupId) =>
                {
                    var adHocDimensions =
                        new List<EntityAdHocDimension> { new EntityAdHocDimension(), new EntityAdHocDimension() };

                    return adHocDimensions;
                };

            BusinessLogicFakes.ShimAdHocDimensionGroupPubcodeMap.AllInstances.SelectInt32 = (adHocDimension, groupId) =>
                {
                    var adHocDimensionsGroupPubcodeMaps =
                        new List<EntityAdHocDimensionGroupPubcodeMap>
                            {
                                new EntityAdHocDimensionGroupPubcodeMap(),
                                new EntityAdHocDimensionGroupPubcodeMap(),
                                new EntityAdHocDimensionGroupPubcodeMap(),
                            };

                    return adHocDimensionsGroupPubcodeMaps;
                };
        }
    }
}
