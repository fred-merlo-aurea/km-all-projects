using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.Entity;
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
    public class SetMafFieldRowsTest
    {
        private const string MafFieldKey = "MAFField";
        private const string SampleValue1 = "Sample Value 1";
        private const string SampleValue2 = "Sample Value 2";

        private HashSet<SubscriberDemographicTransformed> _demographicTransformedList;
        private HashSet<SubscriberDemographicInvalid> _demographicInvalidList;
        private DataTable _dataTable;
        private PrivateObject _validatorPrivateObject;
        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();

            _dataTable = new DataTable();
            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void SetMafFieldRowsForTransformedList_WhenDataRowNull_ThrowsException()
        {
            // Arrange
            _demographicTransformedList = new HashSet<SubscriberDemographicTransformed>();

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetMafFieldRowsForTransformedListMethod, null, _demographicTransformedList));
        }

        [Test]
        public void SetMafFieldRowsForTransformedList_WhenDemographicTransformedListIsNull_ThrowsException()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicTransformedList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetMafFieldRowsForTransformedListMethod, _dataTable.Rows[0], _demographicTransformedList));
        }

        [Test]
        public void SetMafFieldRowsForTransformedList_WhenDemographicTransformedListIsGreatherThanOne_ShouldAddValuesInRow()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicTransformedList = CreateDemographicTransformedList();
            var result = CreateResultForTransformedList();

            // Act
            _validatorPrivateObject.Invoke(SetMafFieldRowsForTransformedListMethod, _dataTable.Rows[0], _demographicTransformedList);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(result);
        }

        [Test]
        public void SetMafFieldRowsForTransformedList_WhenDemographicTransformedListIsNotGreatherThanOne_ShouldAddValuesInRow()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicTransformedList = new HashSet<SubscriberDemographicTransformed>
            {
                new SubscriberDemographicTransformed {MAFField = MafFieldKey, Value = SampleValue1}
            };

            var result = _demographicTransformedList.ElementAt(0).Value;

            // Act
            _validatorPrivateObject.Invoke(SetMafFieldRowsForTransformedListMethod, _dataTable.Rows[0], _demographicTransformedList);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(result);
        }

        [Test]
        public void SetMafFieldRowsForInvalidList_WhenDataRowNull_ThrowsException()
        {
            // Arrange
            _demographicInvalidList = new HashSet<SubscriberDemographicInvalid>();

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetMafFieldRowsForInvalidListMethod, null, _demographicInvalidList));
        }

        [Test]
        public void SetMafFieldRowsForInvalidList_WhenDemographicInvalidListIsNull_ThrowsException()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicInvalidList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetMafFieldRowsForInvalidListMethod, _dataTable.Rows[0], _demographicInvalidList));
        }

        [Test]
        public void SetMafFieldRowsForTransformedList_WhenDemographicInvalidListIsGreatherThanOne_ShouldAddValuesInRow()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicInvalidList = CreateDemographicInvalidList();
            var result = CreateResultForTransformedList();

            // Act
            _validatorPrivateObject.Invoke(SetMafFieldRowsForInvalidListMethod, _dataTable.Rows[0], _demographicInvalidList);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(result);
        }

        [Test]
        public void SetMafFieldRowsForInvalidList_WhenDemographicInvalidListIsNotGreatherThanOne_ShouldAddValuesInRow()
        {
            // Arrange
            _dataTable = CreateDataTableForTests();
            _demographicInvalidList = new HashSet<SubscriberDemographicInvalid>
            {
                new SubscriberDemographicInvalid {MAFField = MafFieldKey, Value = SampleValue1}
            };

            var result = _demographicInvalidList.ElementAt(0).Value;

            // Act
            _validatorPrivateObject.Invoke(SetMafFieldRowsForInvalidListMethod, _dataTable.Rows[0], _demographicInvalidList);

            // Assert
            _dataTable.Rows[0][MafFieldKey].ShouldBe(result);
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

        private static HashSet<SubscriberDemographicTransformed> CreateDemographicTransformedList()
        {
            return new HashSet<SubscriberDemographicTransformed>
            {
                new SubscriberDemographicTransformed { MAFField = MafFieldKey, Value = SampleValue1 },
                new SubscriberDemographicTransformed { MAFField = MafFieldKey, Value = SampleValue2 }
            };
        }

        private static HashSet<SubscriberDemographicInvalid> CreateDemographicInvalidList()
        {
            return new HashSet<SubscriberDemographicInvalid>
            {
                new SubscriberDemographicInvalid { MAFField = MafFieldKey, Value = SampleValue1 },
                new SubscriberDemographicInvalid { MAFField = MafFieldKey, Value = SampleValue2 }
            };
        }

        private string CreateResultForTransformedList()
        {
            return $"{_dataTable.Rows[0][MafFieldKey]},{SampleValue1},{SampleValue2}";
        }
    }
}
