using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SetIncomeFieldTests
    {
        private const string MafFieldKey = "MAFField";
        private const string TextValue1 = "Value1";
        private const string TextValue2 = "Value2";
        private const string TextValue3 = "Value3";

        private IList<string> _incomingColumns;
        private DataTable _dataTable;
        private FieldMapping _fieldMapping;
        private PropertyDescriptor _property;
        private SubscriberInvalid _subscriberInvalid;
        private SubscriberTransformed _subscriberTransformed;
        private PrivateObject _validatorPrivateObject;
        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));

            _dataTable = CreateDataTableForTests();
            _fieldMapping = new FieldMapping();
            _incomingColumns = new List<string>();
            _subscriberInvalid = new SubscriberInvalid();
            _subscriberTransformed = new SubscriberTransformed();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void SetIncomeField_WhenIncomingColumnsIsNull_ThrowsException()
        {
            // Arrange
            _incomingColumns = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberTransformed, _dataTable.Rows[0], _property, _fieldMapping));
        }

        [Test]
        public void SetIncomeField_WhenSubscriberItemIsNull_ThrowsException()
        {
            // Arrange
            _subscriberTransformed = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberTransformed, _dataTable.Rows[0], _property, _fieldMapping));
        }

        [Test]
        public void SetIncomeField_WhenRowIsNull_ThrowsException()
        {
            // Arrange
            _property = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberInvalid, null, _property, _fieldMapping));
        }

        [Test]
        public void SetIncomeField_WhenPropertyIsNull_ThrowsException()
        {
            // Arrange
            _property = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberInvalid, _dataTable.Rows[0], _property, _fieldMapping));
        }

        [Test]
        public void SetIncomeField_WhenFieldMappingIsNull_ThrowsException()
        {
            // Arrange
            _fieldMapping = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberInvalid, _dataTable.Rows[0], _property, _fieldMapping));
        }

        [Test]
        public void SetIncomeField_WhenIncomingFieldExistsInIncomingColumns_UpdatesRow()
        {
            // Arrange
            _incomingColumns = CreateIncomingColumnsList();
            _fieldMapping = new FieldMapping { IncomingField = MafFieldKey };
            _subscriberTransformed.Title = "Sample Title";

            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            _property = properties.Find("Title", true);

            // Act
            _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberTransformed, _dataTable.Rows[0], _property, _fieldMapping);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(_subscriberTransformed.Title);
        }

        [Test]
        public void SetIncomeField_WhenIncomingFieldValueIsNull_UpdatesRow()
        {
            // Arrange
            _incomingColumns = CreateIncomingColumnsList();
            _fieldMapping = new FieldMapping { IncomingField = MafFieldKey };
            _subscriberTransformed.TransactionDate = null;

            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            _property = properties.Find("TransactionDate", true);

            // Act
            _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberTransformed, _dataTable.Rows[0], _property, _fieldMapping);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(string.Empty);
        }

        [Test]
        public void SetIncomeField_WhenIncomingFieldIsDateTime_UpdatesRow()
        {
            // Arrange
            _incomingColumns = CreateIncomingColumnsList();
            _fieldMapping = new FieldMapping { IncomingField = MafFieldKey };
            _subscriberTransformed.TransactionDate = DateTime.Now;

            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            _property = properties.Find("TransactionDate", true);
            
            var resultDateTime = Convert.ToDateTime(_subscriberTransformed.TransactionDate.ToString());

            // Act
            _validatorPrivateObject.Invoke(SetIncomeFieldMethod, _incomingColumns, _subscriberTransformed, _dataTable.Rows[0], _property, _fieldMapping);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(resultDateTime.ToShortDateString());
        }

        private static DataTable CreateDataTableForTests()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(MafFieldKey, typeof(string));

            var dataRow1 = dataTable.NewRow();
            dataRow1[MafFieldKey] = MafFieldKey;
            dataTable.Rows.Add(dataRow1);

            return dataTable;
        }

        private static IList<string> CreateIncomingColumnsList()
        {
            return new List<string>
            {
                MafFieldKey, TextValue1, TextValue2, TextValue3
            };
        }
    }
}
