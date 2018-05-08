using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///  Unit Tests for <see cref="ADMS_Validator.ToDataTable_SubscriberInvalid"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ToDataTableSubscriberInvalidTest : Fakes
    {
        private const string SampleDemoColumn = "SampleDemoColumn";
        private const string ToDataTableSubscriberInvalidMethodName = "ToDataTable_SubscriberInvalid";
        private const string TitleField = "Title";
        private const string TitleValue1 = "SampleTitle1";
        private const string TitleValue2 = "SampleTitle2";
        private const string SampleAccountNumber = "Account98765";

        private TestEntity testEntity;
        private List<SubscriberInvalid> dedupedInvalidList;
        private List<string> incomingColumns;
        private List<string> tmpDemoColumns;
        private List<FieldMapping> fieldMappings;

        /// <summary>
        /// Test Setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            dedupedInvalidList = new List<SubscriberInvalid>();
            incomingColumns = new List<string>();
            tmpDemoColumns = new List<string> { SampleDemoColumn };
            fieldMappings = new List<FieldMapping>();
        }

        /// <summary>
        /// <see cref="ADMS_Validator.ToDataTable_SubscriberInvalid"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberInvalid_WithOutDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            dedupedInvalidList = new List<SubscriberInvalid>
            {
                new SubscriberInvalid {AccountNumber= SampleAccountNumber}
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTableSubscriberInvalidMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberInvalid)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0]["AccountNumber"]);
        }

        /// <summary>
        /// <see cref="ADMS_Validator.ToDataTable_SubscriberInvalid"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberInvalid_WithDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            dedupedInvalidList = new List<SubscriberInvalid>
            {
                new SubscriberInvalid
                {
                    AccountNumber = SampleAccountNumber,
                    DemographicInvalidList = new HashSet<SubscriberDemographicInvalid>
                    {
                        new SubscriberDemographicInvalid
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                        new SubscriberDemographicInvalid
                        {
                            MAFField = TitleField,
                            Value= TitleValue2
                        },
                    }
                }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTableSubscriberInvalidMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberInvalid)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0]["AccountNumber"]);
            Assert.AreEqual(string.Join(",", TitleValue1, TitleValue2), resultDataTable.Rows[0]["Title"]);
        }

        [Test]
        public void ToDataTableSubscriberInvalid_WhenDedupedInvalidListIsNull_ThrowsException()
        {
            // Arrange
            dedupedInvalidList = CreateDedupedInvalidList() as List<SubscriberInvalid>;
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList = null,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTableSubscriberInvalidMethodName,parameters,testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberInvalid_WhenIncomingColumnsIsNull_ThrowsException()
        {
            // Arrange
            dedupedInvalidList = CreateDedupedInvalidList() as List<SubscriberInvalid>;
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList,
                incomingColumns = null,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTableSubscriberInvalidMethodName, parameters, testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberInvalid_WhenDemoColumnsIsNull_ThrowsException()
        {
            // Arrange
            dedupedInvalidList = CreateDedupedInvalidList() as List<SubscriberInvalid>;
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList,
                incomingColumns,
                tmpDemoColumns = null,
                fieldMappings
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTableSubscriberInvalidMethodName, parameters, testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberInvalid_WhenFieldMappingsIsNull_ThrowsException()
        {
            // Arrange
            dedupedInvalidList = CreateDedupedInvalidList() as List<SubscriberInvalid>;
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                dedupedInvalidList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings = null
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTableSubscriberInvalidMethodName, parameters, testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        private static IList<SubscriberInvalid> CreateDedupedInvalidList()
        {
            return new List<SubscriberInvalid>
            {
                new SubscriberInvalid
                {
                    AccountNumber = SampleAccountNumber,
                    DemographicInvalidList = new HashSet<SubscriberDemographicInvalid>
                    {
                        new SubscriberDemographicInvalid
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                        new SubscriberDemographicInvalid
                        {
                            MAFField = TitleField,
                            Value = TitleValue2
                        },
                    }
                }
            };
        }

        private void SetTestData(List<FieldMapping> mappings, List<string> incomingColumns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SubscriberInvalid));
            foreach (PropertyDescriptor prop in properties)
            {
                incomingColumns.Add(prop.DisplayName);
                mappings.Add(new FieldMapping { MAFField = prop.DisplayName, IncomingField = prop.DisplayName });
            }
        }
    }
}
