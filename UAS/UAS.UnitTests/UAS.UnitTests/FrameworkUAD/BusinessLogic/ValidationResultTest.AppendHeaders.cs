using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic;
using NUnit.Framework;
using Shouldly;
using ObjectValidationResult = FrameworkUAD.Object.ValidationResult;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class ValidationResultTest
    {
        private StringBuilder _sbDetailResult = new StringBuilder();

        private const string BadDataOriginalHeadersKey = "BadDataOriginalHeaders";
        private const string HeadersOriginalKey = "HeadersOriginal";
        private const string OriginalImportRow = "originalimportrow";
        private const string KeySample1 = "KeySample1";
        private const string KeySample2 = "KeySample2";
        private const string KeySample3 = "KeySample3";
        private const string SampleText1 = "sample text 1";
        private const string SampleText2 = "sample text 2";
        private string[] _myKeys;

        [Test]
        public void AppendHeaders_WhenValidationResultIsNull_ThrowsException()
        {
            // Arrange
            validationResultObject = null;

            // Act
            Should.Throw<ArgumentNullException>(() => { 
                ValidationResult.AppendHeaders(
                    validationResultObject, _sbDetail, _myKeys, HeadersOriginalKey, _isTextQualifier);
            });

        }

        [Test]
        public void AppendHeaders_WhenSbDetailIsNull_ThrowsException()
        {
            // Arrange
            _sbDetail = null;

            // Act
            Should.Throw<ArgumentNullException>(() => { 
                ValidationResult.AppendHeaders(
                    validationResultObject, _sbDetail, _myKeys, HeadersOriginalKey, _isTextQualifier);
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AppendHeaders_WhenHeaderTypeIsBadDataOriginalHeaders_ShouldFillSbDetail(bool isTextQualifier)
        {
            // Arrange
            validationResultObject.BadDataOriginalHeaders = CreateStringDictionary();
            CreateKeys();
            CreateStringBuilderResult(validationResultObject, BadDataOriginalHeadersKey, isTextQualifier);

            // Act
            ValidationResult.AppendHeaders(validationResultObject, _sbDetail, _myKeys, BadDataOriginalHeadersKey, isTextQualifier);

            // Assert
            _sbDetail.ToString().ShouldBe(_sbDetailResult.ToString());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AppendHeaders_WhenHeaderTypeIsHeadersOriginal_ShouldFillSbDetail(bool isTextQualifier)
        {
            // Arrange
            validationResultObject.HeadersOriginal = CreateStringDictionary();
            CreateKeys();
            CreateStringBuilderResult(validationResultObject, HeadersOriginalKey, isTextQualifier);

            // Act
            ValidationResult.AppendHeaders(validationResultObject, _sbDetail, _myKeys, HeadersOriginalKey, isTextQualifier);

            // Assert
            _sbDetail.ToString().ShouldBe(_sbDetailResult.ToString());
        }

        private void CreateKeys()
        {
            _myKeys = new string[4];
            _myKeys[0] = OriginalImportRow;
            _myKeys[1] = KeySample1;
            _myKeys[2] = KeySample2;
            _myKeys[3] = KeySample3;
        }

        private static StringDictionary CreateStringDictionary()
        {
            return new StringDictionary
            {
                [KeySample1] = SampleText1, 
                [KeySample2] = SampleText2
            };
        }

        private void CreateStringBuilderResult(ObjectValidationResult validationResult, string headerType, bool isTextQualifier)
        {
            var headers = new StringBuilder();
            _sbDetailResult = new StringBuilder();

            foreach (var key in _myKeys.OrderBy(name => name))
            {
                if (key.Equals(OriginalImportRow, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (headerType.Equals(HeadersOriginalKey))
                {
                    if (validationResult.HeadersOriginal.ContainsKey(key))
                    {
                        headers.Append(isTextQualifier ? $"\"{key}\"," : $"{key},");
                    }
                }
                else if (headerType.Equals(BadDataOriginalHeadersKey))
                {
                    if (validationResult.BadDataOriginalHeaders.ContainsKey(key))
                    {
                        headers.Append(isTextQualifier ? $"\"{key}\"," : $"{key},");
                    }
                }
            }
            _sbDetailResult.AppendLine(headers.ToString().Trim().TrimEnd(CommaSeparator));
        }
    }
}
