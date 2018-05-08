using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    /// Unit Tests for <see cref="ADMS_Validator.ToDataTable_SubscriberTransformed_DimensionErrors"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToDataTableSubscriberTransformedDimensionErrorsTest : Fakes
    {
        private const string SampleDemoColumn = "SampleDemoColumn";
        private const string SampleAccountNumber = "Account98765";
        private const string TitleField = "Title";
        private const string TitleValue1 = "SampleTitle1";
        private const string TitleValue2 = "SampleTitle2";
        private const string SamplePubCode = "SamplePubCode";
        private const string AccountNumberField = "AccountNumber";
        private const string PubCodeField = "PubCode";
        private const string ToDataTable_SubscriberTransformed_DimensionErrorsMethodName = "ToDataTable_SubscriberTransformed_DimensionErrors";
        private const string AddReturnDataTableRowsMethodName = "AddReturnDataTableRows";

        private TestEntity testEntity;
        private HashSet<SubscriberTransformed> subList;
        private List<string> incomingColumns;
        private List<string> tmpDemoColumns;
        private List<FieldMapping> fieldMappings;
        private List<ImportErrorSummary> summaryList;

        /// <summary>
        /// Test SetUp
        /// </summary>
        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            subList = new HashSet<SubscriberTransformed>();
            incomingColumns = new List<string>();
            tmpDemoColumns = new List<string> { SampleDemoColumn };
            fieldMappings = new List<FieldMapping>();
            summaryList = new List<ImportErrorSummary>();
        }

        /// <summary>
        /// <see cref="Validator.ToDataTable_SubscriberTransformed_DimensionErrors"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_DimensionErrors_WithSummaryList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    AccountNumber = SampleAccountNumber,
                    PubCode = SamplePubCode,
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                    {
                        new SubscriberDemographicTransformed
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        }
                    }
                }
            };
            summaryList.Add(new ImportErrorSummary
            {
                PubCode = SamplePubCode,
                MAFField = TitleField,
                Value = TitleValue1
            });
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings,
                summaryList,
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0][AccountNumberField]);
            Assert.AreEqual(SamplePubCode, resultDataTable.Rows[0][PubCodeField]);
            Assert.AreEqual(TitleValue1, resultDataTable.Rows[0][TitleField]);
        }

        /// <summary>
        /// <see cref="Validator.ToDataTable_SubscriberTransformed_DimensionErrors"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_DimensionErrors_WithMultipleDemographics_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            SetTestData(fieldMappings, incomingColumns);
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    AccountNumber = SampleAccountNumber,
                    PubCode = SamplePubCode,
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                    {
                        new SubscriberDemographicTransformed
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                        new SubscriberDemographicTransformed
                        {
                            MAFField = TitleField,
                            Value = TitleValue2
                        }
                    }
                }
            };
            summaryList.Add(new ImportErrorSummary
            {
                PubCode = SamplePubCode,
                MAFField = TitleField,
                Value = TitleValue1
            });
            
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings,
                summaryList,
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0][AccountNumberField]);
            Assert.AreEqual(SamplePubCode, resultDataTable.Rows[0][PubCodeField]);
            Assert.AreEqual(string.Join(",", TitleValue1, TitleValue2), resultDataTable.Rows[0][TitleField]);
        }

        /// <summary>
        /// <see cref="Validator.ToDataTable_SubscriberTransformed_DimensionErrors"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_DimensionErrors_WithEmptySummaryList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };

            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings,
                summaryList,
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(0, resultDataTable.Rows.Count);

        }

        [Test]
        public void ToDataTableSubscriberTransformedDimensionErrors_WhenSubListIsNull_ThrowsException()
        {
            // Arrange
            subList = null;
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings,
                summaryList
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberTransformedDimensionErrors_WhenIncomingColumnsIsNull_ThrowsException()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var parameters = new object[]
            {
                subList,
                incomingColumns = null,
                tmpDemoColumns,
                fieldMappings,
                summaryList
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberTransformedDimensionErrors_WhenTmpDemoColumnsIsNull_ThrowsException()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns = null,
                fieldMappings,
                summaryList
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberTransformedDimensionErrors_WhenFieldMappingsIsNull_ThrowsException()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings = null,
                summaryList
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void ToDataTableSubscriberTransformedDimensionErrors_WhenSummaryListIsNull_ThrowsException()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed {AccountNumber = SampleAccountNumber }
            };
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings,
                summaryList = null
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    ToDataTable_SubscriberTransformed_DimensionErrorsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void AddReturnDataTableRows_WhenIncomingColumnsIsNull_ThrowsException()
        {
            // Arrange
            var returnDataTable = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            var subscriberTransformed = new SubscriberTransformed();

            var parameters = new object[]
            {
                incomingColumns = null,
                fieldMappings,
                returnDataTable,
                properties,
                subscriberTransformed,
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    AddReturnDataTableRowsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void AddReturnDataTableRows_WhenFieldMappingsIsNull_ThrowsException()
        {
            // Arrange
            var returnDataTable = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            var subscriberTransformed = new SubscriberTransformed();

            var parameters = new object[]
            {
                incomingColumns,
                fieldMappings = null,
                returnDataTable,
                properties,
                subscriberTransformed,
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    AddReturnDataTableRowsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void AddReturnDataTableRows_WhenDataTableIsNull_ThrowsException()
        {
            // Arrange
            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            var subscriberTransformed = new SubscriberTransformed();

            var parameters = new object[]
            {
                incomingColumns,
                fieldMappings,
                null,
                properties,
                subscriberTransformed,
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    AddReturnDataTableRowsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void AddReturnDataTableRows_WhenPropertiesIsNull_ThrowsException()
        {
            // Arrange
            var returnDataTable = new DataTable();
            var subscriberTransformed = new SubscriberTransformed();

            var parameters = new object[]
            {
                incomingColumns,
                fieldMappings,
                returnDataTable,
                null,
                subscriberTransformed,
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    AddReturnDataTableRowsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        [Test]
        public void AddReturnDataTableRows_WhenSubscriberTransformedIsNull_ThrowsException()
        {
            // Arrange
            var returnDataTable = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));

            var parameters = new object[]
            {
                incomingColumns,
                fieldMappings,
                returnDataTable,
                properties,
                null,
            };

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                testEntity.Validator.GetType().CallMethod(
                    AddReturnDataTableRowsMethodName,
                    parameters,
                    testEntity.Validator));

            // Assert
            Assert.IsInstanceOf<ArgumentNullException>(exception.InnerException);
        }

        private void SetTestData(List<FieldMapping> mappings, List<string> incomingColumns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            foreach (PropertyDescriptor prop in properties)
            {
                incomingColumns.Add(prop.DisplayName);
                mappings.Add(new FieldMapping { MAFField = prop.DisplayName, IncomingField = prop.DisplayName });
            }
        }
    }
}
