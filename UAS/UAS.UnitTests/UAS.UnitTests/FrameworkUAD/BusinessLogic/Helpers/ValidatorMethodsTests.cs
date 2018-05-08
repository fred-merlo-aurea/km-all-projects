using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic.Helpers;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ImportFileObject = FrameworkUAD.Object.ImportFile;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ValidatorMethodsTests
    {
        private const char CommaJoin = ',';
        private const string CommaSeparator = ",";
        private const string DefaultDimensionValue = "10";
        private const string DefaultMatchValue = "1";
        private const string DummyText = "Dummy Text";
        private const string SampleText = "Sample Text";
        private const string QDateColumnHeaderKey = "QDateColumnHeader";
        private const string OriginalImportrowKey = "originalimportrow";

        private List<AdHocDimension> _adHocList;
        private int _matchValue;
        private int _sourceInt;
        private string _sourceString;
        private string _generatedValue;
        private bool _isTypeAnd;

        private StringDictionary _rowQDate;
        private string _qDateFormat;
        private string _qDatePattern2;
        private string _qDateValue;

        private Dictionary<int, int> _keyRest;
        private int _newKey;

        private ImportErrorEntity _importErrorEntity;
        private ImportFileObject _importFile;
        private StringBuilder _sbBadData;
        private StringDictionary _rowData;
        private int _rowNumber;
        private string _errorMsg;
        private bool _insertSeparator;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();

            _adHocList = new List<AdHocDimension>();
            _matchValue = ConvertDefaultMatchValueToInt();
            _sourceInt = 0;
            _sourceString = SampleText;
            _generatedValue = string.Empty;
            _isTypeAnd = false;

            _rowQDate = new StringDictionary();
            _qDateFormat = "DDMMYYYY";
            _qDatePattern2 = "dd/MM/yyyy hh:mm:ss";
            _qDateValue = string.Empty;

            _keyRest = new Dictionary<int, int>();
            _newKey = 10;

            _importErrorEntity = new ImportErrorEntity();
            _importFile = new ImportFileObject();
            _sbBadData = new StringBuilder();
            _rowData = new StringDictionary();
            _rowNumber = 0;
            _errorMsg = string.Empty;
            _insertSeparator = false;
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Dispose();
        }

        [Test]
        public void ValidatorMethods_WhenAdHocDimensionListIsNull_ThrowsException()
        {
            // Arrange
            _adHocList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ValidatorMethods.SetGeneratedValue(_adHocList, _sourceInt, _sourceString, _generatedValue, _isTypeAnd));
        }

        [Test]
        public void ValidatorMethods_WhenSourceStringIsNullOrEmpty_ThrowsException()
        {
            // Arrange
            _sourceString = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ValidatorMethods.SetGeneratedValue(_adHocList, _sourceInt, _sourceString, _generatedValue, _isTypeAnd));
        }

        [Test]
        [TestCase(0, "greater_than")]
        [TestCase(10, "greater_than")]
        public void ValidatorMethods_WhenOperatorNameIsGreatherThan_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt > _matchValue ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "less_than")]
        [TestCase(10, "less_than")]
        public void ValidatorMethods_WhenOperatorNameIsLessThan_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt < _matchValue ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "greater_than_or_equal_to")]
        [TestCase(1, "greater_than_or_equal_to")]
        public void ValidatorMethods_WhenOperatorNameIsGreatherThanOrEqualTo_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt >= _matchValue ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "less_than_or_equal_to")]
        [TestCase(10, "less_than_or_equal_to")]
        public void ValidatorMethods_WhenOperatorNameIsLessThanOrEqualTo_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt <= _matchValue ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "is_not_less_than")]
        [TestCase(10, "is_not_less_than")]
        public void ValidatorMethods_WhenOperatorNameIsNotLessThan_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = !(sourceInt < _matchValue) ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }


        [Test]
        [TestCase(0, "is_not_greater_than")]
        [TestCase(10, "is_not_greater_than")]
        public void ValidatorMethods_WhenOperatorNameIsNotGreatherThan_ReturnsGeneratedValue(int sourceInt, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = !(sourceInt > _matchValue) ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "0", "equal")]
        [TestCase(1, "1", "equal")]
        [TestCase(0, "0", "equal")]
        [TestCase(1, "1", "equal")]
        public void ValidatorMethods_WhenOperatorNameIsEqualsAndIsTypeAndIsTrue_ReturnsGeneratedValue(
            int sourceInt, 
            string sourceString, 
            string operatorName)
        {
            // Arrange
            _isTypeAnd = true;
            _sourceString = sourceString;
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt == _matchValue && _sourceString.Equals(_matchValue.ToString(), StringComparison.CurrentCultureIgnoreCase) 
                ? _adHocList?.ElementAt(0).DimensionValue 
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "0", "equal")]
        [TestCase(1, "1", "equal")]
        [TestCase(0, "0", "equal")]
        [TestCase(1, "1", "equal")]
        public void ValidatorMethods_WhenOperatorNameIsEqualsAndIsTypeAndIsFalse_ReturnsGeneratedValue(
            int sourceInt, 
            string sourceString, 
            string operatorName)
        {
            // Arrange
            _isTypeAnd = false;
            _sourceString = sourceString;
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt == _matchValue || _matchValue.ToString().Equals(sourceString, StringComparison.CurrentCultureIgnoreCase) 
                ? _adHocList?.ElementAt(0).DimensionValue 
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "0", "not_equal")]
        [TestCase(1, "1", "not_equal")]
        [TestCase(0, "0", "not_equal")]
        [TestCase(1, "1", "not_equal")]
        public void ValidatorMethods_WhenOperatorNameIsNotEqualsAndIsTypeAndIsTrue_ReturnsGeneratedValue(
            int sourceInt,
            string sourceString,
            string operatorName)
        {
            // Arrange
            _isTypeAnd = true;
            _sourceString = sourceString;
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt != _matchValue && !_sourceString.Equals(_matchValue.ToString(), StringComparison.CurrentCultureIgnoreCase)
                ? _adHocList?.ElementAt(0).DimensionValue
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase(0, "0", "not_equal")]
        [TestCase(1, "1", "not_equal")]
        [TestCase(0, "0", "not_equal")]
        [TestCase(1, "1", "not_equal")]
        public void ValidatorMethods_WhenOperatorNameIsNotEqualsAndIsTypeAndIsFalse_ReturnsGeneratedValue(
            int sourceInt,
            string sourceString,
            string operatorName)
        {
            // Arrange
            _isTypeAnd = false;
            _sourceString = sourceString;
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            var generatedValue = sourceInt != _matchValue || !_sourceString.Equals(_matchValue.ToString(), StringComparison.CurrentCultureIgnoreCase)
                ? _adHocList?.ElementAt(0).DimensionValue
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase("SampleText", "contains")]
        [TestCase("Sample Text", "contains")]
        public void ValidatorMethods_WhenOperatorNameIsContains_ReturnsGeneratedValue(string sourceString, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            _sourceString = sourceString;
            var generatedValue = _sourceString.ToLower().Contains(SampleText.ToLower()) 
                ? _adHocList?.ElementAt(0).DimensionValue 
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, _sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase("Sample", "starts_with")]
        [TestCase("Sample T", "starts_with")]
        public void ValidatorMethods_WhenOperatorNameIsStartsWith_ReturnsGeneratedValue(string sourceString, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            _sourceString = sourceString;
            var generatedValue = _sourceString.ToLower().StartsWith(SampleText.ToLower()) 
                ? _adHocList?.ElementAt(0).DimensionValue 
                : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, _sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        [TestCase("Text", "ends_with")]
        [TestCase(" Text", "ends_with")]
        public void ValidatorMethods_WhenOperatorNameIsEndsWith_ReturnsGeneratedValue(string sourceString, string operatorName)
        {
            // Arrange
            _adHocList = CreateAdHocListWithSpecificElement(operatorName) as List<AdHocDimension>;
            _sourceString = sourceString;
            var generatedValue = sourceString.ToLower().EndsWith(SampleText.ToLower()) ? _adHocList?.ElementAt(0).DimensionValue : string.Empty;

            // Act
            var result = ValidatorMethods.SetGeneratedValue(_adHocList, _sourceInt, _sourceString, _generatedValue, _isTypeAnd);

            // Assert
            result.ShouldBe(generatedValue);
        }

        [Test]
        public void TrySetDateColumnHeader_WhenStringDictionaryIsNull_ThrowsException()
        {
            // Arrange
            _rowQDate = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ValidatorMethods.TryParseDateColumnHeader(_rowQDate, _qDateFormat, QDateColumnHeaderKey, _qDatePattern2, _qDateValue));
        }

        [Test]
        public void TrySetDateColumnHeader_WhenQDateValueIsDifferentToQDateConvertValue_ReturnsFalse()
        {
            // Arrange
            _qDateValue = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            _rowQDate = CreateQDateDictionary(_qDateValue);
            var sampleData = GetSampleDateResult(_qDateValue);

            // Act
            var result = ValidatorMethods.TryParseDateColumnHeader(
                _rowQDate, _qDateFormat, QDateColumnHeaderKey, _qDatePattern2, _qDateValue) as bool?;

            // Arrange
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Value.ShouldBeFalse(),
                () => _rowQDate[QDateColumnHeaderKey].ShouldBe(sampleData.ToShortDateString()));
        }

        [Test]
        public void TrySetDateColumnHeader_WhenQDateValueIsEqualsToQDateConvertValue_Returnstrue()
        {
            // Arrange
            _qDateValue = new DateTime().ToString(CultureInfo.CurrentCulture);
            _rowQDate = CreateQDateDictionary(_qDateValue);

            // Act
            var result = ValidatorMethods.TryParseDateColumnHeader(
                _rowQDate, _qDateFormat, QDateColumnHeaderKey, _qDatePattern2, _qDateValue) as bool?;

            // Arrange
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Value.ShouldBeTrue(),
                () => _rowQDate[QDateColumnHeaderKey].ShouldBe(_qDateValue));
        }

        [Test]
        public void ResetKeyValuesOnTransformedDictionary_WhenImportFileObjectIsNotNull_ShouldChangeKeys()
        {
            // Arrange
            _importFile.DataTransformed = new Dictionary<int, StringDictionary>
            {
                [1] = new StringDictionary(),
                [2] = new StringDictionary()
            };

            // Act
            ValidatorMethods.ResetKeyValuesOnTransformedDictionary(_importFile, out _keyRest, out _newKey);

            // Assert
            _importFile.ShouldNotBeNull();
            _importFile.ShouldSatisfyAllConditions(
                () => _newKey.ShouldBe(_importFile.DataTransformed.Count + 1),
                () => _importFile.DataTransformed.Count.ShouldBe(_keyRest.Count));
        }

        [Test]
        public void CreateImportErrorEntity_WhenImportFileObjectIsNull_ThrowsException()
        {
            // Arrange
            _importFile = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ValidatorMethods.CreateImportErrorEntity(_importFile, _rowData, _rowNumber, _errorMsg, _insertSeparator));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateImportErrorEntity_WhenRowDataIsNull_ReturnsImportErrorEntity(bool insertSeparator)
        {
            // Arrange
            _rowData = null;
            _rowNumber = 0;
            _errorMsg = SampleText;
            var badDataRow = insertSeparator ? _sbBadData.ToString().Trim().TrimEnd(CommaJoin) : _sbBadData.ToString().Trim();

            // Act
            var result =
                ValidatorMethods.CreateImportErrorEntity(_importFile, _rowData, _rowNumber, _errorMsg, insertSeparator);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<ImportErrorEntity>(),
                () => result.BadDataRow.ShouldBe(badDataRow),
                () => result.ClientMessage.ShouldBe(_errorMsg),
                () => result.RowNumber.ShouldBe(_rowNumber));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateImportErrorEntity_WhenRowDataIsNotNull_ReturnsImportErrorEntity(bool insertSeparator)
        {
            // Arrange
            _importFile = CreateImportFileObject();
            _rowData = CreateRowDataElements();
            _rowNumber = 0;
            _errorMsg = SampleText;
            var badDataRow = CreateBadDataRowResult(_importFile, _rowData, insertSeparator);

            // Act
            var result =
                ValidatorMethods.CreateImportErrorEntity(_importFile, _rowData, _rowNumber, _errorMsg, insertSeparator);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<ImportErrorEntity>(),
                () => result.BadDataRow.ShouldBe(badDataRow),
                () => result.ClientMessage.ShouldBe(_errorMsg),
                () => result.RowNumber.ShouldBe(_rowNumber));
        }

        [Test]
        public void SetImportErrorsValue_WhenImportFileObjectIsNull_ThrowsException()
        {
            // Arrange
            _importFile = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ValidatorMethods.SetImportErrorsValue(_importFile, _importErrorEntity));
        }

        [Test]
        public void SetImportErrorsValue_WhenImportErrorEntityIsNull_ThrowsException()
        {
            // Arrange
            _importErrorEntity = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ValidatorMethods.SetImportErrorsValue(_importFile, _importErrorEntity));
        }

        [Test]
        public void SetImportErrorsValue_WhenImportErrorsIsNull_ReturnsCheckValue()
        {
            // Arrange
            _importFile.ImportErrors = null;
            _importFile.DataTransformed = new Dictionary<int, StringDictionary>();
            var check = CalcCheckValue(_importFile);

            // Act
            var result = ValidatorMethods.SetImportErrorsValue(_importFile, _importErrorEntity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<int>(),
                () => result.ShouldBe(check));
        }

        [Test]
        public void SetImportErrorsValue_WhenDataTransformedIsZero_ReturnsCheckValue()
        {
            // Arrange
            _importFile.DataTransformed = new Dictionary<int, StringDictionary>();
            var check = CalcCheckValue(_importFile);

            // Act
            var result = ValidatorMethods.SetImportErrorsValue(_importFile, _importErrorEntity);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<int>(),
                () => result.ShouldBe(check));
        }

        [Test]
        public void SetImportErrorsValue_WhenDataTransformedIsGreatherThanZero_ReturnsCheckValue()
        {
            // Arrange
            _importFile.ImportErrorCount = 10;
            _importFile.DataTransformed = new Dictionary<int, StringDictionary>
            {
                [1] = new StringDictionary(),
                [2] = new StringDictionary()
            };

            // Act
            var result = ValidatorMethods.SetImportErrorsValue(_importFile, _importErrorEntity);
            var check = CalcCheckValue(_importFile);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<int>(),
                () => result.ShouldBe(check));
        }

        private int ConvertDefaultMatchValueToInt()
        {
            int match;
            int.TryParse(DefaultMatchValue, out match);

            return match;
        }

        private static DateTime GetSampleDateResult(string qDateValue)
        {
            var sampleDate = new DateTime();
            DateTime.TryParse(qDateValue, out sampleDate);

            return sampleDate;
        }

        public void ResetKeyValuesOnTransformedDictionary_WhenImportFileObjectIsNull_ThrowsException()
        {
            // Arrange
            _importFile = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ValidatorMethods.ResetKeyValuesOnTransformedDictionary(_importFile, out _keyRest, out _newKey));
        }

        private static IList<AdHocDimension> CreateAdHocListWithSpecificElement(string operatorName)
        {
            var adHocList = CreateAdHocList() as List<AdHocDimension>;

            return adHocList?.Where(adHoc => adHoc.Operator.Equals(operatorName)).ToList();
        }

        private static IList<AdHocDimension> CreateAdHocList()
        {
            return new List<AdHocDimension>
            {
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "greater_than" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "less_than" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "greater_than_or_equal_to" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "less_than_or_equal_to" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "is_not_less_than" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "is_not_greater_than" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "equal" },
                new AdHocDimension { MatchValue = DefaultMatchValue, DimensionValue = DefaultDimensionValue, Operator = "not_equal" },
                new AdHocDimension { MatchValue = SampleText, DimensionValue = DefaultDimensionValue, Operator = "contains" },
                new AdHocDimension { MatchValue = SampleText, DimensionValue = DefaultDimensionValue, Operator = "starts_with" },
                new AdHocDimension { MatchValue = SampleText, DimensionValue = DefaultDimensionValue, Operator = "ends_with" }
            };
        }

        private static StringDictionary CreateQDateDictionary(string dateValue)
        {
            return new StringDictionary
            {
                [QDateColumnHeaderKey] = dateValue
            };
        }

        private static ImportFileObject CreateImportFileObject()
        {
            return new ImportFileObject
            {
                BadDataOriginalHeaders = new StringDictionary
                {
                    [DummyText] = DummyText,
                    [SampleText] = SampleText,
                    [OriginalImportrowKey] = OriginalImportrowKey
                }
            };
        }

        private static StringDictionary CreateRowDataElements()
        {
            return new StringDictionary
            {
                [SampleText] = SampleText,
                [OriginalImportrowKey] = OriginalImportrowKey
            };
        }

        private static string CreateBadDataRowResult(ImportFileObject importFile, StringDictionary rowData, bool insertSeparator)
        {
            var sbBadData = new StringBuilder();
            var originalHeaders = new string[importFile.BadDataOriginalHeaders.Count];
            importFile.BadDataOriginalHeaders.Keys.CopyTo(originalHeaders, 0);

            foreach (var header in originalHeaders.OrderBy(x => x))
            {
                if (header.Equals(OriginalImportrowKey, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (importFile.BadDataOriginalHeaders.ContainsKey(header))
                {
                    sbBadData.Append(rowData.ContainsKey(header) ? $"{rowData[header]}{CommaSeparator}" : CommaSeparator);
                }
            }

            return insertSeparator ? sbBadData.ToString().Trim().TrimEnd(CommaJoin) : sbBadData.ToString().Trim();
        }

        private static int CalcCheckValue(ImportFileObject importFile)
        {
            return importFile.DataTransformed.Count > 0
                ? (int)(importFile.ImportErrorCount / (decimal)importFile.DataTransformed.Count * 100)
                : 0;
        }
    }
}
