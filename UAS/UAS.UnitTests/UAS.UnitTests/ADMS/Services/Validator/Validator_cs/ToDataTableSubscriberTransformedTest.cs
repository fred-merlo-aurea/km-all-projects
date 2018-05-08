using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Data;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    /// Unit Tests for <see cref="ADMS_Validator.ToDataTable_SubscriberTransformed"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToDataTableSubscriberTransformedTest : Fakes
    {
        private const string SampleDemoColumn = "SampleDemoColumn";
        private const string SampleAccountNumber = "Account98765";
        private const string TitleField = "Title";
        private const string TitleValue1 = "SampleTitle1";
        private const string TitleValue2 = "SampleTitle2";
        private const string AccountNumberField = "AccountNumber";
        private const string ToDataTable_SubscriberTransformedMethodName = "ToDataTable_SubscriberTransformed";

        private TestEntity testEntity;
        private HashSet<SubscriberTransformed> subList;
        private List<string> incomingColumns;
        private List<string> tmpDemoColumns;
        private List<FieldMapping> fieldMappings;

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
        }

        /// <summary>
        /// <see cref="Validator.ToDataTable_SubscriberTransformed"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_WithOutDemographicTransformedList_ReturnsDataTableWithDefaultValues()
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
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformedMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0][AccountNumberField]);
        }

        // <summary>
        /// <see cref="ADMS_Validator.ToDataTable_SubscriberTransformedMethodName"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_WithDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    AccountNumber = SampleAccountNumber,
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
                            Value= TitleValue2
                        },
                    }
                }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformedMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0][AccountNumberField]);
            Assert.AreEqual(string.Join(",", TitleValue1, TitleValue2), resultDataTable.Rows[0][TitleField]);
        }

        // <summary>
        /// <see cref="ADMS_Validator.ToDataTable_SubscriberTransformedMethodName"/>
        /// </summary>
        [Test]
        public void ToDataTable_SubscriberTransformed_WithSingleDemographicTransformedList_ReturnsDataTableWithDefaultValues()
        {
            // Arrange
            subList = new HashSet<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    AccountNumber = SampleAccountNumber,
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                    {
                        new SubscriberDemographicTransformed
                        {
                            MAFField = TitleField,
                            Value = TitleValue1
                        },
                    }
                }
            };
            SetTestData(fieldMappings, incomingColumns);
            var parameters = new object[]
            {
                subList,
                incomingColumns,
                tmpDemoColumns,
                fieldMappings
            };

            // Act
            var resultDataTable = (DataTable)ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                ToDataTable_SubscriberTransformedMethodName,
                parameters,
                testEntity.Validator);

            // Assert
            Assert.IsNotNull(resultDataTable);
            Assert.AreEqual(
                TypeDescriptor.GetProperties(typeof(SubscriberTransformed)).Count,
                resultDataTable.Columns.Count);
            Assert.AreEqual(1, resultDataTable.Rows.Count);
            Assert.AreEqual(SampleAccountNumber, resultDataTable.Rows[0][AccountNumberField]);
            Assert.AreEqual(TitleValue1, resultDataTable.Rows[0][TitleField]);
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
