using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Shouldly;
using Core.ADMS.Events;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Common.Import;
using KMPlatform.Entity;

namespace UAS.UnitTests.Helpers
{
    public class AdmsClientMethodsHelper
    {
        public static void TestAdHockMethod(
            Action<FileMoved> testMethod,
            string[] headers,
            string[] values,
            string dimensionValue,
            string matchValue,
            string operatorExpected,
            bool updateUADExpected = true)
        {
            TestAdHockMethod(testMethod, headers, values, dimensionValue, matchValue, operatorExpected, false, null, updateUADExpected);
        }

        public static DataTable BuildDataTableByParams(string[] headers, string[] values)
        {
            var dTable = new DataTable();
            dTable.Columns.AddRange(headers.Select(h => new DataColumn(h)).ToArray());
            dTable.Rows.Add(values);
            return dTable;
        }

        public static void TestAdHockMethod(string dimensionValue, Action<FileMoved> testMethod)
        {
            TestAdHockMethod(testMethod, new[] { "COMPANY" }, new[] { "Test" }, dimensionValue, "Test", "contains");
        }

        public static void TestAdHockMethod(
            Action<FileMoved> testMethod,
            string[] headers,
            string[] values,
            string dimensionValue,
            string matchValue,
            string operatorExpected,
            bool setPubCodeSpecific,
            string defaultValue,
            bool updateUADExpected = true)
        {
            TestAdHockMethod(
                testMethod,
                headers,
                values,
                new List<TestAdHockMethodAssertionTarget>()
                {
                    new TestAdHockMethodAssertionTarget()
                    {
                        DimensionValue = dimensionValue,
                        MatchValue = matchValue,
                        OperatorExpected = operatorExpected,
                        DefaultValueExpected = defaultValue,
                        UpdateUADExpected = updateUADExpected
                    }
                },
                setPubCodeSpecific);
        }

        public static void TestAdHockMethod(
            Action<FileMoved> testMethod,
            string[] headers,
            string[] values,
            IList<TestAdHockMethodAssertionTarget> targets,
            bool setPubCodeSpecific,
            FileConfiguration fileConfigExpected = null)
        {
            // Arrange
            FileConfiguration fileConfigurationForAssert = null;

            if (targets == null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            var targetsQueue = new Queue<TestAdHockMethodAssertionTarget>();
            var validationList = new List<KeyValuePair<TestAdHockMethodAssertionTarget, List<AdHocDimension>>>();
            foreach (var target in targets)
            {
                targetsQueue.Enqueue(target);
            }

            var currentTarget = targetsQueue.Dequeue();
            var firstDimensionGroupCall = true;
            var eventMessage = new FileMoved
            {
                SourceFile = new SourceFile(),
                ImportFile = new FileInfo("test.tst"),
                Client = new Client()
            };

            ShimAdHocDimension.AllInstances.DeleteInt32 = (_, __) => true;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (_, data) =>
            {
                validationList.Add(
                    new KeyValuePair<TestAdHockMethodAssertionTarget, List<AdHocDimension>>(
                        currentTarget, data
                    ));
                if (targetsQueue.Count > 0)
                {
                    currentTarget = targetsQueue.Dequeue();
                }

                return true;
            };
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (_, __, fileConfiguration) =>
            {
                fileConfigurationForAssert = fileConfiguration;
                return BuildDataTableByParams(headers, values);
            };
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (_, grp) =>
            {
                if (currentTarget.DefaultValueExpected != null)
                {
                    grp.DefaultValue.ShouldBe(currentTarget.DefaultValueExpected);
                }
                return true;
            };
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (grp, __, ___, _4, _5) =>
            {
                if (!firstDimensionGroupCall)
                {
                    return new AdHocDimensionGroup()
                    {
                        IsPubcodeSpecific = setPubCodeSpecific
                    };
                }

                firstDimensionGroupCall = false;
                return null;
            };

            // Act
            testMethod(eventMessage);

            // Assert
            foreach (var validationPair in validationList)
            {
                AssertAdHockMethodStageOnSave(validationPair.Value, validationPair.Key);
            }

            if (fileConfigExpected != null)
            {
                fileConfigurationForAssert.ShouldNotBeNull();
                fileConfigurationForAssert.FileExtension.ShouldBe(fileConfigExpected.FileExtension);
                fileConfigurationForAssert.FileColumnDelimiter.ShouldBe(fileConfigExpected.FileColumnDelimiter);
            }
        }

        private static void AssertAdHockMethodStageOnSave(List<AdHocDimension> result, TestAdHockMethodAssertionTarget target)
        {
            result.ShouldNotBeNull();
            var dimension = result.First();
            dimension.MatchValue.ShouldBe(target.MatchValue);
            dimension.Operator.ShouldBe(target.OperatorExpected);
            dimension.DimensionValue.ShouldBe(target.DimensionValue);
            dimension.CreatedByUserID.ShouldBe(1);
            dimension.UpdateUAD.ShouldBe(target.UpdateUADExpected);
        }
    }
}
