using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    [TestFixture()]
    public class CombineValuesTest
    {
        private const string TestValue1 = "test1";
        private const string TestValue2 = "test2";
        private const string CombineValuesMethodName = "CombineValues";

        private static readonly object[] DelimitersList = {
            new object[] {"comma", ","},
            new object[] {"colon", ":"},
            new object[] { "semicolon", ";"},
            new object[] {"tab", "\t"},
            new object[] { "tild", "~"},
            new object[] {"pipe", "|"}
        };

        [Test]
        [TestCaseSource(nameof(DelimitersList))]
        public void CombineValues_IterateDelimiters_ReturnsString(string delimiterName, string expectedDelimiter)
        {
            // Arrange
            var privateObject = new PrivateObject(typeof(global::ADMS.Services.Validator.Validator));
            var data = new Dictionary<int, StringDictionary>()
            {
                {  1, new StringDictionary()
                        {
                            { "originalimportrow", "row" },
                            { "test", TestValue1 },
                        }
                },
                {
                    2, new StringDictionary()
                        {
                            { "originalimportrow", "row" },
                            { "test", TestValue2 },
                        }
                }
            };
            const string originalImportRow = "row";
            const string columnName = "test";

            // Act 
            var result = privateObject.Invoke(CombineValuesMethodName, new object[] { data, columnName, delimiterName, originalImportRow }) as string;

            // Assert
            result.ShouldBe(string.Format("{0}{2}{1}", TestValue1, TestValue2, expectedDelimiter));
        }
    }
}
